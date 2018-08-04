namespace Utage
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    [AddComponentMenu("Utage/Lib/UI/ViewTransition CrossFade"), RequireComponent(typeof(Utage.UguiView))]
    public class UguiViewTransitionCrossFade : MonoBehaviour, ITransition
    {
        private bool isPlaying;
        public float time = 1f;
        private Utage.UguiView uguiView;

        public void CancelClosing()
        {
            base.StopCoroutine(this.CoClose());
            this.EndClose();
            this.isPlaying = false;
        }

        public void Close()
        {
            base.StopCoroutine(this.CoOpen());
            base.StartCoroutine(this.CoClose());
        }

        [DebuggerHidden]
        private IEnumerator CoClose()
        {
            return new <CoClose>c__Iterator1 { $this = this };
        }

        [DebuggerHidden]
        private IEnumerator CoOpen()
        {
            return new <CoOpen>c__Iterator0 { $this = this };
        }

        private void EndClose()
        {
            CanvasGroup canvasGroup = this.UguiView.CanvasGroup;
            canvasGroup.set_alpha(0f);
            canvasGroup.set_interactable(true);
            canvasGroup.set_blocksRaycasts(true);
            this.isPlaying = false;
        }

        public void Open()
        {
            base.StopCoroutine(this.CoClose());
            base.StartCoroutine(this.CoOpen());
        }

        public bool IsPlaying
        {
            get
            {
                return this.isPlaying;
            }
        }

        public Utage.UguiView UguiView
        {
            get
            {
                if (this.uguiView == null)
                {
                }
                return (this.uguiView = base.GetComponent<Utage.UguiView>());
            }
        }

        [CompilerGenerated]
        private sealed class <CoClose>c__Iterator1 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal bool $disposing;
            internal int $PC;
            internal UguiViewTransitionCrossFade $this;
            internal CanvasGroup <canvasGroup>__0;
            internal float <currentTime>__0;

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
                        this.$this.isPlaying = true;
                        this.<canvasGroup>__0 = this.$this.UguiView.CanvasGroup;
                        this.<canvasGroup>__0.set_interactable(false);
                        this.<canvasGroup>__0.set_blocksRaycasts(false);
                        this.<currentTime>__0 = 0f;
                        break;

                    case 1:
                        break;

                    default:
                        goto Label_00E8;
                }
                if (this.<currentTime>__0 < this.$this.time)
                {
                    this.<canvasGroup>__0.set_alpha(1f - (this.<currentTime>__0 / this.$this.time));
                    this.<currentTime>__0 += Time.get_deltaTime();
                    this.$current = null;
                    if (!this.$disposing)
                    {
                        this.$PC = 1;
                    }
                    return true;
                }
                this.$this.EndClose();
                goto Label_00E8;
                this.$PC = -1;
            Label_00E8:
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
        private sealed class <CoOpen>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal bool $disposing;
            internal int $PC;
            internal UguiViewTransitionCrossFade $this;
            internal CanvasGroup <canvasGroup>__0;
            internal float <currentTime>__0;

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
                        this.$this.isPlaying = true;
                        this.<canvasGroup>__0 = this.$this.UguiView.CanvasGroup;
                        this.<canvasGroup>__0.set_interactable(false);
                        this.<canvasGroup>__0.set_blocksRaycasts(false);
                        this.<currentTime>__0 = 0f;
                        break;

                    case 1:
                        break;

                    default:
                        goto Label_010B;
                }
                if (this.<currentTime>__0 < this.$this.time)
                {
                    this.<canvasGroup>__0.set_alpha(this.<currentTime>__0 / this.$this.time);
                    this.<currentTime>__0 += Time.get_deltaTime();
                    this.$current = null;
                    if (!this.$disposing)
                    {
                        this.$PC = 1;
                    }
                    return true;
                }
                this.<canvasGroup>__0.set_alpha(1f);
                this.<canvasGroup>__0.set_interactable(true);
                this.<canvasGroup>__0.set_blocksRaycasts(true);
                this.$this.isPlaying = false;
                goto Label_010B;
                this.$PC = -1;
            Label_010B:
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

