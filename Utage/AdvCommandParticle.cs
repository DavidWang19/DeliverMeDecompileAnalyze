namespace Utage
{
    using System;
    using UnityEngine;

    internal class AdvCommandParticle : AdvCommand
    {
        protected AdvGraphicInfo graphic;
        protected AdvGraphicOperaitonArg graphicOperaitonArg;
        protected string label;
        protected string layerName;

        public AdvCommandParticle(StringGridRow row, AdvSettingDataManager dataManager) : base(row)
        {
            this.label = base.ParseCell<string>(AdvColumnName.Arg1);
            if (!dataManager.ParticleSetting.Dictionary.ContainsKey(this.label))
            {
                Debug.LogError(base.ToErrorString(this.label + " is not contained in file setting"));
            }
            this.graphic = dataManager.ParticleSetting.LabelToGraphic(this.label);
            base.AddLoadGraphic(this.graphic);
            this.layerName = base.ParseCellOptional<string>(AdvColumnName.Arg3, string.Empty);
            if (!string.IsNullOrEmpty(this.layerName) && !dataManager.LayerSetting.Contains(this.layerName))
            {
                Debug.LogError(base.ToErrorString(this.layerName + " is not contained in layer setting"));
            }
            this.graphicOperaitonArg = new AdvGraphicOperaitonArg(this, this.graphic, 0f);
        }

        public override void DoCommand(AdvEngine engine)
        {
            string layerName = this.layerName;
            if (string.IsNullOrEmpty(layerName))
            {
                layerName = engine.GraphicManager.SpriteManager.DefaultLayer.get_name();
            }
            engine.GraphicManager.DrawObject(layerName, this.label, this.graphicOperaitonArg);
        }
    }
}

