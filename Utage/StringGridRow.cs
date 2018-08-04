namespace Utage
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Runtime.InteropServices;
    using UnityEngine;

    [Serializable]
    public class StringGridRow
    {
        [NonSerialized]
        private int debugIndex = -1;
        [NonSerialized]
        private string debugInfo;
        [NonSerialized]
        private StringGrid grid;
        [SerializeField]
        private bool isCommentOut;
        [SerializeField]
        private bool isEmpty;
        [SerializeField]
        private int rowIndex;
        [SerializeField]
        private string[] strings;

        public StringGridRow(StringGrid gird, int rowIndex)
        {
            int num = rowIndex;
            this.DebugIndex = num;
            this.rowIndex = num;
            this.InitLink(gird);
        }

        private bool CheckCommentOut()
        {
            if (this.Strings.Length <= 0)
            {
                return false;
            }
            return this.Strings[0].StartsWith("//");
        }

        private bool CheckEmpty()
        {
            foreach (string str in this.strings)
            {
                if (!string.IsNullOrEmpty(str))
                {
                    return false;
                }
            }
            return true;
        }

        public void InitFromCsvText(CsvType type, string text)
        {
            char[] separator = new char[] { (type != CsvType.Tsv) ? ',' : '\t' };
            this.strings = text.Split(separator);
            this.isEmpty = this.CheckEmpty();
            this.isCommentOut = this.CheckCommentOut();
        }

        public void InitFromStringArray(string[] strings)
        {
            this.strings = strings;
            this.isEmpty = this.CheckEmpty();
            this.isCommentOut = this.CheckCommentOut();
        }

        public void InitFromStringList(List<string> stringList)
        {
            this.InitFromStringArray(stringList.ToArray());
        }

        public void InitLink(StringGrid grid)
        {
            this.grid = grid;
        }

        internal bool IsAllEmptyCellNamedColumn()
        {
            foreach (KeyValuePair<string, int> pair in this.Grid.ColumnIndexTbl)
            {
                if (!this.IsEmptyCell(pair.Value))
                {
                    return false;
                }
            }
            return true;
        }

        public bool IsEmptyCell(int index)
        {
            return ((index >= this.Length) || string.IsNullOrEmpty(this.strings[index]));
        }

        public bool IsEmptyCell(string columnName)
        {
            int num;
            if (this.Grid.TryGetColumnIndex(columnName, out num))
            {
                return this.IsEmptyCell(num);
            }
            return true;
        }

        public T ParseCell<T>(int index)
        {
            T local;
            if (!this.TryParseCell<T>(index, out local))
            {
                Debug.LogError(this.ToErrorStringWithPraseColumnIndex(index));
            }
            return local;
        }

        public T ParseCell<T>(string columnName)
        {
            T local;
            if (!this.TryParseCell<T>(columnName, out local))
            {
                Debug.LogError(this.ToErrorStringWithPraseColumnName(columnName));
            }
            return local;
        }

        public T[] ParseCellArray<T>(int index)
        {
            T[] localArray;
            if (!this.TryParseCellArray<T>(index, out localArray))
            {
                Debug.LogError(this.ToErrorStringWithPraseColumnIndex(index));
            }
            return localArray;
        }

        public T[] ParseCellArray<T>(string columnName)
        {
            T[] localArray;
            if (!this.TryParseCellArray<T>(columnName, out localArray))
            {
                Debug.LogError(this.ToErrorStringWithPraseColumnName(columnName));
            }
            return localArray;
        }

        public T ParseCellOptional<T>(int index, T defaultVal)
        {
            T local;
            return (!this.TryParseCell<T>(index, out local) ? defaultVal : local);
        }

        public T ParseCellOptional<T>(string columnName, T defaultVal)
        {
            T local;
            return (!this.TryParseCell<T>(columnName, out local) ? defaultVal : local);
        }

        public T[] ParseCellOptionalArray<T>(int index, T[] defaultVal)
        {
            T[] localArray;
            return (!this.TryParseCellArray<T>(index, out localArray) ? defaultVal : localArray);
        }

        public T[] ParseCellOptionalArray<T>(string columnName, T[] defaultVal)
        {
            T[] localArray;
            return (!this.TryParseCellArray<T>(columnName, out localArray) ? defaultVal : localArray);
        }

        internal string ToDebugString()
        {
            char csvSeparator = this.Grid.CsvSeparator;
            string str = string.Empty;
            foreach (string str2 in this.strings)
            {
                string str3 = str;
                object[] objArray1 = new object[] { str3, " ", str2, csvSeparator };
                str = string.Concat(objArray1);
            }
            return str;
        }

        public string ToErrorString(string msg)
        {
            if (!msg.EndsWith("\n"))
            {
                msg = msg + "\n";
            }
            int num = this.DebugIndex + 1;
            if (string.IsNullOrEmpty(this.DebugInfo))
            {
                string sheetName = this.Grid.SheetName;
                string str2 = msg;
                object[] objArray1 = new object[] { str2, sheetName, ":", num, " " };
                msg = string.Concat(objArray1);
            }
            else
            {
                msg = msg + this.DebugInfo;
            }
            object[] objArray2 = new object[] { msg, ColorUtil.AddColorTag(this.ToDebugString(), Color.get_red()), "\n<b>", this.Grid.Name, "</b>  : ", num };
            return string.Concat(objArray2);
        }

        private string ToErrorStringWithPrase(string column, int index)
        {
            object[] args = new object[] { index, column };
            return this.ToErrorString(LanguageErrorMsg.LocalizeTextFormat(ErrorMsg.StringGridRowPrase, args));
        }

        private string ToErrorStringWithPraseColumnIndex(int index)
        {
            object[] args = new object[] { index };
            return this.ToErrorString(LanguageErrorMsg.LocalizeTextFormat(ErrorMsg.StringGridRowPraseColumnIndex, args));
        }

        private string ToErrorStringWithPraseColumnName(string columnName)
        {
            object[] args = new object[] { columnName };
            return this.ToErrorString(LanguageErrorMsg.LocalizeTextFormat(ErrorMsg.StringGridRowPraseColumnName, args));
        }

        public string ToStringOfFileSheetLine()
        {
            int num = this.rowIndex + 1;
            object[] objArray1 = new object[] { "<b>", this.Grid.Name, "</b>  : ", num };
            return string.Concat(objArray1);
        }

        public static bool TryParse<T>(string str, out T val)
        {
            try
            {
                Type type = typeof(T);
                if (type == typeof(string))
                {
                    val = (T) str;
                }
                else if (type.IsEnum)
                {
                    val = (T) Enum.Parse(typeof(T), str);
                }
                else
                {
                    if (type == typeof(Color))
                    {
                        Color color = Color.get_white();
                        bool flag = ColorUtil.TryParseColor(str, ref color);
                        val = !flag ? default(T) : ((T) color);
                        return flag;
                    }
                    if (type == typeof(int))
                    {
                        val = (T) int.Parse(str);
                    }
                    else if (type == typeof(float))
                    {
                        val = (T) float.Parse(str);
                    }
                    else if (type == typeof(double))
                    {
                        val = (T) double.Parse(str);
                    }
                    else if (type == typeof(bool))
                    {
                        val = (T) bool.Parse(str);
                    }
                    else
                    {
                        TypeConverter converter = TypeDescriptor.GetConverter(type);
                        val = (T) converter.ConvertFromString(str);
                    }
                }
                return true;
            }
            catch
            {
                val = default(T);
                return false;
            }
        }

        private bool TryParseArray<T>(string str, out T[] val)
        {
            char[] separator = new char[] { ',' };
            string[] strArray = str.Split(separator);
            int length = strArray.Length;
            val = new T[length];
            for (int i = 0; i < length; i++)
            {
                T local;
                if (!TryParse<T>(strArray[i], out local))
                {
                    return false;
                }
                val[i] = local;
            }
            return true;
        }

        public bool TryParseCell<T>(int index, out T val)
        {
            if (!this.IsEmptyCell(index))
            {
                if (TryParse<T>(this.strings[index], out val))
                {
                    return true;
                }
                Debug.LogError(this.ToErrorStringWithPrase(this.strings[index], index));
                return false;
            }
            val = default(T);
            return false;
        }

        public bool TryParseCell<T>(string columnName, out T val)
        {
            int num;
            if (this.Grid.TryGetColumnIndex(columnName, out num))
            {
                return this.TryParseCell<T>(num, out val);
            }
            val = default(T);
            return false;
        }

        public bool TryParseCellArray<T>(int index, out T[] val)
        {
            if (!this.IsEmptyCell(index))
            {
                if (this.TryParseArray<T>(this.strings[index], out val))
                {
                    return true;
                }
                Debug.LogError(this.ToErrorStringWithPrase(this.strings[index], index));
                return false;
            }
            val = null;
            return false;
        }

        public bool TryParseCellArray<T>(string columnName, out T[] val)
        {
            int num;
            if (this.Grid.TryGetColumnIndex(columnName, out num))
            {
                return this.TryParseCellArray<T>(num, out val);
            }
            val = null;
            return false;
        }

        public bool TryParseCellTypeOptional<T>(int index, T defaultVal, out T val)
        {
            if (!this.IsEmptyCell(index))
            {
                if (TryParse<T>(this.strings[index], out val))
                {
                    return true;
                }
                val = defaultVal;
                return false;
            }
            val = defaultVal;
            return false;
        }

        public int DebugIndex
        {
            get
            {
                return this.debugIndex;
            }
            set
            {
                this.debugIndex = value;
            }
        }

        internal string DebugInfo
        {
            get
            {
                return this.debugInfo;
            }
            set
            {
                this.debugInfo = value;
            }
        }

        public StringGrid Grid
        {
            get
            {
                return this.grid;
            }
        }

        public bool IsCommentOut
        {
            get
            {
                return this.isCommentOut;
            }
        }

        public bool IsEmpty
        {
            get
            {
                return this.isEmpty;
            }
        }

        public bool IsEmptyOrCommantOut
        {
            get
            {
                return (this.IsEmpty || this.IsCommentOut);
            }
        }

        public int Length
        {
            get
            {
                return this.strings.Length;
            }
        }

        public int RowIndex
        {
            get
            {
                return this.rowIndex;
            }
        }

        public string[] Strings
        {
            get
            {
                return this.strings;
            }
        }
    }
}

