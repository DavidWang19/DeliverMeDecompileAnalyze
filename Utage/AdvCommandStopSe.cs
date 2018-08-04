namespace Utage
{
    using System;
    using UnityEngine;

    internal class AdvCommandStopSe : AdvCommand
    {
        private float fadeTime;
        private string label;

        public AdvCommandStopSe(StringGridRow row, AdvSettingDataManager dataManager) : base(row)
        {
            this.label = base.ParseCellOptional<string>(AdvColumnName.Arg1, string.Empty);
            if (!string.IsNullOrEmpty(this.label) && !dataManager.SoundSetting.Contains(this.label, SoundType.Se))
            {
                Debug.LogError(base.ToErrorString(this.label + " is not contained in file setting"));
            }
            this.fadeTime = base.ParseCellOptional<float>(AdvColumnName.Arg6, 0.2f);
        }

        public override void DoCommand(AdvEngine engine)
        {
            if (string.IsNullOrEmpty(this.label))
            {
                engine.SoundManager.StopSeAll(this.fadeTime);
            }
            else
            {
                engine.SoundManager.StopSe(this.label, this.fadeTime);
            }
        }
    }
}

