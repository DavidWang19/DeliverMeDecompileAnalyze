namespace Utage
{
    using System;
    using UnityEngine;

    internal class AdvCommandWait : AdvCommand
    {
        private float time;
        private float waitEndTime;

        public AdvCommandWait(StringGridRow row) : base(row)
        {
            this.time = base.ParseCell<float>(AdvColumnName.Arg6);
        }

        public override void DoCommand(AdvEngine engine)
        {
            this.waitEndTime = Time.get_time() + (!engine.Page.CheckSkip() ? this.time : (this.time / engine.Config.SkipSpped));
        }

        public override bool Wait(AdvEngine engine)
        {
            return (Time.get_time() < this.waitEndTime);
        }
    }
}

