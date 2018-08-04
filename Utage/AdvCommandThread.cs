namespace Utage
{
    using System;

    internal class AdvCommandThread : AdvCommand
    {
        private string label;
        private string name;

        public AdvCommandThread(StringGridRow row) : base(row)
        {
            this.label = base.ParseScenarioLabel(AdvColumnName.Arg1);
            this.name = base.ParseCellOptional<string>(AdvColumnName.Arg2, this.label);
        }

        public override void DoCommand(AdvEngine engine)
        {
            base.CurrentTread.StartSubThread(this.label, this.name);
        }
    }
}

