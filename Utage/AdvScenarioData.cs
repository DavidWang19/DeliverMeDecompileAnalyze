namespace Utage
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UtageExtensions;

    public class AdvScenarioData
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private AdvImportScenarioSheet <DataGrid>k__BackingField;
        private bool isAlreadyBackGroundLoad;
        private bool isInit;
        private List<AdvScenarioJumpData> jumpDataList = new List<AdvScenarioJumpData>();
        private string name;
        private Dictionary<string, AdvScenarioLabelData> scenarioLabels = new Dictionary<string, AdvScenarioLabelData>();

        public AdvScenarioData(AdvImportScenarioSheet grid)
        {
            this.name = grid.SheetName;
            this.DataGrid = grid;
        }

        private void AddExtraCommand(List<AdvCommand> commandList, AdvSettingDataManager dataManager)
        {
            int index = 0;
            while (index < commandList.Count)
            {
                AdvCommand command = commandList[index];
                AdvCommand next = ((index + 1) >= commandList.Count) ? null : commandList[index + 1];
                index++;
                string[] extraCommandIdArray = command.GetExtraCommandIdArray(next);
                if (extraCommandIdArray != null)
                {
                    foreach (string str in extraCommandIdArray)
                    {
                        AdvCommand item = AdvCommandParser.CreateCommand(str, command.RowData, dataManager);
                        if (command.IsEntityType)
                        {
                            item.EntityData = command.EntityData;
                        }
                        commandList.Insert(index, item);
                        index++;
                    }
                }
            }
        }

        public void Download(AdvDataManager dataManager)
        {
            foreach (KeyValuePair<string, AdvScenarioLabelData> pair in this.ScenarioLabels)
            {
                pair.Value.Download(dataManager);
            }
            this.isAlreadyBackGroundLoad = true;
        }

        public AdvScenarioLabelData FindNextScenarioLabelData(string scenarioLabel)
        {
            AdvScenarioLabelData data = this.FindScenarioLabelData(scenarioLabel);
            if (data != null)
            {
                return data.Next;
            }
            return null;
        }

        public AdvScenarioLabelData FindScenarioLabelData(string scenarioLabel)
        {
            return this.ScenarioLabels.GetValueOrGetNullIfMissing<string, AdvScenarioLabelData>(scenarioLabel);
        }

        public void Init(AdvSettingDataManager dataManager)
        {
            this.isInit = false;
            List<AdvCommand> commandList = this.DataGrid.CreateCommandList(dataManager);
            this.AddExtraCommand(commandList, dataManager);
            this.MakeScanerioLabelData(commandList);
            this.MakeJumpDataList(commandList);
            this.isInit = true;
        }

        public bool IsContainsScenarioLabel(string scenarioLabel)
        {
            return (this.FindScenarioLabelData(scenarioLabel) != null);
        }

        private void MakeJumpDataList(List<AdvCommand> commandList)
        {
            this.JumpDataList.Clear();
            commandList.ForEach(delegate (AdvCommand command) {
                string[] jumpLabels = command.GetJumpLabels();
                if (jumpLabels != null)
                {
                    foreach (string str in jumpLabels)
                    {
                        this.JumpDataList.Add(new AdvScenarioJumpData(str, command.RowData));
                    }
                }
            });
        }

        private void MakeScanerioLabelData(List<AdvCommand> commandList)
        {
            if (commandList.Count <= 0)
            {
                return;
            }
            string name = this.Name;
            AdvCommandScenarioLabel scenarioLabelCommand = null;
            AdvScenarioLabelData data = null;
            int num = 0;
            while (true)
            {
                int index = num;
                while (num < commandList.Count)
                {
                    if (commandList[num] is AdvCommandScenarioLabel)
                    {
                        break;
                    }
                    num++;
                }
                if (this.IsContainsScenarioLabel(name))
                {
                    object[] args = new object[] { name, this.DataGridName };
                    Debug.LogError(LanguageAdvErrorMsg.LocalizeTextFormat(AdvErrorMsg.RedefinitionScenarioLabel, args));
                }
                else
                {
                    AdvScenarioLabelData data2 = new AdvScenarioLabelData(name, scenarioLabelCommand, commandList.GetRange(index, num - index));
                    if (data != null)
                    {
                        data.Next = data2;
                    }
                    data = data2;
                    this.scenarioLabels.Add(name, data2);
                }
                if (num >= commandList.Count)
                {
                    return;
                }
                scenarioLabelCommand = commandList[num] as AdvCommandScenarioLabel;
                name = scenarioLabelCommand.ScenarioLabel;
                num++;
            }
        }

        public AdvImportScenarioSheet DataGrid { get; private set; }

        public string DataGridName
        {
            get
            {
                return this.DataGrid.Name;
            }
        }

        public bool IsAlreadyBackGroundLoad
        {
            get
            {
                return this.isAlreadyBackGroundLoad;
            }
        }

        public bool IsInit
        {
            get
            {
                return this.isInit;
            }
        }

        public List<AdvScenarioJumpData> JumpDataList
        {
            get
            {
                return this.jumpDataList;
            }
        }

        private string Name
        {
            get
            {
                return this.name;
            }
        }

        public Dictionary<string, AdvScenarioLabelData> ScenarioLabels
        {
            get
            {
                return this.scenarioLabels;
            }
        }
    }
}

