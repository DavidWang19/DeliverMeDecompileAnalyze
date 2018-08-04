namespace Utage
{
    using System;
    using System.Runtime.InteropServices;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UtageExtensions;

    [AddComponentMenu("Utage/ADV/UiManager")]
    public class AdvUguiManager : AdvUiManager
    {
        [SerializeField]
        protected AdvUguiBacklogManager backLog;
        [SerializeField]
        protected bool disableMouseWheelBackLog;
        [SerializeField]
        protected AdvUguiMessageWindowManager messageWindow;
        [SerializeField]
        protected AdvUguiSelectionManager selection;

        protected override void ChangeStatus(AdvUiManager.UiStatus newStatus)
        {
            switch (newStatus)
            {
                case AdvUiManager.UiStatus.Default:
                    this.MessageWindow.Open();
                    if (this.selection != null)
                    {
                        this.selection.Open();
                    }
                    if (this.backLog != null)
                    {
                        this.backLog.Close();
                    }
                    base.Engine.UiManager.HideMenuButton();
                    break;

                case AdvUiManager.UiStatus.Backlog:
                    if (this.backLog != null)
                    {
                        this.MessageWindow.Close();
                        if (this.selection != null)
                        {
                            this.selection.Close();
                        }
                        if (this.backLog != null)
                        {
                            this.backLog.Open();
                        }
                        base.Engine.Config.IsSkip = false;
                        break;
                    }
                    return;

                case AdvUiManager.UiStatus.HideMessageWindow:
                    this.MessageWindow.Close();
                    if (this.selection != null)
                    {
                        this.selection.Close();
                    }
                    if (this.backLog != null)
                    {
                        this.backLog.Close();
                    }
                    base.Engine.Config.IsSkip = false;
                    break;

                case AdvUiManager.UiStatus.Menu:
                    base.Engine.UiManager.ShowMenuButton();
                    break;
            }
            base.status = newStatus;
        }

        public override void Close()
        {
            base.get_gameObject().SetActive(false);
            this.MessageWindow.Close();
            if (this.selection != null)
            {
                this.selection.Close();
            }
            if (this.backLog != null)
            {
                this.backLog.Close();
            }
        }

        public AdvUguiMessageWindowManager GetMessageWindowManagerCreateIfMissing()
        {
            AdvUguiMessageWindowManager[] componentsInChildren = base.GetComponentsInChildren<AdvUguiMessageWindowManager>(true);
            if (componentsInChildren.Length > 0)
            {
                return componentsInChildren[0];
            }
            AdvUguiMessageWindowManager manager = base.get_transform().AddChildGameObjectComponent<AdvUguiMessageWindowManager>("MessageWindowManager");
            RectTransform transform = manager.get_gameObject().AddComponent<RectTransform>();
            transform.set_anchorMin(Vector2.get_zero());
            transform.set_anchorMax(Vector2.get_one());
            transform.set_sizeDelta(Vector2.get_zero());
            transform.SetAsFirstSibling();
            foreach (AdvUguiMessageWindow window in base.GetComponentsInChildren<AdvUguiMessageWindow>(true))
            {
                window.get_transform().SetParent(transform, true);
                if (window.get_transform().get_localScale() == Vector3.get_zero())
                {
                    window.get_transform().set_localScale(Vector3.get_one());
                }
            }
            return manager;
        }

        public virtual void OnInput(BaseEventData data = null)
        {
            AdvUiManager.UiStatus status = base.Status;
            if (status != AdvUiManager.UiStatus.Backlog)
            {
                if (status == AdvUiManager.UiStatus.HideMessageWindow)
                {
                    base.Status = AdvUiManager.UiStatus.Default;
                }
                else if (status == AdvUiManager.UiStatus.Default)
                {
                    if (base.Engine.Config.IsSkip)
                    {
                        base.Engine.Config.ToggleSkip();
                    }
                    else
                    {
                        if (base.IsShowingMessageWindow && !base.Engine.Config.IsSkip)
                        {
                            base.Engine.Page.InputSendMessage();
                        }
                        if (IMManager.active && IMManager.waitIM)
                        {
                            IMManager.waitIM = false;
                        }
                        if ((data != null) && (data is PointerEventData))
                        {
                            base.OnPointerDown(data as PointerEventData);
                        }
                    }
                }
            }
        }

        public virtual void OnPointerDown(BaseEventData data)
        {
            if (((data == null) || !(data is PointerEventData)) || ((data as PointerEventData).get_button() == null))
            {
                this.OnInput(data);
            }
        }

        protected virtual void OnTapCloseWindow()
        {
            base.Status = AdvUiManager.UiStatus.HideMessageWindow;
        }

        public override void Open()
        {
            base.get_gameObject().SetActive(true);
            this.ChangeStatus(AdvUiManager.UiStatus.Default);
        }

        protected virtual void Update()
        {
            bool flag = (base.Engine.Config.IsMouseWheelSendMessage && InputUtil.IsInputScrollWheelDown()) || InputUtil.IsInputKeyboadReturnDown();
            switch (base.Status)
            {
                case AdvUiManager.UiStatus.Default:
                    if (base.IsShowingMessageWindow)
                    {
                        base.Engine.Page.UpdateText();
                    }
                    if (!base.IsShowingMessageWindow && !base.Engine.SelectionManager.IsWaitInput)
                    {
                        if (flag)
                        {
                            base.IsInputTrig = false;
                        }
                        break;
                    }
                    if (InputUtil.IsMouseRightButtonDown())
                    {
                        base.Status = AdvUiManager.UiStatus.Menu;
                    }
                    else if (!this.disableMouseWheelBackLog && InputUtil.IsInputScrollWheelUp())
                    {
                        base.Status = AdvUiManager.UiStatus.Backlog;
                    }
                    else if (flag)
                    {
                        base.Engine.Page.InputSendMessage();
                        base.IsInputTrig = true;
                    }
                    break;

                case AdvUiManager.UiStatus.HideMessageWindow:
                    if (InputUtil.IsMouseRightButtonDown())
                    {
                        base.Status = AdvUiManager.UiStatus.Default;
                    }
                    else if (!this.disableMouseWheelBackLog && InputUtil.IsInputScrollWheelUp())
                    {
                        base.Status = AdvUiManager.UiStatus.Backlog;
                    }
                    break;

                case AdvUiManager.UiStatus.Menu:
                    if (InputUtil.IsMouseRightButtonDown())
                    {
                        base.Status = AdvUiManager.UiStatus.Default;
                    }
                    break;
            }
        }

        public AdvUguiMessageWindowManager MessageWindow
        {
            get
            {
                if (this.messageWindow == null)
                {
                }
                return (this.messageWindow = this.GetMessageWindowManagerCreateIfMissing());
            }
        }
    }
}

