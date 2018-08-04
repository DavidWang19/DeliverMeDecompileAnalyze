namespace Utage
{
    using System;

    internal class AdvCommandBgEventOff : AdvCommand
    {
        private float fadeTime;

        public AdvCommandBgEventOff(StringGridRow row) : base(row)
        {
            this.fadeTime = base.ParseCellOptional<float>(AdvColumnName.Arg6, 0.2f);
        }

        public override void DoCommand(AdvEngine engine)
        {
            engine.GraphicManager.BgManager.FadeOutAll(this.fadeTime);
        }
    }
}

