namespace Utage
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.UI;

    public class CountDownManager : MonoBehaviour
    {
        public Slider countdown;
        public AdvEngine Engine;
        public UnityEvent OnCountDownFinished;
        public string timeupScenarioLabel;
        public float totalTime;

        private void AfterCountDown()
        {
            base.get_gameObject().SetActive(false);
            this.OnCountDownFinished.Invoke();
            this.Engine.EndScenario();
            this.Engine.StartScenarioLabel = this.timeupScenarioLabel;
            this.Engine.StartGame();
        }

        [DebuggerHidden]
        private IEnumerator CountingDown()
        {
            return new <CountingDown>c__Iterator0 { $this = this };
        }

        private void Start()
        {
            this.countdown.set_value(1f);
            base.get_gameObject().SetActive(false);
        }

        public void StartCountdown()
        {
            base.get_gameObject().SetActive(true);
            base.StartCoroutine("CountingDown");
        }

        public void StopCountdown()
        {
            base.StopCoroutine("CountingDown");
            base.get_gameObject().SetActive(false);
        }

        private void Update()
        {
        }

        [CompilerGenerated]
        private sealed class <CountingDown>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal bool $disposing;
            internal int $PC;
            internal CountDownManager $this;
            internal float <upTime>__0;

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
                        this.<upTime>__0 = this.$this.totalTime;
                        break;

                    case 1:
                        break;

                    default:
                        goto Label_00BD;
                }
                if (this.<upTime>__0 > 0f)
                {
                    this.$this.countdown.set_value(this.<upTime>__0 / this.$this.totalTime);
                    this.<upTime>__0 -= Time.get_deltaTime();
                    this.$current = null;
                    if (!this.$disposing)
                    {
                        this.$PC = 1;
                    }
                    return true;
                }
                this.$this.countdown.set_value(0f);
                this.$this.AfterCountDown();
                this.$PC = -1;
            Label_00BD:
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

