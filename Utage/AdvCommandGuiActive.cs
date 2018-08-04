namespace Utage
{
    using System;
    using UnityEngine;

    internal class AdvCommandGuiActive : AdvCommand
    {
        private bool isActive;
        private string name;

        public AdvCommandGuiActive(StringGridRow row) : base(row)
        {
            this.name = base.ParseCellOptional<string>(AdvColumnName.Arg1, string.Empty);
            this.isActive = base.ParseCell<bool>(AdvColumnName.Arg2);
        }

        public override void DoCommand(AdvEngine engine)
        {
            if (string.IsNullOrEmpty(this.name))
            {
                foreach (AdvGuiBase base2 in engine.UiManager.GuiManager.Objects.Values)
                {
                    base2.SetActive(this.isActive);
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
                    base3.SetActive(this.isActive);
                }
            }
        }
    }
}

