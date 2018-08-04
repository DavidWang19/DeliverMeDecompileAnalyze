namespace Utage
{
    using System;

    internal class AdvCommandHideMenuButton : AdvCommand
    {
        public AdvCommandHideMenuButton(StringGridRow row) : base(row)
        {
        }

        public override void DoCommand(AdvEngine engine)
        {
            engine.UiManager.HideMenuButton();
        }
    }
}

