namespace Utage
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UtageExtensions;

    public class AdvParamStructTbl
    {
        private Dictionary<string, AdvParamStruct> tbl = new Dictionary<string, AdvParamStruct>();
        private const int Version = 0;

        public void AddSingle(StringGrid grid)
        {
            AdvParamStruct struct2;
            if (!this.Tbl.TryGetValue(string.Empty, out struct2))
            {
                struct2 = new AdvParamStruct();
                this.Tbl.Add(string.Empty, struct2);
            }
            struct2.AddData(grid);
        }

        public void AddTbl(StringGrid grid)
        {
            if (grid.Rows.Count < 3)
            {
                Debug.LogError(grid.Name + " is not Param Sheet");
            }
            else
            {
                StringGridRow names = grid.Rows[0];
                StringGridRow types = grid.Rows[1];
                StringGridRow fileTypes = grid.Rows[2];
                AdvParamStruct header = new AdvParamStruct(names, types, fileTypes);
                for (int i = 3; i < grid.Rows.Count; i++)
                {
                    StringGridRow values = grid.Rows[i];
                    if (!values.IsEmptyOrCommantOut)
                    {
                        AdvParamStruct struct3 = new AdvParamStruct(header, values);
                        string key = values.Strings[0];
                        if (this.Tbl.ContainsKey(key))
                        {
                            values.ToErrorString(key + " is already contains ");
                        }
                        else
                        {
                            this.Tbl.Add(key, struct3);
                        }
                    }
                }
            }
        }

        internal AdvParamStructTbl Clone()
        {
            AdvParamStructTbl tbl = new AdvParamStructTbl();
            foreach (KeyValuePair<string, AdvParamStruct> pair in this.Tbl)
            {
                tbl.Tbl.Add(pair.Key, pair.Value.Clone());
            }
            return tbl;
        }

        internal void InitDefaultNormal(AdvParamStructTbl src)
        {
            foreach (KeyValuePair<string, AdvParamStruct> pair in src.Tbl)
            {
                AdvParamStruct struct2;
                if (this.Tbl.TryGetValue(pair.Key, out struct2))
                {
                    struct2.InitDefaultNormal(pair.Value);
                }
                else
                {
                    Debug.LogError("Param: " + pair.Key + "  is not found in default param");
                }
            }
        }

        public void Read(BinaryReader reader, AdvParamData.FileType fileType)
        {
            <Read>c__AnonStorey3 storey = new <Read>c__AnonStorey3 {
                fileType = fileType,
                $this = this
            };
            int num = reader.ReadInt32();
            if ((num < 0) || (num > 0))
            {
                object[] args = new object[] { num };
                Debug.LogError(LanguageErrorMsg.LocalizeTextFormat(ErrorMsg.UnknownVersion, args));
            }
            else
            {
                int num2 = reader.ReadInt32();
                for (int i = 0; i < num2; i++)
                {
                    <Read>c__AnonStorey2 storey2 = new <Read>c__AnonStorey2 {
                        <>f__ref$3 = storey,
                        key = reader.ReadString()
                    };
                    if (this.Tbl.ContainsKey(storey2.key))
                    {
                        reader.ReadBuffer(new Action<BinaryReader>(storey2.<>m__0));
                    }
                    else
                    {
                        reader.SkipBuffer();
                    }
                }
            }
        }

        public void Write(BinaryWriter writer, AdvParamData.FileType fileType)
        {
            <Write>c__AnonStorey1 storey = new <Write>c__AnonStorey1 {
                fileType = fileType
            };
            writer.Write(0);
            writer.Write(this.Tbl.Count);
            using (Dictionary<string, AdvParamStruct>.Enumerator enumerator = this.Tbl.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    <Write>c__AnonStorey0 storey2 = new <Write>c__AnonStorey0 {
                        <>f__ref$1 = storey,
                        keyValue = enumerator.Current
                    };
                    writer.Write(storey2.keyValue.Key);
                    writer.WriteBuffer(new Action<BinaryWriter>(storey2.<>m__0));
                }
            }
        }

        public Dictionary<string, AdvParamStruct> Tbl
        {
            get
            {
                return this.tbl;
            }
        }

        [CompilerGenerated]
        private sealed class <Read>c__AnonStorey2
        {
            internal AdvParamStructTbl.<Read>c__AnonStorey3 <>f__ref$3;
            internal string key;

            internal void <>m__0(BinaryReader x)
            {
                this.<>f__ref$3.$this.Tbl[this.key].Read(x, this.<>f__ref$3.fileType);
            }
        }

        [CompilerGenerated]
        private sealed class <Read>c__AnonStorey3
        {
            internal AdvParamStructTbl $this;
            internal AdvParamData.FileType fileType;
        }

        [CompilerGenerated]
        private sealed class <Write>c__AnonStorey0
        {
            internal AdvParamStructTbl.<Write>c__AnonStorey1 <>f__ref$1;
            internal KeyValuePair<string, AdvParamStruct> keyValue;

            internal void <>m__0(BinaryWriter x)
            {
                this.keyValue.Value.Write(x, this.<>f__ref$1.fileType);
            }
        }

        [CompilerGenerated]
        private sealed class <Write>c__AnonStorey1
        {
            internal AdvParamData.FileType fileType;
        }
    }
}

