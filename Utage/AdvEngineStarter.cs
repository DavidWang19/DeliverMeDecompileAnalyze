namespace Utage
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    [AddComponentMenu("Utage/ADV/EngineStarter")]
    public class AdvEngineStarter : MonoBehaviour
    {
        [CompilerGenerated]
        private static Action <>f__am$cache0;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <IsLoadStart>k__BackingField;
        [SerializeField]
        private List<string> chapterNames;
        [SerializeField]
        private AdvEngine engine;
        [SerializeField]
        private bool isAutomaticPlay;
        [SerializeField]
        private bool isLoadOnAwake = true;
        [SerializeField]
        private string rootResourceDir;
        [SerializeField]
        private AdvImportScenarios scenarios;
        [SerializeField]
        private string scenariosName;
        [SerializeField]
        private string serverUrl;
        [SerializeField]
        private string startScenario = string.Empty;
        [SerializeField]
        private StrageType strageType;
        [SerializeField]
        private bool useChapter;

        private void Awake()
        {
            if (this.isLoadOnAwake)
            {
                if (<>f__am$cache0 == null)
                {
                    <>f__am$cache0 = () => Debug.LogError("Load Error");
                }
                base.StartCoroutine(this.LoadEngineAsync(<>f__am$cache0));
            }
        }

        [DebuggerHidden]
        private IEnumerator CoPlayEngine()
        {
            return new <CoPlayEngine>c__Iterator7 { $this = this };
        }

        private string GetDynamicStrageRoot()
        {
            switch (this.Strage)
            {
                case StrageType.StreamingAssets:
                case StrageType.StreamingAssetsAndLocalScenario:
                {
                    string[] args = new string[] { this.RootResourceDir, AssetBundleHelper.RuntimeAssetBundleTarget().ToString() };
                    return FilePathUtil.ToStreamingAssetsPath(FilePathUtil.Combine(args));
                }
                case StrageType.Server:
                case StrageType.ServerAndLocalScenario:
                case StrageType.LocalAndServerScenario:
                {
                    string[] textArray1 = new string[] { this.ServerUrl, AssetBundleHelper.RuntimeAssetBundleTarget().ToString() };
                    return FilePathUtil.Combine(textArray1);
                }
            }
            Debug.LogError("UnDefine");
            return string.Empty;
        }

        [DebuggerHidden]
        private IEnumerator LoadAssetBundleManifestAsync(bool fromCache, Action onFailed)
        {
            return new <LoadAssetBundleManifestAsync>c__Iterator4 { fromCache = fromCache, onFailed = onFailed, $this = this };
        }

        [DebuggerHidden]
        private IEnumerator LoadChaptersAsync(string rootDir)
        {
            return new <LoadChaptersAsync>c__Iterator6 { rootDir = rootDir, $this = this };
        }

        [DebuggerHidden]
        public IEnumerator LoadEngineAsync(Action onFailed)
        {
            return new <LoadEngineAsync>c__Iterator0 { onFailed = onFailed, $this = this };
        }

        [DebuggerHidden]
        public IEnumerator LoadEngineAsyncFromCacheManifest(Action onFailed)
        {
            return new <LoadEngineAsyncFromCacheManifest>c__Iterator1 { onFailed = onFailed, $this = this };
        }

        [DebuggerHidden]
        private IEnumerator LoadEngineAsyncSub()
        {
            return new <LoadEngineAsyncSub>c__Iterator3 { $this = this };
        }

        [DebuggerHidden]
        private IEnumerator LoadEngineAsyncSub(bool loadManifestFromCache, Action onFailed)
        {
            return new <LoadEngineAsyncSub>c__Iterator2 { loadManifestFromCache = loadManifestFromCache, onFailed = onFailed, $this = this };
        }

        [DebuggerHidden]
        private IEnumerator LoadScenariosAsync(string rootDir)
        {
            return new <LoadScenariosAsync>c__Iterator5 { rootDir = rootDir, $this = this };
        }

        public void StartEngine()
        {
            base.StartCoroutine(this.CoPlayEngine());
        }

        private string ToScenariosFilePath(string rootDir)
        {
            string scenariosName = this.ScenariosName;
            if (string.IsNullOrEmpty(scenariosName))
            {
                scenariosName = this.RootResourceDir + ".scenarios.asset";
            }
            string[] args = new string[] { rootDir, scenariosName };
            return FilePathUtil.Combine(args);
        }

        public List<string> ChapterNames
        {
            get
            {
                return this.chapterNames;
            }
        }

        public AdvEngine Engine
        {
            get
            {
                if (this.engine == null)
                {
                }
                return (this.engine = Object.FindObjectOfType<AdvEngine>());
            }
        }

        public bool IsLoadStart { get; set; }

        public string RootResourceDir
        {
            get
            {
                return this.rootResourceDir;
            }
            set
            {
                this.rootResourceDir = value;
            }
        }

        public AdvImportScenarios Scenarios
        {
            get
            {
                return this.scenarios;
            }
            set
            {
                this.scenarios = value;
            }
        }

        public string ScenariosName
        {
            get
            {
                return this.scenariosName;
            }
            set
            {
                this.scenariosName = value;
            }
        }

        public string ServerUrl
        {
            get
            {
                return this.serverUrl;
            }
            set
            {
                this.serverUrl = value;
            }
        }

        public StrageType Strage
        {
            get
            {
                return this.strageType;
            }
            set
            {
                this.strageType = value;
            }
        }

        public bool UseChapter
        {
            get
            {
                return this.useChapter;
            }
            set
            {
                this.useChapter = value;
            }
        }

        [CompilerGenerated]
        private sealed class <CoPlayEngine>c__Iterator7 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal bool $disposing;
            internal int $PC;
            internal AdvEngineStarter $this;

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
                        if (this.$this.Engine.IsWaitBootLoading)
                        {
                            this.$current = null;
                            if (!this.$disposing)
                            {
                                this.$PC = 1;
                            }
                            return true;
                        }
                        if (string.IsNullOrEmpty(this.$this.startScenario))
                        {
                            this.$this.Engine.StartGame();
                        }
                        else
                        {
                            this.$this.Engine.StartGame(this.$this.startScenario);
                        }
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
        private sealed class <LoadAssetBundleManifestAsync>c__Iterator4 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal bool $disposing;
            private <LoadAssetBundleManifestAsync>c__AnonStorey9 $locvar0;
            internal int $PC;
            internal AdvEngineStarter $this;
            private static Action <>f__am$cache0;
            private static Action <>f__am$cache1;
            internal bool fromCache;
            internal Action onFailed;

            private static void <>m__0()
            {
            }

            private static void <>m__1()
            {
            }

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
                        this.$locvar0 = new <LoadAssetBundleManifestAsync>c__AnonStorey9();
                        this.$locvar0.<>f__ref$4 = this;
                        this.$locvar0.onFailed = this.onFailed;
                        if (this.$this.Strage != AdvEngineStarter.StrageType.Local)
                        {
                            if (this.fromCache)
                            {
                                if (<>f__am$cache0 == null)
                                {
                                    <>f__am$cache0 = new Action(AdvEngineStarter.<LoadAssetBundleManifestAsync>c__Iterator4.<>m__0);
                                }
                                this.$current = AssetFileManager.GetInstance().AssetBundleInfoManager.LoadCacheManifestAsync(this.$this.GetDynamicStrageRoot(), AssetBundleHelper.RuntimeAssetBundleTarget().ToString(), <>f__am$cache0, new Action(this.$locvar0.<>m__0));
                                if (!this.$disposing)
                                {
                                    this.$PC = 1;
                                }
                            }
                            else
                            {
                                if (<>f__am$cache1 == null)
                                {
                                    <>f__am$cache1 = new Action(AdvEngineStarter.<LoadAssetBundleManifestAsync>c__Iterator4.<>m__1);
                                }
                                this.$current = AssetFileManager.GetInstance().AssetBundleInfoManager.DownloadManifestAsync(this.$this.GetDynamicStrageRoot(), AssetBundleHelper.RuntimeAssetBundleTarget().ToString(), <>f__am$cache1, new Action(this.$locvar0.<>m__1));
                                if (!this.$disposing)
                                {
                                    this.$PC = 2;
                                }
                            }
                            return true;
                        }
                        break;

                    case 1:
                    case 2:
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

            private sealed class <LoadAssetBundleManifestAsync>c__AnonStorey9
            {
                internal AdvEngineStarter.<LoadAssetBundleManifestAsync>c__Iterator4 <>f__ref$4;
                internal Action onFailed;

                internal void <>m__0()
                {
                    this.onFailed();
                }

                internal void <>m__1()
                {
                    this.onFailed();
                }
            }
        }

        [CompilerGenerated]
        private sealed class <LoadChaptersAsync>c__Iterator6 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal bool $disposing;
            internal List<string>.Enumerator $locvar0;
            internal int $PC;
            internal AdvEngineStarter $this;
            internal AdvChapterData <chapter>__2;
            internal string <chapterName>__1;
            internal AssetFile <file>__2;
            internal AdvImportScenarios <scenarios>__0;
            internal string <url>__2;
            internal string rootDir;

            [DebuggerHidden]
            public void Dispose()
            {
                uint num = (uint) this.$PC;
                this.$disposing = true;
                this.$PC = -1;
                switch (num)
                {
                    case 1:
                        try
                        {
                        }
                        finally
                        {
                            this.$locvar0.Dispose();
                        }
                        break;
                }
            }

            public bool MoveNext()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                bool flag = false;
                switch (num)
                {
                    case 0:
                        this.<scenarios>__0 = ScriptableObject.CreateInstance<AdvImportScenarios>();
                        this.$locvar0 = this.$this.ChapterNames.GetEnumerator();
                        num = 0xfffffffd;
                        break;

                    case 1:
                        break;

                    default:
                        goto Label_0174;
                }
                try
                {
                    switch (num)
                    {
                        case 1:
                            goto Label_00CF;
                    }
                    while (this.$locvar0.MoveNext())
                    {
                        this.<chapterName>__1 = this.$locvar0.Current;
                        string[] args = new string[] { this.rootDir, this.<chapterName>__1 };
                        this.<url>__2 = FilePathUtil.Combine(args) + ".chapter.asset";
                        this.<file>__2 = AssetFileManager.Load(this.<url>__2, this.$this);
                    Label_00CF:
                        while (!this.<file>__2.IsLoadEnd)
                        {
                            this.$current = null;
                            if (!this.$disposing)
                            {
                                this.$PC = 1;
                            }
                            flag = true;
                            return true;
                        }
                        this.<chapter>__2 = this.<file>__2.UnityObject as AdvChapterData;
                        if (this.<scenarios>__0 == null)
                        {
                            Debug.LogError(this.<url>__2 + " is  not scenario file");
                            goto Label_0174;
                        }
                        this.<scenarios>__0.AddChapter(this.<chapter>__2);
                    }
                }
                finally
                {
                    if (!flag)
                    {
                    }
                    this.$locvar0.Dispose();
                }
                this.$this.Scenarios = this.<scenarios>__0;
                this.$PC = -1;
            Label_0174:
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
        private sealed class <LoadEngineAsync>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal bool $disposing;
            internal int $PC;
            internal AdvEngineStarter $this;
            internal Action onFailed;

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
                        this.$current = this.$this.LoadEngineAsyncSub(false, this.onFailed);
                        if (!this.$disposing)
                        {
                            this.$PC = 1;
                        }
                        return true;

                    case 1:
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
        private sealed class <LoadEngineAsyncFromCacheManifest>c__Iterator1 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal bool $disposing;
            internal int $PC;
            internal AdvEngineStarter $this;
            internal Action onFailed;

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
                        this.$current = this.$this.LoadEngineAsyncSub(true, this.onFailed);
                        if (!this.$disposing)
                        {
                            this.$PC = 1;
                        }
                        return true;

                    case 1:
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
        private sealed class <LoadEngineAsyncSub>c__Iterator2 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal bool $disposing;
            private <LoadEngineAsyncSub>c__AnonStorey8 $locvar0;
            internal int $PC;
            internal AdvEngineStarter $this;
            internal bool loadManifestFromCache;
            internal Action onFailed;

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
                        this.$locvar0 = new <LoadEngineAsyncSub>c__AnonStorey8();
                        this.$locvar0.<>f__ref$2 = this;
                        this.$this.IsLoadStart = true;
                        this.$locvar0.isFailed = false;
                        if (this.$this.Strage == AdvEngineStarter.StrageType.Local)
                        {
                            break;
                        }
                        this.$current = this.$this.LoadAssetBundleManifestAsync(this.loadManifestFromCache, new Action(this.$locvar0.<>m__0));
                        if (!this.$disposing)
                        {
                            this.$PC = 1;
                        }
                        goto Label_00EE;

                    case 1:
                        break;

                    case 2:
                        goto Label_00E5;

                    default:
                        goto Label_00EC;
                }
                if (this.$locvar0.isFailed)
                {
                    this.onFailed();
                }
                else
                {
                    this.$current = this.$this.LoadEngineAsyncSub();
                    if (!this.$disposing)
                    {
                        this.$PC = 2;
                    }
                    goto Label_00EE;
                }
            Label_00E5:
                this.$PC = -1;
            Label_00EC:
                return false;
            Label_00EE:
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

            private sealed class <LoadEngineAsyncSub>c__AnonStorey8
            {
                internal AdvEngineStarter.<LoadEngineAsyncSub>c__Iterator2 <>f__ref$2;
                internal bool isFailed;

                internal void <>m__0()
                {
                    this.isFailed = true;
                }
            }
        }

        [CompilerGenerated]
        private sealed class <LoadEngineAsyncSub>c__Iterator3 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal bool $disposing;
            internal int $PC;
            internal AdvEngineStarter $this;
            internal bool <needsToLoadScenario>__0;

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
                        if (!string.IsNullOrEmpty(this.$this.startScenario))
                        {
                            this.$this.Engine.StartScenarioLabel = this.$this.startScenario;
                        }
                        switch (this.$this.Strage)
                        {
                            case AdvEngineStarter.StrageType.Local:
                            case AdvEngineStarter.StrageType.LocalAndServerScenario:
                                AssetFileManager.InitLoadTypeSetting(AssetFileManagerSettings.LoadType.Local);
                                goto Label_00D2;

                            case AdvEngineStarter.StrageType.StreamingAssets:
                            case AdvEngineStarter.StrageType.StreamingAssetsAndLocalScenario:
                                AssetFileManager.InitLoadTypeSetting(AssetFileManagerSettings.LoadType.StreamingAssets);
                                goto Label_00D2;

                            case AdvEngineStarter.StrageType.Server:
                            case AdvEngineStarter.StrageType.ServerAndLocalScenario:
                                AssetFileManager.InitLoadTypeSetting(AssetFileManagerSettings.LoadType.Server);
                                goto Label_00D2;
                        }
                        Debug.LogError("Unkonw Strage" + this.$this.Strage.ToString());
                        break;

                    case 1:
                    case 2:
                        goto Label_0195;

                    default:
                        goto Label_0259;
                }
            Label_00D2:
                this.<needsToLoadScenario>__0 = false;
                switch (this.$this.Strage)
                {
                    case AdvEngineStarter.StrageType.Local:
                    case AdvEngineStarter.StrageType.StreamingAssetsAndLocalScenario:
                    case AdvEngineStarter.StrageType.ServerAndLocalScenario:
                        break;

                    default:
                        this.<needsToLoadScenario>__0 = true;
                        break;
                }
                if (this.<needsToLoadScenario>__0)
                {
                    if (this.$this.UseChapter)
                    {
                        this.$current = this.$this.LoadChaptersAsync(this.$this.GetDynamicStrageRoot());
                        if (!this.$disposing)
                        {
                            this.$PC = 1;
                        }
                    }
                    else
                    {
                        this.$current = this.$this.LoadScenariosAsync(this.$this.GetDynamicStrageRoot());
                        if (!this.$disposing)
                        {
                            this.$PC = 2;
                        }
                    }
                    return true;
                }
            Label_0195:
                if (this.$this.Scenarios == null)
                {
                    Debug.LogError("Scenarios is Blank. Please set .scenarios Asset", this.$this);
                }
                else
                {
                    switch (this.$this.Strage)
                    {
                        case AdvEngineStarter.StrageType.Local:
                        case AdvEngineStarter.StrageType.LocalAndServerScenario:
                            this.$this.Engine.BootFromExportData(this.$this.Scenarios, this.$this.RootResourceDir);
                            break;

                        default:
                            this.$this.Engine.BootFromExportData(this.$this.Scenarios, this.$this.GetDynamicStrageRoot());
                            break;
                    }
                    if (this.$this.isAutomaticPlay)
                    {
                        this.$this.StartEngine();
                    }
                    this.$PC = -1;
                }
            Label_0259:
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
        private sealed class <LoadScenariosAsync>c__Iterator5 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal bool $disposing;
            internal int $PC;
            internal AdvEngineStarter $this;
            internal AssetFile <file>__0;
            internal AdvImportScenarios <scenarios>__0;
            internal string <url>__0;
            internal string rootDir;

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
                        this.<url>__0 = this.$this.ToScenariosFilePath(this.rootDir);
                        this.<file>__0 = AssetFileManager.Load(this.<url>__0, this.$this);
                        break;

                    case 1:
                        break;

                    default:
                        goto Label_00D8;
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
                this.<scenarios>__0 = this.<file>__0.UnityObject as AdvImportScenarios;
                if (this.<scenarios>__0 == null)
                {
                    Debug.LogError(this.<url>__0 + " is  not scenario file");
                }
                else
                {
                    this.$this.Scenarios = this.<scenarios>__0;
                    this.$PC = -1;
                }
            Label_00D8:
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

        public enum StrageType
        {
            Local,
            StreamingAssets,
            Server,
            StreamingAssetsAndLocalScenario,
            ServerAndLocalScenario,
            LocalAndServerScenario
        }
    }
}

