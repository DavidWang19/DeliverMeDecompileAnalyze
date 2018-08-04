namespace Utage
{
    using System;

    internal class AdvCommandJumpSubroutineRandomEnd : AdvCommand
    {
        public AdvCommandJumpSubroutineRandomEnd(StringGridRow row, AdvSettingDataManager dataManager) : base(row)
        {
        }

        public override void DoCommand(AdvEngine engine)
        {
            AdvCommandJumpSubroutineRandom randomJumpCommand = base.CurrentTread.JumpManager.GetRandomJumpCommand() as AdvCommandJumpSubroutineRandom;
            if (randomJumpCommand != null)
            {
                randomJumpCommand.DoRandomEnd(base.CurrentTread, engine);
            }
        }

        public override bool IsTypePage()
        {
            return true;
        }

        public override bool IsTypePageEnd()
        {
            return true;
        }
    }
}

