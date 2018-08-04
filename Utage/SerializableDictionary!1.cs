namespace Utage
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;

    [Serializable]
    public class SerializableDictionary<T> where T: SerializableDictionaryKeyValue
    {
        protected Dictionary<string, T> dictionary;
        [SerializeField]
        private List<T> list;

        public SerializableDictionary()
        {
            this.dictionary = new Dictionary<string, T>();
        }

        public void Add(T val)
        {
            if (this.dictionary.ContainsKey(val.Key))
            {
                Debug.LogError("<color=red>" + val.Key + "</color>  is already contains");
            }
            this.InitDic();
            this.dictionary.Add(val.Key, val);
            this.List.Add(val);
        }

        public void Clear()
        {
            this.dictionary.Clear();
            this.List.Clear();
        }

        public bool ContainsKey(string key)
        {
            this.InitDic();
            return this.dictionary.ContainsKey(key);
        }

        public bool ContainsValue(T val)
        {
            this.InitDic();
            return this.dictionary.ContainsValue(val);
        }

        public T GetValue(string key)
        {
            this.InitDic();
            return this.dictionary[key];
        }

        private void InitDic()
        {
            if (this.dictionary.Count == 0)
            {
                this.RefreshDictionary();
            }
        }

        public void RefreshDictionary()
        {
            this.dictionary.Clear();
            foreach (T local in this.List)
            {
                this.dictionary.Add(local.Key, local);
            }
        }

        public bool Remove(string key)
        {
            <Remove>c__AnonStorey0<T> storey = new <Remove>c__AnonStorey0<T> {
                key = key
            };
            this.InitDic();
            bool flag = this.dictionary.Remove(storey.key);
            if (flag)
            {
                this.List.RemoveAll(new Predicate<T>(storey.<>m__0));
            }
            return flag;
        }

        public void Swap(int index0, int index1)
        {
            if (((index0 >= 0) && (this.Count > index0)) && ((index1 >= 0) && (this.Count > index1)))
            {
                T local = this.List[index0];
                this.List[index0] = this.List[index1];
                this.List[index1] = local;
                this.RefreshDictionary();
            }
        }

        public bool TryGetValue(string key, out T val)
        {
            this.InitDic();
            return this.dictionary.TryGetValue(key, out val);
        }

        public int Count
        {
            get
            {
                this.InitDic();
                return this.dictionary.Count;
            }
        }

        public Dictionary<string, T>.KeyCollection Keys
        {
            get
            {
                this.InitDic();
                return this.dictionary.Keys;
            }
        }

        public List<T> List
        {
            get
            {
                if (this.list == null)
                {
                }
                return (this.list = new List<T>());
            }
        }

        public Dictionary<string, T>.ValueCollection Values
        {
            get
            {
                this.InitDic();
                return this.dictionary.Values;
            }
        }

        [CompilerGenerated]
        private sealed class <Remove>c__AnonStorey0
        {
            internal string key;

            internal bool <>m__0(T x)
            {
                return (x.Key.CompareTo(this.key) == 0);
            }
        }
    }
}

