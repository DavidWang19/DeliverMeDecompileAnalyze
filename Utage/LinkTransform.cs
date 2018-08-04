namespace Utage
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    [AddComponentMenu("Utage/Lib/Effect/LinkTransform")]
    public class LinkTransform : MonoBehaviour
    {
        private Transform cachedTransform;
        private bool isInit;
        private Vector3 startEuler;
        private Vector3 startPosition;
        private Vector3 startScale;
        public Transform target;
        private Vector3 targetEuler;
        private Vector3 targetPosition;
        private Vector3 targetScale;

        [DebuggerHidden]
        private IEnumerator CoUpdate()
        {
            return new <CoUpdate>c__Iterator0 { $this = this };
        }

        private void Init()
        {
            this.targetPosition = this.target.get_position();
            this.targetScale = this.target.get_localScale();
            this.targetEuler = this.target.get_eulerAngles();
            this.startPosition = this.CachedTransform.get_position();
            this.startScale = this.CachedTransform.get_localScale();
            this.startEuler = this.CachedTransform.get_eulerAngles();
            this.isInit = true;
        }

        private void Start()
        {
            base.StartCoroutine(this.CoUpdate());
        }

        private Transform CachedTransform
        {
            get
            {
                if (null == this.cachedTransform)
                {
                    this.cachedTransform = base.get_transform();
                }
                return this.cachedTransform;
            }
        }

        [CompilerGenerated]
        private sealed class <CoUpdate>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal bool $disposing;
            internal int $PC;
            internal LinkTransform $this;
            internal RectTransform <rectTransform>__1;

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
                        break;

                    case 1:
                        goto Label_00AB;

                    case 2:
                        break;
                        this.$PC = -1;
                        goto Label_01B7;

                    default:
                        goto Label_01B7;
                }
                if (!this.$this.target.get_gameObject().get_activeSelf())
                {
                    goto Label_0190;
                }
                if (this.$this.isInit || !this.$this.target.get_gameObject().get_activeSelf())
                {
                    goto Label_00B6;
                }
                this.<rectTransform>__1 = this.$this.target as RectTransform;
                if (this.<rectTransform>__1 != null)
                {
                    this.$current = null;
                    if (!this.$disposing)
                    {
                        this.$PC = 1;
                    }
                    goto Label_01B9;
                }
            Label_00AB:
                this.$this.Init();
            Label_00B6:
                if (this.$this.target.get_transform().get_hasChanged())
                {
                    this.$this.CachedTransform.set_position(this.$this.startPosition + (this.$this.target.get_position() - this.$this.targetPosition));
                    this.$this.CachedTransform.set_localScale(this.$this.startScale + (this.$this.target.get_localScale() - this.$this.targetScale));
                    this.$this.CachedTransform.set_eulerAngles(this.$this.startEuler + (this.$this.target.get_eulerAngles() - this.$this.targetEuler));
                }
            Label_0190:
                this.$current = null;
                if (!this.$disposing)
                {
                    this.$PC = 2;
                }
                goto Label_01B9;
            Label_01B7:
                return false;
            Label_01B9:
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

