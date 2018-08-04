namespace Utage
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    [Serializable]
    public class AssetFileManagerSettings
    {
        [SerializeField]
        private List<AssetFileSetting> fileSettings;
        [SerializeField]
        private LoadType loadType;
        [NonSerialized]
        private List<AssetFileSetting> rebuildFileSettings;

        public AssetFileManagerSettings()
        {
            List<AssetFileSetting> list = new List<AssetFileSetting>();
            string[] extensions = new string[] { ".txt", ".json", ".html", ".htm", ".xml", ".yaml", ".fnt", ".bin", ".bytes", ".csv", ".tsv" };
            list.Add(new AssetFileSetting(AssetFileType.Text, extensions));
            string[] textArray2 = new string[] { ".png", ".jpg", ".bmp", ".psd", ".tif", ".tga", ".gif", ".iff", ".pict" };
            list.Add(new AssetFileSetting(AssetFileType.Texture, textArray2));
            string[] textArray3 = new string[] { ".mp3", ".ogg", ".wav", ".aif", ".aiff", ".xm", ".mod", ".it", ".s3m" };
            list.Add(new AssetFileSetting(AssetFileType.Sound, textArray3));
            string[] textArray4 = new string[] { string.Empty };
            list.Add(new AssetFileSetting(AssetFileType.UnityObject, textArray4));
            this.fileSettings = list;
        }

        public void AddExtensions(AssetFileType type, string[] extensions)
        {
            this.Find(type).AddExtensions(extensions);
        }

        public void BootInit(LoadType loadType)
        {
            this.loadType = loadType;
            foreach (AssetFileSetting setting in this.FileSettings)
            {
                setting.InitLink(this);
            }
        }

        private List<AssetFileSetting> DefaultFileSettings()
        {
            List<AssetFileSetting> list = new List<AssetFileSetting>();
            string[] extensions = new string[] { ".txt", ".json", ".html", ".htm", ".xml", ".yaml", ".fnt", ".bin", ".bytes", ".csv", ".tsv" };
            list.Add(new AssetFileSetting(AssetFileType.Text, extensions));
            string[] textArray2 = new string[] { ".png", ".jpg", ".bmp", ".psd", ".tif", ".tga", ".gif", ".iff", ".pict" };
            list.Add(new AssetFileSetting(AssetFileType.Texture, textArray2));
            string[] textArray3 = new string[] { ".mp3", ".ogg", ".wav", ".aif", ".aiff", ".xm", ".mod", ".it", ".s3m" };
            list.Add(new AssetFileSetting(AssetFileType.Sound, textArray3));
            string[] textArray4 = new string[] { string.Empty };
            list.Add(new AssetFileSetting(AssetFileType.UnityObject, textArray4));
            return list;
        }

        public AssetFileSetting Find(AssetFileType type)
        {
            <Find>c__AnonStorey0 storey = new <Find>c__AnonStorey0 {
                type = type
            };
            return this.FileSettings.Find(new Predicate<AssetFileSetting>(storey.<>m__0));
        }

        public AssetFileSetting FindSettingFromPath(string path)
        {
            <FindSettingFromPath>c__AnonStorey1 storey = new <FindSettingFromPath>c__AnonStorey1 {
                path = path
            };
            AssetFileSetting setting = this.fileSettings.Find(new Predicate<AssetFileSetting>(storey.<>m__0));
            if (setting == null)
            {
                setting = this.Find(AssetFileType.UnityObject);
            }
            return setting;
        }

        private void RebuildFileSettings()
        {
            if (this.rebuildFileSettings == null)
            {
                if (this.fileSettings.Count != Enum.GetValues(typeof(AssetFileType)).Length)
                {
                    this.rebuildFileSettings = this.fileSettings = this.DefaultFileSettings();
                }
                else
                {
                    this.rebuildFileSettings = this.fileSettings;
                }
                foreach (AssetFileSetting setting in this.rebuildFileSettings)
                {
                    setting.InitLink(this);
                }
            }
        }

        public List<AssetFileSetting> FileSettings
        {
            get
            {
                this.RebuildFileSettings();
                return this.rebuildFileSettings;
            }
        }

        public LoadType LoadTypeSetting
        {
            get
            {
                return this.loadType;
            }
            private set
            {
                this.loadType = value;
            }
        }

        [CompilerGenerated]
        private sealed class <Find>c__AnonStorey0
        {
            internal AssetFileType type;

            internal bool <>m__0(AssetFileSetting x)
            {
                return (x.FileType == this.type);
            }
        }

        [CompilerGenerated]
        private sealed class <FindSettingFromPath>c__AnonStorey1
        {
            internal string path;

            internal bool <>m__0(AssetFileSetting x)
            {
                return x.ContainsExtensions(this.path);
            }
        }

        public enum LoadType
        {
            Local,
            Server,
            StreamingAssets,
            Advanced
        }
    }
}

