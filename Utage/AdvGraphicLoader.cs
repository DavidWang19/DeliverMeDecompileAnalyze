namespace Utage
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.Events;

    [AddComponentMenu("Utage/ADV/Internal/GraphicObject/AdvGraphicLoader")]
    public class AdvGraphicLoader : MonoBehaviour
    {
        private AdvGraphicInfo graphic;
        public UnityEvent OnComplete = new UnityEvent();

        [DebuggerHidden]
        private IEnumerator CoLoadWait(Action onComplete)
        {
            return new <CoLoadWait>c__Iterator0 { onComplete = onComplete, $this = this };
        }

        public void LoadGraphic(AdvGraphicInfo graphic, Action onComplete)
        {
            this.Unload();
            this.graphic = graphic;
            AssetFileManager.Load(graphic.File, this);
            base.StartCoroutine(this.CoLoadWait(onComplete));
        }

        private void OnDestroy()
        {
            this.Unload();
        }

        public void Unload()
        {
            if (this.graphic != null)
            {
                this.graphic.File.Unuse(this);
                this.graphic = null;
            }
        }

        public bool IsLoading
        {
            get
            {
                if (this.graphic == null)
                {
                    return false;
                }
                return !this.graphic.File.IsLoadEnd;
            }
        }

        [CompilerGenerated]
        private sealed class <CoLoadWait>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal bool $disposing;
            internal int $PC;
            internal AdvGraphicLoader $this;
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
                    case 1:
                        if (this.$this.IsLoading)
                        {
                            this.$current = null;
                            if (!this.$disposing)
                            {
                                this.$PC = 1;
                            }
                            return true;
                        }
                        this.$this.OnComplete.Invoke();
                        if (this.onComplete != null)
                        {
                            this.onComplete();
                        }
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

