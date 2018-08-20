namespace Utage
{
    using System;

    public class AdvCommmandHidePower : AdvCommand
    {
        private PowerManager powerManager;

        public AdvCommmandHidePower(StringGridRow row, PowerManager powerManager) : base(row)
        {
            this.powerManager = powerManager;
        }

        public override void DoCommand(AdvEngine engine)
        {
            this.powerManager.Hide();
        }
    }
}

