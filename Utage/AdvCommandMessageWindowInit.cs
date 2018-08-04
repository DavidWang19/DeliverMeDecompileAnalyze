namespace Utage
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    internal class AdvCommandMessageWindowInit : AdvCommand
    {
        private List<string> names;

        public AdvCommandMessageWindowInit(StringGridRow row) : base(row)
        {
            this.names = new List<string>();
            if (!base.IsEmptyCell(AdvColumnName.Arg1))
            {
                this.names.Add(base.ParseCell<string>(AdvColumnName.Arg1));
            }
            if (!base.IsEmptyCell(AdvColumnName.Arg2))
            {
                this.names.Add(base.ParseCell<string>(AdvColumnName.Arg2));
            }
            if (!base.IsEmptyCell(AdvColumnName.Arg3))
            {
                this.names.Add(base.ParseCell<string>(AdvColumnName.Arg3));
            }
            if (!base.IsEmptyCell(AdvColumnName.Arg4))
            {
                this.names.Add(base.ParseCell<string>(AdvColumnName.Arg4));
            }
            if (this.names.Count <= 0)
            {
                Debug.LogError(base.ToErrorString("Not set data in this command"));
            }
        }

        public override void DoCommand(AdvEngine engine)
        {
            engine.MessageWindowManager.ChangeActiveWindows(this.names);
        }

        public override void InitFromPageData(AdvScenarioPageData pageData)
        {
            if (this.names.Count > 0)
            {
                pageData.InitMessageWindowName(this, this.names[0]);
            }
        }
    }
}

