namespace Utage
{
    using System;

    public class AdvCommmandShowInput : AdvCommand
    {
        private InputCanvas inputCanvas;

        public AdvCommmandShowInput(StringGridRow row, InputCanvas inputCanvas) : base(row)
        {
            this.inputCanvas = inputCanvas;
        }

        public override void DoCommand(AdvEngine engine)
        {
            this.inputCanvas.ShowInputPanel(engine);
        }

        public override bool Wait(AdvEngine engine)
        {
            return this.inputCanvas.wait;
        }
    }
}

