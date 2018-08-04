namespace Utage
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class AdvTextureSetting : AdvSettingDataDictinoayBase<AdvTextureSettingData>
    {
        public override void BootInit(AdvSettingDataManager dataManager)
        {
            foreach (AdvTextureSettingData data in base.List)
            {
                data.BootInit(dataManager);
            }
        }

        public bool ContainsLabel(string label)
        {
            if (!FilePathUtil.IsAbsoluteUri(label) && (this.FindData(label) == null))
            {
                return false;
            }
            return true;
        }

        public List<string> CreateCgGalleryCategoryList()
        {
            List<string> list = new List<string>();
            foreach (AdvTextureSettingData data in base.List)
            {
                if ((((data.TextureType == AdvTextureSettingData.Type.Event) && !string.IsNullOrEmpty(data.ThumbnailPath)) && !string.IsNullOrEmpty(data.CgCategory)) && !list.Contains(data.CgCategory))
                {
                    list.Add(data.CgCategory);
                }
            }
            return list;
        }

        public List<AdvCgGalleryData> CreateCgGalleryList(AdvGallerySaveData saveData)
        {
            return this.CreateCgGalleryList(saveData, string.Empty);
        }

        public List<AdvCgGalleryData> CreateCgGalleryList(AdvGallerySaveData saveData, string category)
        {
            List<AdvCgGalleryData> list = new List<AdvCgGalleryData>();
            AdvCgGalleryData item = null;
            foreach (AdvTextureSettingData data2 in base.List)
            {
                if (((data2.TextureType == AdvTextureSettingData.Type.Event) && !string.IsNullOrEmpty(data2.ThumbnailPath)) && (string.IsNullOrEmpty(category) || (data2.CgCategory == category)))
                {
                    string thumbnailPath = data2.ThumbnailPath;
                    if (item == null)
                    {
                        item = new AdvCgGalleryData(thumbnailPath, saveData);
                        list.Add(item);
                    }
                    else if (thumbnailPath != item.ThumbnailPath)
                    {
                        item = new AdvCgGalleryData(thumbnailPath, saveData);
                        list.Add(item);
                    }
                    item.AddTextureData(data2);
                }
            }
            return list;
        }

        public override void DownloadAll()
        {
            foreach (AdvTextureSettingData data in base.List)
            {
                data.Graphic.DownloadAll();
                if (!string.IsNullOrEmpty(data.ThumbnailPath))
                {
                    AssetFileManager.Download(data.ThumbnailPath);
                }
            }
        }

        private AdvTextureSettingData FindData(string label)
        {
            AdvTextureSettingData data;
            if (!base.Dictionary.TryGetValue(label, out data))
            {
                return null;
            }
            return data;
        }

        public graphic LabelToGraphic(string label)
        {
            AdvTextureSettingData data = this.FindData(label);
            if (data == null)
            {
                Debug.LogError("Not contains " + label + " in Texture sheet");
                return null;
            }
            return data.Graphic;
        }

        protected override bool TryParseContinues(AdvTextureSettingData last, StringGridRow row)
        {
            if (last == null)
            {
                return false;
            }
            if (!string.IsNullOrEmpty(AdvParser.ParseCellOptional<string>(row, AdvColumnName.Label, string.Empty)))
            {
                return false;
            }
            last.AddGraphicInfo(row);
            return true;
        }
    }
}

