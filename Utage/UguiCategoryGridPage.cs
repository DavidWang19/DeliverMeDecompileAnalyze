namespace Utage
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.UI;

    [AddComponentMenu("Utage/Lib/UI/CategoryGridPage")]
    public class UguiCategoryGridPage : MonoBehaviour
    {
        public List<Sprite> buttonSpriteList;
        public UguiAlignGroup categoryAlignGroup;
        private string[] categoryList;
        public GameObject categoryPrefab;
        public GameObject categoryPrefab2;
        public GameObject categoryPrefab3;
        public GameObject categoryPrefab4;
        public UguiToggleGroupIndexed categoryToggleGroup;
        public UguiGridPage gridPage;

        public void Clear()
        {
            this.categoryToggleGroup.ClearToggles();
            this.categoryAlignGroup.DestroyAllChildren();
            this.gridPage.ClearItems();
        }

        private void CreateTabButton(GameObject go, int index)
        {
            Text componentInChildren = go.GetComponentInChildren<Text>();
            if ((componentInChildren != null) && (index < this.categoryList.Length))
            {
                componentInChildren.set_text(this.categoryList[index]);
            }
            Image image = go.GetComponentInChildren<Image>();
            if ((image != null) && (index < this.buttonSpriteList.Count))
            {
                image.set_sprite(this.buttonSpriteList[index]);
            }
        }

        public void Init(string[] categoryList, Action<UguiCategoryGridPage> OpenCurrentCategory)
        {
            <Init>c__AnonStorey0 storey = new <Init>c__AnonStorey0 {
                OpenCurrentCategory = OpenCurrentCategory,
                $this = this
            };
            this.categoryList = categoryList;
            this.categoryToggleGroup.ClearToggles();
            this.categoryAlignGroup.DestroyAllChildren();
            if (categoryList.Length > 1)
            {
                foreach (GameObject obj2 in this.categoryAlignGroup.AddChildrenFromPrefab(categoryList.Length, this.categoryPrefab, new Action<GameObject, int>(this.CreateTabButton)))
                {
                    this.categoryToggleGroup.Add(obj2.GetComponent<Toggle>());
                }
                this.categoryToggleGroup.CurrentIndex = 0;
            }
            this.categoryToggleGroup.OnValueChanged.AddListener(new UnityAction<int>(storey, (IntPtr) this.<>m__0));
            storey.OpenCurrentCategory(this);
        }

        public void OpenCurrentCategory(int itemCount, Action<GameObject, int> CreateItem)
        {
            this.gridPage.Init(itemCount, CreateItem);
            this.gridPage.CreateItems(0);
        }

        public string CurrentCategory
        {
            get
            {
                if (this.categoryList == null)
                {
                    return string.Empty;
                }
                if (this.categoryToggleGroup.CurrentIndex >= this.categoryList.Length)
                {
                    return string.Empty;
                }
                return this.categoryList[this.categoryToggleGroup.CurrentIndex];
            }
        }

        [CompilerGenerated]
        private sealed class <Init>c__AnonStorey0
        {
            internal UguiCategoryGridPage $this;
            internal Action<UguiCategoryGridPage> OpenCurrentCategory;

            internal void <>m__0(int index)
            {
                this.OpenCurrentCategory(this.$this);
            }
        }
    }
}

