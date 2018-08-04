namespace Utage
{
    using System;
    using UnityEngine;

    internal class AdvCommandSprite : AdvCommand
    {
        protected float fadeTime;
        protected Utage.graphic graphic;
        protected string layerName;
        protected string spriteName;

        public AdvCommandSprite(StringGridRow row, AdvSettingDataManager dataManager) : base(row)
        {
            this.spriteName = base.ParseCell<string>(AdvColumnName.Arg1);
            string label = base.ParseCellOptional<string>(AdvColumnName.Arg2, this.spriteName);
            if (!dataManager.TextureSetting.ContainsLabel(label))
            {
                Debug.LogError(base.ToErrorString(label + " is not contained in file setting"));
            }
            this.graphic = dataManager.TextureSetting.LabelToGraphic(label);
            base.AddLoadGraphic(this.graphic);
            this.layerName = base.ParseCellOptional<string>(AdvColumnName.Arg3, string.Empty);
            if (string.IsNullOrEmpty(this.layerName))
            {
                this.layerName = dataManager.LayerSetting.FindDefaultLayer(AdvLayerSettingData.LayerType.Sprite).Name;
            }
            else if (!dataManager.LayerSetting.Contains(this.layerName))
            {
                Debug.LogError(base.ToErrorString(this.layerName + " is not contained in layer setting"));
            }
            this.fadeTime = base.ParseCellOptional<float>(AdvColumnName.Arg6, 0.2f);
        }

        public override void DoCommand(AdvEngine engine)
        {
            AdvGraphicOperaitonArg graphicOperaitonArg = new AdvGraphicOperaitonArg(this, this.graphic.Main, this.fadeTime);
            engine.GraphicManager.DrawObject(this.layerName, this.spriteName, graphicOperaitonArg);
            AdvGraphicObject obj2 = engine.GraphicManager.FindObject(this.spriteName);
            if (obj2 != null)
            {
                obj2.SetCommandPostion(this);
                obj2.TargetObject.SetCommandArg(this);
            }
        }
    }
}

