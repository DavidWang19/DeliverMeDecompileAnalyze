namespace Utage
{
    using System;

    public class AdvCommandSetIMContact : AdvCommand
    {
        private string avatar;
        private string contact;
        private IMManager imControl;

        public AdvCommandSetIMContact(StringGridRow row, IMManager imManager) : base(row)
        {
            this.imControl = imManager;
            this.contact = base.ParseCell<string>(AdvColumnName.Arg1);
            this.avatar = base.ParseCell<string>(AdvColumnName.Arg2);
        }

        public override void DoCommand(AdvEngine engine)
        {
            this.imControl.SetContact(this.contact, this.avatar);
        }
    }
}

