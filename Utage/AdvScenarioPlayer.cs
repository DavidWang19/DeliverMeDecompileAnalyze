namespace Utage
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UtageExtensions;

    [AddComponentMenu("Utage/ADV/Internal/ScenarioPlayer")]
    public class AdvScenarioPlayer : MonoBehaviour, IBinaryIO
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <CurrentGallerySceneLabel>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <IsEndScenario>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <IsPausing>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <IsReservedEndScenario>k__BackingField;
        [SerializeField, EnumFlags]
        private DebugOutPut debugOutPut;
        private AdvEngine engine;
        private AdvScenarioThread mainThread;
        [SerializeField]
        private int maxFilePreload = 20;
        [SerializeField]
        public AdvCommandEvent onBeginCommand = new AdvCommandEvent();
        [SerializeField]
        public AdvCommandEvent onEndCommand = new AdvCommandEvent();
        [SerializeField]
        public AdvScenarioPlayerEvent onEndOrPauseScenario = new AdvScenarioPlayerEvent();
        [SerializeField]
        public AdvScenarioPlayerEvent onEndScenario = new AdvScenarioPlayerEvent();
        [SerializeField]
        public AdvScenarioPlayerEvent onPauseScenario = new AdvScenarioPlayerEvent();
        [SerializeField]
        public AdvCommandEvent onUpdatePreWaitingCommand = new AdvCommandEvent();
        [SerializeField]
        public AdvCommandEvent onUpdateWaitingCommand = new AdvCommandEvent();
        [SerializeField]
        private int preloadDeep = 5;
        [SerializeField]
        private GameObject sendMessageTarget;
        private const int Version = 0;

        public void Clear()
        {
            this.MainThread.Clear();
            this.CurrentGallerySceneLabel = string.Empty;
        }

        [DebuggerHidden]
        internal IEnumerator CoStartSaveData(AdvSaveData saveData)
        {
            return new <CoStartSaveData>c__Iterator0 { saveData = saveData, $this = this };
        }

        public void EndScenario()
        {
            this.OnEndScenario.Invoke(this);
            this.OnEndOrPauseScenario.Invoke(this);
            this.Engine.ClearOnEnd();
            this.MainThread.Clear();
            this.IsEndScenario = true;
        }

        public void EndSceneGallery(AdvEngine engine)
        {
            if (string.IsNullOrEmpty(this.CurrentGallerySceneLabel))
            {
                Debug.LogError(LanguageAdvErrorMsg.LocalizeTextFormat(AdvErrorMsg.EndSceneGallery, new object[0]));
            }
            else
            {
                engine.SystemSaveData.GalleryData.AddSceneLabel(this.CurrentGallerySceneLabel);
                this.CurrentGallerySceneLabel = string.Empty;
            }
        }

        public void OnRead(BinaryReader reader)
        {
            if (reader.ReadInt32() <= 0)
            {
                this.MainThread.JumpManager.Read(this.Engine, reader);
                string label = reader.ReadString();
                int page = reader.ReadInt32();
                string str2 = reader.ReadString();
                bool skipPageHeaer = reader.ReadBoolean();
                this.MainThread.ScenarioPlayer.CurrentGallerySceneLabel = str2;
                this.MainThread.StartScenario(label, page, skipPageHeaer);
            }
        }

        public void OnWrite(BinaryWriter writer)
        {
            writer.Write(0);
            this.MainThread.JumpManager.Write(writer);
            writer.Write(this.Engine.Page.ScenarioLabel);
            writer.Write(this.Engine.Page.PageNo);
            writer.Write(this.CurrentGallerySceneLabel);
            writer.Write(this.MainThread.SkipPageHeaerOnSave);
        }

        public void Pause()
        {
            this.IsPausing = true;
            this.OnPauseScenario.Invoke(this);
            this.OnEndOrPauseScenario.Invoke(this);
        }

        public void Resume()
        {
            this.IsPausing = false;
        }

        public void StartScenario(string label, int page)
        {
            this.IsPausing = false;
            this.IsEndScenario = false;
            this.IsReservedEndScenario = false;
            this.CurrentGallerySceneLabel = string.Empty;
            this.MainThread.Clear();
            this.MainThread.StartScenario(label, page, false);
        }

        internal void UpdateSceneGallery(string label, AdvEngine engine)
        {
            if (engine.DataManager.SettingDataManager.SceneGallerySetting.Contains(label) && (this.CurrentGallerySceneLabel != label))
            {
                if (!string.IsNullOrEmpty(this.CurrentGallerySceneLabel))
                {
                    object[] args = new object[] { this.CurrentGallerySceneLabel, label };
                    Debug.LogError(LanguageAdvErrorMsg.LocalizeTextFormat(AdvErrorMsg.UpdateSceneLabel, args));
                }
                this.CurrentGallerySceneLabel = label;
            }
        }

        public string CurrentGallerySceneLabel { get; set; }

        internal bool DebugOutputCommandEnd
        {
            get
            {
                return ((this.debugOutPut & DebugOutPut.CommandEnd) == DebugOutPut.CommandEnd);
            }
        }

        internal bool DebugOutputLog
        {
            get
            {
                return ((this.debugOutPut & DebugOutPut.Log) == DebugOutPut.Log);
            }
        }

        internal bool DebugOutputWaiting
        {
            get
            {
                return ((this.debugOutPut & DebugOutPut.Waiting) == DebugOutPut.Waiting);
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

        public bool IsEndScenario { get; set; }

        public bool IsLoading
        {
            get
            {
                return this.MainThread.IsLoadingDeep;
            }
        }

        public bool IsPausing { get; set; }

        public bool IsReservedEndScenario { get; set; }

        public AdvScenarioThread MainThread
        {
            get
            {
                if (this.mainThread == null)
                {
                    this.mainThread = base.get_gameObject().GetComponentCreateIfMissing<AdvScenarioThread>();
                    this.mainThread.Init(this, "MainThread", null);
                }
                return this.mainThread;
            }
        }

        internal int MaxFilePreload
        {
            get
            {
                return this.maxFilePreload;
            }
        }

        public AdvCommandEvent OnBeginCommand
        {
            get
            {
                return this.onBeginCommand;
            }
        }

        public AdvCommandEvent OnEndCommand
        {
            get
            {
                return this.onEndCommand;
            }
        }

        public AdvScenarioPlayerEvent OnEndOrPauseScenario
        {
            get
            {
                return this.onEndOrPauseScenario;
            }
        }

        public AdvScenarioPlayerEvent OnEndScenario
        {
            get
            {
                return this.onEndScenario;
            }
        }

        public AdvScenarioPlayerEvent OnPauseScenario
        {
            get
            {
                return this.onPauseScenario;
            }
        }

        public AdvCommandEvent OnUpdatePreWaitingCommand
        {
            get
            {
                return this.onUpdatePreWaitingCommand;
            }
        }

        public AdvCommandEvent OnUpdateWaitingCommand
        {
            get
            {
                return this.onUpdateWaitingCommand;
            }
        }

        internal int PreloadDeep
        {
            get
            {
                return this.preloadDeep;
            }
        }

        public string SaveKey
        {
            get
            {
                return "ScenarioPlayer";
            }
        }

        public GameObject SendMessageTarget
        {
            get
            {
                return this.sendMessageTarget;
            }
        }

        [CompilerGenerated]
        private sealed class <CoStartSaveData>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal bool $disposing;
            internal int $PC;
            internal AdvScenarioPlayer $this;
            internal AdvSaveData saveData;

            [DebuggerHidden]
            public void Dispose()
            {
                this.$disposing = true;
                this.$PC = -1;
            }

            public bool MoveNext()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                switch (num)
                {
                    case 0:
                        this.$this.IsPausing = false;
                        this.$this.IsEndScenario = false;
                        this.$this.IsReservedEndScenario = false;
                        this.$this.MainThread.Clear();
                        this.saveData.LoadGameData(this.$this.Engine, this.$this.Engine.SaveManager.CustomSaveDataIOList, this.$this.Engine.SaveManager.GetSaveIoListCreateIfMissing(this.$this.Engine));
                        this.$current = null;
                        if (!this.$disposing)
                        {
                            this.$PC = 1;
                        }
                        return true;

                    case 1:
                        this.saveData.Buffer.Overrirde((IBinaryIO) this.$this);
                        this.$PC = -1;
                        break;
                }
                return false;
            }

            [DebuggerHidden]
            public void Reset()
            {
                throw new NotSupportedException();
            }

            object IEnumerator<object>.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.$current;
                }
            }

            object IEnumerator.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.$current;
                }
            }
        }

        [Flags]
        private enum DebugOutPut
        {
            CommandEnd = 4,
            Log = 1,
            Waiting = 2
        }
    }
}

