namespace Utage
{
    using System;

    internal class AdvCommandCaptureImage : AdvCommand
    {
        private string cameraName;
        private bool isWaiting;
        private string layerName;
        private string objName;

        public AdvCommandCaptureImage(StringGridRow row) : base(row)
        {
            this.objName = base.ParseCell<string>(AdvColumnName.Arg1);
            this.cameraName = base.ParseCell<string>(AdvColumnName.Arg2);
            this.layerName = base.ParseCell<string>(AdvColumnName.Arg3);
        }

        public override void DoCommand(AdvEngine engine)
        {
            this.isWaiting = true;
            engine.GraphicManager.CreateCaptureImageObject(this.objName, this.cameraName, this.layerName);
        }

        public override bool Wait(AdvEngine engine)
        {
            if (!this.isWaiting)
            {
                return false;
            }
            this.isWaiting = false;
            return true;
        }
    }
}

