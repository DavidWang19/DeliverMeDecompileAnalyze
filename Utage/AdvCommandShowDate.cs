namespace Utage
{
    using System;

    public class AdvCommandShowDate : AdvCommand
    {
        private DateManager dateControl;

        public AdvCommandShowDate(StringGridRow row, DateManager dateManager) : base(row)
        {
            this.dateControl = dateManager;
        }

        public override void DoCommand(AdvEngine engine)
        {
            this.dateControl.ShowDate(engine);
        }
    }
}

