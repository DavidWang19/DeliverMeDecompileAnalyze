namespace Utage
{
    using System;
    using System.Collections.Generic;

    public class AdvSceneGallerySetting : AdvSettingDataDictinoayBase<AdvSceneGallerySettingData>
    {
        public override void BootInit(AdvSettingDataManager dataManager)
        {
            foreach (AdvSceneGallerySettingData data in base.List)
            {
                data.BootInit(dataManager);
            }
        }

        public bool Contains(string key)
        {
            return base.Dictionary.ContainsKey(key);
        }

        public List<string> CreateCategoryList()
        {
            List<string> list = new List<string>();
            foreach (AdvSceneGallerySettingData data in base.List)
            {
                if (!string.IsNullOrEmpty(data.ThumbnailPath) && !list.Contains(data.Category))
                {
                    list.Add(data.Category);
                }
            }
            return list;
        }

        public List<AdvSceneGallerySettingData> CreateGalleryDataList(string category)
        {
            List<AdvSceneGallerySettingData> list = new List<AdvSceneGallerySettingData>();
            foreach (AdvSceneGallerySettingData data in base.List)
            {
                if (data.Category == category)
                {
                    list.Add(data);
                }
            }
            return list;
        }

        public override void DownloadAll()
        {
            foreach (AdvSceneGallerySettingData data in base.List)
            {
                AssetFileManager.Download(data.ThumbnailPath);
            }
        }
    }
}

