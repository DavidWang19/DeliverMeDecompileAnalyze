namespace Utage
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.Events;
    using UtageExtensions;

    [AddComponentMenu("Utage/ADV/UiSelectionManager")]
    public class AdvUguiSelectionManager : MonoBehaviour
    {
        private UnityEngine.CanvasGroup canvasGroup;
        [SerializeField]
        private AdvEngine engine;
        private List<GameObject> items = new List<GameObject>();
        private UguiListView listView;
        [SerializeField]
        private List<GameObject> prefabList;
        [SerializeField]
        private Color selectedColor = new Color(0.8f, 0.8f, 0.8f);
        [SerializeField]
        private SelectedColorMode selectedColorMode;

        private void Awake()
        {
            this.SelectionManager.OnClear.AddListener(new UnityAction<AdvSelectionManager>(this, (IntPtr) this.OnClear));
            this.SelectionManager.OnBeginShow.AddListener(new UnityAction<AdvSelectionManager>(this, (IntPtr) this.OnBeginShow));
            this.SelectionManager.OnBeginWaitInput.AddListener(new UnityAction<AdvSelectionManager>(this, (IntPtr) this.OnBeginWaitInput));
            this.ClearAll();
        }

        private void CallbackCreateItem(GameObject go, int index)
        {
            AdvSelection data = this.SelectionManager.Selections[index];
            go.GetComponentInChildren<AdvUguiSelection>().Init(data, new Action<AdvUguiSelection>(this.OnTap));
        }

        private void ClearAll()
        {
            this.ListView.ClearItems();
            foreach (GameObject obj2 in this.Items)
            {
                Object.Destroy(obj2);
            }
            this.Items.Clear();
        }

        public void Close()
        {
            base.get_gameObject().SetActive(false);
        }

        private void CreateItems()
        {
            this.ClearAll();
            List<GameObject> items = new List<GameObject>();
            foreach (AdvSelection selection in this.SelectionManager.Selections)
            {
                GameObject item = Object.Instantiate<GameObject>(this.GetPrefab(selection));
                AdvUguiSelection componentInChildren = item.GetComponentInChildren<AdvUguiSelection>();
                if (componentInChildren != null)
                {
                    componentInChildren.Init(selection, new Action<AdvUguiSelection>(this.OnTap));
                }
                switch (this.selectedColorMode)
                {
                    case SelectedColorMode.Change:
                        if (this.Engine.SystemSaveData.SelectionData.Check(selection))
                        {
                            item.SendMessage("OnInitSelected", this.selectedColor);
                        }
                        break;
                }
                this.Items.Add(item);
                if (!selection.X.HasValue || !selection.Y.HasValue)
                {
                    items.Add(item);
                }
                else
                {
                    base.get_transform().AddChild(item, new Vector3(selection.X.Value, selection.Y.Value, 0f));
                }
            }
            this.ListView.AddItems(items);
            this.ListView.Reposition();
        }

        private GameObject GetPrefab(AdvSelection selectionData)
        {
            <GetPrefab>c__AnonStorey0 storey = new <GetPrefab>c__AnonStorey0 {
                selectionData = selectionData
            };
            GameObject obj2 = null;
            if (!string.IsNullOrEmpty(storey.selectionData.PrefabName))
            {
                obj2 = this.PrefabList.Find(new Predicate<GameObject>(storey.<>m__0));
                if (obj2 != null)
                {
                    return obj2;
                }
                Debug.LogError("Not found Selection Prefab : " + storey.selectionData.PrefabName);
            }
            return ((this.PrefabList.Count <= 0) ? this.ListView.ItemPrefab : this.PrefabList[0]);
        }

        public void OnBeginShow(AdvSelectionManager manager)
        {
            this.CreateItems();
            this.CanvasGroup.set_interactable(false);
        }

        public void OnBeginWaitInput(AdvSelectionManager manager)
        {
            this.CanvasGroup.set_interactable(true);
        }

        public void OnClear(AdvSelectionManager manager)
        {
            this.ClearAll();
        }

        private void OnTap(AdvUguiSelection item)
        {
            this.SelectionManager.Select(item.Data);
            this.ClearAll();
        }

        public void Open()
        {
            base.get_gameObject().SetActive(true);
        }

        private UnityEngine.CanvasGroup CanvasGroup
        {
            get
            {
                return base.get_gameObject().GetComponentCacheCreateIfMissing<UnityEngine.CanvasGroup>(ref this.canvasGroup);
            }
        }

        public AdvEngine Engine
        {
            get
            {
                if (this.engine == null)
                {
                }
                return (this.engine = base.GetComponent<AdvEngine>());
            }
        }

        public List<GameObject> Items
        {
            get
            {
                return this.items;
            }
        }

        public UguiListView ListView
        {
            get
            {
                if (this.listView == null)
                {
                }
                return (this.listView = base.GetComponent<UguiListView>());
            }
        }

        private List<GameObject> PrefabList
        {
            get
            {
                return this.prefabList;
            }
        }

        private AdvSelectionManager SelectionManager
        {
            get
            {
                return this.engine.SelectionManager;
            }
        }

        [CompilerGenerated]
        private sealed class <GetPrefab>c__AnonStorey0
        {
            internal AdvSelection selectionData;

            internal bool <>m__0(GameObject x)
            {
                return (x.get_name() == this.selectionData.PrefabName);
            }
        }

        private enum SelectedColorMode
        {
            None,
            Change
        }
    }
}

