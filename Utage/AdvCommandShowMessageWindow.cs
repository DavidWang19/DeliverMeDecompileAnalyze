namespace Utage
{
    using System;

    internal class AdvCommandShowMessageWindow : AdvCommand
    {
        public AdvCommandShowMessageWindow(StringGridRow row) : base(row)
        {
        }

        public override void DoCommand(AdvEngine engine)
        {
            engine.UiManager.ShowMessageWindow();
        }
    }
}

