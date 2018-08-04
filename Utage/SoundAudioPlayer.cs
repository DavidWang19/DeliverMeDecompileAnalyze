namespace Utage
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UtageExtensions;

    [AddComponentMenu("Utage/Lib/Sound/AudioPlayer")]
    internal class SoundAudioPlayer : MonoBehaviour
    {
        [CompilerGenerated]
        private static Predicate<SoundAudio> <>f__am$cache0;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private SoundAudio <Audio>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private List<SoundAudio> <AudioList>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private List<SoundAudio> <CurrentFrameAudioList>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private SoundAudio <FadeOutAudio>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private SoundGroup <Group>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <Label>k__BackingField;
        private const int Version = 0;

        private SoundAudio CreateNewAudio(SoundData soundData)
        {
            SoundAudio item = base.get_transform().AddChildGameObjectComponent<SoundAudio>(soundData.Name);
            item.Init(this, soundData);
            this.AudioList.Add(item);
            return item;
        }

        internal float GetSamplesVolume()
        {
            return (!this.IsPlaying() ? 0f : this.Audio.GetSamplesVolume());
        }

        internal void Init(string label, SoundGroup group)
        {
            this.Group = group;
            this.Label = label;
            this.AudioList = new List<SoundAudio>();
            this.CurrentFrameAudioList = new List<SoundAudio>();
        }

        public bool IsPlaying()
        {
            foreach (SoundAudio audio in this.AudioList)
            {
                if ((audio != null) && audio.IsPlaying())
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsPlayingLoop()
        {
            foreach (SoundAudio audio in this.AudioList)
            {
                if ((audio != null) && audio.IsPlayingLoop())
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsStop()
        {
            foreach (SoundAudio audio in this.AudioList)
            {
                if (audio != null)
                {
                    return false;
                }
            }
            return true;
        }

        private void LateUpdate()
        {
            this.CurrentFrameAudioList.Clear();
        }

        private void OnDestroy()
        {
            this.Group.Remove(this.Label);
        }

        internal void Play(SoundData data, float fadeInTime, float fadeOutTime)
        {
            SoundPlayMode playMode = data.PlayMode;
            if (playMode == SoundPlayMode.Add)
            {
                this.PlayAdd(data, fadeInTime, fadeOutTime);
            }
            else if (playMode == SoundPlayMode.Replay)
            {
                this.PlayFade(data, fadeInTime, fadeOutTime, true);
            }
            else if ((playMode == SoundPlayMode.NotPlaySame) && ((this.Audio == null) || !this.Audio.IsPlaying(data.Clip)))
            {
                this.PlayFade(data, fadeInTime, fadeOutTime, false);
            }
        }

        private void PlayAdd(SoundData data, float fadeInTime, float fadeOutTime)
        {
            foreach (SoundAudio audio in this.CurrentFrameAudioList)
            {
                if ((audio != null) && audio.IsEqualClip(data.Clip))
                {
                    return;
                }
            }
            SoundAudio item = this.CreateNewAudio(data);
            item.Play(fadeInTime, 0f);
            this.CurrentFrameAudioList.Add(item);
        }

        private void PlayFade(SoundData data, float fadeInTime, float fadeOutTime, bool corssFade)
        {
            if (this.FadeOutAudio != null)
            {
                Object.Destroy(this.FadeOutAudio.get_gameObject());
            }
            if (this.Audio == null)
            {
                this.Audio = this.CreateNewAudio(data);
                this.Audio.Play(fadeInTime, 0f);
            }
            else
            {
                this.FadeOutAudio = this.Audio;
                this.Audio = this.CreateNewAudio(data);
                this.FadeOutAudio.FadeOut(fadeOutTime);
                if (corssFade)
                {
                    this.Audio.Play(fadeInTime, 0f);
                }
                else if (this.Audio != null)
                {
                    this.Audio.Play(fadeInTime, fadeOutTime);
                }
            }
        }

        internal void Read(BinaryReader reader)
        {
            int num = reader.ReadInt32();
            if (num <= 0)
            {
                <Read>c__AnonStorey0 storey = new <Read>c__AnonStorey0 {
                    $this = this
                };
                int num2 = reader.ReadInt32();
                for (int i = 0; i < num2; i++)
                {
                    if (reader.ReadBoolean())
                    {
                        SoundData data = new SoundData();
                        reader.ReadBuffer(new Action<BinaryReader>(data.Read));
                        this.Play(data, 0.1f, 0f);
                    }
                }
                storey.audioName = reader.ReadString();
                if (!string.IsNullOrEmpty(storey.audioName))
                {
                    this.Audio = this.AudioList.Find(new Predicate<SoundAudio>(storey.<>m__0));
                }
                if (this.Group.AutoDestoryPlayer && (this.AudioList.Count == 0))
                {
                    Object.Destroy(base.get_gameObject());
                }
            }
            else
            {
                object[] args = new object[] { num };
                Debug.LogError(LanguageErrorMsg.LocalizeTextFormat(ErrorMsg.UnknownVersion, args));
            }
        }

        internal void Remove(SoundAudio audio)
        {
            this.AudioList.Remove(audio);
            if (this.Group.AutoDestoryPlayer && (this.AudioList.Count == 0))
            {
                Object.Destroy(base.get_gameObject());
            }
        }

        public void Stop(float fadeTime)
        {
            foreach (SoundAudio audio in this.AudioList)
            {
                if (audio != null)
                {
                    audio.FadeOut(fadeTime);
                }
            }
        }

        internal void Write(BinaryWriter writer)
        {
            writer.Write(0);
            writer.Write(this.AudioList.Count);
            foreach (SoundAudio audio in this.AudioList)
            {
                bool enableSave = audio.EnableSave;
                writer.Write(enableSave);
                if (enableSave)
                {
                    writer.WriteBuffer(new Action<BinaryWriter>(audio.Data.Write));
                }
            }
            writer.Write((this.Audio != null) ? this.Audio.get_gameObject().get_name() : string.Empty);
        }

        public SoundAudio Audio { get; private set; }

        private List<SoundAudio> AudioList { get; set; }

        private List<SoundAudio> CurrentFrameAudioList { get; set; }

        private SoundAudio FadeOutAudio { get; set; }

        internal SoundGroup Group { get; set; }

        public bool IsLoading
        {
            get
            {
                if (<>f__am$cache0 == null)
                {
                    <>f__am$cache0 = x => x.IsLoading;
                }
                return this.AudioList.Exists(<>f__am$cache0);
            }
        }

        internal string Label { get; private set; }

        [CompilerGenerated]
        private sealed class <Read>c__AnonStorey0
        {
            internal SoundAudioPlayer $this;
            internal string audioName;

            internal bool <>m__0(SoundAudio x)
            {
                return ((x != this.$this.FadeOutAudio) && (x.get_gameObject().get_name() == this.audioName));
            }
        }
    }
}

