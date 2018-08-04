namespace Utage
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UtageExtensions;

    [AddComponentMenu("Utage/Lib/UI/LipSynchDicing")]
    public class LipSynchDicing : LipSynch2d
    {
        private DicingImage dicing;

        [DebuggerHidden]
        protected override IEnumerator CoUpdateLipSync()
        {
            return new <CoUpdateLipSync>c__Iterator0 { $this = this };
        }

        private DicingImage Dicing
        {
            get
            {
                return base.get_gameObject().GetComponentCache<DicingImage>(ref this.dicing);
            }
        }

        [CompilerGenerated]
        private sealed class <CoUpdateLipSync>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal bool $disposing;
            internal List<MiniAnimationData.Data>.Enumerator $locvar0;
            internal int $PC;
            internal LipSynchDicing $this;
            internal MiniAnimationData.Data <data>__2;
            internal string <pattern>__1;

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
                    case 2:
                        if (this.$this.IsPlaying)
                        {
                            this.<pattern>__1 = this.$this.Dicing.MainPattern;
                            this.$locvar0 = this.$this.AnimationData.DataList.GetEnumerator();
                            num = 0xfffffffd;
                            break;
                        }
                        goto Label_017C;

                    case 1:
                        break;

                    default:
                        goto Label_018F;
                }
                try
                {
                    while (this.$locvar0.MoveNext())
                    {
                        this.<data>__2 = this.$locvar0.Current;
                        this.$this.Dicing.TryChangePatternWithOption(this.<pattern>__1, this.$this.LipTag, this.<data>__2.ComvertNameSimple());
                        this.$current = new WaitForSeconds(this.<data>__2.Duration);
                        if (!this.$disposing)
                        {
                            this.$PC = 1;
                        }
                        flag = true;
                        goto Label_0191;
                    }
                }
                finally
                {
                    if (!flag)
                    {
                    }
                    this.$locvar0.Dispose();
                }
                this.$this.Dicing.TryChangePatternWithOption(this.<pattern>__1, this.$this.LipTag, string.Empty);
                if (this.$this.IsPlaying)
                {
                    this.$current = new WaitForSeconds(this.$this.Interval);
                    if (!this.$disposing)
                    {
                        this.$PC = 2;
                    }
                    goto Label_0191;
                }
            Label_017C:
                this.$this.coLypSync = null;
                this.$PC = -1;
            Label_018F:
                return false;
            Label_0191:
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

