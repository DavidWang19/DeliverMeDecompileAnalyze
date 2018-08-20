namespace Utage
{
    using System;

    public class AdvCommmandShowPower : AdvCommand
    {
        private PowerManager powerManager;

        public AdvCommmandShowPower(StringGridRow row, PowerManager powerManager) : base(row)
        {
            this.powerManager = powerManager;
        }

        public override void DoCommand(AdvEngine engine)
        {
            this.powerManager.Show();
        }
    }
}

