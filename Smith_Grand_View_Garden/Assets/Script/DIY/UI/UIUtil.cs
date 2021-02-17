using DIY.UI;
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

        public static UITree AutoCreateUITree(Transform root,params UINode[] nodes) {
            return new UITree(root, nodes);
        }

    }
}



