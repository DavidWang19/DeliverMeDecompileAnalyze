namespace Utage
{
    using System;
    using UnityEngine;

    public class AdvCommandJumpParam : AdvCommand
    {
        private ExpressionParser exp;
        private string paramName;
        private StringGridRow thisRow;

        public AdvCommandJumpParam(StringGridRow row, AdvSettingDataManager dataManager) : base(row)
        {
            this.paramName = AdvParser.ParseCell<string>(row, AdvColumnName.Arg1);
            this.thisRow = row;
            string str = base.ParseCellOptional<string>(AdvColumnName.Arg2, string.Empty);
            if (string.IsNullOrEmpty(str))
            {
                this.exp = null;
            }
            else
            {
                this.exp = dataManager.DefaultParam.CreateExpressionBoolean(str);
                if (this.exp.ErrorMsg != null)
                {
                    Debug.LogError(base.ToErrorString(this.exp.ErrorMsg));
                }
            }
        }

        public override void DoCommand(AdvEngine engine)
        {
            string parameter = (string) engine.Param.GetParameter(this.paramName);
            if ((parameter.Length >= 3) && (parameter[1] == '*'))
            {
                parameter = this.thisRow.Grid.SheetName + '*' + parameter.Substring(2);
            }
            else
            {
                parameter = parameter.Substring(1);
            }
            if (this.IsEnable(engine.Param))
            {
                base.CurrentTread.JumpManager.RegistoreLabel(parameter);
            }
        }

        private bool IsEnable(AdvParamManager param)
        {
            return ((this.exp == null) || param.CalcExpressionBoolean(this.exp));
        }
    }
}

