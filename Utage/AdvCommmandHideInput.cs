namespace Utage
{
    using System;

    public class AdvCommmandHideInput : AdvCommand
    {
        private InputCanvas inputCanvas;

        public AdvCommmandHideInput(StringGridRow row, InputCanvas inputCanvas) : base(row)
        {
            this.inputCanvas = inputCanvas;
        }

        public override void DoCommand(AdvEngine engine)
        {
        }
    }
}

