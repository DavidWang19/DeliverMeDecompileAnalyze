namespace Utage
{
    using System;

    public class AdvCommmandStopCountdown : AdvCommand
    {
        private CountDownManager countDownManager;

        public AdvCommmandStopCountdown(StringGridRow row, CountDownManager countDownManager) : base(row)
        {
            this.countDownManager = countDownManager;
        }

        public override void DoCommand(AdvEngine engine)
        {
            this.countDownManager.StopCountdown();
        }
    }
}

