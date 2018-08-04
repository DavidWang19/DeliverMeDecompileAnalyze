namespace Utage
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;

    [AddComponentMenu("Utage/Lib/Sound/Audio")]
    internal class SoundAudio : MonoBehaviour
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private UnityEngine.AudioSource <Audio0>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private UnityEngine.AudioSource <Audio1>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private UnityEngine.AudioSource <AudioSource>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private UnityEngine.AudioSource <AudioSourceForIntroLoop>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private SoundData <Data>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <IsLoading>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private SoundAudioPlayer <Player>k__BackingField;
        private const int channel = 0;
        private LinearValue fadeValue = new LinearValue();
        private const int samples = 0x100;
        private SoundStreamStatus status;
        private static float[] waveData = new float[0x100];

        [DebuggerHidden]
        private IEnumerator CoWaitDelay(float fadeInTime, float delay)
        {
            return new <CoWaitDelay>c__Iterator0 { delay = delay, fadeInTime = fadeInTime, $this = this };
        }

        public void End()
        {
            if (this.Audio0 != null)
            {
                this.Audio0.Stop();
            }
            if (this.Audio1 != null)
            {
                this.Audio1.Stop();
            }
            Object.Destroy(base.get_gameObject());
        }

        private void EndFadeOut()
        {
            this.End();
        }

        public void FadeOut(float fadeTime)
        {
            if ((fadeTime > 0f) && this.IsPlaying())
            {
                this.status = SoundStreamStatus.FadeOut;
                this.fadeValue.Init(fadeTime, this.fadeValue.GetValue(), 0f);
            }
            else
            {
                this.EndFadeOut();
            }
        }

        public float GetSamplesVolume()
        {
            if (this.AudioSource.get_isPlaying())
            {
                return this.GetSamplesVolume(this.AudioSource);
            }
            return 0f;
        }

        private float GetSamplesVolume(UnityEngine.AudioSource audio)
        {
            audio.GetOutputData(waveData, 0);
            float num = 0f;
            foreach (float num2 in waveData)
            {
                num += Mathf.Abs(num2);
            }
            return (num / 256f);
        }

        private float GetVolume()
        {
            return ((this.fadeValue.GetValue() * this.Data.Volume) * this.Player.Group.GetVolume(this.Data.Tag));
        }

        public void Init(SoundAudioPlayer player, SoundData soundData)
        {
            this.Player = player;
            this.Data = soundData;
            this.Audio0 = base.get_gameObject().AddComponent<UnityEngine.AudioSource>();
            this.Audio0.set_playOnAwake(false);
            if (this.Data.EnableIntroLoop)
            {
                this.Audio1 = base.get_gameObject().AddComponent<UnityEngine.AudioSource>();
                this.Audio1.set_playOnAwake(false);
                this.Audio1.set_clip(this.Data.Clip);
                this.Audio1.set_loop(false);
            }
            this.AudioSource = this.Audio0;
            this.AudioSource.set_clip(this.Data.Clip);
            this.AudioSource.set_loop(this.Data.IsLoop && !this.Data.EnableIntroLoop);
            if (this.Data.File != null)
            {
                this.Data.File.AddReferenceComponent(base.get_gameObject());
            }
        }

        private void IntroUpdate()
        {
            if (this.Data.EnableIntroLoop)
            {
                if ((this.AudioSourceForIntroLoop == null) && (this.AudioSource.get_time() > 0f))
                {
                    this.SetNextIntroLoop();
                }
                if (this.IsEndCurrentAudio())
                {
                    this.AudioSource = this.AudioSourceForIntroLoop;
                    if ((this.AudioSource != null) && !this.AudioSource.get_isPlaying())
                    {
                        this.AudioSource.Play();
                    }
                    this.SetNextIntroLoop();
                }
            }
        }

        private bool IsEndCurrentAudio()
        {
            if (this.AudioSource == null)
            {
                return false;
            }
            if (this.AudioSource.get_isPlaying())
            {
                return false;
            }
            if (((this.AudioSource.get_clip().get_length() - this.AudioSource.get_time()) >= 0.001f) && (!Mathf.Approximately(this.AudioSource.get_time(), 0f) && !Mathf.Approximately(this.AudioSource.get_time(), this.Data.IntroTime)))
            {
                return false;
            }
            return true;
        }

        public bool IsEqualClip(AudioClip clip)
        {
            return ((this.AudioSource != null) && (this.AudioSource.get_clip() == clip));
        }

        public bool IsPlaying()
        {
            SoundStreamStatus status = this.status;
            if ((status != SoundStreamStatus.FadeIn) && (status != SoundStreamStatus.Play))
            {
                return false;
            }
            return true;
        }

        public bool IsPlaying(AudioClip clip)
        {
            return ((this.IsEqualClip(clip) && this.IsPlaying()) && (this.status == SoundStreamStatus.Play));
        }

        public bool IsPlayingLoop()
        {
            return (this.IsPlaying() && this.Data.IsLoop);
        }

        private void LateUpdate()
        {
            if (this.AudioSource != null)
            {
                float volume = this.GetVolume();
                if (!Mathf.Approximately(volume, this.AudioSource.get_volume()))
                {
                    if (this.Audio0 != null)
                    {
                        this.Audio0.set_volume(volume);
                    }
                    if (this.Audio1 != null)
                    {
                        this.Audio1.set_volume(volume);
                    }
                }
            }
        }

        private void OnDestroy()
        {
            this.Player.Remove(this);
        }

        internal void Play(float fadeInTime, float delay = 0f)
        {
            base.StartCoroutine(this.CoWaitDelay(fadeInTime, delay));
        }

        private void PlaySub(float fadeInTime)
        {
            float volume = this.GetVolume();
            this.AudioSource.set_clip(this.Data.Clip);
            if (this.Data.EnableIntroLoop)
            {
                this.Audio1.set_clip(this.Data.Clip);
                this.Audio1.set_volume(volume);
            }
            if (fadeInTime > 0f)
            {
                this.status = SoundStreamStatus.FadeIn;
                this.fadeValue.Init(fadeInTime, this.fadeValue.GetValue(), 1f);
            }
            else
            {
                this.status = SoundStreamStatus.Play;
                this.fadeValue.Init(0f, 1f, 1f);
            }
            this.AudioSource.set_volume(volume);
            if (this.Data.EnableIntroLoop)
            {
                this.AudioSource.PlayScheduled(AudioSettings.get_dspTime() + 0.10000000149011612);
            }
            else
            {
                this.AudioSource.Play();
            }
        }

        private void SetNextIntroLoop()
        {
            this.AudioSourceForIntroLoop = (this.AudioSource != this.Audio0) ? this.Audio0 : this.Audio1;
            this.AudioSourceForIntroLoop.Stop();
            this.AudioSourceForIntroLoop.set_time(this.Data.IntroTime);
            if ((this.AudioSource != null) && (this.AudioSource.get_clip() != null))
            {
                float num = Mathf.Max(0f, this.AudioSource.get_clip().get_length() - this.AudioSource.get_time());
                this.AudioSourceForIntroLoop.PlayScheduled(AudioSettings.get_dspTime() + num);
            }
        }

        private void Update()
        {
            switch (this.status)
            {
                case SoundStreamStatus.FadeIn:
                    this.IntroUpdate();
                    this.UpdateFadeIn();
                    break;

                case SoundStreamStatus.Play:
                    this.IntroUpdate();
                    this.UpdatePlay();
                    break;

                case SoundStreamStatus.FadeOut:
                    this.IntroUpdate();
                    this.UpdateFadeOut();
                    break;
            }
        }

        private void UpdateFadeIn()
        {
            this.fadeValue.IncTime();
            if (this.fadeValue.IsEnd())
            {
                this.status = SoundStreamStatus.Play;
            }
        }

        private void UpdateFadeOut()
        {
            this.fadeValue.IncTime();
            if (this.fadeValue.IsEnd())
            {
                this.EndFadeOut();
            }
        }

        private void UpdatePlay()
        {
            if (!this.Data.IsLoop && this.IsEndCurrentAudio())
            {
                this.EndFadeOut();
            }
        }

        private UnityEngine.AudioSource Audio0 { get; set; }

        private UnityEngine.AudioSource Audio1 { get; set; }

        public UnityEngine.AudioSource AudioSource { get; private set; }

        private UnityEngine.AudioSource AudioSourceForIntroLoop { get; set; }

        internal SoundData Data { get; private set; }

        internal bool EnableSave
        {
            get
            {
                SoundStreamStatus status = this.Status;
                if ((status != SoundStreamStatus.FadeIn) && (status != SoundStreamStatus.Play))
                {
                    return false;
                }
                return this.Data.EnableSave;
            }
        }

        internal bool IsLoading { get; private set; }

        private SoundAudioPlayer Player { get; set; }

        private SoundStreamStatus Status
        {
            get
            {
                return this.status;
            }
        }

        [CompilerGenerated]
        private sealed class <CoWaitDelay>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal bool $disposing;
            internal int $PC;
            internal SoundAudio $this;
            internal float delay;
            internal float fadeInTime;

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
                        this.$this.IsLoading = (this.$this.Data.File != null) && !this.$this.Data.File.IsLoadEnd;
                        if (this.$this.IsLoading)
                        {
                            AssetFileManager.Load(this.$this.Data.File, this.$this);
                        }
                        if (this.delay > 0f)
                        {
                            this.$current = new WaitForSeconds(this.delay);
                            if (!this.$disposing)
                            {
                                this.$PC = 1;
                            }
                            goto Label_014C;
                        }
                        break;

                    case 1:
                        break;

                    case 2:
                        goto Label_00F1;

                    default:
                        goto Label_014A;
                }
                if (!this.$this.IsLoading)
                {
                    goto Label_0126;
                }
            Label_00F1:
                while (!this.$this.Data.File.IsLoadEnd)
                {
                    this.$current = null;
                    if (!this.$disposing)
                    {
                        this.$PC = 2;
                    }
                    goto Label_014C;
                }
                this.$this.Data.File.Unuse(this.$this);
            Label_0126:
                this.$this.IsLoading = false;
                this.$this.PlaySub(this.fadeInTime);
                this.$PC = -1;
            Label_014A:
                return false;
            Label_014C:
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

        private enum SoundStreamStatus
        {
            None,
            FadeIn,
            Play,
            FadeOut
        }
    }
}

