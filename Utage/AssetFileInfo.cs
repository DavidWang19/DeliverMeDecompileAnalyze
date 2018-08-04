namespace Utage
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class AssetFileInfo
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Utage.AssetBundleInfo <AssetBundleInfo>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <FileName>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private AssetFileSetting <Setting>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private AssetFileStrageType <StrageType>k__BackingField;

        public AssetFileInfo(string path, AssetFileManagerSettings settings, Utage.AssetBundleInfo assetBundleInfo)
        {
            this.FileName = path;
            this.Setting = settings.FindSettingFromPath(path);
            this.AssetBundleInfo = assetBundleInfo;
            this.StrageType = this.ParseStrageType();
        }

        private AssetFileStrageType ParseStrageType()
        {
            if (this.Setting.IsStreamingAssets)
            {
                return AssetFileStrageType.StreamingAssets;
            }
            if (FilePathUtil.IsAbsoluteUri(this.FileName))
            {
                return AssetFileStrageType.Server;
            }
            if (this.Setting.LoadType == AssetFileManagerSettings.LoadType.Server)
            {
                return AssetFileStrageType.Server;
            }
            return AssetFileStrageType.Resources;
        }

        public Utage.AssetBundleInfo AssetBundleInfo { get; set; }

        public string FileName { get; private set; }

        public AssetFileType FileType
        {
            get
            {
                return this.Setting.FileType;
            }
        }

        public AssetFileSetting Setting { get; private set; }

        public AssetFileStrageType StrageType { get; set; }
    }
}

