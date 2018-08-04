namespace Utage
{
    using System;

    internal class AdvCommandEndIf : AdvCommand
    {
        public AdvCommandEndIf(StringGridRow row) : base(row)
        {
        }

        public override void DoCommand(AdvEngine engine)
        {
            base.CurrentTread.IfManager.EndIf();
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

