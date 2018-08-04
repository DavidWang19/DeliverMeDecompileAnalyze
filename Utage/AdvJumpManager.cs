namespace Utage
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    internal class AdvJumpManager
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <Label>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private SubRoutineInfo <SubRoutineReturnInfo>k__BackingField;
        private List<RandomInfo> randomInfoList = new List<RandomInfo>();
        private Stack<SubRoutineInfo> subRoutineCallStack = new Stack<SubRoutineInfo>();
        private const int Version = 0;

        internal void AddRandom(AdvCommand command, float rate)
        {
            this.randomInfoList.Add(new RandomInfo(command, rate));
        }

        internal void Clear()
        {
            this.ClearOnJump();
            this.subRoutineCallStack.Clear();
        }

        internal void ClearOnJump()
        {
            this.Label = string.Empty;
            this.SubRoutineReturnInfo = null;
            this.randomInfoList.Clear();
        }

        internal void EndSubroutine()
        {
            this.SubRoutineReturnInfo = this.subRoutineCallStack.Pop();
        }

        internal AdvCommand GetRandomJumpCommand()
        {
            <GetRandomJumpCommand>c__AnonStorey0 storey = new <GetRandomJumpCommand>c__AnonStorey0 {
                sum = 0f
            };
            this.randomInfoList.ForEach(new Action<RandomInfo>(storey.<>m__0));
            if (storey.sum > 0f)
            {
                float num = Random.Range(0f, storey.sum);
                foreach (RandomInfo info in this.randomInfoList)
                {
                    num -= info.rate;
                    if (num <= 0f)
                    {
                        return info.command;
                    }
                }
            }
            return null;
        }

        internal void Read(AdvEngine engine, BinaryReader reader)
        {
            this.Clear();
            if (reader.BaseStream.Length > 0L)
            {
                int num = reader.ReadInt32();
                if (num == 0)
                {
                    int num2 = reader.ReadInt32();
                    SubRoutineInfo[] infoArray = new SubRoutineInfo[num2];
                    for (int i = 0; i < num2; i++)
                    {
                        infoArray[i] = new SubRoutineInfo(engine, reader);
                    }
                    for (int j = num2 - 1; j >= 0; j--)
                    {
                        this.subRoutineCallStack.Push(infoArray[j]);
                    }
                }
                else
                {
                    object[] args = new object[] { num };
                    Debug.LogError(LanguageErrorMsg.LocalizeTextFormat(ErrorMsg.UnknownVersion, args));
                }
            }
        }

        internal void RegistoreLabel(string jumpLabel)
        {
            this.Label = jumpLabel;
        }

        internal void RegistoreSubroutine(string label, SubRoutineInfo calledInfo)
        {
            this.Label = label;
            this.subRoutineCallStack.Push(calledInfo);
        }

        internal void Write(BinaryWriter writer)
        {
            writer.Write(0);
            writer.Write(this.subRoutineCallStack.Count);
            foreach (SubRoutineInfo info in this.subRoutineCallStack)
            {
                info.Write(writer);
            }
        }

        internal bool IsReserved
        {
            get
            {
                return (!string.IsNullOrEmpty(this.Label) || (this.SubRoutineReturnInfo != null));
            }
        }

        internal string Label { get; private set; }

        internal Stack<SubRoutineInfo> SubRoutineCallStack
        {
            get
            {
                return this.subRoutineCallStack;
            }
        }

        internal SubRoutineInfo SubRoutineReturnInfo { get; private set; }

        [CompilerGenerated]
        private sealed class <GetRandomJumpCommand>c__AnonStorey0
        {
            internal float sum;

            internal void <>m__0(AdvJumpManager.RandomInfo item)
            {
                this.sum += item.rate;
            }
        }

        private class RandomInfo
        {
            public AdvCommand command;
            public float rate;

            public RandomInfo(AdvCommand command, float rate)
            {
                this.command = command;
                this.rate = rate;
            }
        }
    }
}

