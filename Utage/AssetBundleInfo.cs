namespace Utage
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class AssetBundleInfo
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Hash128 <Hash>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <Url>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <Version>k__BackingField;

        internal AssetBundleInfo(string url, int version)
        {
            this.Url = url;
            this.Version = version;
        }

        internal AssetBundleInfo(string url, Hash128 hash)
        {
            this.Url = url;
            this.Hash = hash;
            this.Version = -2147483648;
        }

        public Hash128 Hash { get; private set; }

        public string Url { get; private set; }

        public int Version { get; private set; }
    }
}

