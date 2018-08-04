namespace Utage
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class AdvScenarioLabelData
    {
        [CompilerGenerated]
        private static Action<AdvScenarioPageData> <>f__am$cache0;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private List<AdvCommand> <CommandList>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private AdvScenarioLabelData <Next>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private List<AdvScenarioPageData> <PageDataList>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <ScenarioLabel>k__BackingField;
        private AdvCommandScenarioLabel scenarioLabelCommand;

        internal AdvScenarioLabelData(string scenarioLabel, AdvCommandScenarioLabel scenarioLabelCommand, List<AdvCommand> commandList)
        {
            this.ScenarioLabel = scenarioLabel;
            this.scenarioLabelCommand = scenarioLabelCommand;
            this.CommandList = commandList;
            this.PageDataList = new List<AdvScenarioPageData>();
            if (this.CommandList.Count > 0)
            {
                int num = 0;
                do
                {
                    int begin = num;
                    int pageEndCommandIndex = this.GetPageEndCommandIndex(begin);
                    this.PageDataList.Add(new AdvScenarioPageData(this, this.PageDataList.Count, this.CommandList.GetRange(begin, (pageEndCommandIndex - begin) + 1)));
                    num = pageEndCommandIndex + 1;
                }
                while (num < this.CommandList.Count);
                if (<>f__am$cache0 == null)
                {
                    <>f__am$cache0 = new Action<AdvScenarioPageData>(AdvScenarioLabelData.<AdvScenarioLabelData>m__0);
                }
                this.PageDataList.ForEach(<>f__am$cache0);
            }
        }

        [CompilerGenerated]
        private static void <AdvScenarioLabelData>m__0(AdvScenarioPageData x)
        {
            x.Init();
        }

        internal int CountSubroutineCommandIndex(AdvCommand command)
        {
            int num = 0;
            foreach (AdvScenarioPageData data in this.PageDataList)
            {
                foreach (AdvCommand command2 in data.CommandList)
                {
                    Type type = command2.GetType();
                    if ((type == typeof(AdvCommandJumpSubroutine)) || (type == typeof(AdvCommandJumpSubroutineRandom)))
                    {
                        if (command2 == command)
                        {
                            return num;
                        }
                        num++;
                    }
                }
            }
            Debug.LogError("Not found Subroutine Command");
            return -1;
        }

        public void Download(AdvDataManager dataManager)
        {
            <Download>c__AnonStorey0 storey = new <Download>c__AnonStorey0 {
                dataManager = dataManager
            };
            this.PageDataList.ForEach(new Action<AdvScenarioPageData>(storey.<>m__0));
        }

        public AdvScenarioPageData GetPageData(int page)
        {
            return ((page >= this.PageDataList.Count) ? null : this.PageDataList[page]);
        }

        private int GetPageEndCommandIndex(int begin)
        {
            for (int i = begin; i < this.CommandList.Count; i++)
            {
                if (this.CommandList[i].IsTypePageEnd())
                {
                    for (int j = i; j < this.CommandList.Count; j++)
                    {
                        if (this.CommandList[j].IsTypePage())
                        {
                            return i;
                        }
                        if (this.CommandList[j] is AdvCommandEndPage)
                        {
                            return j;
                        }
                    }
                    return i;
                }
            }
            return (this.CommandList.Count - 1);
        }

        private bool IsEndPreLoad()
        {
            if (this.CommandList.Count <= 0)
            {
                return false;
            }
            AdvCommand command = this.CommandList[this.CommandList.Count - 1];
            if (command is AdvCommandPageControler)
            {
                if ((this.CommandList.Count - 2) < 0)
                {
                    return false;
                }
                command = this.CommandList[this.CommandList.Count - 2];
            }
            return ((command is AdvCommandEndScenario) || ((command is AdvCommandSelectionEnd) || ((command is AdvCommandSelectionClickEnd) || ((command is AdvCommandJumpRandomEnd) || ((((command is AdvCommandJump) || (command is AdvCommandJumpSubroutine)) || (command is AdvCommandJumpSubroutineRandom)) && command.IsEmptyCell(AdvColumnName.Arg2))))));
        }

        internal HashSet<AssetFile> MakePreloadFileListSub(AdvDataManager dataManager, int page, int maxFilePreload, int preloadDeep)
        {
            AdvScenarioLabelData next = this;
            HashSet<AssetFile> fileSet = new HashSet<AssetFile>();
            do
            {
                for (int i = page; i < next.PageNum; i++)
                {
                    next.GetPageData(i).AddToFileSet(fileSet);
                    if (fileSet.Count >= maxFilePreload)
                    {
                        return fileSet;
                    }
                }
                if (next.IsEndPreLoad())
                {
                    next.PreloadDeep(dataManager, page, fileSet, maxFilePreload, preloadDeep);
                    return fileSet;
                }
                page = 0;
                next = next.Next;
            }
            while (next != null);
            return fileSet;
        }

        private void PreloadDeep(AdvDataManager dataManager, HashSet<AssetFile> fileSet, int maxFilePreload, int deepLevel)
        {
            <PreloadDeep>c__AnonStorey2 storey = new <PreloadDeep>c__AnonStorey2 {
                dataManager = dataManager,
                fileSet = fileSet,
                maxFilePreload = maxFilePreload,
                deepLevel = deepLevel
            };
            if (storey.deepLevel > 0)
            {
                storey.deepLevel--;
                if ((this.PageNum > 0) && (storey.fileSet.Count < storey.maxFilePreload))
                {
                    this.GetPageData(0).AddToFileSet(storey.fileSet);
                    if (storey.fileSet.Count < storey.maxFilePreload)
                    {
                        this.GetPageData(0).GetAutoJumpLabels(storey.dataManager).ForEach(new Action<AdvScenarioLabelData>(storey.<>m__0));
                    }
                }
            }
        }

        private void PreloadDeep(AdvDataManager dataManager, int startPage, HashSet<AssetFile> fileSet, int maxFilePreload, int deepLevel)
        {
            <PreloadDeep>c__AnonStorey1 storey = new <PreloadDeep>c__AnonStorey1 {
                dataManager = dataManager,
                fileSet = fileSet,
                maxFilePreload = maxFilePreload,
                deepLevel = deepLevel
            };
            if ((storey.fileSet.Count < storey.maxFilePreload) && (storey.deepLevel > 0))
            {
                for (int i = startPage; i < this.PageNum; i++)
                {
                    this.GetPageData(i).GetJumpScenarioLabelDataList(storey.dataManager).ForEach(new Action<AdvScenarioLabelData>(storey.<>m__0));
                }
            }
        }

        public string ToErrorString(string str, string gridName)
        {
            if (this.scenarioLabelCommand != null)
            {
                return this.scenarioLabelCommand.RowData.ToErrorString(str);
            }
            return (str + " " + gridName);
        }

        internal bool TrySetSubroutineRetunInfo(int subroutineCommandIndex, SubRoutineInfo info)
        {
            info.ReturnLabel = this.ScenarioLabel;
            AdvCommand command = null;
            int num = 0;
            foreach (AdvScenarioPageData data in this.PageDataList)
            {
                foreach (AdvCommand command2 in data.CommandList)
                {
                    Type type = command2.GetType();
                    if (command == null)
                    {
                        if ((type == typeof(AdvCommandJumpSubroutine)) || (type == typeof(AdvCommandJumpSubroutineRandom)))
                        {
                            if (num == subroutineCommandIndex)
                            {
                                command = command2;
                            }
                            else
                            {
                                num++;
                            }
                        }
                    }
                    else
                    {
                        if (command.GetType() == typeof(AdvCommandJumpSubroutine))
                        {
                            info.ReturnPageNo = data.PageNo;
                            info.ReturnCommand = command2;
                            return true;
                        }
                        if (((command.GetType() == typeof(AdvCommandJumpSubroutineRandom)) && (type != typeof(AdvCommandJumpSubroutineRandom))) && (type != typeof(AdvCommandJumpSubroutineRandom)))
                        {
                            info.ReturnPageNo = data.PageNo;
                            info.ReturnCommand = command2;
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private List<AdvCommand> CommandList { get; set; }

        public bool IsSavePoint
        {
            get
            {
                return ((this.scenarioLabelCommand != null) ? (this.scenarioLabelCommand.Type == AdvCommandScenarioLabel.ScenarioLabelType.SavePoint) : false);
            }
        }

        public AdvScenarioLabelData Next { get; internal set; }

        public List<AdvScenarioPageData> PageDataList { get; private set; }

        public int PageNum
        {
            get
            {
                return this.PageDataList.Count;
            }
        }

        public string SaveTitle
        {
            get
            {
                return ((this.scenarioLabelCommand != null) ? this.scenarioLabelCommand.Title : string.Empty);
            }
        }

        public string ScenarioLabel { get; private set; }

        [CompilerGenerated]
        private sealed class <Download>c__AnonStorey0
        {
            internal AdvDataManager dataManager;

            internal void <>m__0(AdvScenarioPageData item)
            {
                item.Download(this.dataManager);
            }
        }

        [CompilerGenerated]
        private sealed class <PreloadDeep>c__AnonStorey1
        {
            internal AdvDataManager dataManager;
            internal int deepLevel;
            internal HashSet<AssetFile> fileSet;
            internal int maxFilePreload;

            internal void <>m__0(AdvScenarioLabelData x)
            {
                if (x != null)
                {
                    x.PreloadDeep(this.dataManager, this.fileSet, this.maxFilePreload, this.deepLevel);
                }
            }
        }

        [CompilerGenerated]
        private sealed class <PreloadDeep>c__AnonStorey2
        {
            internal AdvDataManager dataManager;
            internal int deepLevel;
            internal HashSet<AssetFile> fileSet;
            internal int maxFilePreload;

            internal void <>m__0(AdvScenarioLabelData x)
            {
                if (x != null)
                {
                    x.PreloadDeep(this.dataManager, this.fileSet, this.maxFilePreload, this.deepLevel);
                }
            }
        }
    }
}

