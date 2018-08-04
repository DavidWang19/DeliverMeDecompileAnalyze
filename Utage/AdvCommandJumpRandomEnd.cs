namespace Utage
{
    using System;

    internal class AdvCommandJumpRandomEnd : AdvCommand
    {
        public AdvCommandJumpRandomEnd(StringGridRow row, AdvSettingDataManager dataManager) : base(row)
        {
        }

        public override void DoCommand(AdvEngine engine)
        {
            AdvCommandJumpRandom randomJumpCommand = base.CurrentTread.JumpManager.GetRandomJumpCommand() as AdvCommandJumpRandom;
            if (randomJumpCommand != null)
            {
                randomJumpCommand.DoRandomEnd(engine, base.CurrentTread);
            }
        }
    }
}

