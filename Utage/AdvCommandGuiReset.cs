namespace Utage
{
    using System;
    using UnityEngine;

    internal class AdvCommandGuiReset : AdvCommand
    {
        private string name;

        public AdvCommandGuiReset(StringGridRow row) : base(row)
        {
            this.name = base.ParseCellOptional<string>(AdvColumnName.Arg1, string.Empty);
        }

        public override void DoCommand(AdvEngine engine)
        {
            if (string.IsNullOrEmpty(this.name))
            {
                foreach (AdvGuiBase base2 in engine.UiManager.GuiManager.Objects.Values)
                {
                    base2.Reset();
                }
            }
            else
            {
                AdvGuiBase base3;
                if (!engine.UiManager.GuiManager.TryGet(this.name, out base3))
                {
                    Debug.LogError(base.ToErrorString(this.name + " is not found in GuiManager"));
                }
                else
                {
                    base3.Reset();
                }
            }
        }
    }
}

