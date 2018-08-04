namespace Utage
{
    using System;
    using UnityEngine;

    [Serializable]
    public class StringGridDictionaryKeyValue : SerializableDictionaryKeyValue
    {
        [SerializeField]
        private StringGrid grid;

        public StringGridDictionaryKeyValue(string key, StringGrid grid)
        {
            base.InitKey(key);
            this.grid = grid;
        }

        public StringGrid Grid
        {
            get
            {
                return this.grid;
            }
        }

        public string Name
        {
            get
            {
                return base.Key;
            }
        }
    }
}

