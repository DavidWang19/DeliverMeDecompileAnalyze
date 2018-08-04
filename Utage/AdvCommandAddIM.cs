namespace Utage
{
    using System;

    public class AdvCommandAddIM : AdvCommand
    {
        private bool byPlayer;
        private string content;
        private IMManager imControl;

        public AdvCommandAddIM(StringGridRow row, IMManager imManager) : base(row)
        {
            this.imControl = imManager;
            this.content = base.ParseCell<string>(AdvColumnName.Text);
            this.byPlayer = base.ParseCellOptional<bool>(AdvColumnName.Arg1, false);
        }

        public override void DoCommand(AdvEngine engine)
        {
            IMManager.waitIM = true;
            this.imControl.AddMessage(this.content, this.byPlayer, engine);
        }

        public override bool Wait(AdvEngine engine)
        {
            return (IMManager.active && IMManager.waitIM);
        }
    }
}

