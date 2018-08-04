namespace Utage
{
    using System;

    internal class AdvCommandParticleOff : AdvCommand
    {
        private string name;

        public AdvCommandParticleOff(StringGridRow row) : base(row)
        {
            this.name = base.ParseCellOptional<string>(AdvColumnName.Arg1, string.Empty);
        }

        public override void DoCommand(AdvEngine engine)
        {
            if (string.IsNullOrEmpty(this.name))
            {
                engine.GraphicManager.FadeOutAllParticle();
            }
            else
            {
                engine.GraphicManager.FadeOutParticle(this.name);
            }
        }
    }
}

