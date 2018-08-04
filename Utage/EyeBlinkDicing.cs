namespace Utage
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UtageExtensions;

    [RequireComponent(typeof(DicingImage)), AddComponentMenu("Utage/Lib/UI/EyeBlinkDicing")]
    public class EyeBlinkDicing : EyeBlinkBase
    {
        private DicingImage dicing;

        [DebuggerHidden]
        protected override IEnumerator CoEyeBlink(Action onComplete)
        {
            return new <CoEyeBlink>c__Iterator0 { onComplete = onComplete, $this = this };
        }

        private DicingImage Dicing
        {
            get
            {
                return base.get_gameObject().GetComponentCache<DicingImage>(ref this.dicing);
            }
        }

        [CompilerGenerated]
        private sealed class <CoEyeBlink>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal bool $disposing;
            internal List<MiniAnimationData.Data>.Enumerator $locvar0;
            internal int $PC;
            internal EyeBlinkDicing $this;
            internal MiniAnimationData.Data <data>__1;
            internal Action onComplete;

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
                        this.$locvar0 = this.$this.AnimationData.DataList.GetEnumerator();
                        num = 0xfffffffd;
                        break;

                    case 1:
                        break;

                    default:
                        goto Label_0144;
                }
                try
                {
                    while (this.$locvar0.MoveNext())
                    {
                        this.<data>__1 = this.$locvar0.Current;
                        this.$this.Dicing.TryChangePatternWithOption(this.$this.Dicing.MainPattern, this.$this.EyeTag, this.<data>__1.ComvertNameSimple());
                        this.$current = new WaitForSeconds(this.<data>__1.Duration);
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
                this.$this.Dicing.TryChangePatternWithOption(this.$this.Dicing.MainPattern, this.$this.EyeTag, string.Empty);
                if (this.onComplete != null)
                {
                    this.onComplete();
                    goto Label_0144;
                    this.$PC = -1;
                }
            Label_0144:
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

