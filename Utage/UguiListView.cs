namespace Utage
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using UtageExtensions;

    [RequireComponent(typeof(UnityEngine.UI.ScrollRect)), AddComponentMenu("Utage/Lib/UI/ListView")]
    public class UguiListView : MonoBehaviour
    {
        private RectTransform content;
        [SerializeField]
        private bool isAutoCenteringOnRepostion;
        [SerializeField]
        private bool isStopScroolWithAllInnner = true;
        [SerializeField]
        private GameObject itemPrefab;
        private List<GameObject> items = new List<GameObject>();
        [SerializeField]
        private GameObject maxArrow;
        [SerializeField]
        private GameObject minArrow;
        private UguiAlignGroup positionGroup;
        private UnityEngine.UI.ScrollRect scrollRect;
        private RectTransform scrollRectTransform;
        [SerializeField]
        private Type scrollType;

        public void AddItems(List<GameObject> items)
        {
            foreach (GameObject obj2 in items)
            {
                this.Content.AddChild(obj2);
            }
        }

        public void ClearItems()
        {
            this.items.Clear();
            this.Content.DestroyChildren();
            this.ScrollRect.set_velocity(Vector2.get_zero());
        }

        public void CreateItems(int itemNum, Action<GameObject, int> callbackCreateItem)
        {
            this.ClearItems();
            for (int i = 0; i < itemNum; i++)
            {
                GameObject item = this.Content.AddChildPrefab(this.ItemPrefab.get_gameObject());
                this.items.Add(item);
                if (callbackCreateItem != null)
                {
                    callbackCreateItem(item, i);
                }
            }
            this.Reposition();
        }

        private bool IsContentInnerScrollRect()
        {
            Type scrollType = this.ScrollType;
            if (scrollType != Type.Horizontal)
            {
                return ((scrollType == Type.Vertical) && (this.Content.get_rect().get_height() <= this.ScrollRectTransform.get_rect().get_height()));
            }
            return (this.Content.get_rect().get_width() <= this.ScrollRectTransform.get_rect().get_width());
        }

        private void RefreshArrow()
        {
            if (this.IsContentInnerScrollRect())
            {
                if (null != this.MinArrow)
                {
                    this.MinArrow.SetActive(false);
                }
                if (null != this.MaxArrow)
                {
                    this.MaxArrow.SetActive(false);
                }
            }
            else
            {
                float num;
                switch (this.ScrollType)
                {
                    case Type.Horizontal:
                        num = this.ScrollRect.get_horizontalNormalizedPosition();
                        if (null != this.MinArrow)
                        {
                            this.MinArrow.SetActive(num > 0f);
                        }
                        if (null != this.MaxArrow)
                        {
                            this.MaxArrow.SetActive(num < 1f);
                        }
                        break;

                    case Type.Vertical:
                        num = this.ScrollRect.get_verticalNormalizedPosition();
                        if (null != this.MinArrow)
                        {
                            this.MinArrow.SetActive(num < 1f);
                        }
                        if (null != this.MaxArrow)
                        {
                            this.MaxArrow.SetActive(num > 0f);
                        }
                        break;
                }
            }
        }

        public void Reposition()
        {
            this.Content.set_anchoredPosition(Vector2.get_zero());
            this.ScrollRect.set_velocity(Vector2.get_zero());
            this.PositionGroup.Reposition();
            bool flag = this.IsContentInnerScrollRect() && this.IsStopScroolWithAllInnner;
            switch (this.ScrollType)
            {
                case Type.Horizontal:
                    this.ScrollRect.set_horizontal(!flag);
                    this.ScrollRect.set_vertical(false);
                    if (this.isAutoCenteringOnRepostion)
                    {
                        if (flag)
                        {
                            float num = (this.ScrollRectTransform.get_sizeDelta().x - this.Content.get_sizeDelta().x) / 2f;
                            this.Content.set_anchoredPosition(new Vector3(num, 0f, 0f));
                        }
                        else
                        {
                            this.ScrollRect.set_horizontalNormalizedPosition(0.5f);
                        }
                    }
                    break;

                case Type.Vertical:
                    this.ScrollRect.set_horizontal(false);
                    this.ScrollRect.set_vertical(!flag);
                    if (this.isAutoCenteringOnRepostion)
                    {
                        if (flag)
                        {
                            float num2 = -(this.ScrollRectTransform.get_sizeDelta().y - this.Content.get_sizeDelta().y) / 2f;
                            this.Content.set_anchoredPosition(new Vector3(0f, num2, 0f));
                        }
                        else
                        {
                            this.ScrollRect.set_verticalNormalizedPosition(0.5f);
                        }
                    }
                    break;
            }
            this.ScrollRect.set_enabled(!flag);
        }

        private void Update()
        {
            this.RefreshArrow();
        }

        public RectTransform Content
        {
            get
            {
                if (this.content == null)
                {
                }
                return (this.content = this.ScrollRect.get_content());
            }
        }

        public bool IsAutoCenteringOnRepostion
        {
            get
            {
                return this.isAutoCenteringOnRepostion;
            }
        }

        public bool IsStopScroolWithAllInnner
        {
            get
            {
                return this.isStopScroolWithAllInnner;
            }
        }

        public GameObject ItemPrefab
        {
            get
            {
                return this.itemPrefab;
            }
            set
            {
                this.itemPrefab = value;
            }
        }

        public List<GameObject> Items
        {
            get
            {
                return this.items;
            }
        }

        public GameObject MaxArrow
        {
            get
            {
                return this.maxArrow;
            }
            set
            {
                this.maxArrow = value;
            }
        }

        public GameObject MinArrow
        {
            get
            {
                return this.minArrow;
            }
            set
            {
                this.minArrow = value;
            }
        }

        public UguiAlignGroup PositionGroup
        {
            get
            {
                if (this.positionGroup == null)
                {
                    this.positionGroup = this.Content.GetComponent<UguiAlignGroup>();
                    if (this.positionGroup == null)
                    {
                        Debug.LogError("AlignGroup Component is not attached on ScrollRect Content");
                    }
                }
                return this.positionGroup;
            }
        }

        public UnityEngine.UI.ScrollRect ScrollRect
        {
            get
            {
                if (this.scrollRect == null)
                {
                }
                return (this.scrollRect = base.GetComponent<UnityEngine.UI.ScrollRect>());
            }
        }

        public RectTransform ScrollRectTransform
        {
            get
            {
                if (this.scrollRectTransform == null)
                {
                }
                return (this.scrollRectTransform = this.ScrollRect.GetComponent<RectTransform>());
            }
        }

        public Type ScrollType
        {
            get
            {
                return this.scrollType;
            }
        }

        public enum Type
        {
            Horizontal,
            Vertical
        }
    }
}

