namespace Utage
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class AdvSoundSettingData : AdvSettingDictinoayItemBase, IAssetFileSoundSettingData, IAssetFileSettingData
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <FilePath>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private float <IntroTime>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <Title>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private SoundType <Type>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private float <Volume>k__BackingField;
        private string fileName;

        public void BootInit(AdvSettingDataManager dataManager)
        {
            this.FilePath = this.FileNameToPath(this.fileName, dataManager.BootSetting);
            AssetFileManager.GetFileCreateIfMissing(this.FilePath, this);
        }

        private string FileNameToPath(string fileName, AdvBootSetting settingData)
        {
            switch (this.Type)
            {
                case SoundType.Se:
                    return settingData.SeDirInfo.FileNameToPath(fileName);

                case SoundType.Ambience:
                    return settingData.AmbienceDirInfo.FileNameToPath(fileName);
            }
            return settingData.BgmDirInfo.FileNameToPath(fileName);
        }

        public override bool InitFromStringGridRow(StringGridRow row)
        {
            if (row.IsEmptyOrCommantOut)
            {
                return false;
            }
            base.RowData = row;
            string str = AdvParser.ParseCell<string>(row, AdvColumnName.Label);
            if (string.IsNullOrEmpty(str))
            {
                return false;
            }
            base.InitKey(str);
            this.Type = AdvParser.ParseCell<SoundType>(row, AdvColumnName.Type);
            this.fileName = AdvParser.ParseCell<string>(row, AdvColumnName.FileName);
            this.Title = AdvParser.ParseCellOptional<string>(row, AdvColumnName.Title, string.Empty);
            this.IntroTime = AdvParser.ParseCellOptional<float>(row, AdvColumnName.IntroTime, 0f);
            this.Volume = AdvParser.ParseCellOptional<float>(row, AdvColumnName.Volume, 1f);
            return true;
        }

        public string FilePath { get; private set; }

        public float IntroTime { get; private set; }

        public string Title { get; private set; }

        public SoundType Type { get; private set; }

        public float Volume { get; private set; }
    }
}

