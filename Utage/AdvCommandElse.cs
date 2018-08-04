namespace Utage
{
    using System;

    internal class AdvCommandElse : AdvCommand
    {
        public AdvCommandElse(StringGridRow row) : base(row)
        {
        }

        public override void DoCommand(AdvEngine engine)
        {
            base.CurrentTread.IfManager.Else();
        }

        public override bool IsIfCommand
        {
            get
            {
                return true;
            }
        }
    }
}

