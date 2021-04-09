using DIY.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DIY.UI
{
    public static class UIUtil
    {
        private static UIUtil_Image _image;
        public static UIUtil_Image Image {
            get {
                if (null == _image)
                {
                    _image = new UIUtil_Image();
                }
                return _image;
            }
        }

        private static UIUtil_Button _button;
        public static UIUtil_Button Button
        {
            get
            {
                if (null == _button)
                {
                    _button = new UIUtil_Button();
                }
                return _button;
            }
        }

        public static UITree AutoCreateUITree(params UINode[] nodes) {
            UITree uiTree = new UITree(nodes);
            return uiTree;
        }

        public static void OpenPanel<T>(Action<T> callback_show) where T : Base_UIPanel,new()
        {
            T targetPanel = new T();
            UICache.Instance.Join(targetPanel);
            targetPanel.Show();
            //遍历UI栈，该冻结冻结，该关闭关闭
            UICache.Instance.Walk((uiPanel) =>
            {
                switch (targetPanel.panelType)
                {
                    case PanelType.HideOther:
                        uiPanel.Hide();
                        break;
                    case PanelType.Transparent:
                        break;
                    default:
                        break;
                }
                if (!uiPanel.isFreeze)
                {
                    uiPanel.Freeze();
                }
            });
        }

        public static void ClosePanel_Top(Action<Base_UIPanel> callback_hide) 
        {
            Base_UIPanel panel_top = UICache.Instance.Remove();
            panel_top.Hide();
            //遍历UI栈，该冻结冻结，该关闭关闭
            UICache.Instance.Walk((uiPanel) =>
            {
                uiPanel.Thaw();
            });
        }

        

    }
}



