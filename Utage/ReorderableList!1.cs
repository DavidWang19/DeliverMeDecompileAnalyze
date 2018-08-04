namespace Utage
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    [Serializable]
    public class ReorderableList<T>
    {
        [SerializeField]
        private List<T> list;

        public ReorderableList()
        {
            this.list = new List<T>();
        }

        public List<T> List
        {
            get
            {
                return this.list;
            }
        }
    }
}

