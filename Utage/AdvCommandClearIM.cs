namespace Utage
{
    using System;

    public class AdvCommandClearIM : AdvCommand
    {
        private IMManager imControl;

        public AdvCommandClearIM(StringGridRow row, IMManager imManager) : base(row)
        {
            this.imControl = imManager;
        }

        public override void DoCommand(AdvEngine engine)
        {
            this.imControl.ClearMessage();
        }
    }
}

