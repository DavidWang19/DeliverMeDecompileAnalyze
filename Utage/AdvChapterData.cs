namespace Utage
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class AdvChapterData : ScriptableObject
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <IsInited>k__BackingField;
        [SerializeField]
        private string chapterName = string.Empty;
        [SerializeField]
        private List<AdvImportBook> dataList = new List<AdvImportBook>();
        [SerializeField]
        private List<StringGrid> settingList = new List<StringGrid>();

        public void AddScenario(Dictionary<string, AdvScenarioData> scenarioDataTbl)
        {
            foreach (AdvImportBook book in this.DataList)
            {
                foreach (AdvImportScenarioSheet sheet in book.ImportGridList)
                {
                    if (scenarioDataTbl.ContainsKey(sheet.SheetName))
                    {
                        object[] objArray1 = new object[] { sheet.SheetName };
                        Debug.LogErrorFormat("{0} is already contains", objArray1);
                    }
                    else
                    {
                        sheet.InitLink();
                        AdvScenarioData data = new AdvScenarioData(sheet);
                        scenarioDataTbl.Add(sheet.SheetName, data);
                    }
                }
            }
        }

        public void BootInit(AdvSettingDataManager settingDataManager)
        {
            this.IsInited = true;
            foreach (StringGrid grid in this.settingList)
            {
                IAdvSetting setting = AdvSheetParser.FindSettingData(settingDataManager, grid.SheetName);
                if (setting != null)
                {
                    setting.ParseGrid(grid);
                }
            }
            foreach (StringGrid grid2 in this.settingList)
            {
                IAdvSetting setting2 = AdvSheetParser.FindSettingData(settingDataManager, grid2.SheetName);
                if (setting2 != null)
                {
                    setting2.BootInit(settingDataManager);
                }
            }
        }

        public void Init(string name)
        {
            this.chapterName = name;
        }

        public void MakeScenarioImportData(AdvSettingDataManager dataManager, AdvMacroManager macroManager)
        {
            foreach (AdvImportBook book in this.DataList)
            {
                if (book.Reimport)
                {
                    book.MakeImportData(dataManager, macroManager);
                }
            }
        }

        public void MakeSettingImportData(AdvMacroManager macroManager)
        {
            foreach (AdvImportBook book in this.DataList)
            {
                foreach (StringGrid grid in book.GridList)
                {
                    string sheetName = grid.SheetName;
                    if (AdvSheetParser.IsDisableSheetName(sheetName))
                    {
                        Debug.LogError(sheetName + " is invalid name");
                    }
                    else if (AdvSheetParser.IsSettingsSheet(sheetName))
                    {
                        this.settingList.Add(grid);
                    }
                    else
                    {
                        macroManager.TryAddMacroData(grid.SheetName, grid);
                    }
                }
            }
        }

        public string ChapterName
        {
            get
            {
                return this.chapterName;
            }
        }

        public List<AdvImportBook> DataList
        {
            get
            {
                return this.dataList;
            }
        }

        public bool IsInited { get; set; }

        public List<StringGrid> SettingList
        {
            get
            {
                return this.settingList;
            }
        }
    }
}

