namespace Utage
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    [AddComponentMenu("Utage/ADV/Internal/DataManager ")]
    public class AdvDataManager : MonoBehaviour
    {
        [SerializeField]
        private bool isBackGroundDownload = true;
        private AdvMacroManager macroManager = new AdvMacroManager();
        private Dictionary<string, AdvScenarioData> scenarioDataTbl = new Dictionary<string, AdvScenarioData>();
        private AdvSettingDataManager settingDataManager = new AdvSettingDataManager();

        public void BootInit(string rootDirResource)
        {
            this.settingDataManager.BootInit(rootDirResource);
        }

        public void BootInitChapter(AdvChapterData chapter)
        {
            chapter.BootInit(this.SettingDataManager);
            Dictionary<string, AdvScenarioData> scenarioDataTbl = new Dictionary<string, AdvScenarioData>();
            chapter.AddScenario(scenarioDataTbl);
            foreach (KeyValuePair<string, AdvScenarioData> pair in scenarioDataTbl)
            {
                this.scenarioDataTbl.Add(pair.Key, pair.Value);
            }
            foreach (KeyValuePair<string, AdvScenarioData> pair2 in scenarioDataTbl)
            {
                pair2.Value.Init(this.settingDataManager);
            }
        }

        public void BootInitScenario(bool async)
        {
            if (async)
            {
                base.StartCoroutine(this.CoBootInitScenariodData());
            }
            else
            {
                this.BootInitScenariodData();
                this.StartBackGroundDownloadResource();
            }
        }

        public void BootInitScenariodData()
        {
            if (this.settingDataManager.ImportedScenarios != null)
            {
                this.settingDataManager.ImportedScenarios.Chapters.ForEach(x => x.AddScenario(this.scenarioDataTbl));
            }
            foreach (AdvScenarioData data in this.scenarioDataTbl.Values)
            {
                data.Init(this.settingDataManager);
            }
        }

        [DebuggerHidden]
        public IEnumerator CoBootInitScenariodData()
        {
            return new <CoBootInitScenariodData>c__Iterator0 { $this = this };
        }

        public void DownloadAll()
        {
            foreach (AdvScenarioData data in this.scenarioDataTbl.Values)
            {
                data.Download(this);
            }
            this.SettingDataManager.DownloadAll();
        }

        public AdvScenarioData FindScenarioData(string label)
        {
            foreach (AdvScenarioData data in this.scenarioDataTbl.Values)
            {
                if (data.IsContainsScenarioLabel(label))
                {
                    return data;
                }
            }
            return null;
        }

        public AdvScenarioLabelData FindScenarioLabelData(string scenarioLabel)
        {
            foreach (AdvScenarioData data in this.scenarioDataTbl.Values)
            {
                AdvScenarioLabelData data2 = data.FindScenarioLabelData(scenarioLabel);
                if (data2 != null)
                {
                    return data2;
                }
            }
            return null;
        }

        public bool IsLoadEndScenarioLabel(string label)
        {
            if (this.FindScenarioData(label) != null)
            {
                return true;
            }
            object[] args = new object[] { label };
            Debug.LogError(LanguageAdvErrorMsg.LocalizeTextFormat(AdvErrorMsg.NotFoundScnarioLabel, args));
            return false;
        }

        public bool IsLoadEndScenarioLabel(AdvScenarioJumpData jumpData)
        {
            return this.IsLoadEndScenarioLabel(jumpData.ToLabel);
        }

        public HashSet<AssetFile> MakePreloadFileList(string scenarioLabel, int page, int maxFilePreload, int preloadDeep)
        {
            foreach (AdvScenarioData data in this.scenarioDataTbl.Values)
            {
                if (data.IsContainsScenarioLabel(scenarioLabel))
                {
                    AdvScenarioLabelData data2 = data.FindScenarioLabelData(scenarioLabel);
                    if (data2 == null)
                    {
                        return null;
                    }
                    return data2.MakePreloadFileListSub(this, page, maxFilePreload, preloadDeep);
                }
            }
            return null;
        }

        public AdvScenarioLabelData NextScenarioLabelData(string scenarioLabel)
        {
            foreach (AdvScenarioData data in this.scenarioDataTbl.Values)
            {
                AdvScenarioLabelData data2 = data.FindNextScenarioLabelData(scenarioLabel);
                if (data2 != null)
                {
                    return data2;
                }
            }
            return null;
        }

        internal void SetSubroutineRetunInfo(string scenarioLabel, int subroutineCommandIndex, SubRoutineInfo info)
        {
            foreach (AdvScenarioData data in this.scenarioDataTbl.Values)
            {
                AdvScenarioLabelData data2 = data.FindScenarioLabelData(scenarioLabel);
                if (data2 != null)
                {
                    if (!data2.TrySetSubroutineRetunInfo(subroutineCommandIndex, info))
                    {
                        AdvScenarioLabelData data3 = this.NextScenarioLabelData(scenarioLabel);
                        info.ReturnLabel = data3.ScenarioLabel;
                        info.ReturnPageNo = 0;
                        info.ReturnCommand = null;
                    }
                    break;
                }
            }
        }

        public void StartBackGroundDownloadResource()
        {
            if (this.isBackGroundDownload)
            {
                this.DownloadAll();
            }
        }

        public bool IsBackGroundDownload
        {
            get
            {
                return this.isBackGroundDownload;
            }
            set
            {
                this.isBackGroundDownload = value;
            }
        }

        public bool IsReadySettingData
        {
            get
            {
                return (this.settingDataManager != null);
            }
        }

        public AdvMacroManager MacroManager
        {
            get
            {
                return this.macroManager;
            }
        }

        public AdvSettingDataManager SettingDataManager
        {
            get
            {
                return this.settingDataManager;
            }
        }

        [CompilerGenerated]
        private sealed class <CoBootInitScenariodData>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal bool $disposing;
            internal Dictionary<string, AdvScenarioData>.ValueCollection.Enumerator $locvar0;
            internal int $PC;
            internal AdvDataManager $this;
            internal AdvScenarioData <data>__1;

            internal void <>m__0(AdvChapterData x)
            {
                x.AddScenario(this.$this.scenarioDataTbl);
            }

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
                        if (this.$this.settingDataManager.ImportedScenarios != null)
                        {
                            this.$this.settingDataManager.ImportedScenarios.Chapters.ForEach(new Action<AdvChapterData>(this.<>m__0));
                        }
                        this.$locvar0 = this.$this.scenarioDataTbl.Values.GetEnumerator();
                        num = 0xfffffffd;
                        break;

                    case 1:
                        break;

                    default:
                        goto Label_0114;
                }
                try
                {
                    while (this.$locvar0.MoveNext())
                    {
                        this.<data>__1 = this.$locvar0.Current;
                        this.<data>__1.Init(this.$this.settingDataManager);
                        this.$current = null;
                        if (!this.$disposing)
                        {
                            this.$PC = 1;
                        }
                        flag = true;
                        return true;
                    }
                }
                finally
                {
                    if (!flag)
                    {
                    }
                    this.$locvar0.Dispose();
                }
                this.$this.StartBackGroundDownloadResource();
                this.$PC = -1;
            Label_0114:
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

