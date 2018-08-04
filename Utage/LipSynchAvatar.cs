namespace Utage
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UtageExtensions;

    [AddComponentMenu("Utage/Lib/UI/LipSynchAvatar")]
    public class LipSynchAvatar : LipSynch2d
    {
        private AvatarImage avator;

        [DebuggerHidden]
        protected override IEnumerator CoUpdateLipSync()
        {
            return new <CoUpdateLipSync>c__Iterator0 { $this = this };
        }

        private AvatarImage Avator
        {
            get
            {
                return base.get_gameObject().GetComponentCache<AvatarImage>(ref this.avator);
            }
        }

        [CompilerGenerated]
        private sealed class <CoUpdateLipSync>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal bool $disposing;
            internal List<MiniAnimationData.Data>.Enumerator $locvar0;
            internal int $PC;
            internal LipSynchAvatar $this;
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
                            this.<pattern>__1 = this.$this.Avator.AvatarPattern.GetPatternName(this.$this.LipTag);
                            this.$locvar0 = this.$this.AnimationData.DataList.GetEnumerator();
                            num = 0xfffffffd;
                            break;
                        }
                        goto Label_0185;

                    case 1:
                        break;

                    default:
                        goto Label_019D;
                }
                try
                {
                    while (this.$locvar0.MoveNext())
                    {
                        this.<data>__2 = this.$locvar0.Current;
                        this.$this.Avator.ChangePattern(this.$this.LipTag, this.<data>__2.ComvertName(this.<pattern>__1));
                        this.$current = new WaitForSeconds(this.<data>__2.Duration);
                        if (!this.$disposing)
                        {
                            this.$PC = 1;
                        }
                        flag = true;
                        goto Label_019F;
                    }
                }
                finally
                {
                    if (!flag)
                    {
                    }
                    this.$locvar0.Dispose();
                }
                this.$this.Avator.ChangePattern(this.$this.LipTag, this.<pattern>__1);
                if (this.$this.IsPlaying)
                {
                    this.$current = new WaitForSeconds(this.$this.Interval);
                    if (!this.$disposing)
                    {
                        this.$PC = 2;
                    }
                    goto Label_019F;
                }
            Label_0185:
                this.$this.coLypSync = null;
                goto Label_019D;
                this.$PC = -1;
            Label_019D:
                return false;
            Label_019F:
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

