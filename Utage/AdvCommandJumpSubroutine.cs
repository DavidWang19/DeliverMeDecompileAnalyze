namespace Utage
{
    using System;
    using UnityEngine;

    internal class AdvCommandJumpSubroutine : AdvCommand
    {
        private ExpressionParser exp;
        private string jumpLabel;
        private string returnLabel;
        private string scenarioLabel;
        private int subroutineCommandIndex;

        public AdvCommandJumpSubroutine(StringGridRow row, AdvSettingDataManager dataManager) : base(row)
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
        }

        public override void DoCommand(AdvEngine engine)
        {
            if (this.IsEnable(engine.Param))
            {
                SubRoutineInfo calledInfo = new SubRoutineInfo(engine, this.returnLabel, this.scenarioLabel, this.subroutineCommandIndex);
                base.CurrentTread.JumpManager.RegistoreSubroutine(this.jumpLabel, calledInfo);
            }
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

        public override bool IsTypePage()
        {
            return true;
        }

        public override bool IsTypePageEnd()
        {
            return true;
        }
    }
}

