namespace Utage
{
    using System;
    using UnityEngine;

    internal class AdvCommandSelectionClick : AdvCommand
    {
        private ExpressionParser exp;
        private bool isPolygon;
        private string jumpLabel;
        private string name;
        private ExpressionParser selectedExp;

        public AdvCommandSelectionClick(StringGridRow row, AdvSettingDataManager dataManager) : base(row)
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
                this.selectedExp = null;
            }
            else
            {
                this.selectedExp = dataManager.DefaultParam.CreateExpression(str2);
                if (this.selectedExp.ErrorMsg != null)
                {
                    Debug.LogError(base.ToErrorString(this.selectedExp.ErrorMsg));
                }
            }
            this.name = base.ParseCell<string>(AdvColumnName.Arg4);
            this.isPolygon = base.ParseCellOptional<bool>(AdvColumnName.Arg5, true);
        }

        public override void DoCommand(AdvEngine engine)
        {
            if ((this.exp == null) || engine.Param.CalcExpressionBoolean(this.exp))
            {
                engine.SelectionManager.AddSelectionClick(this.jumpLabel, this.name, this.isPolygon, this.selectedExp, base.RowData);
            }
        }

        public override string[] GetExtraCommandIdArray(AdvCommand next)
        {
            if ((next != null) && ((next is AdvCommandSelection) || (next is AdvCommandSelectionClick)))
            {
                return null;
            }
            if (AdvPageController.IsPageEndType(base.ParseCellOptional<AdvPageControllerType>(AdvColumnName.PageCtrl, AdvPageControllerType.InputBrPage)))
            {
                return new string[] { "SelectionEnd", "PageControl" };
            }
            return new string[] { "SelectionEnd" };
        }

        public override string[] GetJumpLabels()
        {
            return new string[] { this.jumpLabel };
        }

        public override bool IsTypePage()
        {
            return true;
        }
    }
}

