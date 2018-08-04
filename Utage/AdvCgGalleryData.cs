namespace Utage
{
    using System;
    using System.Collections.Generic;

    public class AdvCgGalleryData
    {
        private List<AdvTextureSettingData> list;
        private AdvGallerySaveData saveData;
        private string thumbnailPath;

        public AdvCgGalleryData(string thumbnailPath, AdvGallerySaveData saveData)
        {
            this.thumbnailPath = thumbnailPath;
            this.list = new List<AdvTextureSettingData>();
            this.saveData = saveData;
        }

        public void AddTextureData(AdvTextureSettingData data)
        {
            this.list.Add(data);
        }

        public AdvTextureSettingData GetDataOpened(int index)
        {
            int num = 0;
            foreach (AdvTextureSettingData data in this.list)
            {
                if (this.saveData.CheckCgLabel(data.Key))
                {
                    if (index == num)
                    {
                        return data;
                    }
                    num++;
                }
            }
            return null;
        }

        public int NumOpen
        {
            get
            {
                int num = 0;
                if (this.saveData == null)
                {
                    return 0;
                }
                foreach (AdvTextureSettingData data in this.list)
                {
                    if (this.saveData.CheckCgLabel(data.Key))
                    {
                        num++;
                    }
                }
                return num;
            }
        }

        public int NumTotal
        {
            get
            {
                return this.list.Count;
            }
        }

        public string ThumbnailPath
        {
            get
            {
                return this.thumbnailPath;
            }
        }
    }
}

