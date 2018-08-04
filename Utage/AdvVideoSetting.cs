namespace Utage
{
    using System;
    using UnityEngine;

    public class AdvVideoSetting : AdvSettingDataDictinoayBase<AdvVideoSettingData>
    {
        public override void BootInit(AdvSettingDataManager dataManager)
        {
            foreach (AdvVideoSettingData data in base.List)
            {
                data.BootInit(dataManager);
            }
        }

        public override void DownloadAll()
        {
            foreach (AdvVideoSettingData data in base.List)
            {
                AssetFileManager.Download(data.Graphic.File);
            }
        }

        private AdvVideoSettingData FindData(string label)
        {
            AdvVideoSettingData data;
            if (!base.Dictionary.TryGetValue(label, out data))
            {
                return null;
            }
            return data;
        }

        public AdvGraphicInfo LabelToGraphic(string label)
        {
            AdvVideoSettingData data = this.FindData(label);
            if (data == null)
            {
                Debug.LogError("Not found " + label + " in Particle sheet");
                return null;
            }
            return data.Graphic;
        }
    }
}

