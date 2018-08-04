namespace Utage
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class AdvSelectedHistorySaveData : IBinaryIO
    {
        private List<AdvSelectedHistoryData> dataList = new List<AdvSelectedHistoryData>();
        private const string Ignore = "Alaways";
        private const int VERSION = 0;

        public void AddData(AdvSelection selection)
        {
            if ((selection.Label != "Alaways") && !this.Check(selection))
            {
                this.dataList.Add(new AdvSelectedHistoryData(selection));
            }
        }

        public bool Check(AdvSelection selection)
        {
            <Check>c__AnonStorey0 storey = new <Check>c__AnonStorey0 {
                selection = selection
            };
            if (storey.selection.Label == "Alaways")
            {
                return false;
            }
            return (this.dataList.Find(new Predicate<AdvSelectedHistoryData>(storey.<>m__0)) != null);
        }

        public void OnRead(BinaryReader reader)
        {
            int version = reader.ReadInt32();
            if (version == 0)
            {
                this.dataList.Clear();
                int num2 = reader.ReadInt32();
                for (int i = 0; i < num2; i++)
                {
                    this.dataList.Add(new AdvSelectedHistoryData(reader, version));
                }
            }
            else
            {
                object[] args = new object[] { version };
                Debug.LogError(LanguageErrorMsg.LocalizeTextFormat(ErrorMsg.UnknownVersion, args));
            }
        }

        public void OnWrite(BinaryWriter writer)
        {
            writer.Write(0);
            writer.Write(this.dataList.Count);
            foreach (AdvSelectedHistoryData data in this.dataList)
            {
                data.Write(writer);
            }
        }

        public string SaveKey
        {
            get
            {
                return "AdvSelectedHistorySaveData";
            }
        }

        [CompilerGenerated]
        private sealed class <Check>c__AnonStorey0
        {
            internal AdvSelection selection;

            internal bool <>m__0(AdvSelectedHistorySaveData.AdvSelectedHistoryData x)
            {
                return x.Check(this.selection);
            }
        }

        private class AdvSelectedHistoryData
        {
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private string <JumpLabel>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private string <Label>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private string <Text>k__BackingField;

            public AdvSelectedHistoryData(AdvSelection selection)
            {
                this.Label = selection.Label;
                this.Text = selection.Text;
                this.JumpLabel = selection.JumpLabel;
            }

            public AdvSelectedHistoryData(BinaryReader reader, int version)
            {
                if (version == 0)
                {
                    this.Label = reader.ReadString();
                    this.Text = reader.ReadString();
                    this.JumpLabel = reader.ReadString();
                }
                else
                {
                    object[] args = new object[] { version };
                    Debug.LogError(LanguageErrorMsg.LocalizeTextFormat(ErrorMsg.UnknownVersion, args));
                }
            }

            public bool Check(AdvSelection selection)
            {
                if (!string.IsNullOrEmpty(this.Label) || !string.IsNullOrEmpty(selection.Label))
                {
                    return (this.Label == selection.Label);
                }
                return ((this.Text == selection.Text) && (this.JumpLabel == selection.JumpLabel));
            }

            public void Write(BinaryWriter writer)
            {
                writer.Write(this.Label);
                writer.Write(this.Text);
                writer.Write(this.JumpLabel);
            }

            private string JumpLabel { get; set; }

            private string Label { get; set; }

            private string Text { get; set; }
        }
    }
}

