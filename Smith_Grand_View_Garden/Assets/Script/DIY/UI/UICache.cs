using System;
using System.Collections.Generic;

namespace DIY.UI
{
    public class  UICache
    {
        private static UICache _instance;
        public static UICache Instance
        {
            get
            {
                if (null == _instance)
                {
                    _instance = new UICache();
                }
                return _instance;
            }
        }

        private Stack<Base_UIPanel> stack_panels;

        private UICache() {
            stack_panels = new Stack<Base_UIPanel>();
        }

        /// <summary>
        /// 加入管理
        /// </summary>
        /// <param name="uiPanel"></param>
        public void Join(Base_UIPanel uiPanel) {
            stack_panels.Push(uiPanel);
        }

        /// <summary>
        /// 临时借用第一个页面
        /// </summary>
        /// <param name="uiPanel"></param>
        public Base_UIPanel GetPanel_Top()
        {
            return stack_panels.Peek();
        }

        /// <summary>
        /// 删除最上端界面
        /// </summary>
        public Base_UIPanel Remove() {
            return stack_panels.Pop();
        }

        public Base_UIPanel GetPanel_ByName(string name) {
            Base_UIPanel targetPanel = FindUIPanel((uiPanel) =>
            {
                return name.Equals(uiPanel.name);
            });
            return targetPanel;
        }

        public Base_UIPanel FindUIPanel(Func<Base_UIPanel,bool> func) {
            foreach (Base_UIPanel uiPanel in stack_panels)
            {
                if (func(uiPanel))
                {
                    return uiPanel;
                }
            }
            return null;
        }

        public void Walk(Action<Base_UIPanel> action) {
            foreach (Base_UIPanel uiPanel in stack_panels)
            {
                action(uiPanel);
            }
        }
        public void Clear()
        {
            stack_panels.Clear();
        }
    }
}
