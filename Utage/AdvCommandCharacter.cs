namespace Utage
{
    using System;
    using UnityEngine;

    public class AdvCommandCharacter : AdvCommand
    {
        protected AdvCharacterInfo characterInfo;
        protected float fadeTime;
        protected string layerName;

        public AdvCommandCharacter(StringGridRow row, AdvSettingDataManager dataManager) : base(row)
        {
            this.characterInfo = AdvCharacterInfo.Create(this, dataManager);
            if (this.characterInfo.Graphic != null)
            {
                base.AddLoadGraphic(this.characterInfo.Graphic);
            }
            this.layerName = base.ParseCellOptional<string>(AdvColumnName.Arg3, string.Empty);
            if (!string.IsNullOrEmpty(this.layerName) && !dataManager.LayerSetting.Contains(this.layerName, AdvLayerSettingData.LayerType.Character))
            {
                Debug.LogError(base.ToErrorString(this.layerName + " is not contained in layer setting"));
            }
            this.fadeTime = base.ParseCellOptional<float>(AdvColumnName.Arg6, 0.2f);
        }

        private bool CheckDrawCharacter(AdvEngine engine)
        {
            if ((this.characterInfo.Graphic == null) || (this.characterInfo.Graphic.Main == null))
            {
                return false;
            }
            if (engine.GraphicManager.IsEventMode)
            {
                return false;
            }
            if (string.IsNullOrEmpty(this.characterInfo.Pattern) && engine.GraphicManager.CharacterManager.IsContians(this.layerName, this.characterInfo.Label))
            {
                return false;
            }
            return true;
        }

        private bool CheckNewCharacterInfo(AdvEngine engine)
        {
            return ((engine.Page.CharacterLabel != this.characterInfo.Label) || ((engine.Page.NameText != this.characterInfo.NameText) || !string.IsNullOrEmpty(this.characterInfo.Pattern)));
        }

        public override void DoCommand(AdvEngine engine)
        {
            if (this.characterInfo.IsHide)
            {
                engine.GraphicManager.CharacterManager.FadeOut(this.characterInfo.Label, engine.Page.ToSkippedTime(this.fadeTime));
            }
            else if (this.CheckDrawCharacter(engine))
            {
                engine.GraphicManager.CharacterManager.DrawCharacter(this.layerName, this.characterInfo.Label, new AdvGraphicOperaitonArg(this, this.characterInfo.Graphic.Main, this.fadeTime));
            }
            if (this.CheckNewCharacterInfo(engine))
            {
                engine.Page.CharacterInfo = this.characterInfo;
            }
            AdvGraphicObject obj2 = engine.GraphicManager.CharacterManager.FindObject(this.characterInfo.Label);
            if (obj2 != null)
            {
                obj2.SetCommandPostion(this);
                obj2.TargetObject.SetCommandArg(this);
            }
        }

        public override string[] GetExtraCommandIdArray(AdvCommand next)
        {
            if (base.IsEmptyCell(AdvColumnName.Text) && base.IsEmptyCell(AdvColumnName.PageCtrl))
            {
                return null;
            }
            return new string[] { "Text" };
        }
    }
}

