namespace Utage
{
    using System;
    using UnityEngine;

    internal class AdvCommandJumpRandom : AdvCommand
    {
        private ExpressionParser exp;
        private ExpressionParser expRate;
        private string jumpLabel;

        public AdvCommandJumpRandom(StringGridRow row, AdvSettingDataManager dataManager) : base(row)
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
            string str2 = base.ParseCellOptional<string>(AdvColumnName.Arg3, string.Empty);
            if (string.IsNullOrEmpty(str2))
            {
                this.expRate = null;
            }
            else
            {
                this.expRate = dataManager.DefaultParam.CreateExpression(str2);
                if (this.expRate.ErrorMsg != null)
                {
                    Debug.LogError(base.ToErrorString(this.expRate.ErrorMsg));
                }
            }
        }

        private float CalcRate(AdvParamManager param)
        {
            if (this.expRate == null)
            {
                return 1f;
            }
            return param.CalcExpressionFloat(this.expRate);
        }

        public override void DoCommand(AdvEngine engine)
        {
            if (this.IsEnable(engine.Param))
            {
                base.CurrentTread.JumpManager.AddRandom(this, this.CalcRate(engine.Param));
            }
        }

        internal void DoRandomEnd(AdvEngine engine, AdvScenarioThread thread)
        {
            if (!string.IsNullOrEmpty(this.jumpLabel))
            {
                thread.JumpManager.ClearOnJump();
                thread.JumpManager.RegistoreLabel(this.jumpLabel);
            }
        }

        public override string[] GetExtraCommandIdArray(AdvCommand next)
        {
            if ((next != null) && (next is AdvCommandJumpRandom))
            {
                return null;
            }
            return new string[] { "JumpRandomEnd" };
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

