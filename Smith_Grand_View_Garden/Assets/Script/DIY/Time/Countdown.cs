using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace LuaFramework
{
    /// <summary>
    /// 注：
    ///     1、采用Thread.Sleep进行计数，不直接使用FixUpdate进行轮训，由于Thread不能直接调用Unity组件，所以还是要在FixUpdate里执行
    ///     2、不再采用{key:timestamp,value:{events}}这种结构，这种需要遍历两级字典，采用{key:clockName,value:Countdown}结构，即只有一级字典，一个事件绑定一个时间戳，实际上在计数时互相不影响
    ///     
    /// </summary>
    public class UIClock : MonoBehaviour
    {
        public Thread _thread;
        public Dictionary<string, Countdown> dic_countdown;
        public Queue<Countdown> queue_updating;
        public bool alive = true;
        #region 生命周期处理
        private void Awake()
        {
            dic_countdown = new Dictionary<string, Countdown>();
            queue_updating = new Queue<Countdown>();
            _thread = new Thread(_Launch);
            _thread.Start();
        }
        private void FixedUpdate()
        {
            lock (queue_updating)
            {
                while (queue_updating.Count > 0)
                {
                    Countdown countdown = queue_updating.Dequeue();
                    switch (countdown.state)
                    {
                        case CountdownState.Normal:
                            //Debug.Log("UIClock.FixedUpdate----正常计数" + "该log不应该输出，只要输出，说明有问题！");
                            break;
                        case CountdownState.UnitReachEnd:
                            //Debug.Log("UIClock.FixedUpdate----单位计数用完，要更新----" + countdown.name + "剩余秒数" + countdown.seconds + "  剩余单位计数数" + countdown.countdown_unit);
                            countdown.AutoUpdate();
                            break;
                        case CountdownState.End:
                            //Debug.Log("UIClock.FixedUpdate----计时器到达终点，end----" + countdown.name + "剩余秒数" + countdown.seconds + "  剩余单位计数" + countdown.countdown_unit);
                            countdown.AutoDoEnd();
                            RemoveClock(countdown.name);
                            break;
                        default:
                            break;
                    }
                }
            }
        }
        public void OnApplicationQuit()
        {
            Debug.Log("关闭了游戏，Clock销毁");
            alive = false;
            DestroyImmediate(gameObject);
        }

        private void OnDestroy()
        {
            alive = false;
            if (_thread != null)
            {
                _thread.Abort();
                _thread = null;
            }
        }
        #endregion


        private void _Launch()
        {
            //Debug.Log("UIClock启动");
            while (alive)
            {
                _Tick();
                Thread.Sleep(1000);
            }
        }

        /// <summary>
        /// 只管秒数更替，不关注事件执行（关注了也执行不了。。。）
        /// </summary>
        private void _Tick()
        {
            lock (dic_countdown)
            {
                foreach (KeyValuePair<string, Countdown> _kv in dic_countdown)
                {

                    CountdownState state = _kv.Value.Tick();
                    //Debug.Log("UIClock正在tick  " + _kv.Key + "  " + state.ToString() + "  剩余秒数" + _kv.Value.seconds + "  单位剩余秒数" + _kv.Value.countdown_unit);
                    switch (state)
                    {
                        case CountdownState.Normal:
                            break;
                        case CountdownState.UnitReachEnd:
                            queue_updating.Enqueue(_kv.Value);
                            break;
                        case CountdownState.End:
                            queue_updating.Enqueue(_kv.Value);
                            break;
                        default:
                            break;
                    }

                }
            }
        }

        public Countdown AddOrGetClock(string _name, int _howLong, Action<int> _updateEvent)
        {
            if (dic_countdown.ContainsKey(_name))
            {
                return dic_countdown[_name];
            }
            lock (dic_countdown)
            {
                Countdown newClock = new Countdown(_name, _howLong, _updateEvent);
                dic_countdown.Add(_name, newClock);
                return newClock;
            }
        }
        public void AddClock(string _name, int _howLong, Action<int> _updateEvent)
        {
            AddOrGetClock(_name, _howLong, _updateEvent);
        }

        public void AddClock(string _name, int _howLong, Action<int> _updateEvent, int _newUnit)
        {
            Countdown newClock = AddOrGetClock(_name, _howLong, _updateEvent);
            newClock.ResetUnitSecond(_newUnit);
        }

        public void AddClock(string _name, int _howLong, Action<int> _updateEvent, Action<int> _endEvent)//, Func<int, bool> _checkEndEvent)
        {
            Countdown newClock = AddOrGetClock(_name, _howLong, _updateEvent);
            if (_endEvent != null)
            {
                newClock.BindEvent_End(_endEvent);
            }
            //if (_checkEndEvent != null)
            //{
            //    newClock.BindEvent_CheckEnd(_checkEndEvent);
            //}
        }

        public void AddClock(string _name, int _howLong, Action<int> _updateEvent, int _newUnit, Action<int> _endEvent)//, Func<int, bool> _checkEndEvent)
        {
            AddOrGetClock(_name, _howLong, _updateEvent);
            AddClock(_name, _howLong, _updateEvent, _newUnit);
            AddClock(_name, _howLong, _updateEvent, _endEvent);
            //AddClock(_name, _howLong, _updateEvent, _endEvent, _checkEndEvent);
        }

        public void RemoveClock(string _name)
        {
            lock (dic_countdown)
            {
                dic_countdown.Remove(_name);
            }
        }

        public void Clear()
        {
            dic_countdown.Clear();
            queue_updating.Clear();
        }
    }

    public enum CountdownState
    {
        //("倒计时正在运转计时，不需要任何操作")
        Normal = 0,
        //("倒计时单位计时已经走完，需要刷新操作")
        UnitReachEnd = 1,
        //Flags("倒计时全部走完，需要终止操作")
        End = -1,
    }

    public class Countdown
    {
        //计时器总共有多久（每秒减一个单位）
        public int seconds;
        //计时单位，默认1秒
        public int unit = 1;
        //计时单位倒计时，为0时，timestamp减去unit
        public int countdown_unit;
        //计时器名称
        public string name;
        //记录了几圈了
        public int laps = 0;
        //计时器每次更迭一次unit后执行
        public Action<int> event_update;
        //计时器终止事件
        public Action<int> event_end;
        //计时器终止（有可能是中途满足一定条件，倒计时终止）（由于线程中不能调用组件，该事件不再关注，在游戏里自行在Clock里除名）
        //public Func<int, bool> event_checkEnd;
        public CountdownState state = CountdownState.Normal;
        public Countdown(string _name, int _howLong, Action<int> _event_update)
        {
            name = _name;
            seconds = _howLong;
            event_update = _event_update;
        }

        public void BindEvent_End(Action<int> _event_end)
        {
            event_end = _event_end;
        }

        //public void BindEvent_CheckEnd(Func<int, bool> _event_checkEnd)
        //{
        //    event_checkEnd = _event_checkEnd;
        //}

        public void ResetUnitSecond(int _newUnit)
        {
            unit = _newUnit;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>0:正常计数，1:已经走完一个单位，需要执行更新，-1：到达计时终点</returns>
        public CountdownState Tick()
        {
            state = CountdownState.Normal;
            if (seconds < 1)
            {
                state = CountdownState.End;
            }
            else
            {
                countdown_unit--;
                if (countdown_unit < 1)
                {
                    countdown_unit = unit;
                    if (laps < 1)
                    {
                        //启动的首次刷新

                    }
                    else
                    {
                        seconds -= unit;
                    }
                    laps++;
                    state = CountdownState.UnitReachEnd;
                }
            }
            return state;
        }
        public void AutoUpdate()
        {
            if (event_update != null)
            {
                event_update(seconds);
            }
        }
        public void AutoDoEnd()
        {
            if (event_end != null)
            {
                event_end(seconds);
            }
        }
        //public bool AutoCheckIsEnd()
        //{
        //    if (event_checkEnd != null)
        //    {
        //        if (event_checkEnd(seconds))
        //        {
        //            if (event_end != null)
        //            {
        //                event_end(seconds);
        //            }
        //            return true;
        //        }
        //    }
        //    return false;
        //}

        public bool Check_ReachEnd()
        {
            return state == CountdownState.End;
        }

        public bool Check_ReachUpdate()
        {
            return state == CountdownState.UnitReachEnd;
        }
    }
}
