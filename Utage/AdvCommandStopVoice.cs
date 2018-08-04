namespace Utage
{
    using System;

    internal class AdvCommandStopVoice : AdvCommand
    {
        private float fadeTime;

        public AdvCommandStopVoice(StringGridRow row) : base(row)
        {
            this.fadeTime = base.ParseCellOptional<float>(AdvColumnName.Arg6, 0.2f);
        }

        public override void DoCommand(AdvEngine engine)
        {
            engine.SoundManager.StopVoice(this.fadeTime);
        }
    }
}

