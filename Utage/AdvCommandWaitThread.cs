namespace Utage
{
    using System;

    internal class AdvCommandWaitThread : AdvCommand
    {
        private bool cancelInput;
        private string label;

        public AdvCommandWaitThread(StringGridRow row) : base(row)
        {
            this.label = base.ParseScenarioLabel(AdvColumnName.Arg1);
            this.cancelInput = base.ParseCellOptional<bool>(AdvColumnName.Arg2, false);
        }

        public override void DoCommand(AdvEngine engine)
        {
            if (this.cancelInput)
            {
                engine.Page.IsWaitingInputCommand = true;
            }
        }

        private bool IsWaiting(AdvEngine engine)
        {
            if ((!this.cancelInput || !engine.UiManager.IsInputTrig) && !engine.Page.CheckSkip())
            {
                return base.CurrentTread.IsPlayingSubThread(this.label);
            }
            base.CurrentTread.CancelSubThread(this.label);
            return false;
        }

        public override bool Wait(AdvEngine engine)
        {
            if (this.IsWaiting(engine))
            {
                return true;
            }
            if (this.cancelInput)
            {
                engine.Page.IsWaitingInputCommand = false;
            }
            return false;
        }
    }
}

