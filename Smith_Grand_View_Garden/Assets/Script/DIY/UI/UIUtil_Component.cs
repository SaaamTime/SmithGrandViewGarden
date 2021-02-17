using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DIY.UI {
    public abstract class UIUtil_Component {
         
        protected virtual T _Bind<T>(Transform target) where T : Component
        {
            T component_target = target.GetComponent<T>();
            if (null == component_target)
            {
                component_target = target.gameObject.AddComponent<T>();
            }
            return component_target;
        }
    }
}



