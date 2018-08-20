namespace Utage
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.UI;
    using UtageExtensions;

    [AddComponentMenu("Utage/Lib/UI/GridPages")]
    public class UguiGridPage : MonoBehaviour
    {
        protected RectTransform cachedRectTransform;
        private Action<GameObject, int> CallbackCreateItem;
        private int currentPage;
        public GridLayoutGroup grid;
        public GameObject itemPrefab;
        private List<GameObject> items = new List<GameObject>();
        private int maxItemNum;
        private int maxItemPerPage = -1;
        public UguiAlignGroup pageCarouselAlignGroup;
        public List<GameObject> pageCarouselPrefab;
        public UguiToggleGroupIndexed pageCarouselToggles;

        public void ClearItems()
        {
            this.grid.get_transform().DestroyChildren();
        }

        public void CreateItems(int page)
        {
            this.currentPage = page;
            this.pageCarouselToggles.CurrentIndex = page;
            this.ClearItems();
            int num = this.MaxItemPerPage * this.CurrentPage;
            for (int i = 0; i < this.MaxItemPerPage; i++)
            {
                int num3 = num + i;
                if (num3 >= this.maxItemNum)
                {
                    break;
                }
                GameObject item = this.grid.get_transform().AddChildPrefab(this.itemPrefab);
                this.items.Add(item);
                if (this.CallbackCreateItem != null)
                {
                    this.CallbackCreateItem(item, num3);
                }
            }
        }

        private int GetCellCount(float cellSize, float rectSize, float space)
        {
            int num = 0;
            float num2 = 0f;
            while (true)
            {
                num2 += cellSize;
                if (num2 > rectSize)
                {
                    return num;
                }
                num++;
                num2 += space;
            }
        }

        public void Init(int maxItemNum, Action<GameObject, int> callbackCreateItem)
        {
            this.maxItemNum = maxItemNum;
            this.CallbackCreateItem = callbackCreateItem;
            if (this.pageCarouselToggles != null)
            {
                this.pageCarouselToggles.ClearToggles();
                this.pageCarouselAlignGroup.DestroyAllChildren();
                if ((this.MaxPage > 0) && (this.pageCarouselPrefab.Count > 0))
                {
                    foreach (GameObject obj2 in this.pageCarouselAlignGroup.AddChildrenFromPrefab(this.MaxPage + 1, this.pageCarouselPrefab, null))
                    {
                        this.pageCarouselToggles.Add(obj2.GetComponent<Toggle>());
                    }
                    this.pageCarouselToggles.OnValueChanged.AddListener(new UnityAction<int>(this, (IntPtr) this.CreateItems));
                    this.pageCarouselToggles.CurrentIndex = 0;
                    this.pageCarouselToggles.SetActiveLRButtons(true);
                }
                else
                {
                    this.pageCarouselToggles.SetActiveLRButtons(false);
                }
            }
        }

        public void OnClickNextPage()
        {
            int nextPage = this.NextPage;
            if (nextPage != this.CurrentPage)
            {
                this.CreateItems(nextPage);
            }
        }

        public void OnClickPrevPage()
        {
            int prevPage = this.PrevPage;
            if (prevPage != this.CurrentPage)
            {
                this.CreateItems(prevPage);
            }
        }

        protected RectTransform CachedRectTransform
        {
            get
            {
                if (this.cachedRectTransform == null)
                {
                    this.cachedRectTransform = base.GetComponent<RectTransform>();
                }
                return this.cachedRectTransform;
            }
        }

        public int CurrentPage
        {
            get
            {
                return this.currentPage;
            }
        }

        public List<GameObject> Items
        {
            get
            {
                return this.items;
            }
        }

        public int MaxItemPerPage
        {
            get
            {
                if (this.maxItemPerPage < 0)
                {
                    Rect rect = this.CachedRectTransform.get_rect();
                    int num = this.GetCellCount(this.grid.get_cellSize().x, rect.get_size().x, this.grid.get_spacing().x);
                    int num2 = this.GetCellCount(this.grid.get_cellSize().y, rect.get_size().y, this.grid.get_spacing().y);
                    switch (this.grid.get_constraint())
                    {
                        case 1:
                            num = Mathf.Min(num, this.grid.get_constraintCount());
                            break;

                        case 2:
                            num2 = Mathf.Min(num2, this.grid.get_constraintCount());
                            break;
                    }
                    this.maxItemPerPage = num * num2;
                }
                return this.maxItemPerPage;
            }
        }

        public int MaxPage
        {
            get
            {
                return ((this.maxItemNum - 1) / this.MaxItemPerPage);
            }
        }

        public int NextPage
        {
            get
            {
                return Mathf.Min(this.CurrentPage + 1, this.MaxPage);
            }
        }

        public int PrevPage
        {
            get
            {
                return Mathf.Max(this.CurrentPage - 1, 0);
            }
        }
    }
}

