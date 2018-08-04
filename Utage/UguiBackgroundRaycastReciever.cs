namespace Utage
{
    using System;
    using UnityEngine;

    [AddComponentMenu("Utage/Lib/UI/BackgroundRaycastReciever")]
    public class UguiBackgroundRaycastReciever : MonoBehaviour
    {
        [SerializeField]
        private UguiBackgroundRaycaster raycaster;

        private void OnDisable()
        {
            this.Raycaster.RemoveTarget(base.get_gameObject());
        }

        private void OnEnable()
        {
            this.Raycaster.AddTarget(base.get_gameObject());
        }

        public UguiBackgroundRaycaster Raycaster
        {
            get
            {
                if (this.raycaster == null)
                {
                }
                return (this.raycaster = Object.FindObjectOfType<UguiBackgroundRaycaster>());
            }
            set
            {
                this.raycaster = value;
            }
        }
    }
}

