namespace Utage
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class AdvImportBook : ScriptableObject
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <Reimport>k__BackingField;
        [NonSerialized]
        private List<StringGrid> gridList = new List<StringGrid>();
        [SerializeField]
        private List<AdvImportScenarioSheet> importGridList = new List<AdvImportScenarioSheet>();
        [SerializeField]
        private int importVersion;
        private const int Version = 0;

        public void AddSrourceBook(StringGridDictionary book)
        {
            foreach (StringGridDictionaryKeyValue value2 in book.List)
            {
                this.GridList.Add(value2.Grid);
            }
        }

        public void BootInit()
        {
            foreach (AdvImportScenarioSheet sheet in this.ImportGridList)
            {
                sheet.InitLink();
            }
        }

        public bool CheckVersion()
        {
            return (this.importVersion == 0);
        }

        public void Clear()
        {
            this.Reimport = true;
            this.ImportGridList.Clear();
            this.GridList.Clear();
        }

        public void MakeImportData(AdvSettingDataManager dataManager, AdvMacroManager macroManager)
        {
            foreach (StringGrid grid in this.GridList)
            {
                string sheetName = grid.SheetName;
                if (sheetName.Contains("."))
                {
                    object[] objArray1 = new object[] { grid.Name };
                    Debug.LogErrorFormat("Don't use '.' to sheetname in  {0}", objArray1);
                }
                if (AdvSheetParser.IsScenarioSheet(sheetName) && !AdvMacroManager.IsMacroName(sheetName))
                {
                    this.importGridList.Add(new AdvImportScenarioSheet(grid, dataManager, macroManager));
                }
            }
            this.Reimport = false;
        }

        public List<StringGrid> GridList
        {
            get
            {
                return this.gridList;
            }
        }

        public List<AdvImportScenarioSheet> ImportGridList
        {
            get
            {
                return this.importGridList;
            }
        }

        public bool Reimport { get; set; }
    }
}

