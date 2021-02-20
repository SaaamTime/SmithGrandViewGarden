

using DIY.Debug;
using DIY.Event;
using DIY.Foundation;
using System;
using UnityEditor;
using UnityEngine;

namespace DIY.UI
{
    public enum PanelType {
        HideOther = 1,//遮盖掉下边的，这个需要关闭下边的，可以降低drawcall
        Transparent = 2,//以下的不遮盖
    }

    public abstract class Base_UIPanel
    {
        public string name;
        public string path;
        public PanelType panelType = PanelType.HideOther;//默认是全显示，遮盖掉下边的
        public Transform transform;//界面实体引用
        protected Action callback_show;//显示界面时执行
        protected Action callback_hide;//隐藏界面后执行
        protected Action callback_freeze;//界面冻结时执行
        protected Action callback_thaw;//界面解冻时执行
        public UITree uiTree;
        public bool isFreeze;
        public Base_UIPanel() {
            InitBasicInfo();
            uiTree = InitUITree();
            //加载该界面的预设体，实例化该界面,并保存引用
            AssetDatabase.
        }
        public abstract void InitBasicInfo();

        public abstract UITree InitUITree();

        #region UIPanel生命周期

        public virtual void Show() {
            Looog.Warn(name, " Show ---------------");
            transform.gameObject.SetActive(true);
            SafeUtil.DoAction(callback_show);
        }

        public virtual void Hide() {
            Looog.Warn(name, " Hide ---------------");
            transform.gameObject.SetActive(false);
            SafeUtil.DoAction(callback_hide);
        }

        //非首层显示，但是没有关闭，依然有可能需要刷新
        public virtual void Freeze() {
            Looog.Warn(name, " Freeze ---------------");
            SafeUtil.DoAction(callback_freeze);
        }

        public virtual void Thaw() {
            Looog.Warn(name, " Thaw ---------------");
            SafeUtil.DoAction(callback_thaw);
        }
        #endregion

    }
}
