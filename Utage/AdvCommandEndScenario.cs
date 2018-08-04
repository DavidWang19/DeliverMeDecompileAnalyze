namespace Utage
{
    using System;

    internal class AdvCommandEndScenario : AdvCommand
    {
        public AdvCommandEndScenario(StringGridRow row) : base(row)
        {
        }

        public override void DoCommand(AdvEngine engine)
        {
            engine.ScenarioPlayer.IsReservedEndScenario = true;
        }
    }
}

