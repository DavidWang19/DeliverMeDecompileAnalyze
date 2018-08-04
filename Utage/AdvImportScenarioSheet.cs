namespace Utage
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    [Serializable]
    public class AdvImportScenarioSheet : StringGrid
    {
        [SerializeField]
        private List<AdvEntityData> entityDataList;
        [SerializeField]
        private List<int> entityIndexTbl;

        public AdvImportScenarioSheet(StringGrid original, AdvSettingDataManager dataManager, AdvMacroManager macroManager) : base(original.Name, original.SheetName, original.Type)
        {
            this.entityIndexTbl = new List<int>();
            this.entityDataList = new List<AdvEntityData>();
            base.headerRow = original.HeaderRow;
            for (int i = 0; i < original.DataTopRow; i++)
            {
                base.AddRow(original.Rows[i].Strings);
            }
            List<StringGridRow> outputList = new List<StringGridRow>();
            foreach (StringGridRow row in original.Rows)
            {
                if (((row.RowIndex >= original.DataTopRow) && !row.IsEmptyOrCommantOut) && !macroManager.TryMacroExpansion(row, outputList, string.Empty))
                {
                    outputList.Add(row);
                }
            }
            foreach (StringGridRow row2 in outputList)
            {
                string[] strings;
                if (AdvEntityData.ContainsEntitySimple(row2))
                {
                    string[] strArray2;
                    if (AdvEntityData.TryCreateEntityStrings(row2, new Func<string, object>(dataManager.DefaultParam.GetParameter), out strArray2))
                    {
                        AdvEntityData item = new AdvEntityData(row2.Strings);
                        strings = strArray2;
                        this.entityDataList.Add(item);
                        this.entityIndexTbl.Add(base.Rows.Count);
                    }
                    else
                    {
                        strings = row2.Strings;
                    }
                }
                else
                {
                    strings = row2.Strings;
                }
                StringGridRow row3 = base.AddRow(strings);
                row3.DebugIndex = row2.DebugIndex;
                row3.DebugInfo = row2.DebugInfo;
            }
            base.InitLink();
        }

        public List<AdvCommand> CreateCommandList(AdvSettingDataManager dataManager)
        {
            List<AdvCommand> list = new List<AdvCommand>();
            foreach (StringGridRow row in base.Rows)
            {
                if ((row.RowIndex >= base.DataTopRow) && !row.IsEmptyOrCommantOut)
                {
                    AdvCommand item = AdvCommandParser.CreateCommand(row, dataManager);
                    int entityIndex = this.GetEntityIndex(row.RowIndex);
                    if (entityIndex >= 0)
                    {
                        item.EntityData = this.entityDataList[entityIndex];
                    }
                    if (item != null)
                    {
                        list.Add(item);
                    }
                }
            }
            return list;
        }

        private int GetEntityIndex(int index)
        {
            for (int i = 0; i < this.entityIndexTbl.Count; i++)
            {
                if (this.entityIndexTbl[i] == index)
                {
                    return i;
                }
            }
            return -1;
        }

        public class StringGridRowMacroed
        {
            public AdvEntityData entityData;
            public StringGridRow row;

            public StringGridRowMacroed(StringGridRow row)
            {
                this.row = row;
            }

            public StringGridRowMacroed(StringGridRow row, AdvEntityData entityData)
            {
                this.row = row;
                this.entityData = entityData;
            }
        }
    }
}

