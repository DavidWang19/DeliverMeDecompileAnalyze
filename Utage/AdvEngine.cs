namespace Utage
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.Serialization;
    using UtageExtensions;

    [AddComponentMenu("Utage/ADV/MainEngine"), RequireComponent(typeof(DontDestoryOnLoad)), RequireComponent(typeof(AdvDataManager)), RequireComponent(typeof(AdvScenarioPlayer)), RequireComponent(typeof(AdvPage)), RequireComponent(typeof(AdvMessageWindowManager)), RequireComponent(typeof(AdvSelectionManager)), RequireComponent(typeof(AdvBacklogManager)), RequireComponent(typeof(AdvConfig)), RequireComponent(typeof(AdvSystemSaveData)), RequireComponent(typeof(AdvSaveManager))]
    public class AdvEngine : MonoBehaviour
    {
        [CompilerGenerated]
        private static Action<IBinaryIO> <>f__am$cache0;
        [CompilerGenerated]
        private static Action<IBinaryIO> <>f__am$cache1;
        private AdvBacklogManager backlogManager;
        [SerializeField]
        private bool bootAsync;
        [SerializeField]
        private Utage.CameraManager cameraManager;
        private AdvConfig config;
        private List<AdvCustomCommandManager> customCommandManagerList;
        private AdvDataManager dataManager;
        [SerializeField]
        private AdvEffectManager effectManager;
        [SerializeField]
        private AdvGraphicManager graphicManager;
        private bool isSceneGallery;
        private bool isStarted;
        [SerializeField]
        private bool isStopSoundOnEnd = true;
        [SerializeField]
        private bool isStopSoundOnStart = true;
        private bool isWaitBootLoading = true;
        private AdvMessageWindowManager messageWindowManager;
        [SerializeField]
        public AdvEvent onChangeLanguage = new AdvEvent();
        public AdvEvent OnClear;
        [SerializeField]
        private OpenDialogEvent onOpenDialog;
        [SerializeField]
        private AdvEvent onPageTextChange = new AdvEvent();
        public UnityEvent onPreInit;
        private AdvPage page;
        private AdvParamManager param = new AdvParamManager();
        private AdvSaveManager saveManager;
        private AdvScenarioPlayer scenarioPlayer;
        private AdvSelectionManager selectionManager;
        [SerializeField, FormerlySerializedAs("soundManger")]
        private Utage.SoundManager soundManager;
        private string startScenarioLabel = "Start";
        private AdvSystemSaveData systemSaveData;
        [SerializeField]
        private AdvUiManager uiManager;

        public void BootFromExportData(AdvImportScenarios scenarios, string resourceDir)
        {
            base.get_gameObject().SetActive(true);
            base.StopAllCoroutines();
            base.StartCoroutine(this.CoBootFromExportData(scenarios, resourceDir));
        }

        private void BootInit(string rootDirResource)
        {
            this.BootInitCustomCommand();
            this.DataManager.BootInit(rootDirResource);
            this.GraphicManager.BootInit(this, this.DataManager.SettingDataManager.LayerSetting);
            this.Param.InitDefaultAll(this.DataManager.SettingDataManager.DefaultParam);
            AdvGraphicInfo.CallbackExpression = new Func<string, bool>(this.Param.CalcExpressionBoolean);
            TextParser.CallbackCalcExpression = (Func<string, object>) Delegate.Combine(TextParser.CallbackCalcExpression, new Func<string, object>(this.Param.CalcExpressionNotSetParam));
            iTweenData.CallbackGetValue = (Func<string, object>) Delegate.Combine(iTweenData.CallbackGetValue, new Func<string, object>(this.Param.GetParameter));
            LanguageManagerBase.Instance.OnChangeLanugage = new Action(this.ChangeLanguage);
            this.SystemSaveData.Init(this);
            this.SaveManager.Init();
            this.DataManager.BootInitScenario(this.bootAsync);
        }

        public void BootInitCustomCommand()
        {
            AdvCommandParser.OnCreateCustomCommandFromID = null;
            foreach (AdvCustomCommandManager manager in this.CustomCommandManagerList)
            {
                manager.OnBootInit();
            }
        }

        private void ChangeLanguage()
        {
            this.Page.OnChangeLanguage();
            this.OnChangeLanguage.Invoke(this);
        }

        public void ClearCustomCommand()
        {
            foreach (AdvCustomCommandManager manager in this.CustomCommandManagerList)
            {
                manager.OnClear();
            }
        }

        public void ClearOnEnd()
        {
            this.ClearSub(this.isStopSoundOnEnd);
        }

        private void ClearOnLaod()
        {
            this.ClearSub(true);
        }

        public void ClearOnStart()
        {
            this.ClearSub(this.isStopSoundOnStart);
        }

        private void ClearSub(bool isStopSound)
        {
            this.Page.Clear();
            this.SelectionManager.Clear();
            this.BacklogManager.Clear();
            this.GraphicManager.Clear();
            this.GraphicManager.get_gameObject().SetActive(true);
            if (this.UiManager != null)
            {
                this.UiManager.Close();
            }
            this.ClearCustomCommand();
            this.ScenarioPlayer.Clear();
            if (isStopSound && (this.SoundManager != null))
            {
                this.SoundManager.StopBgm();
                this.SoundManager.StopAmbience();
            }
            if (this.MessageWindowManager == null)
            {
                Debug.LogError("MessageWindowManager is Missing");
            }
            this.CameraManager.OnClear();
            if (<>f__am$cache0 == null)
            {
                <>f__am$cache0 = x => ((IAdvSaveData) x).OnClear();
            }
            this.SaveManager.GetSaveIoListCreateIfMissing(this).ForEach(<>f__am$cache0);
            if (<>f__am$cache1 == null)
            {
                <>f__am$cache1 = x => ((IAdvSaveData) x).OnClear();
            }
            this.SaveManager.CustomSaveDataIOList.ForEach(<>f__am$cache1);
            this.OnClear.Invoke(this);
        }

        [DebuggerHidden]
        private IEnumerator CoBootFromExportData(AdvImportScenarios scenarios, string resourceDir)
        {
            return new <CoBootFromExportData>c__Iterator0 { scenarios = scenarios, resourceDir = resourceDir, $this = this };
        }

        [DebuggerHidden]
        private IEnumerator CoStartGameSub(string scenarioLabel)
        {
            return new <CoStartGameSub>c__Iterator2 { scenarioLabel = scenarioLabel, $this = this };
        }

        [DebuggerHidden]
        private IEnumerator CoStartSaveData(AdvSaveData saveData)
        {
            return new <CoStartSaveData>c__Iterator4 { saveData = saveData, $this = this };
        }

        [DebuggerHidden]
        private IEnumerator CoStartScenario(string label, int page)
        {
            return new <CoStartScenario>c__Iterator3 { label = label, page = page, $this = this };
        }

        public void EndScenario()
        {
            this.ScenarioPlayer.EndScenario();
        }

        public bool ExitsChapter(string url)
        {
            <ExitsChapter>c__AnonStorey5 storey = new <ExitsChapter>c__AnonStorey5 {
                chapterAssetName = FilePathUtil.GetFileNameWithoutExtension(url)
            };
            return this.DataManager.SettingDataManager.ImportedScenarios.Chapters.Exists(new Predicate<AdvChapterData>(storey.<>m__0));
        }

        public void JumpScenario(string label)
        {
            if (this.ScenarioPlayer.MainThread.IsPlaying)
            {
                if (this.ScenarioPlayer.IsPausing)
                {
                    this.ScenarioPlayer.Resume();
                }
                this.ScenarioPlayer.MainThread.JumpManager.RegistoreLabel(label);
            }
            else
            {
                this.StartScenario(label, 0);
            }
        }

        [DebuggerHidden]
        public IEnumerator LoadChapterAsync(string url)
        {
            return new <LoadChapterAsync>c__Iterator1 { url = url, $this = this };
        }

        private void LoadSaveData(AdvSaveData saveData)
        {
            this.ClearOnLaod();
            base.StartCoroutine(this.CoStartSaveData(saveData));
        }

        private void OnClicked()
        {
        }

        public void OpenLoadGame(AdvSaveData saveData)
        {
            this.isSceneGallery = false;
            this.LoadSaveData(saveData);
        }

        public bool QuickLoad()
        {
            if (this.SaveManager.ReadQuickSaveData())
            {
                this.LoadSaveData(this.SaveManager.QuickSaveData);
                return true;
            }
            return false;
        }

        public void QuickSave()
        {
            this.WriteSaveData(this.SaveManager.QuickSaveData);
        }

        public bool ResumeScenario()
        {
            if (!this.ScenarioPlayer.IsPausing)
            {
                return false;
            }
            this.ScenarioPlayer.Resume();
            return true;
        }

        public void StartGame()
        {
            this.StartGame(this.StartScenarioLabel);
        }

        public void StartGame(string scenarioLabel)
        {
            this.isSceneGallery = false;
            this.StartGameSub(scenarioLabel);
        }

        private void StartGameSub(string scenarioLabel)
        {
            base.StartCoroutine(this.CoStartGameSub(scenarioLabel));
        }

        private void StartScenario(string label, int page)
        {
            base.StartCoroutine(this.CoStartScenario(label, page));
        }

        public void StartSceneGallery(string label)
        {
            this.isSceneGallery = true;
            this.StartGameSub(label);
        }

        public void WriteSaveData(AdvSaveData saveData)
        {
            this.SaveManager.WriteSaveData(this, saveData);
        }

        public void WriteSystemData()
        {
            this.systemSaveData.Write();
        }

        public AdvBacklogManager BacklogManager
        {
            get
            {
                if (this.backlogManager == null)
                {
                }
                return (this.backlogManager = base.GetComponent<AdvBacklogManager>());
            }
        }

        public Utage.CameraManager CameraManager
        {
            get
            {
                if (this.cameraManager == null)
                {
                }
                return (this.cameraManager = Object.FindObjectOfType<Utage.CameraManager>());
            }
        }

        public AdvConfig Config
        {
            get
            {
                if (this.config == null)
                {
                }
                return (this.config = base.GetComponent<AdvConfig>());
            }
        }

        public List<AdvCustomCommandManager> CustomCommandManagerList
        {
            get
            {
                if (this.customCommandManagerList == null)
                {
                    this.customCommandManagerList = new List<AdvCustomCommandManager>();
                    base.GetComponentsInChildren<AdvCustomCommandManager>(true, this.customCommandManagerList);
                }
                return this.customCommandManagerList;
            }
        }

        public AdvDataManager DataManager
        {
            get
            {
                if (this.dataManager == null)
                {
                }
                return (this.dataManager = base.GetComponent<AdvDataManager>());
            }
        }

        public AdvEffectManager EffectManager
        {
            get
            {
                if (this.effectManager == null)
                {
                }
                return (this.effectManager = base.get_transform().GetCompoentInChildrenCreateIfMissing<AdvEffectManager>());
            }
        }

        public AdvGraphicManager GraphicManager
        {
            get
            {
                if (this.graphicManager == null)
                {
                    this.graphicManager = base.get_transform().GetCompoentInChildrenCreateIfMissing<AdvGraphicManager>();
                    this.graphicManager.get_transform().set_localPosition(new Vector3(0f, 0f, 20f));
                }
                return this.graphicManager;
            }
        }

        public bool IsEndOrPauseScenario
        {
            get
            {
                return (this.IsEndScenario || this.IsPausingScenario);
            }
        }

        public bool IsEndScenario
        {
            get
            {
                if (this.ScenarioPlayer == null)
                {
                    return false;
                }
                if (this.IsLoading)
                {
                    return false;
                }
                return this.ScenarioPlayer.IsEndScenario;
            }
        }

        public bool IsLoading
        {
            get
            {
                return (this.IsWaitBootLoading || (this.GraphicManager.IsLoading || this.ScenarioPlayer.IsLoading));
            }
        }

        public bool IsPausingScenario
        {
            get
            {
                return this.ScenarioPlayer.IsPausing;
            }
        }

        public bool IsSceneGallery
        {
            get
            {
                return this.isSceneGallery;
            }
        }

        public bool IsStarted
        {
            get
            {
                return this.isStarted;
            }
        }

        public bool IsWaitBootLoading
        {
            get
            {
                return this.isWaitBootLoading;
            }
        }

        public AdvMessageWindowManager MessageWindowManager
        {
            get
            {
                if (this.messageWindowManager == null)
                {
                }
                return (this.messageWindowManager = base.get_gameObject().GetComponentCreateIfMissing<AdvMessageWindowManager>());
            }
        }

        public AdvEvent OnChangeLanguage
        {
            get
            {
                return this.onChangeLanguage;
            }
        }

        public OpenDialogEvent OnOpenDialog
        {
            get
            {
                if ((this.onOpenDialog.GetPersistentEventCount() == 0) && (SystemUi.GetInstance() != null))
                {
                    this.onOpenDialog.AddListener(new UnityAction<string, List<ButtonEventInfo>>(SystemUi.GetInstance(), (IntPtr) this.OpenDialog));
                }
                return this.onOpenDialog;
            }
            set
            {
                this.onOpenDialog = value;
            }
        }

        public AdvEvent OnPageTextChange
        {
            get
            {
                return this.onPageTextChange;
            }
        }

        public AdvPage Page
        {
            get
            {
                if (this.page == null)
                {
                }
                return (this.page = base.GetComponent<AdvPage>());
            }
        }

        public AdvParamManager Param
        {
            get
            {
                return this.param;
            }
        }

        public AdvSaveManager SaveManager
        {
            get
            {
                if (this.saveManager == null)
                {
                }
                return (this.saveManager = base.GetComponent<AdvSaveManager>());
            }
        }

        public AdvScenarioPlayer ScenarioPlayer
        {
            get
            {
                if (this.scenarioPlayer == null)
                {
                }
                return (this.scenarioPlayer = base.GetComponent<AdvScenarioPlayer>());
            }
        }

        public AdvSelectionManager SelectionManager
        {
            get
            {
                if (this.selectionManager == null)
                {
                }
                return (this.selectionManager = base.GetComponent<AdvSelectionManager>());
            }
        }

        public Utage.SoundManager SoundManager
        {
            get
            {
                if (this.soundManager == null)
                {
                }
                return (this.soundManager = Object.FindObjectOfType<Utage.SoundManager>());
            }
        }

        public string StartScenarioLabel
        {
            get
            {
                return this.startScenarioLabel;
            }
            set
            {
                this.startScenarioLabel = value;
            }
        }

        public AdvSystemSaveData SystemSaveData
        {
            get
            {
                if (this.systemSaveData == null)
                {
                }
                return (this.systemSaveData = base.GetComponent<AdvSystemSaveData>());
            }
        }

        public AdvUiManager UiManager
        {
            get
            {
                if (this.uiManager == null)
                {
                }
                return (this.uiManager = Object.FindObjectOfType<AdvUiManager>());
            }
        }

        [CompilerGenerated]
        private sealed class <CoBootFromExportData>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal bool $disposing;
            internal int $PC;
            internal AdvEngine $this;
            internal string resourceDir;
            internal AdvImportScenarios scenarios;

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
                        this.$this.ClearSub(false);
                        this.$this.isStarted = true;
                        this.$this.isWaitBootLoading = true;
                        this.$this.onPreInit.Invoke();
                        break;

                    case 1:
                        break;

                    case 2:
                        this.$this.DataManager.SettingDataManager.ImportedScenarios = this.scenarios;
                        this.$this.BootInit(this.resourceDir);
                        this.$this.isWaitBootLoading = false;
                        this.$PC = -1;
                        goto Label_00DD;

                    default:
                        goto Label_00DD;
                }
                if (!AssetFileManager.IsInitialized())
                {
                    this.$current = null;
                    if (!this.$disposing)
                    {
                        this.$PC = 1;
                    }
                }
                else
                {
                    this.$current = null;
                    if (!this.$disposing)
                    {
                        this.$PC = 2;
                    }
                }
                return true;
            Label_00DD:
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

        [CompilerGenerated]
        private sealed class <CoStartGameSub>c__Iterator2 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal bool $disposing;
            internal int $PC;
            internal AdvEngine $this;
            internal string scenarioLabel;

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
                    case 1:
                        if (this.$this.IsWaitBootLoading)
                        {
                            this.$current = null;
                            if (!this.$disposing)
                            {
                                this.$PC = 1;
                            }
                            return true;
                        }
                        this.$this.Param.InitDefaultNormal(this.$this.DataManager.SettingDataManager.DefaultParam);
                        this.$this.ClearOnStart();
                        this.$this.StartScenario(this.scenarioLabel, 0);
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

        [CompilerGenerated]
        private sealed class <CoStartSaveData>c__Iterator4 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal bool $disposing;
            internal int $PC;
            internal AdvEngine $this;
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
                    case 1:
                        if (this.$this.IsWaitBootLoading)
                        {
                            this.$current = null;
                            if (!this.$disposing)
                            {
                                this.$PC = 1;
                            }
                            goto Label_0126;
                        }
                        break;

                    case 2:
                        break;

                    case 3:
                        goto Label_00B2;

                    case 4:
                        this.$PC = -1;
                        goto Label_0124;

                    default:
                        goto Label_0124;
                }
                while (this.$this.GraphicManager.IsLoading)
                {
                    this.$current = null;
                    if (!this.$disposing)
                    {
                        this.$PC = 2;
                    }
                    goto Label_0126;
                }
            Label_00B2:
                while (this.$this.SoundManager.IsLoading)
                {
                    this.$current = null;
                    if (!this.$disposing)
                    {
                        this.$PC = 3;
                    }
                    goto Label_0126;
                }
                if (this.$this.UiManager != null)
                {
                    this.$this.UiManager.Open();
                }
                this.$current = this.$this.ScenarioPlayer.CoStartSaveData(this.saveData);
                if (!this.$disposing)
                {
                    this.$PC = 4;
                }
                goto Label_0126;
            Label_0124:
                return false;
            Label_0126:
                return true;
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

        [CompilerGenerated]
        private sealed class <CoStartScenario>c__Iterator3 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal bool $disposing;
            internal int $PC;
            internal AdvEngine $this;
            internal string label;
            internal int page;

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
                    case 1:
                        if (this.$this.IsWaitBootLoading)
                        {
                            this.$current = null;
                            if (!this.$disposing)
                            {
                                this.$PC = 1;
                            }
                            goto Label_0144;
                        }
                        break;

                    case 2:
                        break;

                    case 3:
                        goto Label_00AE;

                    default:
                        goto Label_0142;
                }
                while (this.$this.GraphicManager.IsLoading)
                {
                    this.$current = null;
                    if (!this.$disposing)
                    {
                        this.$PC = 2;
                    }
                    goto Label_0144;
                }
            Label_00AE:
                while (this.$this.SoundManager.IsLoading)
                {
                    this.$current = null;
                    if (!this.$disposing)
                    {
                        this.$PC = 3;
                    }
                    goto Label_0144;
                }
                if (this.$this.UiManager != null)
                {
                    this.$this.UiManager.Open();
                }
                if ((this.label.Length > 1) && (this.label[0] == '*'))
                {
                    this.label = this.label.Substring(1);
                }
                this.$this.ScenarioPlayer.StartScenario(this.label, this.page);
                this.$PC = -1;
            Label_0142:
                return false;
            Label_0144:
                return true;
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

        [CompilerGenerated]
        private sealed class <ExitsChapter>c__AnonStorey5
        {
            internal string chapterAssetName;

            internal bool <>m__0(AdvChapterData x)
            {
                return (x.get_name() == this.chapterAssetName);
            }
        }

        [CompilerGenerated]
        private sealed class <LoadChapterAsync>c__Iterator1 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal bool $disposing;
            internal int $PC;
            internal AdvEngine $this;
            internal AdvChapterData <chapter>__0;
            internal AssetFile <file>__0;
            internal string url;

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
                        this.<file>__0 = AssetFileManager.Load(this.url, this.$this);
                        break;

                    case 1:
                        break;

                    default:
                        goto Label_0125;
                }
                if (!this.<file>__0.IsLoadEnd)
                {
                    this.$current = null;
                    if (!this.$disposing)
                    {
                        this.$PC = 1;
                    }
                    return true;
                }
                this.<chapter>__0 = this.<file>__0.UnityObject as AdvChapterData;
                if (this.<chapter>__0 == null)
                {
                    Debug.LogError(this.url + " is  not scenario file");
                }
                else
                {
                    if (this.$this.DataManager.SettingDataManager.ImportedScenarios == null)
                    {
                        this.$this.DataManager.SettingDataManager.ImportedScenarios = new AdvImportScenarios();
                    }
                    if (this.$this.DataManager.SettingDataManager.ImportedScenarios.TryAddChapter(this.<chapter>__0))
                    {
                        this.$this.DataManager.BootInitChapter(this.<chapter>__0);
                    }
                    this.$PC = -1;
                }
            Label_0125:
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
    }
}

