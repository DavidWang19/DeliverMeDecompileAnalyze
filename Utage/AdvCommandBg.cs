namespace Utage
{
    using System;
    using UnityEngine;

    internal class AdvCommandBg : AdvCommand
    {
        protected float fadeTime;
        protected Utage.graphic graphic;
        protected string layerName;

        public AdvCommandBg(StringGridRow row, AdvSettingDataManager dataManager) : base(row)
        {
            string label = base.ParseCell<string>(AdvColumnName.Arg1);
            if (!dataManager.TextureSetting.ContainsLabel(label))
            {
                Debug.LogError(base.ToErrorString(label + " is not contained in file setting"));
            }
            this.graphic = dataManager.TextureSetting.LabelToGraphic(label);
            base.AddLoadGraphic(this.graphic);
            this.layerName = base.ParseCellOptional<string>(AdvColumnName.Arg3, string.Empty);
            if (!string.IsNullOrEmpty(this.layerName) && !dataManager.LayerSetting.Contains(this.layerName, AdvLayerSettingData.LayerType.Bg))
            {
                Debug.LogError(base.ToErrorString(this.layerName + " is not contained in layer setting"));
            }
            this.fadeTime = base.ParseCellOptional<float>(AdvColumnName.Arg6, 0.2f);
        }

        public override void DoCommand(AdvEngine engine)
        {
            AdvGraphicOperaitonArg arg = new AdvGraphicOperaitonArg(this, this.graphic.Main, this.fadeTime);
            engine.GraphicManager.IsEventMode = false;
            if (string.IsNullOrEmpty(this.layerName))
            {
                engine.GraphicManager.BgManager.DrawToDefault(engine.GraphicManager.BgSpriteName, arg);
            }
            else
            {
                engine.GraphicManager.BgManager.Draw(this.layerName, engine.GraphicManager.BgSpriteName, arg);
            }
            AdvGraphicObject obj2 = engine.GraphicManager.BgManager.FindObject(engine.GraphicManager.BgSpriteName);
            if (obj2 != null)
            {
                obj2.SetCommandPostion(this);
                obj2.TargetObject.SetCommandArg(this);
            }
        }
    }
}

