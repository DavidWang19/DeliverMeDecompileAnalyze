namespace Utage
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using System.Text;
    using UnityEngine;

    [Serializable]
    public class StringGrid
    {
        private Dictionary<string, int> columnIndexTbl;
        [SerializeField]
        protected int headerRow;
        [SerializeField]
        private string name;
        [SerializeField]
        private List<StringGridRow> rows;
        private string sheetName;
        [SerializeField]
        private int textLength;
        [SerializeField]
        private CsvType type;

        public StringGrid(string name, string sheetName, CsvType type)
        {
            this.name = name;
            this.sheetName = sheetName;
            this.type = type;
        }

        public StringGrid(string name, CsvType type, string csvText)
        {
            this.Create(name, type, csvText, 0);
        }

        public StringGrid(string name, CsvType type, string csvText, int headerRow)
        {
            this.Create(name, type, csvText, headerRow);
        }

        public void AddRow(List<string> stringList)
        {
            StringGridRow item = new StringGridRow(this, this.Rows.Count);
            item.InitFromStringList(stringList);
            this.Rows.Add(item);
            foreach (string str in stringList)
            {
                this.textLength += str.Length;
            }
        }

        public StringGridRow AddRow(string[] stringArray)
        {
            StringGridRow item = new StringGridRow(this, this.Rows.Count);
            item.InitFromStringArray(stringArray);
            this.Rows.Add(item);
            foreach (string str in stringArray)
            {
                this.textLength += str.Length;
            }
            return item;
        }

        public bool ContainsColumn(string name)
        {
            return this.ColumnIndexTbl.ContainsKey(name);
        }

        private void Create(string name, CsvType type, string csvText, int headerRow)
        {
            this.name = name;
            this.type = type;
            this.Rows.Clear();
            char[] separator = new char[] { "\n"[0] };
            string[] strArray = csvText.Split(separator);
            for (int i = 0; i < strArray.Length; i++)
            {
                StringGridRow item = new StringGridRow(this, this.Rows.Count);
                item.InitFromCsvText(type, strArray[i]);
                this.Rows.Add(item);
            }
            this.ParseHeader(headerRow);
            this.textLength = csvText.Length;
        }

        public int GetColumnIndex(string name)
        {
            int num;
            if (this.TryGetColumnIndex(name, out num))
            {
                return num;
            }
            object[] args = new object[] { name, this.name };
            Debug.LogError(LanguageErrorMsg.LocalizeTextFormat(ErrorMsg.StringGridGetColumnIndex, args));
            return 0;
        }

        public void InitLink()
        {
            foreach (StringGridRow row in this.Rows)
            {
                row.InitLink(this);
            }
            this.ParseHeader(this.headerRow);
        }

        public void ParseHeader()
        {
            this.ParseHeader(0);
        }

        public void ParseHeader(int headerRow)
        {
            this.headerRow = headerRow;
            this.ColumnIndexTbl = new Dictionary<string, int>();
            if (headerRow < this.Rows.Count)
            {
                StringGridRow row = this.Rows[headerRow];
                for (int i = 0; i < row.Strings.Length; i++)
                {
                    string key = row.Strings[i];
                    if (this.ColumnIndexTbl.ContainsKey(key))
                    {
                        string str2 = string.Empty;
                        if (!string.IsNullOrEmpty(key))
                        {
                            Debug.LogError(str2 + row.ToErrorString(ColorUtil.AddColorTag(key, Color.get_red()) + "  is already contains"));
                        }
                    }
                    else
                    {
                        this.ColumnIndexTbl.Add(key, i);
                    }
                }
            }
            else
            {
                object[] args = new object[] { headerRow, this.name };
                Debug.LogError(LanguageErrorMsg.LocalizeTextFormat(ErrorMsg.StringGridParseHaeder, args));
            }
        }

        public string ToText()
        {
            StringBuilder builder = new StringBuilder();
            char csvSeparator = this.CsvSeparator;
            foreach (StringGridRow row in this.Rows)
            {
                for (int i = 0; i < row.Strings.Length; i++)
                {
                    string str = row.Strings[i].Replace("\n", @"\n");
                    builder.Append(str);
                    if (i < (row.Strings.Length - 1))
                    {
                        builder.Append(csvSeparator);
                    }
                }
                builder.Append("\n");
            }
            return builder.ToString();
        }

        public bool TryGetColumnIndex(string name, out int index)
        {
            return this.ColumnIndexTbl.TryGetValue(name, out index);
        }

        public Dictionary<string, int> ColumnIndexTbl
        {
            get
            {
                return this.columnIndexTbl;
            }
            set
            {
                this.columnIndexTbl = value;
            }
        }

        public char CsvSeparator
        {
            get
            {
                return ((this.Type != CsvType.Csv) ? '\t' : ',');
            }
        }

        public int DataTopRow
        {
            get
            {
                return (this.HeaderRow + 1);
            }
        }

        public int HeaderRow
        {
            get
            {
                return this.headerRow;
            }
        }

        public string Name
        {
            get
            {
                return this.name;
            }
        }

        public List<StringGridRow> Rows
        {
            get
            {
                if (this.rows == null)
                {
                }
                return (this.rows = new List<StringGridRow>());
            }
        }

        public string SheetName
        {
            get
            {
                if (string.IsNullOrEmpty(this.sheetName))
                {
                    int num = this.Name.LastIndexOf(":");
                    this.sheetName = this.Name;
                    if (num > 0)
                    {
                        this.sheetName = this.sheetName.Remove(0, num + 1);
                    }
                    if (this.sheetName.Contains("."))
                    {
                        this.sheetName = FilePathUtil.GetFileNameWithoutDoubleExtension(this.Name).Replace("%7B", "{").Replace("%7D", "}");
                    }
                }
                return this.sheetName;
            }
        }

        public int TextLength
        {
            get
            {
                return this.textLength;
            }
        }

        public CsvType Type
        {
            get
            {
                return this.type;
            }
        }
    }
}

