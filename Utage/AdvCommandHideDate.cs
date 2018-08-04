namespace Utage
{
    using System;

    public class AdvCommandHideDate : AdvCommand
    {
        private DateManager dateControl;

        public AdvCommandHideDate(StringGridRow row, DateManager dateManager) : base(row)
        {
            this.dateControl = dateManager;
        }

        public override void DoCommand(AdvEngine engine)
        {
            this.dateControl.HideDate();
        }
    }
}

