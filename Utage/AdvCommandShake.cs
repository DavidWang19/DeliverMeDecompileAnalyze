namespace Utage
{
    using System;

    internal class AdvCommandShake : AdvCommandTween
    {
        public AdvCommandShake(StringGridRow row, AdvSettingDataManager dataManager) : base(row, dataManager)
        {
        }

        protected override void InitTweenData()
        {
            string defaultVal = " x=30 y=30";
            string arg = base.ParseCellOptional<string>(AdvColumnName.Arg3, defaultVal);
            if (!arg.Contains("x=") && !arg.Contains("y="))
            {
                arg = arg + defaultVal;
            }
            string easeType = base.ParseCellOptional<string>(AdvColumnName.Arg4, string.Empty);
            string loopType = base.ParseCellOptional<string>(AdvColumnName.Arg5, string.Empty);
            iTweenType shakePosition = iTweenType.ShakePosition;
            base.tweenData = new iTweenData(shakePosition.ToString(), arg, easeType, loopType);
        }
    }
}

