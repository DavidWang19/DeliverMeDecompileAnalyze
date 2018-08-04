namespace Utage
{
    using System;
    using UnityEngine;

    internal class AdvCommandGuiPosition : AdvCommand
    {
        private string name;
        private float? x;
        private float? y;

        public AdvCommandGuiPosition(StringGridRow row) : base(row)
        {
            this.name = base.ParseCell<string>(AdvColumnName.Arg1);
            this.x = base.ParseCellOptionalNull<float>(AdvColumnName.Arg2);
            this.y = base.ParseCellOptionalNull<float>(AdvColumnName.Arg3);
        }

        public override void DoCommand(AdvEngine engine)
        {
            AdvGuiBase base2;
            if (!engine.UiManager.GuiManager.TryGet(this.name, out base2))
            {
                Debug.LogError(base.ToErrorString(this.name + " is not found in GuiManager"));
            }
            else
            {
                base2.SetPosition(this.x, this.y);
            }
        }
    }
}

