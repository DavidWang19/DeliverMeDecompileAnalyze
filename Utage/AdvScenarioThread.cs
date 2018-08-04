namespace Utage
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    [AddComponentMenu("Utage/ADV/Internal/ScenarioThread")]
    public class AdvScenarioThread : MonoBehaviour
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <IsLoading>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <IsMainThread>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <IsPlaying>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private AdvScenarioThread <ParenetThread>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private AdvScenarioPlayer <ScenarioPlayer>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <SkipPageHeaerOnSave>k__BackingField;
        private AdvCommand currentCommand;
        private AdvIfManager ifManager = new AdvIfManager();
        private AdvJumpManager jumpManager = new AdvJumpManager();
        private HashSet<AssetFile> preloadFileSet = new HashSet<AssetFile>();
        private List<AdvScenarioThread> subThreadList = new List<AdvScenarioThread>();
        [SerializeField, NotEditable]
        private string threadName;
        private AdvWaitManager waitManager = new AdvWaitManager();

        internal void Cancel()
        {
            this.Clear();
            Object.Destroy(this);
        }

        internal void CancelSubThread(string name)
        {
            foreach (AdvScenarioThread thread in this.SubThreadList)
            {
                if ((thread != null) && (thread.ThreadName == name))
                {
                    thread.Cancel();
                }
            }
        }

        private void CheckSystemDataWriteIfChanged()
        {
            if (this.Engine.Param.HasChangedSystemParam)
            {
                this.Engine.Param.HasChangedSystemParam = false;
                this.Engine.SystemSaveData.Write();
            }
        }

        internal void Clear()
        {
            this.IsPlaying = false;
            this.CleaSubTreadList();
            this.ResetOnJump();
            this.WaitManager.Clear();
            this.jumpManager.Clear();
            base.StopAllCoroutines();
        }

        private void ClearPreload()
        {
            foreach (AssetFile file in this.preloadFileSet)
            {
                file.Unuse(this);
            }
            this.preloadFileSet.Clear();
        }

        internal void CleaSubTreadList()
        {
            foreach (AdvScenarioThread thread in this.SubThreadList)
            {
                Object.Destroy(thread);
            }
            this.SubThreadList.Clear();
        }

        [DebuggerHidden]
        private IEnumerator CoStartPage(AdvScenarioLabelData labelData, AdvScenarioPageData pageData, AdvCommand returnToCommand, bool skipPageHeaer)
        {
            return new <CoStartPage>c__Iterator1 { skipPageHeaer = skipPageHeaer, pageData = pageData, returnToCommand = returnToCommand, labelData = labelData, $this = this };
        }

        [DebuggerHidden]
        private IEnumerator CoStartScenario(string label, int page, AdvCommand returnToCommand, bool skipPageHeaer)
        {
            return new <CoStartScenario>c__Iterator0 { label = label, page = page, returnToCommand = returnToCommand, skipPageHeaer = skipPageHeaer, $this = this };
        }

        internal bool EnableSaveOnPageTop()
        {
            if (!this.IsMainThread)
            {
                return false;
            }
            if (this.Engine.IsSceneGallery)
            {
                return false;
            }
            AdvSaveManager.SaveType type = this.Engine.SaveManager.Type;
            if (type != AdvSaveManager.SaveType.Default)
            {
                return ((type == AdvSaveManager.SaveType.SavePoint) && ((this.Engine.Page.PageNo == 0) && this.Engine.Page.CurrentData.ScenarioLabelData.IsSavePoint));
            }
            return true;
        }

        internal bool EnableSaveTextTop()
        {
            if (!this.IsMainThread)
            {
                return false;
            }
            if (this.Engine.IsSceneGallery)
            {
                return false;
            }
            if (this.WaitManager.IsWaiting)
            {
                return false;
            }
            return ((this.SubThreadList.Count > 0) && false);
        }

        internal void Init(AdvScenarioPlayer scenarioPlayer, string name, AdvScenarioThread parent)
        {
            this.ScenarioPlayer = scenarioPlayer;
            this.threadName = name;
            this.ParenetThread = parent;
            this.IsMainThread = parent == null;
        }

        public bool IsCurrentCommand(AdvCommand command)
        {
            return ((command != null) && (this.currentCommand == command));
        }

        internal bool IsPlayingSubThread(string name)
        {
            foreach (AdvScenarioThread thread in this.SubThreadList)
            {
                if ((thread != null) && (thread.ThreadName == name))
                {
                    return thread.IsPlaying;
                }
            }
            return false;
        }

        private void JumpToReserved()
        {
            base.StopAllCoroutines();
            if (this.JumpManager.SubRoutineReturnInfo != null)
            {
                SubRoutineInfo subRoutineReturnInfo = this.JumpManager.SubRoutineReturnInfo;
                base.StartCoroutine(this.CoStartScenario(subRoutineReturnInfo.ReturnLabel, subRoutineReturnInfo.ReturnPageNo, subRoutineReturnInfo.ReturnCommand, false));
            }
            else
            {
                base.StartCoroutine(this.CoStartScenario(this.JumpManager.Label, 0, null, false));
            }
        }

        private void OnDestroy()
        {
            this.ClearPreload();
            this.CleaSubTreadList();
            if (this.ParenetThread != null)
            {
                this.ParenetThread.SubThreadList.Remove(this);
            }
        }

        private void OnDisable()
        {
        }

        private void OnEnable()
        {
        }

        private void OnEndThread()
        {
            this.IsPlaying = false;
            if (this.IsMainThread)
            {
                this.ScenarioPlayer.EndScenario();
            }
            else
            {
                Object.Destroy(this);
            }
        }

        private void ResetOnJump()
        {
            this.IsLoading = false;
            this.ifManager.Clear();
            this.jumpManager.ClearOnJump();
            this.ClearPreload();
        }

        internal void StartScenario(string label, int page, bool skipPageHeaer)
        {
            base.StartCoroutine(this.CoStartScenario(label, page, null, skipPageHeaer));
        }

        internal void StartSubThread(string label, string name)
        {
            AdvScenarioThread item = base.get_gameObject().AddComponent<AdvScenarioThread>();
            item.Init(this.ScenarioPlayer, name, this);
            this.SubThreadList.Add(item);
            item.StartScenario(label, 0, false);
        }

        private void UpdatePreLoadFiles(string scenarioLabel, int page)
        {
            HashSet<AssetFile> preloadFileSet = this.preloadFileSet;
            this.preloadFileSet = this.Engine.DataManager.MakePreloadFileList(scenarioLabel, page, this.ScenarioPlayer.MaxFilePreload, this.ScenarioPlayer.PreloadDeep);
            if (this.preloadFileSet == null)
            {
                this.preloadFileSet = new HashSet<AssetFile>();
            }
            foreach (AssetFile file in this.preloadFileSet)
            {
                AssetFileManager.Preload(file, this);
            }
            foreach (AssetFile file2 in preloadFileSet)
            {
                if (!this.preloadFileSet.Contains(file2))
                {
                    file2.Unuse(this);
                }
            }
        }

        public AdvCommand CurrentCommand
        {
            get
            {
                return this.currentCommand;
            }
        }

        internal AdvEngine Engine
        {
            get
            {
                return this.ScenarioPlayer.Engine;
            }
        }

        internal AdvIfManager IfManager
        {
            get
            {
                return this.ifManager;
            }
        }

        private bool IsBreakCommand
        {
            get
            {
                return ((!this.IsPlaying || this.JumpManager.IsReserved) || (this.IsMainThread && this.ScenarioPlayer.IsReservedEndScenario));
            }
        }

        public bool IsLoading { get; private set; }

        public bool IsLoadingDeep
        {
            get
            {
                if (this.IsLoading)
                {
                    return true;
                }
                foreach (AdvScenarioThread thread in this.SubThreadList)
                {
                    if (thread.IsLoading)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public bool IsMainThread { get; private set; }

        public bool IsPlaying { get; set; }

        internal AdvJumpManager JumpManager
        {
            get
            {
                return this.jumpManager;
            }
        }

        private AdvScenarioThread ParenetThread { get; set; }

        internal AdvScenarioPlayer ScenarioPlayer { get; private set; }

        internal bool SkipPageHeaerOnSave { get; private set; }

        private List<AdvScenarioThread> SubThreadList
        {
            get
            {
                return this.subThreadList;
            }
        }

        public string ThreadName
        {
            get
            {
                return this.threadName;
            }
        }

        internal AdvWaitManager WaitManager
        {
            get
            {
                return this.waitManager;
            }
        }

        [CompilerGenerated]
        private sealed class <CoStartPage>c__Iterator1 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal bool $disposing;
            internal int $PC;
            internal AdvScenarioThread $this;
            internal AdvCommand <command>__0;
            internal int <index>__0;
            internal AdvScenarioLabelData labelData;
            internal AdvScenarioPageData pageData;
            internal AdvCommand returnToCommand;
            internal bool skipPageHeaer;

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
                        this.<index>__0 = !this.skipPageHeaer ? 0 : this.pageData.IndexTextTopCommand;
                        this.<command>__0 = this.pageData.GetCommand(this.<index>__0);
                        if (this.returnToCommand != null)
                        {
                            while (this.<command>__0 != this.returnToCommand)
                            {
                                this.<command>__0 = this.pageData.GetCommand(++this.<index>__0);
                            }
                        }
                        break;

                    case 1:
                        goto Label_02C9;

                    case 2:
                        goto Label_03CA;

                    case 3:
                        goto Label_03DF;

                    default:
                        goto Label_05BA;
                }
                if (this.$this.IfManager.IsLoadInit)
                {
                    this.<index>__0 = this.pageData.GetIfSkipCommandIndex(this.<index>__0);
                    this.<command>__0 = this.pageData.GetCommand(this.<index>__0);
                }
                if (this.$this.EnableSaveOnPageTop() && this.pageData.EnableSave)
                {
                    this.$this.SkipPageHeaerOnSave = false;
                    this.$this.Engine.SaveManager.UpdateAutoSaveData(this.$this.Engine);
                }
                this.$this.CheckSystemDataWriteIfChanged();
                while (this.<command>__0 != null)
                {
                    if (this.<command>__0.IsEntityType)
                    {
                        this.<command>__0 = AdvEntityData.CreateEntityCommand(this.<command>__0, this.$this.Engine, this.pageData);
                    }
                    if (this.$this.IfManager.CheckSkip(this.<command>__0))
                    {
                        if (this.$this.ScenarioPlayer.DebugOutputLog)
                        {
                            Debug.Log(string.Concat(new object[] { "Command If Skip: ", this.<command>__0.GetType(), " ", this.labelData.ScenarioLabel, ":", this.pageData.PageNo }));
                        }
                        this.<command>__0 = this.pageData.GetCommand(++this.<index>__0);
                        continue;
                    }
                    this.$this.currentCommand = this.<command>__0;
                    this.<command>__0.Load();
                    if (this.$this.EnableSaveTextTop() && this.pageData.EnableSaveTextTop(this.<command>__0))
                    {
                        this.$this.SkipPageHeaerOnSave = true;
                        this.$this.Engine.SaveManager.UpdateAutoSaveData(this.$this.Engine);
                        this.$this.CheckSystemDataWriteIfChanged();
                    }
                Label_02C9:
                    while (!this.<command>__0.IsLoadEnd())
                    {
                        this.$this.IsLoading = true;
                        this.$current = null;
                        if (!this.$disposing)
                        {
                            this.$PC = 1;
                        }
                        goto Label_05BC;
                    }
                    this.$this.IsLoading = false;
                    this.<command>__0.CurrentTread = this.$this;
                    if (this.$this.ScenarioPlayer.DebugOutputLog)
                    {
                        Debug.Log(string.Concat(new object[] { "Command : ", this.<command>__0.GetType(), " ", this.labelData.ScenarioLabel, ":", this.pageData.PageNo }));
                    }
                    this.$this.ScenarioPlayer.OnBeginCommand.Invoke(this.<command>__0);
                    this.<command>__0.DoCommand(this.$this.Engine);
                    this.<command>__0.Unload();
                    this.<command>__0.CurrentTread = null;
                Label_03CA:
                    while (this.$this.ScenarioPlayer.IsPausing)
                    {
                        this.$current = null;
                        if (!this.$disposing)
                        {
                            this.$PC = 2;
                        }
                        goto Label_05BC;
                    }
                Label_03DF:
                    this.<command>__0.CurrentTread = this.$this;
                    this.$this.ScenarioPlayer.OnUpdatePreWaitingCommand.Invoke(this.<command>__0);
                    if (this.<command>__0.Wait(this.$this.Engine))
                    {
                        if (this.$this.ScenarioPlayer.DebugOutputWaiting)
                        {
                            Debug.Log("Wait..." + this.<command>__0.GetType());
                        }
                        this.$this.ScenarioPlayer.OnUpdateWaitingCommand.Invoke(this.<command>__0);
                        this.<command>__0.CurrentTread = null;
                        this.$current = null;
                        if (!this.$disposing)
                        {
                            this.$PC = 3;
                        }
                        goto Label_05BC;
                    }
                    this.<command>__0.CurrentTread = this.$this;
                    if (this.$this.ScenarioPlayer.DebugOutputCommandEnd)
                    {
                        Debug.Log(string.Concat(new object[] { "End :", this.<command>__0.GetType(), " ", this.labelData.ScenarioLabel, ":", this.pageData.PageNo }));
                    }
                    this.$this.ScenarioPlayer.OnEndCommand.Invoke(this.<command>__0);
                    this.<command>__0.CurrentTread = null;
                    this.$this.Engine.UiManager.IsInputTrig = false;
                    this.$this.Engine.UiManager.IsInputTrigCustom = false;
                    if (this.$this.IsBreakCommand)
                    {
                        goto Label_05BA;
                    }
                    this.<command>__0 = this.pageData.GetCommand(++this.<index>__0);
                }
                this.$PC = -1;
            Label_05BA:
                return false;
            Label_05BC:
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
        private sealed class <CoStartScenario>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal bool $disposing;
            internal int $PC;
            internal AdvScenarioThread $this;
            internal AdvScenarioLabelData <currentLabelData>__0;
            internal AdvScenarioPageData <currentPageData>__1;
            internal Coroutine <pageCoroutine>__2;
            internal string label;
            internal int page;
            internal AdvCommand returnToCommand;
            internal bool skipPageHeaer;

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
                        this.$this.IsPlaying = true;
                        this.$this.SkipPageHeaerOnSave = false;
                        if (this.$this.ScenarioPlayer.DebugOutputLog)
                        {
                            Debug.Log(string.Concat(new object[] { "Jump : ", this.label, " :", this.page }));
                        }
                        break;

                    case 1:
                        break;

                    case 2:
                        goto Label_00EE;

                    case 3:
                        goto Label_0262;

                    default:
                        goto Label_03A8;
                }
                if (this.$this.Engine.IsLoading)
                {
                    this.$current = null;
                    if (!this.$disposing)
                    {
                        this.$PC = 1;
                    }
                    goto Label_03AA;
                }
                this.$this.IsLoading = true;
            Label_00EE:
                while (!this.$this.Engine.DataManager.IsLoadEndScenarioLabel(this.label))
                {
                    this.$current = null;
                    if (!this.$disposing)
                    {
                        this.$PC = 2;
                    }
                    goto Label_03AA;
                }
                this.$this.IsLoading = false;
                this.$this.ResetOnJump();
                if (this.page < 0)
                {
                    this.page = 0;
                }
                if (this.page != 0)
                {
                    this.$this.ifManager.IsLoadInit = true;
                }
                this.<currentLabelData>__0 = this.$this.Engine.DataManager.FindScenarioLabelData(this.label);
                while (this.<currentLabelData>__0 != null)
                {
                    this.$this.ScenarioPlayer.UpdateSceneGallery(this.<currentLabelData>__0.ScenarioLabel, this.$this.Engine);
                    this.<currentPageData>__1 = this.<currentLabelData>__0.GetPageData(this.page);
                    while (this.<currentPageData>__1 != null)
                    {
                        this.$this.UpdatePreLoadFiles(this.<currentLabelData>__0.ScenarioLabel, this.page);
                        if (this.$this.IsMainThread)
                        {
                            this.$this.Engine.Page.BeginPage(this.<currentPageData>__1);
                        }
                        this.<pageCoroutine>__2 = this.$this.StartCoroutine(this.$this.CoStartPage(this.<currentLabelData>__0, this.<currentPageData>__1, this.returnToCommand, this.skipPageHeaer));
                        if (this.<pageCoroutine>__2 != null)
                        {
                            this.$current = this.<pageCoroutine>__2;
                            if (!this.$disposing)
                            {
                                this.$PC = 3;
                            }
                            goto Label_03AA;
                        }
                    Label_0262:
                        this.$this.currentCommand = null;
                        this.returnToCommand = null;
                        this.skipPageHeaer = false;
                        if (this.$this.IsMainThread)
                        {
                            this.$this.Engine.Page.EndPage();
                        }
                        if (this.$this.IsBreakCommand)
                        {
                            if (this.$this.IsMainThread && this.$this.ScenarioPlayer.IsReservedEndScenario)
                            {
                                this.$this.ScenarioPlayer.EndScenario();
                            }
                            else if (this.$this.JumpManager.IsReserved)
                            {
                                this.$this.JumpToReserved();
                            }
                            else
                            {
                                this.$this.OnEndThread();
                            }
                            goto Label_03A8;
                        }
                        this.<currentPageData>__1 = this.<currentLabelData>__0.GetPageData(++this.page);
                    }
                    this.$this.IfManager.IsLoadInit = false;
                    this.<currentLabelData>__0 = this.$this.Engine.DataManager.NextScenarioLabelData(this.<currentLabelData>__0.ScenarioLabel);
                    this.page = 0;
                }
                this.$this.OnEndThread();
                this.$PC = -1;
            Label_03A8:
                return false;
            Label_03AA:
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
    }
}

