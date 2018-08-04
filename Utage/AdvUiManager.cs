namespace Utage
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UtageExtensions;

    public abstract class AdvUiManager : MonoBehaviour, IAdvSaveData, IBinaryIO
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private PointerEventData <CurrentPointerData>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <IsHideMenuButton>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <IsInputTrig>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <IsInputTrigCustom>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <IsShowingMessageWindow>k__BackingField;
        [SerializeField]
        protected AdvEngine engine;
        private AdvGuiManager guiManager;
        protected UiStatus status;
        private const int Version = 0;

        protected AdvUiManager()
        {
        }

        protected abstract void ChangeStatus(UiStatus newStatus);
        public void ClearPointerDown()
        {
            this.CurrentPointerData = null;
            this.IsInputTrig = false;
        }

        public abstract void Close();
        internal void HideMenuButton()
        {
            this.IsHideMenuButton = true;
        }

        public void HideMessageWindow()
        {
            this.IsShowingMessageWindow = false;
        }

        protected virtual void LateUpdate()
        {
            this.ClearPointerDown();
            this.IsInputTrigCustom = false;
        }

        public void OnBeginPage()
        {
            this.IsShowingMessageWindow = false;
        }

        public virtual void OnClear()
        {
            this.IsHideMenuButton = false;
            this.IsShowingMessageWindow = false;
        }

        public void OnEndPage()
        {
            this.IsShowingMessageWindow = false;
        }

        public virtual void OnPointerDown(PointerEventData data)
        {
            this.CurrentPointerData = data;
            this.IsInputTrig = true;
        }

        public virtual void OnRead(BinaryReader reader)
        {
            int num = reader.ReadInt32();
            if (num == 0)
            {
                this.IsHideMenuButton = reader.ReadBoolean();
                this.IsShowingMessageWindow = reader.ReadBoolean();
            }
            else
            {
                object[] args = new object[] { num };
                Debug.LogError(LanguageErrorMsg.LocalizeTextFormat(ErrorMsg.UnknownVersion, args));
            }
        }

        public virtual void OnWrite(BinaryWriter writer)
        {
            writer.Write(0);
            writer.Write(this.IsHideMenuButton);
            writer.Write(this.IsShowingMessageWindow);
        }

        public abstract void Open();
        internal void ShowMenuButton()
        {
            this.IsHideMenuButton = false;
        }

        public void ShowMessageWindow()
        {
            this.IsShowingMessageWindow = true;
        }

        public PointerEventData CurrentPointerData { get; private set; }

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

        [SerializeField]
        public AdvGuiManager GuiManager
        {
            get
            {
                return base.get_gameObject().GetComponentCacheCreateIfMissing<AdvGuiManager>(ref this.guiManager);
            }
        }

        [Obsolete]
        public bool IsHide
        {
            get
            {
                return !this.IsShowingMessageWindow;
            }
        }

        public bool IsHideMenuButton { get; private set; }

        public bool IsInputTrig { get; set; }

        public bool IsInputTrigCustom { get; set; }

        public bool IsPointerDowned
        {
            get
            {
                return (this.CurrentPointerData != null);
            }
        }

        public bool IsShowingMenuButton
        {
            get
            {
                return (!this.IsHideMenuButton && (this.IsShowingMessageWindow || this.Engine.SelectionManager.IsShowing));
            }
        }

        public bool IsShowingMessageWindow { get; private set; }

        [Obsolete]
        public bool IsShowingUI
        {
            get
            {
                return (this.IsShowingMessageWindow || this.Engine.SelectionManager.IsShowing);
            }
        }

        public string SaveKey
        {
            get
            {
                return "UiManager";
            }
        }

        public UiStatus Status
        {
            get
            {
                return this.status;
            }
            set
            {
                if (this.status != value)
                {
                    this.ChangeStatus(value);
                }
            }
        }

        public enum UiStatus
        {
            Default,
            Backlog,
            HideMessageWindow,
            Menu
        }
    }
}

