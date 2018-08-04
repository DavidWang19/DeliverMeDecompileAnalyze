namespace Utage
{
    using System;
    using UnityEngine;

    public class AdvCommandGenerateSeed : AdvCommand
    {
        public AdvCommandGenerateSeed(StringGridRow row) : base(row)
        {
        }

        public override void DoCommand(AdvEngine engine)
        {
            engine.Param.SetParameterInt("seed", (int) (Time.get_time() * 10000f));
        }
    }
}

