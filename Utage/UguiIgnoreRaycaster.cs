namespace Utage
{
    using System;
    using UnityEngine;

    [AddComponentMenu("Utage/Lib/UI/IgnoreRaycaster")]
    public class UguiIgnoreRaycaster : MonoBehaviour, ICanvasRaycastFilter
    {
        public bool ignoreRaycaster = true;

        public virtual bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
        {
            return !this.ignoreRaycaster;
        }
    }
}

