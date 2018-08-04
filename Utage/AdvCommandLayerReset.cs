namespace Utage
{
    using System;
    using UnityEngine;

    internal class AdvCommandLayerReset : AdvCommand
    {
        private string name;

        public AdvCommandLayerReset(StringGridRow row, AdvSettingDataManager dataManager) : base(row)
        {
            this.name = base.ParseCell<string>(AdvColumnName.Arg1);
            if (!dataManager.LayerSetting.Contains(this.name))
            {
                Debug.LogError(row.ToErrorString("Not found " + this.name + " Please input Layer name"));
            }
        }

        public override void DoCommand(AdvEngine engine)
        {
            AdvGraphicLayer layer = engine.GraphicManager.FindLayer(this.name);
            if (layer != null)
            {
                layer.ResetCanvasRectTransform();
            }
            else
            {
                Debug.LogError("Not found " + this.name + " Please input Layer name");
            }
        }
    }
}

