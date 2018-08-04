namespace Utage
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;
    using UnityEngine.Events;

    public class WaitTimer : CustomYieldInstruction
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private float <Time>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private float <Time01>k__BackingField;
        private float delay;
        private float duration;
        private float initTime;
        private bool isStarted;
        private UnityAction<WaitTimer> onComplete;
        private UnityAction<WaitTimer> onStart;
        private UnityAction<WaitTimer> onUpdate;

        public WaitTimer(float duration, UnityAction<WaitTimer> onStart = null, UnityAction<WaitTimer> onUpdate = null, UnityAction<WaitTimer> onComplete = null)
        {
            this.Init(duration, 0f, onStart, onUpdate, onComplete);
        }

        public WaitTimer(float duration, float delay, UnityAction<WaitTimer> onStart = null, UnityAction<WaitTimer> onUpdate = null, UnityAction<WaitTimer> onComplete = null)
        {
            this.Init(duration, delay, onStart, onUpdate, onComplete);
        }

        private void Init(float duration, float delay, UnityAction<WaitTimer> onStart, UnityAction<WaitTimer> onUpdate, UnityAction<WaitTimer> onComplete)
        {
            this.duration = duration;
            this.delay = delay;
            this.initTime = UnityEngine.Time.get_time();
            this.onStart = onStart;
            this.onUpdate = onUpdate;
            this.onComplete = onComplete;
        }

        private bool Waiting()
        {
            float num = UnityEngine.Time.get_time();
            if (num < this.StartTimeDelyed)
            {
                return true;
            }
            this.Time = num - this.StartTimeDelyed;
            this.Time01 = Mathf.Min(this.Time / this.duration, 1f);
            if (!this.isStarted)
            {
                if (this.onStart != null)
                {
                    this.onStart.Invoke(this);
                }
                this.isStarted = true;
            }
            if (this.onUpdate != null)
            {
                this.onUpdate.Invoke(this);
            }
            if (num < this.EndTime)
            {
                return true;
            }
            if (this.onComplete != null)
            {
                this.onComplete.Invoke(this);
            }
            return false;
        }

        private float EndTime
        {
            get
            {
                return (this.StartTimeDelyed + this.duration);
            }
        }

        public override bool keepWaiting
        {
            get
            {
                return this.Waiting();
            }
        }

        private float StartTimeDelyed
        {
            get
            {
                return (this.initTime + this.delay);
            }
        }

        public float Time { get; protected set; }

        public float Time01 { get; protected set; }
    }
}

