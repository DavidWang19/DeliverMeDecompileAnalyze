namespace Utage
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;
    using UnityEngine.UI;

    [AddComponentMenu("Utage/Lib/Wrapper/MoviePlayer")]
    public class WrapperMoviePlayer : MonoBehaviour
    {
        public Color bgColor = Color.get_black();
        private bool cancel;
        public float cancelFadeTime = 0.5f;
        public bool ignoreCancel;
        private static WrapperMoviePlayer instance;
        private bool isPlaying;
        private MovieTexture movieTexture;
        [SerializeField]
        private bool overrideRootDirectory;
        [SerializeField]
        private GameObject renderTarget;
        [SerializeField, Hide("NotOverrideRootDirectory")]
        private string rootDirectory;

        private void Awake()
        {
            if (null != instance)
            {
                Object.Destroy(base.get_gameObject());
            }
            else
            {
                instance = this;
            }
        }

        public static void Cancel()
        {
            GetInstance().CancelMovie();
        }

        public void CancelMovie()
        {
            if (this.cancel)
            {
                this.CancelMovieTexture();
            }
        }

        private void CancelMovieTexture()
        {
            base.StartCoroutine(this.CoCancelMovieTexture());
        }

        private void ClearRenderTargetTexture()
        {
            GameObject target = this.Target;
            RawImage component = target.GetComponent<RawImage>();
            if (component != null)
            {
                component.set_texture(null);
                component.CrossFadeAlpha(1f, 0f, true);
                component.set_enabled(false);
            }
            else
            {
                target.GetComponent<Renderer>().get_material().set_mainTexture(null);
            }
        }

        [DebuggerHidden]
        private IEnumerator CoCancelMovieTexture()
        {
            return new <CoCancelMovieTexture>c__Iterator2 { $this = this };
        }

        [DebuggerHidden]
        private IEnumerator CoPlayMovieFromResources(string path, bool isLoop)
        {
            return new <CoPlayMovieFromResources>c__Iterator1 { path = path, isLoop = isLoop, $this = this };
        }

        [DebuggerHidden]
        private IEnumerator CoPlayMovieTexture(MovieTexture movieTexture, bool isLoop)
        {
            return new <CoPlayMovieTexture>c__Iterator0 { movieTexture = movieTexture, isLoop = isLoop, $this = this };
        }

        [DebuggerHidden]
        private IEnumerator CoStopMovieTexture()
        {
            return new <CoStopMovieTexture>c__Iterator3 { $this = this };
        }

        private void FadeOutMovie(float fadeTime)
        {
            RawImage component = this.Target.GetComponent<RawImage>();
            if (component != null)
            {
                component.CrossFadeAlpha(0f, fadeTime, true);
            }
            if ((this.movieTexture != null) && (this.movieTexture.get_audioClip() != null))
            {
                SoundManager.GetInstance().StopBgm(this.cancelFadeTime);
            }
        }

        public static WrapperMoviePlayer GetInstance()
        {
            if (instance == null)
            {
            }
            return (instance = Object.FindObjectOfType<WrapperMoviePlayer>());
        }

        public static bool IsPlaying()
        {
            return GetInstance().isPlaying;
        }

        public static void Play(string path, bool isLoop = false)
        {
            GetInstance().PlayMovie(path, isLoop);
        }

        public static void Play(string path, bool isLoop, bool cancel)
        {
            GetInstance().PlayMovie(path, isLoop, cancel);
        }

        private void PlayMovie(bool isLoop)
        {
            GameObject target = this.Target;
            RawImage component = target.GetComponent<RawImage>();
            if (component != null)
            {
                component.set_enabled(true);
                component.set_texture(this.movieTexture);
            }
            else
            {
                target.GetComponent<Renderer>().get_material().set_mainTexture(this.movieTexture);
            }
            this.movieTexture.set_loop(isLoop);
            this.movieTexture.Play();
            if (this.movieTexture.get_audioClip() != null)
            {
                SoundManager.GetInstance().PlayBgm(this.movieTexture.get_audioClip(), isLoop);
            }
        }

        public void PlayMovie(string path, bool isLoop)
        {
            this.PlayMovie(path, isLoop, true);
        }

        public void PlayMovie(string path, bool isLoop, bool cancel)
        {
            this.cancel = cancel && !this.ignoreCancel;
            this.PlayMovieTextue(path, isLoop);
        }

        private void PlayMovieTextue(string path, bool isLoop)
        {
            this.isPlaying = true;
            base.StartCoroutine(this.CoPlayMovieFromResources(path, isLoop));
        }

        public static void SetRenderTarget(GameObject target)
        {
            GetInstance().Target = target;
        }

        private string ToStreamingPath(string path)
        {
            string[] args = new string[] { (Application.get_platform() != 11) ? "file://" : string.Empty, Application.get_streamingAssetsPath(), path };
            return FilePathUtil.Combine(args);
        }

        private bool NotOverrideRootDirectory
        {
            get
            {
                return !this.OverrideRootDirectory;
            }
        }

        public bool OverrideRootDirectory
        {
            get
            {
                return this.overrideRootDirectory;
            }
            set
            {
                this.overrideRootDirectory = value;
            }
        }

        public string RootDirectory
        {
            get
            {
                return this.rootDirectory;
            }
            set
            {
                this.rootDirectory = value;
            }
        }

        public GameObject Target
        {
            get
            {
                if (this.renderTarget == null)
                {
                    return base.get_gameObject();
                }
                return this.renderTarget;
            }
            set
            {
                if (this.renderTarget != value)
                {
                    this.ClearRenderTargetTexture();
                    this.renderTarget = value;
                }
            }
        }

        [CompilerGenerated]
        private sealed class <CoCancelMovieTexture>c__Iterator2 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal bool $disposing;
            internal int $PC;
            internal WrapperMoviePlayer $this;

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
                        this.$this.FadeOutMovie(this.$this.cancelFadeTime);
                        this.$current = new WaitForSeconds(this.$this.cancelFadeTime);
                        if (!this.$disposing)
                        {
                            this.$PC = 1;
                        }
                        goto Label_009E;

                    case 1:
                        this.$current = this.$this.StartCoroutine(this.$this.CoStopMovieTexture());
                        if (!this.$disposing)
                        {
                            this.$PC = 2;
                        }
                        goto Label_009E;

                    case 2:
                        this.$PC = -1;
                        break;
                }
                return false;
            Label_009E:
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

        [CompilerGenerated]
        private sealed class <CoPlayMovieFromResources>c__Iterator1 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal bool $disposing;
            internal int $PC;
            internal WrapperMoviePlayer $this;
            internal MovieTexture <movieTexture>__0;
            internal bool isLoop;
            internal string path;

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
                        this.path = FilePathUtil.GetPathWithoutExtension(this.path);
                        this.<movieTexture>__0 = Resources.Load<MovieTexture>(this.path);
                        if (this.<movieTexture>__0 != null)
                        {
                            this.$current = this.$this.StartCoroutine(this.$this.CoPlayMovieTexture(this.<movieTexture>__0, this.isLoop));
                            if (!this.$disposing)
                            {
                                this.$PC = 1;
                            }
                            return true;
                        }
                        Debug.LogError("Movie canot load from " + this.path);
                        break;

                    case 1:
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

        [CompilerGenerated]
        private sealed class <CoPlayMovieTexture>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal bool $disposing;
            internal int $PC;
            internal WrapperMoviePlayer $this;
            internal bool isLoop;
            internal MovieTexture movieTexture;

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
                        this.$this.movieTexture = this.movieTexture;
                        this.$this.PlayMovie(this.isLoop);
                        break;

                    case 1:
                        break;

                    case 2:
                        this.$PC = -1;
                        goto Label_00AE;

                    default:
                        goto Label_00AE;
                }
                if (this.movieTexture.get_isPlaying())
                {
                    this.$current = null;
                    if (!this.$disposing)
                    {
                        this.$PC = 1;
                    }
                }
                else
                {
                    this.$current = this.$this.StartCoroutine(this.$this.CoStopMovieTexture());
                    if (!this.$disposing)
                    {
                        this.$PC = 2;
                    }
                }
                return true;
            Label_00AE:
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
        private sealed class <CoStopMovieTexture>c__Iterator3 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal bool $disposing;
            internal int $PC;
            internal WrapperMoviePlayer $this;

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
                        if (this.$this.movieTexture != null)
                        {
                            this.$this.movieTexture.Stop();
                            if (this.$this.movieTexture.get_audioClip() != null)
                            {
                                SoundManager.GetInstance().StopBgm();
                            }
                        }
                        this.$this.ClearRenderTargetTexture();
                        Resources.UnloadAsset(this.$this.movieTexture);
                        this.$this.movieTexture = null;
                        this.$current = Resources.UnloadUnusedAssets();
                        if (!this.$disposing)
                        {
                            this.$PC = 1;
                        }
                        return true;

                    case 1:
                        this.$this.isPlaying = false;
                        this.$this.StopAllCoroutines();
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

