namespace Utage
{
    using System;

    internal class AdvCommandStopBgm : AdvCommand
    {
        private float fadeTime;

        public AdvCommandStopBgm(StringGridRow row) : base(row)
        {
            this.fadeTime = base.ParseCellOptional<float>(AdvColumnName.Arg6, 0.2f);
        }

        public override void DoCommand(AdvEngine engine)
        {
            engine.SoundManager.StopBgm(this.fadeTime);
        }
    }
}

