using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DIY.UI
{
    public class UIUtil_Image : DIY.UI.UIUtil_Component
    {
        public Image Bind(Transform target, bool closeRay=true)
        {
            Image targetImage = base._Bind<Image>(target);
            targetImage.raycastTarget = closeRay != true;
            return targetImage;
        }
    }
}



