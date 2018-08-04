namespace Utage
{
    using System;
    using UnityEngine;

    internal class AdvCommandJump : AdvCommand
    {
        private ExpressionParser exp;
        private string jumpLabel;

        public AdvCommandJump(StringGridRow row, AdvSettingDataManager dataManager) : base(row)
        {
            this.jumpLabel = base.ParseScenarioLabel(AdvColumnName.Arg1);
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
            if (this.IsEnable(engine.Param))
            {
                base.CurrentTread.JumpManager.RegistoreLabel(this.jumpLabel);
            }
        }

        public override string[] GetJumpLabels()
        {
            return new string[] { this.jumpLabel };
        }

        private bool IsEnable(AdvParamManager param)
        {
            return ((this.exp == null) || param.CalcExpressionBoolean(this.exp));
        }
    }
}

