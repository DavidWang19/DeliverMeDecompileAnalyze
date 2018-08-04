namespace Utage
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;

    [AddComponentMenu("Utage/Lib/Effect/CurveAnimation")]
    public class CurveAnimation : MonoBehaviour
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private float <CurrentAnimationTime>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <IsPlaying>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private float <Value>k__BackingField;
        private Coroutine currentCoroutine;
        [SerializeField]
        private AnimationCurve curve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
        [SerializeField]
        private float delay;
        [SerializeField]
        private float duration = 1f;
        [SerializeField]
        private CurveAnimationEvent onComplete = new CurveAnimationEvent();
        [SerializeField]
        private CurveAnimationEvent onStart = new CurveAnimationEvent();
        [SerializeField]
        private CurveAnimationEvent onUpdate = new CurveAnimationEvent();
        [SerializeField]
        private bool unscaledTime = true;

        [DebuggerHidden]
        private IEnumerator CoAnimation(Action<float> onUpdate, Action onComplete)
        {
            return new <CoAnimation>c__Iterator0 { onUpdate = onUpdate, onComplete = onComplete, $this = this };
        }

        public float LerpValue(float from, float to)
        {
            return Mathf.Lerp(from, to, this.Value);
        }

        public void PlayAnimation()
        {
            this.PlayAnimation(null, null);
        }

        public void PlayAnimation(Action<float> onUpdate = null, Action onComplete = null)
        {
            if (this.IsPlaying)
            {
                base.StopCoroutine(this.currentCoroutine);
            }
            this.currentCoroutine = base.StartCoroutine(this.CoAnimation(onUpdate, onComplete));
        }

        protected float CurrentAnimationTime { get; set; }

        public AnimationCurve Curve
        {
            get
            {
                return this.curve;
            }
        }

        public float Delay
        {
            get
            {
                return this.delay;
            }
            set
            {
                this.delay = value;
            }
        }

        protected float DeltaTime
        {
            get
            {
                return TimeUtil.GetDeltaTime(this.UnscaledTime);
            }
        }

        public float Duration
        {
            get
            {
                return this.duration;
            }
            set
            {
                this.duration = value;
            }
        }

        public bool IsPlaying { get; protected set; }

        public CurveAnimationEvent OnComplete
        {
            get
            {
                return this.onComplete;
            }
        }

        public CurveAnimationEvent OnStart
        {
            get
            {
                return this.onStart;
            }
        }

        public CurveAnimationEvent OnUpdate
        {
            get
            {
                return this.onUpdate;
            }
        }

        protected float Time
        {
            get
            {
                return TimeUtil.GetTime(this.UnscaledTime);
            }
        }

        public bool UnscaledTime
        {
            get
            {
                return this.unscaledTime;
            }
            set
            {
                this.unscaledTime = value;
            }
        }

        public float Value { get; set; }

        [CompilerGenerated]
        private sealed class <CoAnimation>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal bool $disposing;
            internal int $PC;
            internal CurveAnimation $this;
            internal float <delayStartTime>__1;
            internal float <endTime>__0;
            internal float <startTime>__0;
            internal Action onComplete;
            internal Action<float> onUpdate;

            [DebuggerHidden]
            public void Dispose()
            {
                this.$disposing = true;
                this.$PC = -1;
            }

            public bool MoveNext()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                switch (num)
                {
                    case 0:
                        this.$this.IsPlaying = true;
                        if (this.$this.Delay < 0f)
                        {
                            goto Label_0099;
                        }
                        this.<delayStartTime>__1 = this.$this.Time;
                        break;

                    case 1:
                        break;

                    case 2:
                        goto Label_01B2;

                    default:
                        goto Label_028D;
                }
                if ((this.$this.Time - this.<delayStartTime>__1) < this.$this.Delay)
                {
                    this.$current = null;
                    if (!this.$disposing)
                    {
                        this.$PC = 1;
                    }
                    goto Label_028F;
                }
            Label_0099:
                this.<endTime>__0 = this.$this.Curve.get_keys()[this.$this.Curve.get_length() - 1].get_time();
                this.$this.Value = this.$this.Curve.Evaluate(0f);
                this.$this.OnStart.Invoke(this.$this);
                this.<startTime>__0 = this.$this.Time;
                this.$this.CurrentAnimationTime = 0f;
                while (this.$this.CurrentAnimationTime < this.$this.Duration)
                {
                    this.$this.Value = this.$this.Curve.Evaluate((this.<endTime>__0 * this.$this.CurrentAnimationTime) / this.$this.Duration);
                    if (this.onUpdate != null)
                    {
                        this.onUpdate(this.$this.Value);
                    }
                    this.$this.OnUpdate.Invoke(this.$this);
                    this.$current = null;
                    if (!this.$disposing)
                    {
                        this.$PC = 2;
                    }
                    goto Label_028F;
                Label_01B2:
                    this.$this.CurrentAnimationTime = this.$this.Time - this.<startTime>__0;
                }
                this.$this.Value = this.$this.Curve.Evaluate(this.<endTime>__0);
                if (this.onUpdate != null)
                {
                    this.onUpdate(this.$this.Value);
                }
                this.$this.OnUpdate.Invoke(this.$this);
                if (this.onComplete != null)
                {
                    this.onComplete();
                }
                this.$this.OnComplete.Invoke(this.$this);
                this.$this.IsPlaying = false;
                this.$this.currentCoroutine = null;
                this.$PC = -1;
            Label_028D:
                return false;
            Label_028F:
                return true;
            }

            [DebuggerHidden]
            public void Reset()
            {
                throw new NotSupportedException();
            }

            object IEnumerator<object>.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.$current;
                }
            }

            object IEnumerator.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.$current;
                }
            }
        }
    }
}

