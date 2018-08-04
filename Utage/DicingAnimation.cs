namespace Utage
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UtageExtensions;

    [RequireComponent(typeof(DicingImage)), AddComponentMenu("Utage/Lib/UI/DicingAnimation")]
    public class DicingAnimation : MonoBehaviour
    {
        private DicingImage dicing;
        [SerializeField]
        private float frameRate = 15f;
        [SerializeField]
        private bool playOnAwake;
        [SerializeField]
        private bool reverse;
        [SerializeField, LimitEnum(new string[] { "Default", "Loop", "PingPong" })]
        private MotionPlayType wrapMode;

        private void Awake()
        {
            if (this.playOnAwake)
            {
                this.Play(null);
            }
        }

        [DebuggerHidden]
        private IEnumerator CoPlay(Action onComplete)
        {
            return new <CoPlay>c__Iterator0 { onComplete = onComplete, $this = this };
        }

        [DebuggerHidden]
        private IEnumerator CoPlayOnce(List<string> patternList)
        {
            return new <CoPlayOnce>c__Iterator1 { patternList = patternList, $this = this };
        }

        public void Play(Action onComplete)
        {
            base.StartCoroutine(this.CoPlay(onComplete));
        }

        private DicingImage Dicing
        {
            get
            {
                return base.get_gameObject().GetComponentCache<DicingImage>(ref this.dicing);
            }
        }

        [CompilerGenerated]
        private sealed class <CoPlay>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal bool $disposing;
            internal int $PC;
            internal DicingAnimation $this;
            internal bool <isEnd>__1;
            internal List<string> <list>__0;
            internal Action onComplete;

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
                        this.<list>__0 = this.$this.Dicing.DicingData.GetPattenNameList();
                        if (this.$this.reverse)
                        {
                            this.<list>__0.Reverse();
                        }
                        if (this.<list>__0.Count <= 0)
                        {
                            goto Label_0104;
                        }
                        this.<isEnd>__1 = false;
                        break;

                    case 1:
                        switch (this.$this.wrapMode)
                        {
                            case MotionPlayType.Default:
                                goto Label_00C2;

                            case MotionPlayType.Loop:
                                goto Label_00F9;

                            case MotionPlayType.PingPong:
                                goto Label_00D3;
                        }
                        goto Label_00E3;

                    default:
                        goto Label_0126;
                }
            Label_00F9:
                while (!this.<isEnd>__1)
                {
                    this.$current = this.$this.CoPlayOnce(this.<list>__0);
                    if (!this.$disposing)
                    {
                        this.$PC = 1;
                    }
                    return true;
                Label_00C2:
                    this.<isEnd>__1 = true;
                    continue;
                Label_00D3:
                    this.<list>__0.Reverse();
                    continue;
                Label_00E3:
                    Debug.LogError("NotSupport");
                    this.<isEnd>__1 = true;
                }
            Label_0104:
                if (this.onComplete != null)
                {
                    this.onComplete();
                    goto Label_0126;
                    this.$PC = -1;
                }
            Label_0126:
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
        private sealed class <CoPlayOnce>c__Iterator1 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal bool $disposing;
            internal List<string>.Enumerator $locvar0;
            internal int $PC;
            internal DicingAnimation $this;
            internal string <pattern>__1;
            internal List<string> patternList;

            [DebuggerHidden]
            public void Dispose()
            {
                uint num = (uint) this.$PC;
                this.$disposing = true;
                this.$PC = -1;
                switch (num)
                {
                    case 1:
                        try
                        {
                        }
                        finally
                        {
                            this.$locvar0.Dispose();
                        }
                        break;
                }
            }

            public bool MoveNext()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                bool flag = false;
                switch (num)
                {
                    case 0:
                        this.$locvar0 = this.patternList.GetEnumerator();
                        num = 0xfffffffd;
                        break;

                    case 1:
                        break;

                    default:
                        goto Label_00D3;
                }
                try
                {
                    while (this.$locvar0.MoveNext())
                    {
                        this.<pattern>__1 = this.$locvar0.Current;
                        this.$this.Dicing.ChangePattern(this.<pattern>__1);
                        this.$current = new WaitForSeconds(1f / this.$this.frameRate);
                        if (!this.$disposing)
                        {
                            this.$PC = 1;
                        }
                        flag = true;
                        return true;
                    }
                }
                finally
                {
                    if (!flag)
                    {
                    }
                    this.$locvar0.Dispose();
                }
                this.$PC = -1;
            Label_00D3:
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

