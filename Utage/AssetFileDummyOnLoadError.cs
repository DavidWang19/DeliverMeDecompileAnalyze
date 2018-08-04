namespace Utage
{
    using System;
    using UnityEngine;

    [Serializable]
    public class AssetFileDummyOnLoadError
    {
        public Object asset;
        public bool isEnable;
        public bool outputErrorLog = true;
        public AudioClip sound;
        public TextAsset text;
        public Texture2D texture;
    }
}

