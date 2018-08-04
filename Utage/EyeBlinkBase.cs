namespace Utage
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public abstract class EyeBlinkBase : MonoBehaviour
    {
        [SerializeField]
        private MiniAnimationData animationData;
        [SerializeField]
        private string eyeTag;
        [SerializeField, Range(0f, 1f)]
        private float intervalDoubleEyeBlink;
        [SerializeField, MinMax(0f, 10f, "min", "max")]
        private MinMaxFloat intervalTime;
        [SerializeField, Range(0f, 1f)]
        private float randomDoubleEyeBlink;

        protected EyeBlinkBase()
        {
            MinMaxFloat num = new MinMaxFloat {
                Min = 2f,
                Max = 6f
            };
            this.intervalTime = num;
            this.randomDoubleEyeBlink = 0.2f;
            this.intervalDoubleEyeBlink = 0.01f;
            this.eyeTag = "eye";
            this.animationData = new MiniAnimationData();
        }

        [DebuggerHidden]
        private IEnumerator CoDoubleEyeBlink()
        {
            return new <CoDoubleEyeBlink>c__Iterator1 { $this = this };
        }

        protected abstract IEnumerator CoEyeBlink(Action onComplete);
        [DebuggerHidden]
        private IEnumerator CoUpateWaiting(float waitTime)
        {
            return new <CoUpateWaiting>c__Iterator0 { waitTime = waitTime, $this = this };
        }

        private void OnEndBlink()
        {
            if (this.randomDoubleEyeBlink > Random.get_value())
            {
                base.StartCoroutine(this.CoDoubleEyeBlink());
            }
            else
            {
                this.StartWaiting();
            }
        }

        private void Start()
        {
            this.StartWaiting();
        }

        private void StartWaiting()
        {
            float waitTime = this.intervalTime.RandomRange();
            base.StartCoroutine(this.CoUpateWaiting(waitTime));
        }

        public MiniAnimationData AnimationData
        {
            get
            {
                return this.animationData;
            }
            set
            {
                this.animationData = value;
            }
        }

        public string EyeTag
        {
            get
            {
                return this.eyeTag;
            }
            set
            {
                this.eyeTag = value;
            }
        }

        public MinMaxFloat IntervalTime
        {
            get
            {
                return this.intervalTime;
            }
            set
            {
                this.intervalTime = value;
            }
        }

        public float RandomDoubleEyeBlink
        {
            get
            {
                return this.randomDoubleEyeBlink;
            }
            set
            {
                this.randomDoubleEyeBlink = value;
            }
        }

        [CompilerGenerated]
        private sealed class <CoDoubleEyeBlink>c__Iterator1 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal bool $disposing;
            internal int $PC;
            internal EyeBlinkBase $this;

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
                        this.$current = new WaitForSeconds(this.$this.intervalDoubleEyeBlink);
                        if (!this.$disposing)
                        {
                            this.$PC = 1;
                        }
                        return true;

                    case 1:
                        this.$this.StartCoroutine(this.$this.CoEyeBlink(new Action(this.$this.StartWaiting)));
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

        [CompilerGenerated]
        private sealed class <CoUpateWaiting>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal bool $disposing;
            internal int $PC;
            internal EyeBlinkBase $this;
            internal float waitTime;

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
                        this.$current = new WaitForSeconds(this.waitTime);
                        if (!this.$disposing)
                        {
                            this.$PC = 1;
                        }
                        return true;

                    case 1:
                        this.$this.StartCoroutine(this.$this.CoEyeBlink(new Action(this.$this.OnEndBlink)));
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

