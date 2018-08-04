namespace Utage
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;
    using UnityEngine.Events;

    [AddComponentMenu("Utage/Lib/Sound/Timer")]
    public class Timer : MonoBehaviour
    {
        [SerializeField]
        private bool autoDestroy;
        [SerializeField]
        private bool autoStart;
        private Action<Timer> callbackComplete;
        private Action<Timer> callbackUpdate;
        [SerializeField]
        private float delay;
        [SerializeField]
        private float duration;
        public TimerEvent onComplete = new TimerEvent();
        public TimerEvent onStart = new TimerEvent();
        public TimerEvent onUpdate = new TimerEvent();
        [SerializeField, NotEditable]
        private float time;
        [SerializeField, NotEditable]
        private float time01;

        public void Cancel()
        {
            this.OnCompleteCallback();
            this.Stop();
        }

        [DebuggerHidden]
        private IEnumerator CoTimer(float duration, float delay)
        {
            return new <CoTimer>c__Iterator0 { duration = duration, delay = delay, $this = this };
        }

        public float GetCurve(float start, float end)
        {
            return this.GetCurve(start, end, EaseType.Linear);
        }

        public Vector2 GetCurve(Vector2 start, Vector2 end)
        {
            return this.GetCurve(start, end, EaseType.Linear);
        }

        public Vector3 GetCurve(Vector3 start, Vector3 end)
        {
            return this.GetCurve(start, end, EaseType.Linear);
        }

        public Vector4 GetCurve(Vector4 start, Vector4 end)
        {
            return this.GetCurve(start, end, EaseType.Linear);
        }

        public float GetCurve(float start, float end, EaseType easeType)
        {
            return Easing.GetCurve(start, end, this.Time01, easeType);
        }

        public Vector2 GetCurve(Vector2 start, Vector2 end, EaseType easeType)
        {
            return Easing.GetCurve(start, end, this.Time01, easeType);
        }

        public Vector3 GetCurve(Vector3 start, Vector3 end, EaseType easeType)
        {
            return Easing.GetCurve(start, end, this.Time01, easeType);
        }

        public Vector4 GetCurve(Vector4 start, Vector4 end, EaseType easeType)
        {
            return Easing.GetCurve(start, end, this.Time01, easeType);
        }

        public float GetCurve01(EaseType easeType = 0)
        {
            return Easing.GetCurve01(this.Time01, easeType);
        }

        public float GetCurve01Inverse(EaseType easeType = 0)
        {
            return Easing.GetCurve01(this.Time01Inverse, easeType);
        }

        public float GetCurveInverse(float start, float end)
        {
            return this.GetCurveInverse(start, end, EaseType.Linear);
        }

        public Vector2 GetCurveInverse(Vector2 start, Vector2 end)
        {
            return this.GetCurveInverse(start, end, EaseType.Linear);
        }

        public Vector3 GetCurveInverse(Vector3 start, Vector3 end)
        {
            return this.GetCurveInverse(start, end, EaseType.Linear);
        }

        public Vector4 GetCurveInverse(Vector4 start, Vector4 end)
        {
            return this.GetCurveInverse(start, end, EaseType.Linear);
        }

        public float GetCurveInverse(float start, float end, EaseType easeType)
        {
            return Easing.GetCurve(start, end, this.Time01Inverse, easeType);
        }

        public Vector2 GetCurveInverse(Vector2 start, Vector2 end, EaseType easeType)
        {
            return Easing.GetCurve(start, end, this.Time01Inverse, easeType);
        }

        public Vector3 GetCurveInverse(Vector3 start, Vector3 end, EaseType easeType)
        {
            return Easing.GetCurve(start, end, this.Time01Inverse, easeType);
        }

        public Vector4 GetCurveInverse(Vector4 start, Vector4 end, EaseType easeType)
        {
            return Easing.GetCurve(start, end, this.Time01Inverse, easeType);
        }

        private void OnComplete(WaitTimer timer)
        {
            this.OnCompleteCallback();
            if (this.AutoDestroy)
            {
                Object.Destroy(this);
            }
        }

        private void OnCompleteCallback()
        {
            this.onComplete.Invoke(this);
            if (this.callbackComplete != null)
            {
                this.callbackComplete(this);
            }
            this.callbackComplete = null;
        }

        private void OnStart(WaitTimer timer)
        {
            this.onStart.Invoke(this);
        }

        private void OnUpdate(WaitTimer timer)
        {
            this.Time = timer.Time;
            this.Time01 = timer.Time01;
            this.onUpdate.Invoke(this);
            if (this.callbackUpdate != null)
            {
                this.callbackUpdate(this);
            }
        }

        private void Start()
        {
            if (this.autoStart)
            {
                base.StartCoroutine(this.CoTimer(this.duration, this.delay));
            }
        }

        public void StartTimer(float duration, float delay = 0f)
        {
            this.autoStart = false;
            this.Stop();
            base.StartCoroutine(this.CoTimer(duration, delay));
        }

        public void StartTimer(float duration, Action<Timer> onUpdate = null, Action<Timer> onComplete = null, float delay = 0f)
        {
            this.callbackUpdate = onUpdate;
            this.callbackComplete = onComplete;
            this.StartTimer(duration, delay);
        }

        private void Stop()
        {
            base.StopAllCoroutines();
        }

        public bool AutoDestroy
        {
            get
            {
                return this.autoDestroy;
            }
            set
            {
                this.autoDestroy = value;
            }
        }

        public float Delay
        {
            get
            {
                return this.delay;
            }
            protected set
            {
                this.delay = value;
            }
        }

        public float Duration
        {
            get
            {
                return this.duration;
            }
            protected set
            {
                this.duration = value;
            }
        }

        public float Time
        {
            get
            {
                return this.time;
            }
            protected set
            {
                this.time = value;
            }
        }

        public float Time01
        {
            get
            {
                return this.time01;
            }
            protected set
            {
                this.time01 = value;
            }
        }

        public float Time01Inverse
        {
            get
            {
                return (1f - this.Time01);
            }
        }

        [CompilerGenerated]
        private sealed class <CoTimer>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal bool $disposing;
            internal int $PC;
            internal Timer $this;
            internal WaitTimer <timer>__0;
            internal float delay;
            internal float duration;

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
                        this.$this.duration = this.duration;
                        this.$this.delay = this.delay;
                        this.<timer>__0 = new WaitTimer(this.duration, this.delay, new UnityAction<WaitTimer>(this.$this, (IntPtr) this.OnStart), new UnityAction<WaitTimer>(this.$this, (IntPtr) this.OnUpdate), new UnityAction<WaitTimer>(this.$this, (IntPtr) this.OnComplete));
                        this.$current = this.<timer>__0;
                        if (!this.$disposing)
                        {
                            this.$PC = 1;
                        }
                        return true;

                    case 1:
                        this.$PC = -1;
                        break;
                }
                return false;
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

