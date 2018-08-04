namespace Utage
{
    using System;

    internal class AdvCommandHideMessageWindow : AdvCommand
    {
        public AdvCommandHideMessageWindow(StringGridRow row) : base(row)
        {
        }

        public override void DoCommand(AdvEngine engine)
        {
            engine.UiManager.HideMessageWindow();
        }
    }
}

