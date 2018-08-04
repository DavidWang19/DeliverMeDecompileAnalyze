namespace Utage
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class AdvScenarioPageData
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private List<AdvScenarioLabelData> <AutoJumpLabelList>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private List<AdvCommand> <CommandList>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <EnableSave>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <IndexTextTopCommand>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private List<AdvScenarioLabelData> <JumpLabelList>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <MessageWindowName>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <PageNo>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private AdvScenarioLabelData <ScenarioLabelData>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private List<AdvCommandText> <TextDataList>k__BackingField;

        public AdvScenarioPageData(AdvScenarioLabelData scenarioLabelData, int pageNo, List<AdvCommand> commandList)
        {
            this.TextDataList = new List<AdvCommandText>();
            this.ScenarioLabelData = scenarioLabelData;
            this.PageNo = pageNo;
            this.CommandList = commandList;
        }

        internal void AddTextData(AdvCommandText command)
        {
            this.TextDataList.Add(command);
        }

        public void AddToFileSet(HashSet<AssetFile> fileSet)
        {
            <AddToFileSet>c__AnonStorey3 storey = new <AddToFileSet>c__AnonStorey3 {
                fileSet = fileSet
            };
            foreach (AdvCommand command in this.CommandList)
            {
                if (command.IsExistLoadFile())
                {
                    command.LoadFileList.ForEach(new Action<AssetFile>(storey.<>m__0));
                }
            }
        }

        internal void ChangeTextDataOnCreateEntity(int index, AdvCommandText entity)
        {
            if (this.TextDataList.Count < index)
            {
                Debug.LogError("  Index error On CreateEntity ");
            }
            else
            {
                this.TextDataList[index] = entity;
            }
        }

        public void Download(AdvDataManager dataManager)
        {
            <Download>c__AnonStorey2 storey = new <Download>c__AnonStorey2 {
                dataManager = dataManager
            };
            this.CommandList.ForEach(new Action<AdvCommand>(storey.<>m__0));
        }

        internal bool EnableSaveTextTop(AdvCommand command)
        {
            if (command == null)
            {
                return false;
            }
            if (!this.EnableSave)
            {
                return false;
            }
            if (command == this.GetCommand(0))
            {
                return false;
            }
            return (command == this.CommandList[this.IndexTextTopCommand]);
        }

        internal List<AdvScenarioLabelData> GetAutoJumpLabels(AdvDataManager dataManager)
        {
            <GetAutoJumpLabels>c__AnonStorey1 storey = new <GetAutoJumpLabels>c__AnonStorey1 {
                dataManager = dataManager,
                $this = this
            };
            if (this.AutoJumpLabelList == null)
            {
                this.AutoJumpLabelList = new List<AdvScenarioLabelData>();
                this.CommandList.ForEach(new Action<AdvCommand>(storey.<>m__0));
            }
            return this.AutoJumpLabelList;
        }

        public AdvCommand GetCommand(int index)
        {
            return ((index >= this.CommandList.Count) ? null : this.CommandList[index]);
        }

        internal int GetIfSkipCommandIndex(int index)
        {
            for (int i = index; i < this.CommandList.Count; i++)
            {
                AdvCommand command = this.CommandList[i];
                if (command.IsIfCommand)
                {
                    if (command.GetType() == typeof(AdvCommandIf))
                    {
                        return index;
                    }
                    for (int j = index + 1; j < this.CommandList.Count; j++)
                    {
                        if (this.CommandList[j].GetType() == typeof(AdvCommandEndIf))
                        {
                            return j;
                        }
                    }
                }
            }
            return index;
        }

        public List<AdvScenarioLabelData> GetJumpScenarioLabelDataList(AdvDataManager dataManager)
        {
            <GetJumpScenarioLabelDataList>c__AnonStorey0 storey = new <GetJumpScenarioLabelDataList>c__AnonStorey0 {
                dataManager = dataManager,
                $this = this
            };
            if (this.JumpLabelList == null)
            {
                this.JumpLabelList = new List<AdvScenarioLabelData>();
                this.CommandList.ForEach(new Action<AdvCommand>(storey.<>m__0));
            }
            return this.JumpLabelList;
        }

        internal void Init()
        {
            this.CommandList.ForEach(command => command.InitFromPageData(this));
            this.EnableSave = true;
            for (int i = 0; i < this.CommandList.Count; i++)
            {
                if (this.CommandList[i].IsTypePage())
                {
                    this.IndexTextTopCommand = i;
                    break;
                }
            }
        }

        internal void InitMessageWindowName(AdvCommand command, string messageWindowName)
        {
            if (!string.IsNullOrEmpty(messageWindowName))
            {
                if (string.IsNullOrEmpty(this.MessageWindowName))
                {
                    this.MessageWindowName = messageWindowName;
                }
                else if (this.MessageWindowName != messageWindowName)
                {
                    Debug.LogError(command.ToErrorString(messageWindowName + ": WindowName already set is this page"));
                }
            }
        }

        private List<AdvScenarioLabelData> AutoJumpLabelList { get; set; }

        public List<AdvCommand> CommandList { get; private set; }

        internal bool EnableSave { get; private set; }

        internal int IndexTextTopCommand { get; private set; }

        public bool IsEmptyText
        {
            get
            {
                return (this.TextDataList.Count <= 0);
            }
        }

        private List<AdvScenarioLabelData> JumpLabelList { get; set; }

        public string MessageWindowName { get; set; }

        public int PageNo { get; private set; }

        public AdvScenarioLabelData ScenarioLabelData { get; private set; }

        public List<AdvCommandText> TextDataList { get; private set; }

        [CompilerGenerated]
        private sealed class <AddToFileSet>c__AnonStorey3
        {
            internal HashSet<AssetFile> fileSet;

            internal void <>m__0(AssetFile item)
            {
                this.fileSet.Add(item);
            }
        }

        [CompilerGenerated]
        private sealed class <Download>c__AnonStorey2
        {
            internal AdvDataManager dataManager;

            internal void <>m__0(AdvCommand item)
            {
                item.Download(this.dataManager);
            }
        }

        [CompilerGenerated]
        private sealed class <GetAutoJumpLabels>c__AnonStorey1
        {
            internal AdvScenarioPageData $this;
            internal AdvDataManager dataManager;

            internal void <>m__0(AdvCommand command)
            {
                string[] jumpLabels = command.GetJumpLabels();
                if ((jumpLabels != null) && (((command is AdvCommandJump) || (command is AdvCommandJumpRandom)) || ((command is AdvCommandJumpSubroutine) || (command is AdvCommandJumpSubroutineRandom))))
                {
                    foreach (string str in jumpLabels)
                    {
                        this.$this.AutoJumpLabelList.Add(this.dataManager.FindScenarioLabelData(str));
                    }
                }
            }
        }

        [CompilerGenerated]
        private sealed class <GetJumpScenarioLabelDataList>c__AnonStorey0
        {
            internal AdvScenarioPageData $this;
            internal AdvDataManager dataManager;

            internal void <>m__0(AdvCommand command)
            {
                string[] jumpLabels = command.GetJumpLabels();
                if (jumpLabels != null)
                {
                    foreach (string str in jumpLabels)
                    {
                        this.$this.JumpLabelList.Add(this.dataManager.FindScenarioLabelData(str));
                    }
                }
            }
        }
    }
}

