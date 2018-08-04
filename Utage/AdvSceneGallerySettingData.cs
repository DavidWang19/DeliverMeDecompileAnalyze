namespace Utage
{
    using System;

    public class AdvSceneGallerySettingData : AdvSettingDictinoayItemBase
    {
        private string category;
        private string thumbnailName;
        private string thumbnailPath;
        private int thumbnailVersion;
        private string title;

        public void BootInit(AdvSettingDataManager dataManager)
        {
            this.thumbnailPath = dataManager.BootSetting.ThumbnailDirInfo.FileNameToPath(this.thumbnailName);
        }

        public override bool InitFromStringGridRow(StringGridRow row)
        {
            string key = AdvCommandParser.ParseScenarioLabel(row, AdvColumnName.ScenarioLabel);
            base.InitKey(key);
            this.title = AdvParser.ParseCellOptional<string>(row, AdvColumnName.Title, string.Empty);
            this.thumbnailName = AdvParser.ParseCell<string>(row, AdvColumnName.Thumbnail);
            this.thumbnailVersion = AdvParser.ParseCellOptional<int>(row, AdvColumnName.ThumbnailVersion, 0);
            this.category = AdvParser.ParseCellOptional<string>(row, AdvColumnName.Categolly, string.Empty);
            base.RowData = row;
            return true;
        }

        public string Category
        {
            get
            {
                return this.category;
            }
        }

        public string ScenarioLabel
        {
            get
            {
                return base.Key;
            }
        }

        public string ThumbnailPath
        {
            get
            {
                return this.thumbnailPath;
            }
        }

        public int ThumbnailVersion
        {
            get
            {
                return this.thumbnailVersion;
            }
        }

        public string Title
        {
            get
            {
                return this.title;
            }
        }
    }
}

