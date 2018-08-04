namespace Utage
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UtageExtensions;

    [AddComponentMenu("Utage/Lib/UI/EyeBlinkAvatar"), RequireComponent(typeof(AvatarImage))]
    public class EyeBlinkAvatar : EyeBlinkBase
    {
        private AvatarImage avator;

        [DebuggerHidden]
        protected override IEnumerator CoEyeBlink(Action onComplete)
        {
            return new <CoEyeBlink>c__Iterator0 { onComplete = onComplete, $this = this };
        }

        private AvatarImage Avator
        {
            get
            {
                return base.get_gameObject().GetComponentCache<AvatarImage>(ref this.avator);
            }
        }

        [CompilerGenerated]
        private sealed class <CoEyeBlink>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal bool $disposing;
            internal List<MiniAnimationData.Data>.Enumerator $locvar0;
            internal int $PC;
            internal EyeBlinkAvatar $this;
            internal MiniAnimationData.Data <data>__1;
            internal string <pattern>__0;
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
                        this.<pattern>__0 = this.$this.Avator.AvatarPattern.GetPatternName(this.$this.EyeTag);
                        this.$locvar0 = this.$this.AnimationData.DataList.GetEnumerator();
                        num = 0xfffffffd;
                        break;

                    case 1:
                        break;

                    default:
                        goto Label_014F;
                }
                try
                {
                    while (this.$locvar0.MoveNext())
                    {
                        this.<data>__1 = this.$locvar0.Current;
                        this.$this.Avator.ChangePattern(this.$this.EyeTag, this.<data>__1.ComvertName(this.<pattern>__0));
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
                this.$this.Avator.ChangePattern(this.$this.EyeTag, this.<pattern>__0);
                if (this.onComplete != null)
                {
                    this.onComplete();
                    goto Label_014F;
                    this.$PC = -1;
                }
            Label_014F:
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

