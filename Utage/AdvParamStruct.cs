namespace Utage
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using UnityEngine;

    public class AdvParamStruct
    {
        private Dictionary<string, AdvParamData> tbl;
        private const int Version = 0;

        public AdvParamStruct()
        {
            this.tbl = new Dictionary<string, AdvParamData>();
        }

        public AdvParamStruct(AdvParamStruct header, StringGridRow values)
        {
            this.tbl = new Dictionary<string, AdvParamData>();
            int index = 1;
            foreach (KeyValuePair<string, AdvParamData> pair in header.Tbl)
            {
                if (!pair.Value.Key.StartsWith("//"))
                {
                    string str = (index >= values.Length) ? string.Empty : values.Strings[index];
                    AdvParamData data = new AdvParamData();
                    if (!data.TryParse(pair.Value, str))
                    {
                        Debug.LogError(values.ToErrorString(" Parse Error <b>" + str + "</b>"));
                        continue;
                    }
                    this.Tbl.Add(data.Key, data);
                }
                index++;
            }
        }

        public AdvParamStruct(StringGridRow names, StringGridRow types, StringGridRow fileTypes)
        {
            this.tbl = new Dictionary<string, AdvParamData>();
            if (names.Length != types.Length)
            {
                Debug.LogError(names.Grid.Name + " Contains Cell in Name or Type");
            }
            else
            {
                for (int i = 1; i < names.Length; i++)
                {
                    AdvParamData data = new AdvParamData();
                    string fileType = (i >= fileTypes.Length) ? string.Empty : fileTypes.Strings[i];
                    if (!data.TryParse(names.Strings[i], types.Strings[i], fileType))
                    {
                        Debug.LogError(string.Format("{0} Header [<b>{1}</b>]: ", names.Grid.Name, i));
                    }
                    else
                    {
                        this.Tbl.Add(data.Key, data);
                    }
                }
            }
        }

        public void AddData(StringGrid grid)
        {
            foreach (StringGridRow row in grid.Rows)
            {
                if ((row.RowIndex >= grid.DataTopRow) && !row.IsEmptyOrCommantOut)
                {
                    AdvParamData data = new AdvParamData();
                    if (!data.TryParse(row))
                    {
                        Debug.LogError(row.ToErrorString(" Parse Error"));
                    }
                    else if (this.Tbl.ContainsKey(data.Key))
                    {
                        Debug.LogError(row.ToErrorString(data.Key + " is already contaisn"));
                    }
                    else
                    {
                        this.Tbl.Add(data.Key, data);
                    }
                }
            }
        }

        internal AdvParamStruct Clone()
        {
            AdvParamStruct struct2 = new AdvParamStruct();
            foreach (KeyValuePair<string, AdvParamData> pair in this.Tbl)
            {
                AdvParamData data = new AdvParamData();
                data.Copy(pair.Value);
                struct2.Tbl.Add(pair.Key, data);
            }
            return struct2;
        }

        public int CountFileType(AdvParamData.FileType fileType)
        {
            int num = 0;
            foreach (KeyValuePair<string, AdvParamData> pair in this.Tbl)
            {
                if (pair.Value.SaveFileType == fileType)
                {
                    num++;
                }
            }
            return num;
        }

        public List<AdvParamData> GetFileTypeList(AdvParamData.FileType fileType)
        {
            List<AdvParamData> list = new List<AdvParamData>();
            foreach (KeyValuePair<string, AdvParamData> pair in this.Tbl)
            {
                if (pair.Value.SaveFileType == fileType)
                {
                    list.Add(pair.Value);
                }
            }
            return list;
        }

        internal void InitDefaultNormal(AdvParamStruct src)
        {
            foreach (KeyValuePair<string, AdvParamData> pair in src.Tbl)
            {
                if (pair.Value.SaveFileType != AdvParamData.FileType.System)
                {
                    AdvParamData data;
                    if (this.Tbl.TryGetValue(pair.Key, out data))
                    {
                        data.Copy(pair.Value);
                    }
                    else
                    {
                        Debug.LogError("Param: " + pair.Key + "  is not found in default param");
                    }
                }
            }
        }

        public void Read(BinaryReader reader, AdvParamData.FileType fileType)
        {
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
                    AdvParamData data;
                    string key = reader.ReadString();
                    string paramString = reader.ReadString();
                    if (this.Tbl.TryGetValue(key, out data) && (data.SaveFileType == fileType))
                    {
                        data.Read(paramString);
                    }
                }
            }
        }

        public void Write(BinaryWriter writer, AdvParamData.FileType fileType)
        {
            int num = this.CountFileType(fileType);
            writer.Write(0);
            writer.Write(num);
            foreach (KeyValuePair<string, AdvParamData> pair in this.Tbl)
            {
                if (pair.Value.SaveFileType == fileType)
                {
                    writer.Write(pair.Value.Key);
                    writer.Write(pair.Value.ParameterString);
                }
            }
        }

        public Dictionary<string, AdvParamData> Tbl
        {
            get
            {
                return this.tbl;
            }
        }
    }
}

