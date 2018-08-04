namespace Utage
{
    using System;
    using UnityEngine;

    internal class AdvCommandParam : AdvCommand
    {
        private ExpressionParser exp;

        public AdvCommandParam(StringGridRow row, AdvSettingDataManager dataManager) : base(row)
        {
            this.exp = dataManager.DefaultParam.CreateExpression(base.ParseCell<string>(AdvColumnName.Arg1));
            if (this.exp.ErrorMsg != null)
            {
                Debug.LogError(base.ToErrorString(this.exp.ErrorMsg));
            }
        }

        public override void DoCommand(AdvEngine engine)
        {
            engine.Param.CalcExpression(this.exp);
        }
    }
}

