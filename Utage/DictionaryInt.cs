namespace Utage
{
    using System;
    using System.Runtime.InteropServices;

    [Serializable]
    public class DictionaryInt : SerializableDictionaryBinaryIO<DictionaryKeyValueInt>
    {
        public void Add(string key, int val)
        {
            base.Add(new DictionaryKeyValueInt(key, val));
        }

        public int Get(string key)
        {
            return base.GetValue(key).value;
        }

        public bool TryGetValue(string key, out int val)
        {
            DictionaryKeyValueInt num;
            if (base.TryGetValue(key, out num))
            {
                val = num.value;
                return true;
            }
            val = 0;
            return false;
        }
    }
}

