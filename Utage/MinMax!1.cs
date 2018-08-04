namespace Utage
{
    using System;
    using UnityEngine;

    public class MinMax<T>
    {
        [SerializeField]
        private T max;
        [SerializeField]
        private T min;

        public T Max
        {
            get
            {
                return this.max;
            }
            set
            {
                this.max = value;
            }
        }

        public T Min
        {
            get
            {
                return this.min;
            }
            set
            {
                this.min = value;
            }
        }
    }
}

