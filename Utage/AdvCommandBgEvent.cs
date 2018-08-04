namespace Utage
{
    using System;
    using UnityEngine;

    internal class AdvCommandBgEvent : AdvCommand
    {
        private float fadeTime;
        private Utage.graphic graphic;
        private string label;

        public AdvCommandBgEvent(StringGridRow row, AdvSettingDataManager dataManager) : base(row)
        {
            this.label = base.ParseCell<string>(AdvColumnName.Arg1);
            if (!dataManager.TextureSetting.ContainsLabel(this.label))
            {
                Debug.LogError(base.ToErrorString(this.label + " is not contained in file setting"));
            }
            this.graphic = dataManager.TextureSetting.LabelToGraphic(this.label);
            base.AddLoadGraphic(this.graphic);
            this.fadeTime = base.ParseCellOptional<float>(AdvColumnName.Arg6, 0.2f);
        }

        public override void DoCommand(AdvEngine engine)
        {
            AdvGraphicOperaitonArg arg = new AdvGraphicOperaitonArg(this, this.graphic.Main, this.fadeTime);
            engine.SystemSaveData.GalleryData.AddCgLabel(this.label);
            engine.GraphicManager.IsEventMode = true;
            engine.GraphicManager.CharacterManager.FadeOutAll(arg.GetSkippedFadeTime(engine));
            engine.GraphicManager.BgManager.DrawToDefault(engine.GraphicManager.BgSpriteName, arg);
            AdvGraphicObject obj2 = engine.GraphicManager.BgManager.FindObject(engine.GraphicManager.BgSpriteName);
            if (obj2 != null)
            {
                obj2.SetCommandPostion(this);
                obj2.TargetObject.SetCommandArg(this);
            }
        }
    }
}

