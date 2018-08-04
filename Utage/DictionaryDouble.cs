namespace Utage
{
    using System;
    using System.Runtime.InteropServices;

    [Serializable]
    public class DictionaryDouble : SerializableDictionaryBinaryIO<DictionaryKeyValueDouble>
    {
        public void Add(string key, double val)
        {
            base.Add(new DictionaryKeyValueDouble(key, val));
        }

        public double Get(string key)
        {
            return base.GetValue(key).value;
        }

        public bool TryGetValue(string key, out double val)
        {
            DictionaryKeyValueDouble num;
            if (base.TryGetValue(key, out num))
            {
                val = num.value;
                return true;
            }
            val = 0.0;
            return false;
        }
    }
}

