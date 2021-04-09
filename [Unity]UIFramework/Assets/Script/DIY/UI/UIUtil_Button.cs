using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DIY.UI
{
    public class UIUtil_Button : DIY.UI.UIUtil_Component
    {
        public Button Bind(Transform target, Action event_click = null)
        {
            Button targetButton = base._Bind<Button>(target);
            if (null!=event_click)
            {
                BindEvent(targetButton, event_click);
            }
            return targetButton;
        }

        public void BindEvent(Button button,Action action) {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() =>
            {
                action.Invoke();
            });
        }
    }
}



