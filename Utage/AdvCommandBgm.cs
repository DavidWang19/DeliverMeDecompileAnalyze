namespace Utage
{
    using System;
    using UnityEngine;

    internal class AdvCommandBgm : AdvCommand
    {
        private float fadeInTime;
        private float fadeOutTime;
        private AssetFile file;
        private float volume;

        public AdvCommandBgm(StringGridRow row, AdvSettingDataManager dataManager) : base(row)
        {
            string label = base.ParseCell<string>(AdvColumnName.Arg1);
            if (!dataManager.SoundSetting.Contains(label, SoundType.Bgm))
            {
                Debug.LogError(base.ToErrorString(label + " is not contained in file setting"));
            }
            this.file = base.AddLoadFile(dataManager.SoundSetting.LabelToFilePath(label, SoundType.Bgm), dataManager.SoundSetting.FindData(label));
            this.volume = base.ParseCellOptional<float>(AdvColumnName.Arg3, 1f);
            this.fadeOutTime = base.ParseCellOptional<float>(AdvColumnName.Arg5, 0.2f);
            this.fadeInTime = base.ParseCellOptional<float>(AdvColumnName.Arg6, 0f);
        }

        public override void DoCommand(AdvEngine engine)
        {
            engine.SoundManager.PlayBgm(this.file, this.volume, this.fadeInTime, this.fadeOutTime);
        }
    }
}

