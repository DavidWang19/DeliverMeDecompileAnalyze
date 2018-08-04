namespace Utage
{
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class AdvCommandTween : AdvCommandEffectBase
    {
        protected iTweenData tweenData;

        public AdvCommandTween(StringGridRow row, AdvSettingDataManager dataManager) : base(row)
        {
            this.InitTweenData();
            if (this.tweenData.Type == iTweenType.Stop)
            {
                base.WaitType = AdvCommandWaitType.Add;
            }
            if (!string.IsNullOrEmpty(this.tweenData.ErrorMsg))
            {
                Debug.LogError(base.ToErrorString(this.tweenData.ErrorMsg));
            }
        }

        protected virtual void InitTweenData()
        {
            string type = base.ParseCell<string>(AdvColumnName.Arg2);
            string arg = base.ParseCellOptional<string>(AdvColumnName.Arg3, string.Empty);
            string easeType = base.ParseCellOptional<string>(AdvColumnName.Arg4, string.Empty);
            string loopType = base.ParseCellOptional<string>(AdvColumnName.Arg5, string.Empty);
            this.tweenData = new iTweenData(type, arg, easeType, loopType);
        }

        private bool IsUnder2DSpace(GameObject target)
        {
            AdvEffectManager.TargetType targetType = base.targetType;
            if (targetType != AdvEffectManager.TargetType.MessageWindow)
            {
                return ((targetType == AdvEffectManager.TargetType.Default) && (target.GetComponent<AdvGraphicObject>() != null));
            }
            return true;
        }

        protected override void OnParse()
        {
            this.ParseEffectTarget(AdvColumnName.Arg1);
            if (!base.IsEmptyCell(AdvColumnName.WaitType))
            {
                this.ParseWait(AdvColumnName.WaitType);
            }
            else if (!base.IsEmptyCell(AdvColumnName.Arg6))
            {
                this.ParseWait(AdvColumnName.Arg6);
            }
            else
            {
                this.ParseWait(AdvColumnName.WaitType);
            }
        }

        protected override void OnStartEffect(GameObject target, AdvEngine engine, AdvScenarioThread thread)
        {
            <OnStartEffect>c__AnonStorey0 storey = new <OnStartEffect>c__AnonStorey0 {
                thread = thread,
                $this = this
            };
            if (!string.IsNullOrEmpty(this.tweenData.ErrorMsg))
            {
                Debug.LogError(this.tweenData.ErrorMsg);
                this.OnComplete(storey.thread);
            }
            else
            {
                AdvITweenPlayer player = target.AddComponent<AdvITweenPlayer>();
                float skipSpeed = !engine.Page.CheckSkip() ? 0f : engine.Config.SkipSpped;
                player.Init(this.tweenData, this.IsUnder2DSpace(target), engine.GraphicManager.PixelsToUnits, skipSpeed, new Action<AdvITweenPlayer>(storey.<>m__0));
                player.Play();
                if (player.IsEndlessLoop)
                {
                }
            }
        }

        [CompilerGenerated]
        private sealed class <OnStartEffect>c__AnonStorey0
        {
            internal AdvCommandTween $this;
            internal AdvScenarioThread thread;

            internal void <>m__0(AdvITweenPlayer x)
            {
                this.$this.OnComplete(this.thread);
            }
        }
    }
}

