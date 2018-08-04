namespace Utage
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    [AddComponentMenu("Utage/Lib/System UI/IndicatorIcon")]
    public class IndicatorIcon : MonoBehaviour
    {
        [SerializeField]
        private float animRotZ = -36f;
        [SerializeField]
        private float animTime = 0.08333334f;
        [SerializeField]
        private GameObject icon;
        [SerializeField]
        private bool isDeviceIndicator;
        private bool isStarting;
        private List<object> objList = new List<object>();
        private float rotZ;

        private void Awake()
        {
            if (this.IsDeviceIndicator())
            {
                WrapperUnityVersion.SetActivityIndicatorStyle();
                this.icon.SetActive(false);
            }
        }

        private void DecRef(object obj)
        {
            if (this.objList.Contains(obj))
            {
                this.objList.Remove(obj);
            }
        }

        private void IncRef(object obj)
        {
            if (!this.objList.Contains(obj))
            {
                this.objList.Add(obj);
            }
        }

        private bool IsDeviceIndicator()
        {
            this.isDeviceIndicator = false;
            return this.isDeviceIndicator;
        }

        private void RotIcon()
        {
            this.icon.get_transform().set_eulerAngles(new Vector3(0f, 0f, this.rotZ));
            this.rotZ += this.animRotZ;
        }

        public void StartIndicator(object obj)
        {
            this.IncRef(obj);
            if ((this.objList.Count > 0) && !this.isStarting)
            {
                base.get_gameObject().SetActive(true);
                this.isStarting = true;
                if (!this.IsDeviceIndicator())
                {
                    base.InvokeRepeating("RotIcon", 0f, this.animTime);
                }
            }
        }

        public void StopIndicator(object obj)
        {
            this.DecRef(obj);
            if ((this.objList.Count <= 0) && this.isStarting)
            {
                if (!this.IsDeviceIndicator())
                {
                    base.CancelInvoke();
                }
                base.get_gameObject().SetActive(false);
                this.isStarting = false;
            }
        }
    }
}

