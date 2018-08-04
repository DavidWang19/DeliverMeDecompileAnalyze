namespace Utage
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class AdvTextureSettingData : AdvSettingDictinoayItemBase
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <CgCategory>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private graphic <Graphic>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Type <TextureType>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <ThumbnailPath>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <ThumbnailVersion>k__BackingField;
        public static ParseCustomFileTypeRootDir CallbackParseCustomFileTypeRootDir;
        private string thumbnailName;

        internal void AddGraphicInfo(StringGridRow row)
        {
            this.Graphic.Add("Texture", row, this);
        }

        internal void BootInit(AdvSettingDataManager dataManager)
        {
            <BootInit>c__AnonStorey0 storey = new <BootInit>c__AnonStorey0 {
                dataManager = dataManager,
                $this = this
            };
            this.Graphic.BootInit(new Func<string, string, string>(storey.<>m__0), storey.dataManager);
            this.ThumbnailPath = storey.dataManager.BootSetting.ThumbnailDirInfo.FileNameToPath(this.thumbnailName);
            if (!string.IsNullOrEmpty(this.ThumbnailPath))
            {
                AssetFileManager.GetFileCreateIfMissing(this.ThumbnailPath, null);
            }
        }

        private string FileNameToPath(string fileName, string fileType, AdvBootSetting settingData)
        {
            string rootDir = null;
            if (CallbackParseCustomFileTypeRootDir != null)
            {
                CallbackParseCustomFileTypeRootDir(fileType, ref rootDir);
                if (rootDir != null)
                {
                    string[] args = new string[] { settingData.ResourceDir, rootDir, fileName };
                    return FilePathUtil.Combine(args);
                }
            }
            switch (this.TextureType)
            {
                case Type.Event:
                    return settingData.EventDirInfo.FileNameToPath(fileName);

                case Type.Sprite:
                    return settingData.SpriteDirInfo.FileNameToPath(fileName);
            }
            return settingData.BgDirInfo.FileNameToPath(fileName);
        }

        public override bool InitFromStringGridRow(StringGridRow row)
        {
            base.RowData = row;
            string key = AdvParser.ParseCell<string>(row, AdvColumnName.Label);
            base.InitKey(key);
            this.TextureType = AdvParser.ParseCell<Type>(row, AdvColumnName.Type);
            this.Graphic = new graphic(key);
            this.thumbnailName = AdvParser.ParseCellOptional<string>(row, AdvColumnName.Thumbnail, string.Empty);
            this.ThumbnailVersion = AdvParser.ParseCellOptional<int>(row, AdvColumnName.ThumbnailVersion, 0);
            this.CgCategory = AdvParser.ParseCellOptional<string>(row, AdvColumnName.CgCategolly, string.Empty);
            this.AddGraphicInfo(row);
            return true;
        }

        public string CgCategory { get; private set; }

        public graphic Graphic { get; private set; }

        public Type TextureType { get; private set; }

        public string ThumbnailPath { get; private set; }

        public int ThumbnailVersion { get; private set; }

        [CompilerGenerated]
        private sealed class <BootInit>c__AnonStorey0
        {
            internal AdvTextureSettingData $this;
            internal AdvSettingDataManager dataManager;

            internal string <>m__0(string fileName, string fileType)
            {
                return this.$this.FileNameToPath(fileName, fileType, this.dataManager.BootSetting);
            }
        }

        public delegate void ParseCustomFileTypeRootDir(string fileType, ref string rootDir);

        public enum Type
        {
            Bg,
            Event,
            Sprite
        }
    }
}

