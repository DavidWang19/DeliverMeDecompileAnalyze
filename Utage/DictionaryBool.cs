namespace Utage
{
    using System;
    using System.Runtime.InteropServices;

    [Serializable]
    public class DictionaryBool : SerializableDictionaryBinaryIO<DictionaryKeyValueBool>
    {
        public void Add(string key, bool val)
        {
            base.Add(new DictionaryKeyValueBool(key, val));
        }

        public bool Get(string key)
        {
            return base.GetValue(key).value;
        }

        public bool TryGetValue(string key, out bool val)
        {
            DictionaryKeyValueBool @bool;
            if (base.TryGetValue(key, out @bool))
            {
                val = @bool.value;
                return true;
            }
            val = false;
            return false;
        }
    }
}

