namespace Utage
{
    using System;
    using UnityEngine;

    public class AdvParticleSetting : AdvSettingDataDictinoayBase<AdvParticleSettingData>
    {
        public override void BootInit(AdvSettingDataManager dataManager)
        {
            foreach (AdvParticleSettingData data in base.List)
            {
                data.BootInit(dataManager);
            }
        }

        public override void DownloadAll()
        {
            foreach (AdvParticleSettingData data in base.List)
            {
                AssetFileManager.Download(data.Graphic.File);
            }
        }

        private AdvParticleSettingData FindData(string label)
        {
            AdvParticleSettingData data;
            if (!base.Dictionary.TryGetValue(label, out data))
            {
                return null;
            }
            return data;
        }

        public AdvGraphicInfo LabelToGraphic(string label)
        {
            AdvParticleSettingData data = this.FindData(label);
            if (data == null)
            {
                Debug.LogError("Not found " + label + " in Particle sheet");
                return null;
            }
            return data.Graphic;
        }
    }
}

