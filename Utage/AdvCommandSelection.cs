namespace Utage
{
    using System;
    using UnityEngine;

    public class AdvCommandSelection : AdvCommand
    {
        private ExpressionParser exp;
        private string jumpLabel;
        private string prefabName;
        private ExpressionParser selectedExp;
        private float? x;
        private float? y;

        public AdvCommandSelection(StringGridRow row, AdvSettingDataManager dataManager) : base(row)
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
            this.prefabName = base.ParseCellOptional<string>(AdvColumnName.Arg4, string.Empty);
            this.x = base.ParseCellOptionalNull<float>(AdvColumnName.Arg5);
            this.y = base.ParseCellOptionalNull<float>(AdvColumnName.Arg6);
            if (AdvCommand.IsEditorErrorCheck)
            {
                TextData data = new TextData(base.ParseCellLocalizedText());
                if (!string.IsNullOrEmpty(data.ErrorMsg))
                {
                    Debug.LogError(base.ToErrorString(data.ErrorMsg));
                }
            }
        }

        public override void DoCommand(AdvEngine engine)
        {
            if ((this.exp == null) || engine.Param.CalcExpressionBoolean(this.exp))
            {
                engine.SelectionManager.AddSelection(this.jumpLabel, base.ParseCellLocalizedText(), this.selectedExp, this.prefabName, this.x, this.y, base.RowData);
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

