namespace Utage
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    [Serializable]
    public class StringGridDictionary : SerializableDictionary<StringGridDictionaryKeyValue>
    {
        public void Add(string key, StringGrid value)
        {
            base.Add(new StringGridDictionaryKeyValue(key, value));
        }

        public void RemoveSheets(string pattern)
        {
            List<string> list = new List<string>();
            foreach (string str in base.Keys)
            {
                if (Regex.IsMatch(str, pattern))
                {
                    list.Add(str);
                }
            }
            foreach (string str2 in list)
            {
                base.Remove(str2);
            }
        }
    }
}

