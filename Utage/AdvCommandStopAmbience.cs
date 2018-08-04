namespace Utage
{
    using System;

    internal class AdvCommandStopAmbience : AdvCommand
    {
        private float fadeTime;

        public AdvCommandStopAmbience(StringGridRow row) : base(row)
        {
            this.fadeTime = base.ParseCellOptional<float>(AdvColumnName.Arg6, 0.2f);
        }

        public override void DoCommand(AdvEngine engine)
        {
            engine.SoundManager.StopAmbience(this.fadeTime);
        }
    }
}

