namespace Utage
{
    using System;

    internal class AdvCommandBgOff : AdvCommand
    {
        private float fadeTime;

        public AdvCommandBgOff(StringGridRow row) : base(row)
        {
            this.fadeTime = base.ParseCellOptional<float>(AdvColumnName.Arg6, 0.2f);
        }

        public override void DoCommand(AdvEngine engine)
        {
            engine.GraphicManager.BgManager.FadeOutAll(this.fadeTime);
        }
    }
}

