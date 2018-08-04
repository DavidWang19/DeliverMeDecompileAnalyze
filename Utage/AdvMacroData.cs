namespace Utage
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class AdvMacroData
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private List<StringGridRow> <DataList>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private StringGridRow <Header>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <Name>k__BackingField;

        public AdvMacroData(string name, StringGridRow header, List<StringGridRow> dataList)
        {
            this.Name = name;
            this.Header = header;
            this.DataList = dataList;
        }

        public List<StringGridRow> MacroExpansion(StringGridRow args, string debugMsg)
        {
            List<StringGridRow> list = new List<StringGridRow>();
            if (this.DataList.Count > 0)
            {
                for (int i = 0; i < this.DataList.Count; i++)
                {
                    StringGridRow row = this.DataList[i];
                    string[] strings = new string[args.Grid.ColumnIndexTbl.Count];
                    foreach (KeyValuePair<string, int> pair in args.Grid.ColumnIndexTbl)
                    {
                        string key = pair.Key;
                        int index = pair.Value;
                        strings[index] = this.ParaseMacroArg(row.ParseCellOptional<string>(key, string.Empty), args);
                    }
                    StringGridRow item = new StringGridRow(args.Grid, args.RowIndex);
                    item.InitFromStringArray(strings);
                    list.Add(item);
                    object[] objArray1 = new object[] { debugMsg, " : ", row.RowIndex + 1, " " };
                    item.DebugInfo = string.Concat(objArray1);
                }
            }
            return list;
        }

        private string ParaseMacroArg(string str, StringGridRow args)
        {
            int num = 0;
            string str2 = string.Empty;
            while (num < str.Length)
            {
                bool flag = false;
                if (str[num] == '%')
                {
                    foreach (string str3 in this.Header.Grid.ColumnIndexTbl.Keys)
                    {
                        if (str3.Length <= 0)
                        {
                            continue;
                        }
                        for (int i = 0; i < str3.Length; i++)
                        {
                            if (str3[i] != str[(num + 1) + i])
                            {
                                break;
                            }
                            if (i == (str3.Length - 1))
                            {
                                flag = true;
                            }
                        }
                        if (flag)
                        {
                            string defaultVal = this.Header.ParseCellOptional<string>(str3, string.Empty);
                            str2 = str2 + args.ParseCellOptional<string>(str3, defaultVal);
                            num += str3.Length;
                            break;
                        }
                    }
                }
                if (!flag)
                {
                    str2 = str2 + str[num];
                }
                num++;
            }
            return str2;
        }

        public List<StringGridRow> DataList { get; private set; }

        public StringGridRow Header { get; private set; }

        public string Name { get; private set; }
    }
}

