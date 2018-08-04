namespace Utage
{
    using System;
    using UnityEngine;

    internal class AdvCommandElseIf : AdvCommand
    {
        private ExpressionParser exp;

        public AdvCommandElseIf(StringGridRow row, AdvSettingDataManager dataManager) : base(row)
        {
            this.exp = dataManager.DefaultParam.CreateExpressionBoolean(base.ParseCell<string>(AdvColumnName.Arg1));
            if (this.exp.ErrorMsg != null)
            {
                Debug.LogError(base.ToErrorString(this.exp.ErrorMsg));
            }
        }

        public override void DoCommand(AdvEngine engine)
        {
            base.CurrentTread.IfManager.ElseIf(engine.Param, this.exp);
        }

        public override bool IsIfCommand
        {
            get
            {
                return true;
            }
        }
    }
}

