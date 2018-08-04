namespace Utage
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class SubRoutineInfo
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <CalledLabel>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <CalledSubroutineCommandIndex>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <JumpLabel>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private AdvCommand <ReturnCommand>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <ReturnLabel>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <ReturnPageNo>k__BackingField;
        private BinaryReader reader;
        private const int Version = 0;

        public SubRoutineInfo(AdvEngine engine, BinaryReader reader)
        {
            int num = reader.ReadInt32();
            if (num == 0)
            {
                this.JumpLabel = reader.ReadString();
                this.CalledLabel = reader.ReadString();
                this.CalledSubroutineCommandIndex = reader.ReadInt32();
                this.InitReturnInfo(engine);
            }
            else
            {
                object[] args = new object[] { num };
                Debug.LogError(LanguageErrorMsg.LocalizeTextFormat(ErrorMsg.UnknownVersion, args));
            }
        }

        public SubRoutineInfo(AdvEngine engine, string jumpLabel, string calledLabel, int calledSubroutineCommandIndex)
        {
            this.JumpLabel = jumpLabel;
            this.CalledLabel = calledLabel;
            this.CalledSubroutineCommandIndex = calledSubroutineCommandIndex;
            this.InitReturnInfo(engine);
        }

        private void InitReturnInfo(AdvEngine engine)
        {
            if (!string.IsNullOrEmpty(this.JumpLabel))
            {
                this.ReturnLabel = this.JumpLabel;
                this.ReturnPageNo = 0;
                this.ReturnCommand = null;
            }
            else
            {
                engine.DataManager.SetSubroutineRetunInfo(this.CalledLabel, this.CalledSubroutineCommandIndex, this);
            }
        }

        internal void Write(BinaryWriter writer)
        {
            writer.Write(0);
            writer.Write(this.JumpLabel);
            writer.Write(this.CalledLabel);
            writer.Write(this.CalledSubroutineCommandIndex);
        }

        internal string CalledLabel { get; private set; }

        internal int CalledSubroutineCommandIndex { get; private set; }

        internal string JumpLabel { get; private set; }

        public AdvCommand ReturnCommand { get; set; }

        public string ReturnLabel { get; set; }

        public int ReturnPageNo { get; set; }
    }
}

