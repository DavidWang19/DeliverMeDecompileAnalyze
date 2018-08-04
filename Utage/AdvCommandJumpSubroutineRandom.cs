namespace Utage
{
    using System;
    using UnityEngine;

    internal class AdvCommandJumpSubroutineRandom : AdvCommand
    {
        private ExpressionParser exp;
        private ExpressionParser expRate;
        private string jumpLabel;
        private string returnLabel;
        private string scenarioLabel;
        private int subroutineCommandIndex;

        public AdvCommandJumpSubroutineRandom(StringGridRow row, AdvSettingDataManager dataManager) : base(row)
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
            this.returnLabel = !base.IsEmptyCell(AdvColumnName.Arg3) ? base.ParseScenarioLabel(AdvColumnName.Arg3) : string.Empty;
            string str2 = base.ParseCellOptional<string>(AdvColumnName.Arg4, string.Empty);
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

        internal void DoRandomEnd(AdvScenarioThread thread, AdvEngine engine)
        {
            SubRoutineInfo calledInfo = new SubRoutineInfo(engine, this.returnLabel, this.scenarioLabel, this.subroutineCommandIndex);
            thread.JumpManager.RegistoreSubroutine(this.jumpLabel, calledInfo);
        }

        public override string[] GetExtraCommandIdArray(AdvCommand next)
        {
            if ((next != null) && (next is AdvCommandJumpSubroutineRandom))
            {
                return null;
            }
            return new string[] { "JumpSubroutineRandomEnd" };
        }

        public override string[] GetJumpLabels()
        {
            if (string.IsNullOrEmpty(this.returnLabel))
            {
                return new string[] { this.jumpLabel };
            }
            return new string[] { this.jumpLabel, this.returnLabel };
        }

        public override void InitFromPageData(AdvScenarioPageData pageData)
        {
            this.scenarioLabel = pageData.ScenarioLabelData.ScenarioLabel;
            this.subroutineCommandIndex = pageData.ScenarioLabelData.CountSubroutineCommandIndex(this);
        }

        private bool IsEnable(AdvParamManager param)
        {
            return ((this.exp == null) || param.CalcExpressionBoolean(this.exp));
        }
    }
}

