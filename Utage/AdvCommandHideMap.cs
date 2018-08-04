namespace Utage
{
    using System;

    public class AdvCommandHideMap : AdvCommand
    {
        private MapManager mapManager;

        public AdvCommandHideMap(StringGridRow row, MapManager mapM) : base(row)
        {
            this.mapManager = mapM;
        }

        public override void DoCommand(AdvEngine engine)
        {
            this.mapManager.HideMap();
        }
    }
}

