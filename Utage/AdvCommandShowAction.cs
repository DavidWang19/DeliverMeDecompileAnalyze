namespace Utage
{
    using System;

    public class AdvCommandShowAction : AdvCommand
    {
        private ActionManager actionManager;
        private bool isWeekend;
        private int stage;

        public AdvCommandShowAction(StringGridRow row, ActionManager actionM) : base(row)
        {
            this.actionManager = actionM;
            this.stage = base.ParseCell<int>(AdvColumnName.Arg1);
            this.isWeekend = base.ParseCell<bool>(AdvColumnName.Arg2);
        }

        public override void DoCommand(AdvEngine engine)
        {
            this.actionManager.wait = true;
            this.actionManager.ShowActionPanel(engine, this.stage, this.isWeekend, false);
        }

        public override bool Wait(AdvEngine engine)
        {
            return this.actionManager.wait;
        }
    }
}

