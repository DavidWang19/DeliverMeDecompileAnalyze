namespace Utage
{
    using System;
    using UnityEngine;

    internal class AdvCommandSe : AdvCommand
    {
        private AssetFile file;
        private bool isLoop;
        private string label;
        private float volume;

        public AdvCommandSe(StringGridRow row, AdvSettingDataManager dataManager) : base(row)
        {
            this.label = base.ParseCell<string>(AdvColumnName.Arg1);
            if (!dataManager.SoundSetting.Contains(this.label, SoundType.Se))
            {
                Debug.LogError(base.ToErrorString(this.label + " is not contained in file setting"));
            }
            this.isLoop = base.ParseCellOptional<bool>(AdvColumnName.Arg2, false);
            this.volume = base.ParseCellOptional<float>(AdvColumnName.Arg3, 1f);
            this.file = base.AddLoadFile(dataManager.SoundSetting.LabelToFilePath(this.label, SoundType.Se), dataManager.SoundSetting.FindData(this.label));
        }

        public override void DoCommand(AdvEngine engine)
        {
            if (!engine.Page.CheckSkip() || !engine.Config.SkipVoiceAndSe)
            {
                engine.SoundManager.PlaySe(this.file, this.volume, this.label, SoundPlayMode.Add, this.isLoop);
            }
        }
    }
}

