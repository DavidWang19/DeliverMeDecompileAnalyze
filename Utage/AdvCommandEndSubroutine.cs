namespace Utage
{
    using System;

    internal class AdvCommandEndSubroutine : AdvCommand
    {
        public AdvCommandEndSubroutine(StringGridRow row, AdvSettingDataManager dataManager) : base(row)
        {
        }

        public override void DoCommand(AdvEngine engine)
        {
            base.CurrentTread.JumpManager.EndSubroutine();
        }
    }
}

