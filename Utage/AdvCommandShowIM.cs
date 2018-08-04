namespace Utage
{
    using System;

    public class AdvCommandShowIM : AdvCommand
    {
        private IMManager imControl;

        public AdvCommandShowIM(StringGridRow row, IMManager imManager) : base(row)
        {
            this.imControl = imManager;
        }

        public override void DoCommand(AdvEngine engine)
        {
            this.imControl.ShowIM();
        }
    }
}

