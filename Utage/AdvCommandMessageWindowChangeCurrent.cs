namespace Utage
{
    using System;

    internal class AdvCommandMessageWindowChangeCurrent : AdvCommand
    {
        private string name;

        public AdvCommandMessageWindowChangeCurrent(StringGridRow row) : base(row)
        {
            this.name = base.ParseCell<string>(AdvColumnName.Arg1);
        }

        public override void DoCommand(AdvEngine engine)
        {
            engine.MessageWindowManager.ChangeCurrentWindow(this.name);
        }

        public override void InitFromPageData(AdvScenarioPageData pageData)
        {
            pageData.InitMessageWindowName(this, this.name);
        }
    }
}

