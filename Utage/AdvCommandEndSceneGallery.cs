namespace Utage
{
    using System;

    internal class AdvCommandEndSceneGallery : AdvCommand
    {
        public AdvCommandEndSceneGallery(StringGridRow row) : base(row)
        {
        }

        public override void DoCommand(AdvEngine engine)
        {
            engine.ScenarioPlayer.EndSceneGallery(engine);
            if (engine.IsSceneGallery)
            {
                engine.ScenarioPlayer.IsReservedEndScenario = true;
            }
        }
    }
}

