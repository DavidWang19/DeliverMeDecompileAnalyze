namespace Utage
{
    using System;

    public class AdvCommandShowMap : AdvCommand
    {
        private string mapDataName;
        private MapManager mapManager;

        public AdvCommandShowMap(StringGridRow row, MapManager mapM) : base(row)
        {
            this.mapDataName = base.ParseCell<string>(AdvColumnName.Arg1);
            this.mapManager = mapM;
        }

        public override void DoCommand(AdvEngine engine)
        {
            this.mapManager.wait = true;
            this.mapManager.ShowMap(engine, this.mapDataName, false);
        }

        public override bool Wait(AdvEngine engine)
        {
            return this.mapManager.wait;
        }
    }
}

