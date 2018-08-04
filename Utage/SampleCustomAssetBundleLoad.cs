namespace Utage
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    [AddComponentMenu("Utage/Sample/CustomAssetBundleLoad")]
    public class SampleCustomAssetBundleLoad : MonoBehaviour
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private AdvImportScenarios <Scenarios>k__BackingField;
        private List<SampleAssetBundleVersionInfo> assetBundleList;
        [SerializeField]
        private AdvEngine engine;
        [SerializeField]
        private string startScenario = string.Empty;

        public SampleCustomAssetBundleLoad()
        {
            List<SampleAssetBundleVersionInfo> list = new List<SampleAssetBundleVersionInfo>();
            SampleAssetBundleVersionInfo item = new SampleAssetBundleVersionInfo {
                resourcePath = "Sample.scenarios.asset",
                url = "http://madnesslabo.net/Utage3CustomLoad/Windows/sample.scenarios.asset",
                version = 0
            };
            list.Add(item);
            item = new SampleAssetBundleVersionInfo {
                resourcePath = "Texture/Character/Utako/utako.png",
                url = "http://madnesslabo.net/Utage3CustomLoad/Windows/texture/character/utako/utako.asset",
                version = 0
            };
            list.Add(item);
            item = new SampleAssetBundleVersionInfo {
                resourcePath = "Texture/BG/TutorialBg1.png",
                url = "http://madnesslabo.net/Utage3Download/Sample/Windows/texture/bg/tutorialbg1.asset",
                version = 0
            };
            list.Add(item);
            item = new SampleAssetBundleVersionInfo {
                resourcePath = "Sound/BGM/MainTheme.wav",
                url = "http://madnesslabo.net/Utage3Download/Sample/Windows/sound/bgm/maintheme.asset",
                version = 0
            };
            list.Add(item);
            this.assetBundleList = list;
        }

        private void Awake()
        {
            base.StartCoroutine(this.LoadEngineAsync());
        }

        [DebuggerHidden]
        private IEnumerator CoPlayEngine()
        {
            return new <CoPlayEngine>c__Iterator2 { $this = this };
        }

        [DebuggerHidden]
        private IEnumerator LoadEngineAsync()
        {
            return new <LoadEngineAsync>c__Iterator0 { $this = this };
        }

        [DebuggerHidden]
        private IEnumerator LoadScenariosAsync(string url)
        {
            return new <LoadScenariosAsync>c__Iterator1 { url = url, $this = this };
        }

        private void StartEngine()
        {
            base.StartCoroutine(this.CoPlayEngine());
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

        private AdvImportScenarios Scenarios { get; set; }

        [CompilerGenerated]
        private sealed class <CoPlayEngine>c__Iterator2 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal bool $disposing;
            internal int $PC;
            internal SampleCustomAssetBundleLoad $this;

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
        private sealed class <LoadEngineAsync>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal bool $disposing;
            internal List<SampleCustomAssetBundleLoad.SampleAssetBundleVersionInfo>.Enumerator $locvar0;
            internal int $PC;
            internal SampleCustomAssetBundleLoad $this;

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
                        this.$locvar0 = this.$this.assetBundleList.GetEnumerator();
                        try
                        {
                            while (this.$locvar0.MoveNext())
                            {
                                SampleCustomAssetBundleLoad.SampleAssetBundleVersionInfo current = this.$locvar0.Current;
                                AssetFileManager.GetInstance().AssetBundleInfoManager.AddAssetBundleInfo(current.resourcePath, current.url, current.version);
                            }
                        }
                        finally
                        {
                            this.$locvar0.Dispose();
                        }
                        if (!string.IsNullOrEmpty(this.$this.startScenario))
                        {
                            this.$this.Engine.StartScenarioLabel = this.$this.startScenario;
                        }
                        AssetFileManager.InitLoadTypeSetting(AssetFileManagerSettings.LoadType.Server);
                        this.$current = this.$this.LoadScenariosAsync("Sample.scenarios.asset");
                        if (!this.$disposing)
                        {
                            this.$PC = 1;
                        }
                        return true;

                    case 1:
                        if (this.$this.Scenarios != null)
                        {
                            this.$this.Engine.BootFromExportData(this.$this.Scenarios, string.Empty);
                            this.$this.StartEngine();
                            this.$PC = -1;
                            break;
                        }
                        Debug.LogError("Scenarios is Blank. Please set .scenarios Asset", this.$this);
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
        private sealed class <LoadScenariosAsync>c__Iterator1 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal bool $disposing;
            internal int $PC;
            internal SampleCustomAssetBundleLoad $this;
            internal AssetFile <file>__0;
            internal AdvImportScenarios <scenarios>__0;
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
                        goto Label_00C1;
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
                    Debug.LogError(this.url + " is  not scenario file");
                }
                else
                {
                    this.$this.Scenarios = this.<scenarios>__0;
                    this.$PC = -1;
                }
            Label_00C1:
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

        [Serializable]
        public class SampleAssetBundleVersionInfo
        {
            public string resourcePath;
            public string url;
            public int version;
        }
    }
}

