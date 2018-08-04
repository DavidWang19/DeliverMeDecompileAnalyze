namespace Utage
{
    using System;

    internal class AdvCommandPauseScenario : AdvCommand
    {
        public AdvCommandPauseScenario(StringGridRow row) : base(row)
        {
        }

        public override void DoCommand(AdvEngine engine)
        {
            engine.ScenarioPlayer.Pause();
        }

        public override bool IsTypePage()
        {
            return true;
        }

        public override bool IsTypePageEnd()
        {
            return true;
        }

        public override bool Wait(AdvEngine engine)
        {
            return engine.ScenarioPlayer.IsPausing;
        }
    }
}

