namespace Utage
{
    using System;

    internal class AdvCommandPageControler : AdvCommand
    {
        private AdvPageControllerType pageCtrlType;

        public AdvCommandPageControler(StringGridRow row, AdvSettingDataManager dataManager) : base(row)
        {
            if (row == null)
            {
                this.pageCtrlType = AdvPageControllerType.InputBrPage;
            }
            else
            {
                this.pageCtrlType = base.ParseCellOptional<AdvPageControllerType>(AdvColumnName.PageCtrl, AdvPageControllerType.InputBrPage);
            }
        }

        public override void DoCommand(AdvEngine engine)
        {
            engine.Page.UpdatePageTextData(this.pageCtrlType);
        }

        public override bool IsTypePage()
        {
            return true;
        }

        public override bool IsTypePageEnd()
        {
            return AdvPageController.IsPageEndType(this.pageCtrlType);
        }

        public override bool Wait(AdvEngine engine)
        {
            return engine.Page.IsWaitTextCommand;
        }
    }
}

