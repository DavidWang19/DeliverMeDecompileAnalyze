namespace Utage
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class graphic
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <Key>k__BackingField;
        private List<AdvGraphicInfo> infoList = new List<AdvGraphicInfo>();

        public graphic(string key)
        {
            this.Key = key;
        }

        internal void Add(string dataType, StringGridRow row, IAdvSettingData settingData)
        {
            this.infoList.Add(new AdvGraphicInfo(dataType, this.InfoList.Count, this.Key, row, settingData));
        }

        internal void BootInit(Func<string, string, string> FileNameToPath, AdvSettingDataManager dataManager)
        {
            foreach (AdvGraphicInfo info in this.infoList)
            {
                info.BootInit(FileNameToPath, dataManager);
            }
        }

        internal void DownloadAll()
        {
            foreach (AdvGraphicInfo info in this.infoList)
            {
                AssetFileManager.Download(info.File);
            }
        }

        public List<AdvGraphicInfo> InfoList
        {
            get
            {
                return this.infoList;
            }
        }

        public bool IsDefaultFileType
        {
            get
            {
                foreach (AdvGraphicInfo info in this.infoList)
                {
                    if (!string.IsNullOrEmpty(info.FileType))
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        public bool IsLoadEnd
        {
            get
            {
                foreach (AdvGraphicInfo info in this.infoList)
                {
                    if (!info.File.IsLoadEnd)
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        public string Key { get; protected set; }

        public AdvGraphicInfo Main
        {
            get
            {
                if (this.InfoList.Count == 0)
                {
                    return null;
                }
                if (this.InfoList.Count == 1)
                {
                    return this.InfoList[0];
                }
                AdvGraphicInfo info = null;
                foreach (AdvGraphicInfo info2 in this.InfoList)
                {
                    if (string.IsNullOrEmpty(info2.ConditionalExpression))
                    {
                        if (info == null)
                        {
                            info = info2;
                        }
                    }
                    else if (info2.CheckConditionalExpression)
                    {
                        return info2;
                    }
                }
                return info;
            }
        }
    }
}

