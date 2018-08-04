namespace Utage
{
    using System;
    using UnityEngine;

    [Serializable]
    public class StaticAsset
    {
        [SerializeField]
        private Object asset;

        public Object Asset
        {
            get
            {
                return this.asset;
            }
        }
    }
}

