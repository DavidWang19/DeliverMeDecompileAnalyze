namespace Utage
{
    using System;

    internal class AdvCommandSpriteOff : AdvCommand
    {
        private float fadeTime;
        private string name;

        public AdvCommandSpriteOff(StringGridRow row) : base(row)
        {
            this.fadeTime = 0.2f;
            this.name = base.ParseCellOptional<string>(AdvColumnName.Arg1, string.Empty);
            this.fadeTime = base.ParseCellOptional<float>(AdvColumnName.Arg6, this.fadeTime);
        }

        public override void DoCommand(AdvEngine engine)
        {
            if (string.IsNullOrEmpty(this.name))
            {
                engine.GraphicManager.SpriteManager.FadeOutAll(this.fadeTime);
            }
            else
            {
                engine.GraphicManager.SpriteManager.FadeOut(this.name, this.fadeTime);
            }
        }
    }
}

