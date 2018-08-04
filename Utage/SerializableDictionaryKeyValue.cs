namespace Utage
{
    using System;
    using UnityEngine;

    [Serializable]
    public abstract class SerializableDictionaryKeyValue
    {
        [SerializeField]
        private string key;

        protected SerializableDictionaryKeyValue()
        {
        }

        public void InitKey(string key)
        {
            this.key = key;
        }

        public string Key
        {
            get
            {
                return this.key;
            }
        }
    }
}

