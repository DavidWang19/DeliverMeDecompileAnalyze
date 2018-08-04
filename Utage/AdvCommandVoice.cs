namespace Utage
{
    using System;

    internal class AdvCommandVoice : AdvCommand
    {
        protected string characterLabel;
        private bool isLoop;
        protected AssetFile voiceFile;
        private float volume;

        public AdvCommandVoice(StringGridRow row, AdvSettingDataManager dataManager) : base(row)
        {
            this.characterLabel = base.ParseCell<string>(AdvColumnName.Arg1);
            string file = base.ParseCell<string>(AdvColumnName.Voice);
            if (!AdvCommand.IsEditorErrorCheck)
            {
                this.voiceFile = base.AddLoadFile(dataManager.BootSetting.GetLocalizeVoiceFilePath(file), null);
            }
            this.isLoop = base.ParseCellOptional<bool>(AdvColumnName.Arg2, false);
            this.volume = base.ParseCellOptional<float>(AdvColumnName.Arg3, 1f);
        }

        public override void DoCommand(AdvEngine engine)
        {
            if (!engine.Page.CheckSkip() || !engine.Config.SkipVoiceAndSe)
            {
                engine.SoundManager.PlayVoice(this.characterLabel, this.voiceFile, this.volume, this.isLoop);
            }
        }
    }
}

