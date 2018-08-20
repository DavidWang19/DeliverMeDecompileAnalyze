namespace Utage
{
    using System;

    public class AdvCommmandStartCountdown : AdvCommand
    {
        private CountDownManager countDownManager;

        public AdvCommmandStartCountdown(StringGridRow row, CountDownManager countDownManager) : base(row)
        {
            this.countDownManager = countDownManager;
        }

        public override void DoCommand(AdvEngine engine)
        {
            this.countDownManager.totalTime = float.Parse(base.RowData.Strings[1]);
            this.countDownManager.timeupScenarioLabel = base.RowData.Strings[2];
            this.countDownManager.StartCountdown();
        }
    }
}

