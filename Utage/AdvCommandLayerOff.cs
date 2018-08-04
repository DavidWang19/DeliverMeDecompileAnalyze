namespace Utage
{
    using System;
    using UnityEngine;

    internal class AdvCommandLayerOff : AdvCommand
    {
        private float fadeTime;
        private string name;

        public AdvCommandLayerOff(StringGridRow row, AdvSettingDataManager dataManager) : base(row)
        {
            this.name = base.ParseCell<string>(AdvColumnName.Arg1);
            if (!dataManager.LayerSetting.Contains(this.name))
            {
                Debug.LogError(row.ToErrorString("Not found " + this.name + " Please input Layer name"));
            }
            this.fadeTime = base.ParseCellOptional<float>(AdvColumnName.Arg6, 0.2f);
        }

        public override void DoCommand(AdvEngine engine)
        {
            AdvGraphicLayer layer = engine.GraphicManager.FindLayer(this.name);
            if (layer != null)
            {
                layer.FadeOutAll(this.fadeTime);
            }
            else
            {
                Debug.LogError("Not found " + this.name + " Please input Layer name");
            }
        }
    }
}

