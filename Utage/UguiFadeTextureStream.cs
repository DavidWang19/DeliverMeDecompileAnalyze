namespace Utage
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    [RequireComponent(typeof(RawImage)), AddComponentMenu("Utage/Lib/UI/FadeTextureStream")]
    public class UguiFadeTextureStream : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
    {
        public bool allowAllSkip;
        public bool allowSkip = true;
        public FadeTextureInfo[] fadeTextures = new FadeTextureInfo[1];
        private bool isInput;
        private bool isPlaying;

        [DebuggerHidden]
        private IEnumerator CoPlay()
        {
            return new <CoPlay>c__Iterator0 { $this = this };
        }

        private bool IsInputSkip(FadeTextureInfo info)
        {
            return (this.isInput && (this.allowSkip || info.allowSkip));
        }

        private void LateUpdate()
        {
            this.isInput = false;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            this.isInput = true;
        }

        public void Play()
        {
            base.StartCoroutine(this.CoPlay());
        }

        private bool IsInputAllSkip
        {
            get
            {
                return (this.isInput && this.allowAllSkip);
            }
        }

        public bool IsPlaying
        {
            get
            {
                return this.isPlaying;
            }
        }

        [CompilerGenerated]
        private sealed class <CoPlay>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal bool $disposing;
            internal UguiFadeTextureStream.FadeTextureInfo[] $locvar0;
            internal int $locvar1;
            internal int $PC;
            internal UguiFadeTextureStream $this;
            internal bool <allSkip>__2;
            internal UguiFadeTextureStream.FadeTextureInfo <info>__1;
            internal RawImage <rawImage>__0;
            internal float <time>__3;

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
                        this.<rawImage>__0 = this.$this.GetComponent<RawImage>();
                        this.<rawImage>__0.CrossFadeAlpha(0f, 0f, true);
                        this.$locvar0 = this.$this.fadeTextures;
                        this.$locvar1 = 0;
                        goto Label_02CF;

                    case 1:
                        this.<time>__3 += Time.get_deltaTime();
                        if (this.<time>__3 <= this.<info>__1.fadeInTime)
                        {
                            break;
                        }
                        goto Label_0150;

                    case 2:
                        this.<time>__3 += Time.get_deltaTime();
                        if (this.<time>__3 <= this.<info>__1.duration)
                        {
                            goto Label_01A8;
                        }
                        goto Label_01BE;

                    case 3:
                        goto Label_0296;

                    case 4:
                        goto Label_0260;

                    case 5:
                        this.$locvar1++;
                        goto Label_02CF;

                    default:
                        goto Label_02F5;
                }
            Label_013A:
                while (!this.$this.IsInputSkip(this.<info>__1))
                {
                    this.$current = null;
                    if (!this.$disposing)
                    {
                        this.$PC = 1;
                    }
                    goto Label_02F7;
                }
            Label_0150:
                this.<time>__3 = 0f;
            Label_01A8:
                while (!this.$this.IsInputSkip(this.<info>__1))
                {
                    this.$current = null;
                    if (!this.$disposing)
                    {
                        this.$PC = 2;
                    }
                    goto Label_02F7;
                }
            Label_01BE:
                this.<allSkip>__2 = this.$this.IsInputAllSkip;
                this.<rawImage>__0.CrossFadeAlpha(0f, this.<info>__1.fadeOutTime, true);
                this.$current = new WaitForSeconds(this.<info>__1.fadeOutTime);
                if (!this.$disposing)
                {
                    this.$PC = 3;
                }
                goto Label_02F7;
            Label_0296:
                if (this.<allSkip>__2)
                {
                    goto Label_02E2;
                }
                this.$current = null;
                if (!this.$disposing)
                {
                    this.$PC = 5;
                }
                goto Label_02F7;
            Label_02CF:
                if (this.$locvar1 < this.$locvar0.Length)
                {
                    this.<info>__1 = this.$locvar0[this.$locvar1];
                    this.<rawImage>__0.set_texture(this.<info>__1.texture);
                    this.<allSkip>__2 = false;
                    if (this.<info>__1.texture == null)
                    {
                        if (!string.IsNullOrEmpty(this.<info>__1.moviePath))
                        {
                            WrapperMoviePlayer.Play(this.<info>__1.moviePath, false);
                            while (WrapperMoviePlayer.IsPlaying())
                            {
                                this.$current = null;
                                if (!this.$disposing)
                                {
                                    this.$PC = 4;
                                }
                                goto Label_02F7;
                            Label_0260:
                                if (this.$this.IsInputSkip(this.<info>__1))
                                {
                                    WrapperMoviePlayer.Cancel();
                                }
                                this.<allSkip>__2 = this.$this.IsInputAllSkip;
                            }
                        }
                        goto Label_0296;
                    }
                    this.<rawImage>__0.CrossFadeAlpha(1f, this.<info>__1.fadeInTime, true);
                    this.<time>__3 = 0f;
                    goto Label_013A;
                }
            Label_02E2:
                this.$this.isPlaying = false;
                this.$PC = -1;
            Label_02F5:
                return false;
            Label_02F7:
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

        [Serializable]
        public class FadeTextureInfo
        {
            public bool allowSkip;
            public float duration = 3f;
            public float fadeInTime = 0.5f;
            public float fadeOutTime = 0.5f;
            public string moviePath;
            public Texture texture;
        }
    }
}

