namespace Utage
{
    using System;

    internal class AdvCommandCharacterOff : AdvCommand
    {
        private string name;
        private float time;

        public AdvCommandCharacterOff(StringGridRow row) : base(row)
        {
            this.name = base.ParseCellOptional<string>(AdvColumnName.Arg1, string.Empty);
            this.time = base.ParseCellOptional<float>(AdvColumnName.Arg6, 0.2f);
        }

        public override void DoCommand(AdvEngine engine)
        {
            float fadeTime = engine.Page.ToSkippedTime(this.time);
            AdvGraphicGroup characterManager = engine.GraphicManager.CharacterManager;
            if (string.IsNullOrEmpty(this.name))
            {
                characterManager.FadeOutAll(fadeTime);
            }
            else
            {
                AdvGraphicLayer layer = characterManager.FindLayerFromObjectName(this.name);
                if (layer != null)
                {
                    layer.FadeOut(this.name, fadeTime);
                }
                else
                {
                    layer = characterManager.FindLayer(this.name);
                    if (layer != null)
                    {
                        layer.FadeOutAll(fadeTime);
                    }
                }
            }
        }
    }
}

