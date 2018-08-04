namespace Utage
{
    using System;
    using UnityEngine;

    internal class AdvCommandAmbience : AdvCommand
    {
        private float fadeInTime;
        private float fadeOutTime;
        private AssetFile file;
        private bool isLoop;
        private float volume;

        public AdvCommandAmbience(StringGridRow row, AdvSettingDataManager dataManager) : base(row)
        {
            string label = base.ParseCell<string>(AdvColumnName.Arg1);
            if (!dataManager.SoundSetting.Contains(label, SoundType.Ambience))
            {
                Debug.LogError(base.ToErrorString(label + " is not contained in file setting"));
            }
            this.file = base.AddLoadFile(dataManager.SoundSetting.LabelToFilePath(label, SoundType.Ambience), dataManager.SoundSetting.FindData(label));
            this.isLoop = base.ParseCellOptional<bool>(AdvColumnName.Arg2, false);
            this.volume = base.ParseCellOptional<float>(AdvColumnName.Arg3, 1f);
            this.fadeOutTime = base.ParseCellOptional<float>(AdvColumnName.Arg5, 0.2f);
            this.fadeInTime = base.ParseCellOptional<float>(AdvColumnName.Arg6, 0f);
        }

        public override void DoCommand(AdvEngine engine)
        {
            engine.SoundManager.PlayAmbience(this.file, this.volume, this.isLoop, this.fadeInTime, this.fadeOutTime);
        }
    }
}

