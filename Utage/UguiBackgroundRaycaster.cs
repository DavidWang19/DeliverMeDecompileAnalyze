namespace Utage
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.EventSystems;

    [AddComponentMenu("Utage/Lib/UI/BackgroundRaycaster "), RequireComponent(typeof(Camera))]
    public class UguiBackgroundRaycaster : BaseRaycaster
    {
        private Camera cachedCamera;
        [SerializeField]
        private LetterBoxCamera letterBoxCamera;
        [SerializeField]
        private int m_Priority = 0x7fffffff;
        [NonSerialized]
        private List<GameObject> targetObjectList = new List<GameObject>();

        public void AddTarget(GameObject go)
        {
            if (!this.targetObjectList.Contains(go))
            {
                this.targetObjectList.Add(go);
            }
        }

        public override void Raycast(PointerEventData eventData, List<RaycastResult> resultAppendList)
        {
            Vector2 vector;
            if (this.letterBoxCamera == null)
            {
                vector = new Vector2(eventData.get_position().x / ((float) Screen.get_width()), eventData.get_position().y / ((float) Screen.get_height()));
            }
            else
            {
                vector = this.letterBoxCamera.CachedCamera.ScreenToViewportPoint(eventData.get_position());
            }
            if (((vector.x >= 0f) && (vector.x <= 1f)) && ((vector.y >= 0f) && (vector.y <= 1f)))
            {
                int num = 0;
                foreach (GameObject obj2 in this.targetObjectList)
                {
                    RaycastResult item = new RaycastResult {
                        distance = float.MaxValue
                    };
                    item.set_gameObject(obj2);
                    item.index = num++;
                    item.module = this;
                    resultAppendList.Add(item);
                }
            }
        }

        public void RemoveTarget(GameObject go)
        {
            if (this.targetObjectList.Contains(go))
            {
                this.targetObjectList.Remove(go);
            }
        }

        private Camera CachedCamera
        {
            get
            {
                if (this.cachedCamera == null)
                {
                }
                return (this.cachedCamera = base.GetComponent<Camera>());
            }
        }

        public override Camera eventCamera
        {
            get
            {
                return this.CachedCamera;
            }
        }

        public override int sortOrderPriority
        {
            get
            {
                return this.m_Priority;
            }
        }
    }
}

