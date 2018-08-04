namespace Utage
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.Events;

    [RequireComponent(typeof(UnityEngine.CanvasGroup))]
    public abstract class UguiView : MonoBehaviour
    {
        [SerializeField]
        protected AudioClip bgm;
        private UnityEngine.CanvasGroup canvasGroup;
        [SerializeField]
        private bool isStopBgmIfNoneBgm;
        protected bool isStoredCanvasGroupInfo;
        public UnityEvent onClose;
        public UnityEvent onOpen;
        [SerializeField]
        protected UguiView prevView;
        private Status status;
        protected bool storedCanvasGroupBlocksRaycasts;
        protected bool storedCanvasGroupInteractable;

        protected UguiView()
        {
        }

        public virtual void Back()
        {
            this.Close();
            if (null != this.prevView)
            {
                this.prevView.Open(this.prevView.prevView);
            }
        }

        protected virtual void CancelClosing()
        {
            foreach (ITransition transition in base.get_gameObject().GetComponents<ITransition>())
            {
                transition.CancelClosing();
            }
            this.RestoreCanvasGroupInput();
            this.EndClose();
        }

        protected virtual void ChangeBgm()
        {
            if (this.Bgm != null)
            {
                if (SoundManager.GetInstance() != null)
                {
                    SoundManager.GetInstance().PlayBgm(this.bgm, true);
                }
            }
            else if (this.IsStopBgmIfNoneBgm && (SoundManager.GetInstance() != null))
            {
                SoundManager.GetInstance().StopBgm();
            }
        }

        public virtual void Close()
        {
            if (base.get_gameObject().get_activeSelf())
            {
                base.get_gameObject().SendMessage("OnBeginClose", 1);
                base.StartCoroutine(this.CoClosing());
            }
        }

        [DebuggerHidden]
        protected virtual IEnumerator CoClosing()
        {
            return new <CoClosing>c__Iterator1 { $this = this };
        }

        [DebuggerHidden]
        protected virtual IEnumerator CoOpening()
        {
            return new <CoOpening>c__Iterator0 { $this = this };
        }

        protected virtual void EndClose()
        {
            base.get_gameObject().SendMessage("OnClose", 1);
            this.onClose.Invoke();
            base.get_gameObject().SetActive(false);
            this.status = Status.Closed;
        }

        public virtual void OnTapBack()
        {
            this.Back();
        }

        public virtual void Open()
        {
            this.Open(this.prevView);
        }

        public virtual void Open(UguiView prevView)
        {
            if (this.status == Status.Closing)
            {
                this.CancelClosing();
            }
            this.status = Status.Opening;
            this.prevView = prevView;
            base.get_gameObject().SetActive(true);
            this.ChangeBgm();
            base.get_gameObject().SendMessage("OnOpen", 1);
            this.onOpen.Invoke();
            base.StartCoroutine(this.CoOpening());
        }

        protected void RestoreCanvasGroupInput()
        {
            if (this.isStoredCanvasGroupInfo)
            {
                this.CanvasGroup.set_interactable(this.storedCanvasGroupInteractable);
                this.CanvasGroup.set_blocksRaycasts(this.storedCanvasGroupBlocksRaycasts);
                this.isStoredCanvasGroupInfo = false;
            }
        }

        protected void StoreAndChangeCanvasGroupInput(bool isActive)
        {
            this.storedCanvasGroupInteractable = this.CanvasGroup.get_interactable();
            this.storedCanvasGroupBlocksRaycasts = this.CanvasGroup.get_blocksRaycasts();
            this.CanvasGroup.set_interactable(false);
            this.CanvasGroup.set_blocksRaycasts(false);
            this.isStoredCanvasGroupInfo = true;
        }

        public virtual void ToggleOpen(bool isOpen)
        {
            if (isOpen)
            {
                this.Open();
            }
            else
            {
                this.Close();
            }
        }

        public AudioClip Bgm
        {
            get
            {
                return this.bgm;
            }
            set
            {
                this.bgm = value;
            }
        }

        public UnityEngine.CanvasGroup CanvasGroup
        {
            get
            {
                if (this.canvasGroup == null)
                {
                }
                return (this.canvasGroup = base.GetComponent<UnityEngine.CanvasGroup>());
            }
        }

        public bool IsStopBgmIfNoneBgm
        {
            get
            {
                return this.isStopBgmIfNoneBgm;
            }
            set
            {
                this.isStopBgmIfNoneBgm = value;
            }
        }

        [CompilerGenerated]
        private sealed class <CoClosing>c__Iterator1 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal bool $disposing;
            internal ITransition[] $locvar0;
            internal int $locvar1;
            internal int $PC;
            internal UguiView $this;
            private static Predicate<ITransition> <>f__am$cache0;
            internal ITransition[] <transitions>__0;

            private static bool <>m__0(ITransition item)
            {
                return !item.IsPlaying;
            }

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
                        this.$this.status = UguiView.Status.Closing;
                        this.$this.StoreAndChangeCanvasGroupInput(true);
                        this.<transitions>__0 = this.$this.get_gameObject().GetComponents<ITransition>();
                        this.$locvar0 = this.<transitions>__0;
                        this.$locvar1 = 0;
                        while (this.$locvar1 < this.$locvar0.Length)
                        {
                            this.$locvar0[this.$locvar1].Close();
                            this.$locvar1++;
                        }
                        break;

                    case 1:
                        break;

                    default:
                        goto Label_0106;
                }
                if (<>f__am$cache0 == null)
                {
                    <>f__am$cache0 = new Predicate<ITransition>(UguiView.<CoClosing>c__Iterator1.<>m__0);
                }
                if (!Array.TrueForAll<ITransition>(this.<transitions>__0, <>f__am$cache0))
                {
                    this.$current = null;
                    if (!this.$disposing)
                    {
                        this.$PC = 1;
                    }
                    return true;
                }
                this.$this.RestoreCanvasGroupInput();
                this.$this.EndClose();
                this.$PC = -1;
            Label_0106:
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
        private sealed class <CoOpening>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal bool $disposing;
            internal ITransition[] $locvar0;
            internal int $locvar1;
            internal int $PC;
            internal UguiView $this;
            private static Predicate<ITransition> <>f__am$cache0;
            internal ITransition[] <transitions>__0;

            private static bool <>m__0(ITransition item)
            {
                return !item.IsPlaying;
            }

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
                        this.<transitions>__0 = this.$this.get_gameObject().GetComponents<ITransition>();
                        this.$locvar0 = this.<transitions>__0;
                        this.$locvar1 = 0;
                        while (this.$locvar1 < this.$locvar0.Length)
                        {
                            this.$locvar0[this.$locvar1].Open();
                            this.$locvar1++;
                        }
                        break;

                    case 1:
                        break;

                    default:
                        goto Label_0105;
                }
                if (<>f__am$cache0 == null)
                {
                    <>f__am$cache0 = new Predicate<ITransition>(UguiView.<CoOpening>c__Iterator0.<>m__0);
                }
                if (!Array.TrueForAll<ITransition>(this.<transitions>__0, <>f__am$cache0))
                {
                    this.$current = null;
                    if (!this.$disposing)
                    {
                        this.$PC = 1;
                    }
                    return true;
                }
                this.$this.RestoreCanvasGroupInput();
                this.$this.status = UguiView.Status.Opened;
                this.$this.get_gameObject().SendMessage("OnEndOpen", 1);
                this.$PC = -1;
            Label_0105:
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

        public enum Status
        {
            None,
            Opening,
            Opened,
            Closing,
            Closed
        }
    }
}

