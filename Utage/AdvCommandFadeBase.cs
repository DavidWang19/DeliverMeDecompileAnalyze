namespace Utage
{
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    internal abstract class AdvCommandFadeBase : AdvCommandEffectBase
    {
        private Color color;
        private bool inverse;
        private float time;

        public AdvCommandFadeBase(StringGridRow row, bool inverse) : base(row)
        {
            this.inverse = inverse;
        }

        protected override void OnParse()
        {
            this.color = base.ParseCellOptional<Color>(AdvColumnName.Arg1, Color.get_white());
            if (base.IsEmptyCell(AdvColumnName.Arg2))
            {
                base.targetName = "SpriteCamera";
            }
            else
            {
                base.targetName = base.ParseCell<string>(AdvColumnName.Arg2);
            }
            this.time = base.ParseCellOptional<float>(AdvColumnName.Arg6, 0.2f);
            base.targetType = AdvEffectManager.TargetType.Camera;
            this.ParseWait(AdvColumnName.WaitType);
        }

        protected override void OnStartEffect(GameObject target, AdvEngine engine, AdvScenarioThread thread)
        {
            bool flag;
            <OnStartEffect>c__AnonStorey0 storey = new <OnStartEffect>c__AnonStorey0 {
                thread = thread,
                $this = this
            };
            Camera componentInChildren = target.GetComponentInChildren<Camera>(true);
            ImageEffectUtil.TryGetComonentCreateIfMissing(ImageEffectType.ColorFade.ToString(), out storey.imageEffect, out flag, componentInChildren.get_gameObject());
            storey.colorFade = storey.imageEffect as ColorFade;
            if (this.inverse)
            {
                storey.start = storey.colorFade.color.a;
                storey.end = 0f;
            }
            else
            {
                storey.start = !flag ? 0f : storey.colorFade.Strength;
                storey.end = this.color.a;
            }
            storey.imageEffect.set_enabled(true);
            storey.colorFade.color = this.color;
            Timer timer = componentInChildren.get_gameObject().AddComponent<Timer>();
            timer.AutoDestroy = true;
            timer.StartTimer(engine.Page.ToSkippedTime(this.time), new Action<Timer>(storey.<>m__0), new Action<Timer>(storey.<>m__1), 0f);
        }

        [CompilerGenerated]
        private sealed class <OnStartEffect>c__AnonStorey0
        {
            internal AdvCommandFadeBase $this;
            internal ColorFade colorFade;
            internal float end;
            internal ImageEffectBase imageEffect;
            internal float start;
            internal AdvScenarioThread thread;

            internal void <>m__0(Timer x)
            {
                this.colorFade.Strength = x.GetCurve(this.start, this.end);
            }

            internal void <>m__1(Timer x)
            {
                this.$this.OnComplete(this.thread);
                if (this.$this.inverse)
                {
                    Object.Destroy(this.imageEffect);
                    this.imageEffect.set_enabled(false);
                }
            }
        }
    }
}

