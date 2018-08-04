namespace Utage
{
    using System;
    using System.Runtime.InteropServices;

    [Serializable]
    public class DictionaryString : SerializableDictionaryBinaryIO<DictionaryKeyValueString>
    {
        public void Add(string key, string val)
        {
            base.Add(new DictionaryKeyValueString(key, val));
        }

        public string Get(string key)
        {
            return base.GetValue(key).value;
        }

        public bool TryGetValue(string key, out string val)
        {
            DictionaryKeyValueString str;
            if (base.TryGetValue(key, out str))
            {
                val = str.value;
                return true;
            }
            val = string.Empty;
            return false;
        }
    }
}

