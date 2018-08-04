namespace Utage
{
    using System;
    using System.Collections.Generic;

    public class AdvSoundSetting : AdvSettingDataDictinoayBase<AdvSoundSettingData>
    {
        public override void BootInit(AdvSettingDataManager dataManager)
        {
            foreach (AdvSoundSettingData data in base.List)
            {
                data.BootInit(dataManager);
            }
        }

        public bool Contains(string label, SoundType type)
        {
            if (!FilePathUtil.IsAbsoluteUri(label) && (this.FindData(label) == null))
            {
                return false;
            }
            return true;
        }

        public override void DownloadAll()
        {
            foreach (AdvSoundSettingData data in base.List)
            {
                AssetFileManager.Download(data.FilePath);
            }
        }

        public AdvSoundSettingData FindData(string label)
        {
            AdvSoundSettingData data;
            if (!base.Dictionary.TryGetValue(label, out data))
            {
                return null;
            }
            return data;
        }

        public StringGridRow FindRowData(string label)
        {
            AdvSoundSettingData data = this.FindData(label);
            if (data == null)
            {
                return null;
            }
            return data.RowData;
        }

        public List<AdvSoundSettingData> GetSoundRoomList()
        {
            List<AdvSoundSettingData> list = new List<AdvSoundSettingData>();
            foreach (AdvSoundSettingData data in base.List)
            {
                if (!string.IsNullOrEmpty(data.Title))
                {
                    list.Add(data);
                }
            }
            return list;
        }

        public string LabelToFilePath(string label, SoundType type)
        {
            if (FilePathUtil.IsAbsoluteUri(label))
            {
                return ExtensionUtil.ChangeSoundExt(label);
            }
            AdvSoundSettingData data = this.FindData(label);
            if (data == null)
            {
                return label;
            }
            return data.FilePath;
        }
    }
}

