namespace Utage
{
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    [AddComponentMenu("Utage/ADV/UiBacklogManager")]
    public class AdvUguiBacklogManager : MonoBehaviour
    {
        [SerializeField]
        private AdvEngine engine;
        [SerializeField]
        private UguiNovelText fullScreenLogText;
        public bool isCloseScrollWheelDown;
        [SerializeField]
        private UguiListView listView;
        [SerializeField]
        private BacklogType type;

        private void Back()
        {
            this.Close();
            this.engine.UiManager.Status = AdvUiManager.UiStatus.Default;
        }

        private void CallbackCreateItem(GameObject go, int index)
        {
            AdvBacklog data = this.BacklogManager.Backlogs[(this.BacklogManager.Backlogs.Count - index) - 1];
            go.GetComponent<AdvUguiBacklog>().Init(data);
        }

        public void Close()
        {
            if (this.ListView != null)
            {
                this.ListView.ClearItems();
            }
            if (this.FullScreenLogText != null)
            {
                this.FullScreenLogText.set_text(string.Empty);
            }
            base.get_gameObject().SetActive(false);
        }

        private void InitialzeAsFullScreenText()
        {
            this.ListView.CreateItems(this.BacklogManager.Backlogs.Count, new Action<GameObject, int>(this.CallbackCreateItem));
        }

        private void InitialzeAsMessageWindow()
        {
            this.ListView.CreateItems(this.BacklogManager.Backlogs.Count, new Action<GameObject, int>(this.CallbackCreateItem));
        }

        private bool IsInputBottomEndScrollWheelDown()
        {
            if (this.isCloseScrollWheelDown && InputUtil.IsInputScrollWheelDown())
            {
                Scrollbar scrollbar = this.ListView.ScrollRect.get_verticalScrollbar();
                if (scrollbar != null)
                {
                    return (scrollbar.get_value() <= 0f);
                }
            }
            return false;
        }

        public void OnTapBack()
        {
            this.Back();
        }

        public void Open()
        {
            base.get_gameObject().SetActive(true);
            switch (this.Type)
            {
                case BacklogType.FullScreenText:
                    this.InitialzeAsFullScreenText();
                    return;
            }
            this.InitialzeAsMessageWindow();
        }

        private void Update()
        {
            if (InputUtil.IsMouseRightButtonDown() || this.IsInputBottomEndScrollWheelDown())
            {
                this.Back();
            }
        }

        private AdvBacklogManager BacklogManager
        {
            get
            {
                return this.engine.BacklogManager;
            }
        }

        public UguiNovelText FullScreenLogText
        {
            get
            {
                return this.fullScreenLogText;
            }
        }

        public bool IsOpen
        {
            get
            {
                return base.get_gameObject().get_activeSelf();
            }
        }

        public UguiListView ListView
        {
            get
            {
                return this.listView;
            }
        }

        private BacklogType Type
        {
            get
            {
                return this.type;
            }
        }

        public enum BacklogType
        {
            MessageWindow,
            FullScreenText
        }
    }
}

