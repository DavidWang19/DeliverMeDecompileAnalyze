namespace Utage
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class LanguageData
    {
        private Dictionary<string, LanguageStrings> dataTbl = new Dictionary<string, LanguageStrings>();
        private List<string> languages = new List<string>();

        public bool ContainsKey(string key)
        {
            return this.dataTbl.ContainsKey(key);
        }

        internal void OverwriteData(TextAsset tsv)
        {
            this.OverwriteData(new StringGrid(tsv.get_name(), CsvType.Tsv, tsv.get_text()));
        }

        internal void OverwriteData(StringGrid grid)
        {
            Dictionary<int, int> dictionary = new Dictionary<int, int>();
            StringGridRow row = grid.Rows[0];
            for (int i = 0; i < row.Length; i++)
            {
                if (i != 0)
                {
                    string str = row.Strings[i];
                    if (!string.IsNullOrEmpty(str))
                    {
                        if (!this.languages.Contains(str))
                        {
                            this.languages.Add(str);
                        }
                        int index = this.languages.IndexOf(str);
                        if (dictionary.ContainsKey(index))
                        {
                            Debug.LogError(str + " already exists in  " + grid.Name);
                        }
                        else
                        {
                            dictionary.Add(index, i);
                        }
                    }
                }
            }
            foreach (StringGridRow row2 in grid.Rows)
            {
                if (!row2.IsEmptyOrCommantOut && (row2.RowIndex != 0))
                {
                    string str2 = row2.Strings[0];
                    if (!string.IsNullOrEmpty(str2))
                    {
                        if (!this.dataTbl.ContainsKey(str2))
                        {
                            this.dataTbl.Add(str2, new LanguageStrings());
                        }
                        int count = this.languages.Count;
                        List<string> strings = new List<string>(count);
                        for (int j = 0; j < count; j++)
                        {
                            string item = string.Empty;
                            if (dictionary.ContainsKey(j))
                            {
                                int num5 = dictionary[j];
                                if (num5 < row2.Strings.Length)
                                {
                                    item = row2.Strings[num5].Replace(@"\n", "\n");
                                }
                            }
                            strings.Add(item);
                        }
                        this.dataTbl[str2].SetData(strings);
                    }
                }
            }
        }

        internal bool TryLocalizeText(out string text, string CurrentLanguage, string DefaultLanguage, string key, string dataName = "")
        {
            text = key;
            if (!this.ContainsKey(key))
            {
                Debug.LogError(key + ": is not found in Language data");
                return false;
            }
            string item = CurrentLanguage;
            if (!this.Languages.Contains(CurrentLanguage))
            {
                if (!this.Languages.Contains(DefaultLanguage))
                {
                    return false;
                }
                item = DefaultLanguage;
            }
            int index = this.Languages.IndexOf(item);
            LanguageStrings strings = this.dataTbl[key];
            if (index >= strings.Strings.Count)
            {
                return false;
            }
            text = strings.Strings[index];
            if (text == string.Empty)
            {
                text = strings.Strings[2];
            }
            return true;
        }

        public List<string> Languages
        {
            get
            {
                return this.languages;
            }
        }

        public class LanguageStrings
        {
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private List<string> <Strings>k__BackingField;

            public LanguageStrings()
            {
                this.Strings = new List<string>();
            }

            internal void SetData(List<string> strings)
            {
                this.Strings = strings;
            }

            public List<string> Strings { get; private set; }
        }
    }
}

