namespace Utage
{
    using System;
    using System.Runtime.InteropServices;

    [Serializable]
    public class DictionaryFloat : SerializableDictionaryBinaryIO<DictionaryKeyValueFloat>
    {
        public void Add(string key, float val)
        {
            base.Add(new DictionaryKeyValueFloat(key, val));
        }

        public float Get(string key)
        {
            return base.GetValue(key).value;
        }

        public bool TryGetValue(string key, out float val)
        {
            DictionaryKeyValueFloat num;
            if (base.TryGetValue(key, out num))
            {
                val = num.value;
                return true;
            }
            val = 0f;
            return false;
        }
    }
}

