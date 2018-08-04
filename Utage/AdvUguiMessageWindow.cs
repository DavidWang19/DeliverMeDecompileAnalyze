namespace Utage
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.Serialization;
    using UnityEngine.UI;

    [AddComponentMenu("Utage/ADV/UiMessageWindow")]
    public class AdvUguiMessageWindow : MonoBehaviour, IAdvMessageWindow
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <IsCurrent>k__BackingField;
        private Color defaultNameTextColor = Color.get_white();
        private Color defaultTextColor = Color.get_white();
        [SerializeField]
        protected AdvEngine engine;
        [SerializeField]
        private GameObject iconBrPage;
        [SerializeField]
        private GameObject iconWaitInput;
        [SerializeField]
        private bool isLinkPositionIconBrPage = true;
        [SerializeField]
        private UnityEngine.UI.Text nameText;
        [SerializeField]
        private Color readColor = new Color(0.8f, 0.8f, 0.8f);
        [SerializeField]
        private ReadColorMode readColorMode;
        [SerializeField]
        private GameObject rootChildren;
        [SerializeField]
        private UguiNovelText text;
        [SerializeField, FormerlySerializedAs("transrateMessageWindowRoot")]
        private CanvasGroup translateMessageWindowRoot;

        private void Clear()
        {
            this.text.set_text(string.Empty);
            this.text.LengthOfView = 0;
            if (this.nameText != null)
            {
                this.nameText.set_text(string.Empty);
            }
            if (this.iconWaitInput != null)
            {
                this.iconWaitInput.SetActive(false);
            }
            if (this.iconBrPage != null)
            {
                this.iconBrPage.SetActive(false);
            }
            this.rootChildren.SetActive(false);
        }

        GameObject IAdvMessageWindow.get_gameObject()
        {
            return base.get_gameObject();
        }

        private void LateUpdate()
        {
            if (this.Engine.UiManager.Status == AdvUiManager.UiStatus.Default)
            {
                this.rootChildren.SetActive(this.Engine.UiManager.IsShowingMessageWindow);
                if (this.Engine.UiManager.IsShowingMessageWindow)
                {
                    this.translateMessageWindowRoot.set_alpha(this.Engine.Config.MessageWindowAlpha);
                }
            }
            this.UpdateCurrent();
        }

        private void LinkIcon()
        {
            if (this.iconWaitInput == null)
            {
                this.LinkIconSub(this.iconBrPage, this.Engine.Page.IsWaitInputInPage || this.Engine.Page.IsWaitBrPage);
            }
            else
            {
                this.LinkIconSub(this.iconWaitInput, this.Engine.Page.IsWaitInputInPage);
                this.LinkIconSub(this.iconBrPage, this.Engine.Page.IsWaitBrPage);
            }
        }

        private void LinkIconSub(GameObject icon, bool isActive)
        {
            if (icon != null)
            {
                if (!this.Engine.UiManager.IsShowingMessageWindow)
                {
                    icon.SetActive(false);
                }
                else
                {
                    icon.SetActive(isActive);
                    if (this.isLinkPositionIconBrPage)
                    {
                        icon.get_transform().set_localPosition(this.text.CurrentEndPosition);
                    }
                }
            }
        }

        public void OnChangeActive(bool isActive)
        {
            base.get_gameObject().SetActive(isActive);
            if (!isActive)
            {
                this.Clear();
            }
            else
            {
                this.rootChildren.SetActive(true);
            }
        }

        public void OnChangeCurrent(bool isCurrent)
        {
            this.IsCurrent = isCurrent;
        }

        public void OnInit(AdvMessageWindowManager windowManager)
        {
            this.defaultTextColor = this.text.get_color();
            if (this.nameText != null)
            {
                this.defaultNameTextColor = this.nameText.get_color();
            }
            this.Clear();
        }

        public void OnReset()
        {
            this.Clear();
        }

        public void OnTapBackLog()
        {
            this.Engine.UiManager.Status = AdvUiManager.UiStatus.Backlog;
        }

        public void OnTapCloseWindow()
        {
            this.Engine.UiManager.Status = AdvUiManager.UiStatus.HideMessageWindow;
        }

        public void OnTextChanged(AdvMessageWindow window)
        {
            if (this.text != null)
            {
                this.text.set_text(string.Empty);
                this.text.set_text(window.Text.OriginalText);
                this.text.LengthOfView = window.TextLength;
            }
            if (this.nameText != null)
            {
                this.nameText.set_text(string.Empty);
                this.nameText.set_text(window.NameText);
            }
            switch (this.readColorMode)
            {
                case ReadColorMode.Change:
                    this.text.set_color(!this.Engine.Page.CheckReadPage() ? this.defaultTextColor : this.readColor);
                    if (this.nameText != null)
                    {
                        this.nameText.set_color(!this.Engine.Page.CheckReadPage() ? this.defaultNameTextColor : this.readColor);
                    }
                    break;
            }
            this.LinkIcon();
        }

        private void UpdateCurrent()
        {
            if (this.IsCurrent && (this.Engine.UiManager.Status == AdvUiManager.UiStatus.Default))
            {
                if (this.Engine.UiManager.IsShowingMessageWindow)
                {
                    this.text.LengthOfView = this.Engine.Page.CurrentTextLength;
                }
                this.LinkIcon();
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

        public bool IsCurrent { get; protected set; }

        public UguiNovelText Text
        {
            get
            {
                return this.text;
            }
        }

        private enum ReadColorMode
        {
            None,
            Change
        }
    }
}

