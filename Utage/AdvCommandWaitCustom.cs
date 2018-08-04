namespace Utage
{
    using System;

    internal class AdvCommandWaitCustom : AdvCommand
    {
        public AdvCommandWaitCustom(StringGridRow row) : base(row)
        {
        }

        public override void DoCommand(AdvEngine engine)
        {
        }

        public override bool Wait(AdvEngine engine)
        {
            return !engine.UiManager.IsInputTrigCustom;
        }
    }
}

