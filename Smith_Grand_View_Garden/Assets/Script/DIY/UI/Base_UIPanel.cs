

using DIY.Event;
using DIY.Foundation;
using System;
using UnityEngine;

namespace DIY.UI
{
    public abstract class Base_UIPanel
    {
        public string name;
        public string path;
        public Transform transform;
        protected Action callback_show;
        protected Action callback_hide;
        protected Action callback_freeze;
        public UITree uiTree;
        public Base_UIPanel(string name,string path,UITree uiTree) {
            this.name = name;
            this.path = path;
            this.uiTree = uiTree;
            //加载该界面，实例化该界面,并保存引用
            
        }
        public void Show() {
            transform.gameObject.SetActive(true);
            SafeUtil.DoAction(callback_show);
        }

        public void Hide() {
            transform.gameObject.SetActive(false);
            SafeUtil.DoAction(callback_hide);
        }

        //非首层显示，但是没有关闭，依然有可能需要刷新
        public void Freeze() {
            SafeUtil.DoAction(callback_freeze);
        }

    }
}
