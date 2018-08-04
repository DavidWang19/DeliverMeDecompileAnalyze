namespace Utage
{
    using System;

    internal class AdvCommandShowMenuButton : AdvCommand
    {
        public AdvCommandShowMenuButton(StringGridRow row) : base(row)
        {
        }

        public override void DoCommand(AdvEngine engine)
        {
            engine.UiManager.ShowMenuButton();
        }
    }
}

