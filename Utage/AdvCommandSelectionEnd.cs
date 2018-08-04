namespace Utage
{
    using System;

    internal class AdvCommandSelectionEnd : AdvCommand
    {
        public AdvCommandSelectionEnd(StringGridRow row, AdvSettingDataManager dataManager) : base(row)
        {
        }

        public override void DoCommand(AdvEngine engine)
        {
            engine.Config.StopSkipInSelection();
            engine.SelectionManager.Show();
        }
    }
}

