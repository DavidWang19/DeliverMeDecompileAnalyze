namespace Utage
{
    using System;

    public class AdvCommandHideIM : AdvCommand
    {
        private IMManager imControl;

        public AdvCommandHideIM(StringGridRow row, IMManager imManager) : base(row)
        {
            this.imControl = imManager;
        }

        public override void DoCommand(AdvEngine engine)
        {
            this.imControl.HideIM();
        }
    }
}

