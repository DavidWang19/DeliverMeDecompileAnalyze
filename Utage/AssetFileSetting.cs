namespace Utage
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    [Serializable]
    public class AssetFileSetting
    {
        [SerializeField]
        private List<string> extensions;
        [SerializeField, HideInInspector]
        private AssetFileType fileType;
        [SerializeField]
        private bool isStreamingAssets;
        [NonSerialized]
        private AssetFileManagerSettings settings;

        public AssetFileSetting(AssetFileType fileType, string[] extensions)
        {
            this.fileType = fileType;
            this.extensions = new List<string>(extensions);
        }

        public void AddExtensions(string[] extensions)
        {
            this.extensions.AddRange(extensions);
        }

        internal bool ContainsExtensions(string path)
        {
            string item = FilePathUtil.GetExtensionWithOutDouble(path, ".utage").ToLower();
            return this.extensions.Contains(item);
        }

        public void InitLink(AssetFileManagerSettings settings)
        {
            this.settings = settings;
        }

        public AssetFileType FileType
        {
            get
            {
                return this.fileType;
            }
        }

        public bool IsStreamingAssets
        {
            get
            {
                switch (this.LoadType)
                {
                    case Utage.AssetFileManagerSettings.LoadType.Local:
                    case Utage.AssetFileManagerSettings.LoadType.Server:
                        return false;

                    case Utage.AssetFileManagerSettings.LoadType.StreamingAssets:
                        return true;
                }
                return this.isStreamingAssets;
            }
            set
            {
                this.isStreamingAssets = value;
            }
        }

        public Utage.AssetFileManagerSettings.LoadType LoadType
        {
            get
            {
                return this.Settings.LoadTypeSetting;
            }
        }

        private AssetFileManagerSettings Settings
        {
            get
            {
                return this.settings;
            }
        }
    }
}

