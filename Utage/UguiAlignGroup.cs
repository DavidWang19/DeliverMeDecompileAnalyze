namespace Utage
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using UtageExtensions;

    [ExecuteInEditMode]
    public abstract class UguiAlignGroup : UguiLayoutControllerBase, ILayoutController
    {
        public bool isAutoResize;
        public float space;

        protected UguiAlignGroup()
        {
        }

        public List<GameObject> AddChildrenFromPrefab(int count, List<GameObject> prefab, Action<GameObject, int> callcackCreateItem)
        {
            List<GameObject> list = new List<GameObject>();
            for (int i = 0; i < count; i++)
            {
                GameObject item = base.CachedRectTransform.AddChildPrefab(prefab[i % prefab.Count]);
                list.Add(item);
                if (callcackCreateItem != null)
                {
                    callcackCreateItem(item, i);
                }
            }
            return list;
        }

        public List<GameObject> AddChildrenFromPrefab(int count, GameObject prefab, Action<GameObject, int> callcackCreateItem)
        {
            List<GameObject> list = new List<GameObject>();
            for (int i = 0; i < count; i++)
            {
                GameObject item = base.CachedRectTransform.AddChildPrefab(prefab);
                list.Add(item);
                if (callcackCreateItem != null)
                {
                    callcackCreateItem(item, i);
                }
            }
            return list;
        }

        public void DestroyAllChildren()
        {
            base.CachedRectTransform.DestroyChildren();
        }

        public abstract void Reposition();
        public void SetLayoutHorizontal()
        {
            this.tracker.Clear();
            this.Reposition();
        }

        public void SetLayoutVertical()
        {
            this.tracker.Clear();
            this.Reposition();
        }
    }
}

