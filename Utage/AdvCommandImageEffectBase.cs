namespace Utage
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    internal class AdvCommandImageEffectBase : AdvCommandEffectBase
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <imageEffectType>k__BackingField;
        private string animationName;
        private bool inverse;
        private float time;

        public AdvCommandImageEffectBase(StringGridRow row, AdvSettingDataManager dataManager, bool inverse) : base(row)
        {
            this.inverse = inverse;
            base.targetType = AdvEffectManager.TargetType.Camera;
            AdvColumnName name = AdvColumnName.Arg2;
            this.imageEffectType = base.RowData.ParseCell<string>(name.ToString());
            this.animationName = base.ParseCellOptional<string>(AdvColumnName.Arg3, string.Empty);
            this.time = base.ParseCellOptional<float>(AdvColumnName.Arg6, 0f);
        }

        private void Complete(ImageEffectBase imageEffect, AdvScenarioThread thread)
        {
            if (this.inverse)
            {
                Object.Destroy(imageEffect);
            }
            this.OnComplete(thread);
        }

        protected override void OnStartEffect(GameObject target, AdvEngine engine, AdvScenarioThread thread)
        {
            bool flag;
            <OnStartEffect>c__AnonStorey1 storey = new <OnStartEffect>c__AnonStorey1 {
                thread = thread,
                $this = this
            };
            Camera componentInChildren = target.GetComponentInChildren<Camera>(true);
            if (!ImageEffectUtil.TryGetComonentCreateIfMissing(this.imageEffectType, out storey.imageEffect, out flag, componentInChildren.get_gameObject()))
            {
                this.Complete(storey.imageEffect, storey.thread);
            }
            else
            {
                if (!this.inverse)
                {
                    storey.imageEffect.set_enabled(true);
                }
                storey.enableAnimation = !string.IsNullOrEmpty(this.animationName);
                bool flag2 = storey.imageEffect is IImageEffectStrength;
                if (!flag2 && !storey.enableAnimation)
                {
                    this.Complete(storey.imageEffect, storey.thread);
                }
                else
                {
                    if (flag2)
                    {
                        <OnStartEffect>c__AnonStorey0 storey2;
                        storey2 = new <OnStartEffect>c__AnonStorey0 {
                            <>f__ref$1 = storey,
                            fade = storey.imageEffect as IImageEffectStrength,
                            start = !this.inverse ? 0f : storey2.fade.Strength,
                            end = !this.inverse ? ((float) 1) : ((float) 0)
                        };
                        Timer timer = componentInChildren.get_gameObject().AddComponent<Timer>();
                        timer.AutoDestroy = true;
                        timer.StartTimer(engine.Page.ToSkippedTime(this.time), new Action<Timer>(storey2.<>m__0), new Action<Timer>(storey2.<>m__1), 0f);
                    }
                    if (storey.enableAnimation)
                    {
                        AdvAnimationData data = engine.DataManager.SettingDataManager.AnimationSetting.Find(this.animationName);
                        if (data == null)
                        {
                            Debug.LogError(base.RowData.ToErrorString("Animation " + this.animationName + " is not found"));
                            this.Complete(storey.imageEffect, storey.thread);
                        }
                        else
                        {
                            AdvAnimationPlayer player = componentInChildren.get_gameObject().AddComponent<AdvAnimationPlayer>();
                            player.AutoDestory = true;
                            player.EnableSave = true;
                            player.Play(data.Clip, engine.Page.SkippedSpeed, new Action(storey.<>m__0));
                        }
                    }
                }
            }
        }

        private string imageEffectType { get; set; }

        [CompilerGenerated]
        private sealed class <OnStartEffect>c__AnonStorey0
        {
            internal AdvCommandImageEffectBase.<OnStartEffect>c__AnonStorey1 <>f__ref$1;
            internal float end;
            internal IImageEffectStrength fade;
            internal float start;

            internal void <>m__0(Timer x)
            {
                this.fade.Strength = x.GetCurve(this.start, this.end);
            }

            internal void <>m__1(Timer x)
            {
                if (!this.<>f__ref$1.enableAnimation)
                {
                    this.<>f__ref$1.$this.Complete(this.<>f__ref$1.imageEffect, this.<>f__ref$1.thread);
                }
            }
        }

        [CompilerGenerated]
        private sealed class <OnStartEffect>c__AnonStorey1
        {
            internal AdvCommandImageEffectBase $this;
            internal bool enableAnimation;
            internal ImageEffectBase imageEffect;
            internal AdvScenarioThread thread;

            internal void <>m__0()
            {
                this.$this.Complete(this.imageEffect, this.thread);
            }
        }
    }
}

