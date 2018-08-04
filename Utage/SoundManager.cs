namespace Utage
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;
    using UnityEngine.Events;

    [AddComponentMenu("Utage/Lib/Sound/SoundManager")]
    public class SoundManager : MonoBehaviour, IBinaryIO
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <CurrentVoiceCharacterLabel>k__BackingField;
        [SerializeField]
        private float defaultFadeTime = 0.2f;
        [SerializeField]
        private float defaultVoiceFadeTime = 0.05f;
        [SerializeField, Range(0f, 1f)]
        private float defaultVolume = 1f;
        [SerializeField, Range(0f, 1f)]
        private float duckFadeTime = 0.1f;
        [SerializeField, Range(0f, 1f)]
        private float duckVolume = 0.5f;
        public const string IdAmbience = "Ambience";
        public const string IdBgm = "Bgm";
        public const string IdSe = "Se";
        public const string IdVoice = "Voice";
        private static SoundManager instance;
        [SerializeField, Range(0f, 1f)]
        private float masterVolume = 1f;
        [SerializeField]
        private SoundManagerEvent onCreateSoundSystem = new SoundManagerEvent();
        private SoundManagerSystemInterface system;
        public const string TaggedMasterVolumeOthers = "Others";
        [SerializeField]
        private List<TaggedMasterVolume> taggedMasterVolumes = new List<TaggedMasterVolume>();
        [SerializeField]
        private SoundPlayMode voicePlayMode = SoundPlayMode.Replay;

        internal float GetCurrentCharacterVoiceSamplesVolume()
        {
            return this.GetVoiceSamplesVolume(this.CurrentVoiceCharacterLabel);
        }

        public float GetGroupVolume(string groupName)
        {
            return this.System.GetGroupVolume(groupName);
        }

        public static SoundManager GetInstance()
        {
            if (null == instance)
            {
                instance = Object.FindObjectOfType<SoundManager>();
            }
            return instance;
        }

        internal float GetVoiceSamplesVolume(string characterLabel)
        {
            return this.System.GetSamplesVolume("Voice", characterLabel);
        }

        public bool IsPlayingVoice()
        {
            return this.IsPlayingVoice(this.CurrentVoiceCharacterLabel);
        }

        internal bool IsPlayingVoice(string characterLabel)
        {
            if (string.IsNullOrEmpty(characterLabel))
            {
                return false;
            }
            return this.System.IsPlaying("Voice", characterLabel);
        }

        public void OnRead(BinaryReader reader)
        {
            this.System.ReadSaveDataBuffer(reader);
        }

        public void OnWrite(BinaryWriter writer)
        {
            this.System.WriteSaveData(writer);
        }

        public void PlayAmbience(AudioClip clip, bool isLoop)
        {
            this.PlayAmbience(clip, isLoop, 0f, this.DefaultFadeTime);
        }

        public void PlayAmbience(AssetFile file, bool isLoop)
        {
            this.PlayAmbience(file, isLoop, 0f, this.DefaultFadeTime);
        }

        public void PlayAmbience(AudioClip clip, bool isLoop, float fadeInTime, float fadeOutTime)
        {
            this.System.Play("Ambience", "Ambience", new SoundData(clip, SoundPlayMode.NotPlaySame, this.DefaultVolume, isLoop), fadeInTime, fadeOutTime);
        }

        public void PlayAmbience(AssetFile file, bool isLoop, float fadeInTime, float fadeOutTime)
        {
            this.PlayAmbience(file, this.DefaultVolume, isLoop, fadeInTime, fadeOutTime);
        }

        public void PlayAmbience(AssetFile file, float volume, bool isLoop, float fadeInTime, float fadeOutTime)
        {
            this.System.Play("Ambience", "Ambience", new SoundData(file, SoundPlayMode.NotPlaySame, volume, isLoop), fadeInTime, fadeOutTime);
        }

        public void PlayBgm(AssetFile file)
        {
            this.PlayBgm(file, 0f, this.DefaultFadeTime);
        }

        public void PlayBgm(AudioClip clip, bool isLoop)
        {
            this.System.Play("Bgm", "Bgm", new SoundData(clip, SoundPlayMode.NotPlaySame, this.DefaultVolume, isLoop), 0f, this.DefaultFadeTime);
        }

        public void PlayBgm(AssetFile file, float fadeInTime, float fadeOutTime)
        {
            this.PlayBgm(file, this.DefaultVolume, fadeInTime, fadeOutTime);
        }

        public void PlayBgm(AssetFile file, float volume, float fadeInTime, float fadeOutTime)
        {
            this.System.Play("Bgm", "Bgm", new SoundData(file, SoundPlayMode.NotPlaySame, volume, true), fadeInTime, fadeOutTime);
        }

        public void PlaySe(AudioClip clip, string label = "", SoundPlayMode playMode = 0, bool isLoop = false)
        {
            this.PlaySe(clip, this.DefaultVolume, label, playMode, isLoop);
        }

        public void PlaySe(AssetFile file, string label = "", SoundPlayMode playMode = 0, bool isLoop = false)
        {
            this.PlaySe(file, this.DefaultVolume, label, playMode, isLoop);
        }

        public void PlaySe(AudioClip clip, float volume, string label = "", SoundPlayMode playMode = 0, bool isLoop = false)
        {
            if (string.IsNullOrEmpty(label))
            {
                label = clip.get_name();
            }
            this.System.Play("Se", label, new SoundData(clip, playMode, volume, isLoop), 0f, 0.02f);
        }

        public void PlaySe(AssetFile file, float volume, string label = "", SoundPlayMode playMode = 0, bool isLoop = false)
        {
            if (string.IsNullOrEmpty(label))
            {
                label = file.Sound.get_name();
            }
            this.System.Play("Se", label, new SoundData(file, playMode, volume, isLoop), 0f, 0f);
        }

        public void PlayVoice(string characterLabel, AssetFile file)
        {
            this.PlayVoice(characterLabel, file, false);
        }

        public void PlayVoice(string characterLabel, AudioClip clip, bool isLoop)
        {
            this.PlayVoice(characterLabel, clip, isLoop, 0f, this.DefaultVoiceFadeTime);
        }

        public void PlayVoice(string characterLabel, AssetFile file, bool isLoop)
        {
            this.PlayVoice(characterLabel, file, this.DefaultVolume, isLoop, 0f, this.DefaultVoiceFadeTime);
        }

        public void PlayVoice(string characterLabel, AssetFile file, float volume, bool isLoop)
        {
            this.PlayVoice(characterLabel, file, volume, isLoop, 0f, this.DefaultVoiceFadeTime);
        }

        public void PlayVoice(string characterLabel, AssetFile file, float fadeInTime, float fadeOutTime)
        {
            this.PlayVoice(characterLabel, file, this.DefaultVolume, false, fadeInTime, fadeOutTime);
        }

        public void PlayVoice(string characterLabel, SoundData data, float fadeInTime, float fadeOutTime)
        {
            <PlayVoice>c__AnonStorey1 storey = new <PlayVoice>c__AnonStorey1 {
                characterLabel = characterLabel
            };
            this.CurrentVoiceCharacterLabel = storey.characterLabel;
            data.Tag = !this.TaggedMasterVolumes.Exists(new Predicate<TaggedMasterVolume>(storey.<>m__0)) ? "Others" : storey.characterLabel;
            this.System.Play("Voice", storey.characterLabel, data, fadeInTime, fadeOutTime);
        }

        public void PlayVoice(string characterLabel, AudioClip clip, bool isLoop, float fadeInTime, float fadeOutTime)
        {
            this.PlayVoice(characterLabel, new SoundData(clip, this.VoicePlayMode, this.DefaultVolume, isLoop), fadeInTime, fadeOutTime);
        }

        public void PlayVoice(string characterLabel, AssetFile file, float volume, bool isLoop, float fadeInTime, float fadeOutTime)
        {
            this.PlayVoice(characterLabel, new SoundData(file, this.VoicePlayMode, volume, isLoop), fadeInTime, fadeOutTime);
        }

        public void SetGroupVolume(string groupName, float volume)
        {
            this.System.SetGroupVolume(groupName, volume);
        }

        public void SetTaggedMasterVolume(string tag, float volmue)
        {
            <SetTaggedMasterVolume>c__AnonStorey0 storey = new <SetTaggedMasterVolume>c__AnonStorey0 {
                tag = tag
            };
            TaggedMasterVolume volume = this.TaggedMasterVolumes.Find(new Predicate<TaggedMasterVolume>(storey.<>m__0));
            if (volume == null)
            {
                TaggedMasterVolume item = new TaggedMasterVolume {
                    Tag = storey.tag,
                    Volume = volmue
                };
                this.TaggedMasterVolumes.Add(item);
            }
            else
            {
                volume.Volume = volmue;
            }
        }

        public void StopAll()
        {
            this.StopAll(this.DefaultFadeTime);
        }

        public void StopAll(float fadeTime)
        {
            this.System.StopAll(fadeTime);
        }

        public void StopAmbience()
        {
            this.StopAmbience(this.DefaultFadeTime);
        }

        public void StopAmbience(float fadeTime)
        {
            this.System.StopGroup("Ambience", fadeTime);
        }

        public void StopBgm()
        {
            this.StopBgm(this.DefaultFadeTime);
        }

        public void StopBgm(float fadeTime)
        {
            this.System.StopGroup("Bgm", fadeTime);
        }

        public void StopGroups(string[] groups)
        {
            this.StopGroups(groups, this.DefaultFadeTime);
        }

        public void StopGroups(string[] groups, float fadeTime)
        {
            foreach (string str in groups)
            {
                this.System.StopGroup(str, fadeTime);
            }
        }

        public void StopSe(string label, float fadeTime)
        {
            this.System.Stop("Se", label, fadeTime);
        }

        public void StopSeAll(float fadeTime)
        {
            this.System.StopGroup("Se", fadeTime);
        }

        public void StopVoice()
        {
            this.StopVoice(this.DefaultVoiceFadeTime);
        }

        public void StopVoice(float fadeTime)
        {
            this.System.StopGroup("Voice", fadeTime);
        }

        public void StopVoice(string characterLabel)
        {
            this.StopVoice(characterLabel, this.DefaultVoiceFadeTime);
        }

        public void StopVoice(string characterLabel, float fadeTime)
        {
            this.System.Stop("Voice", characterLabel, fadeTime);
        }

        public void StopVoiceIgnoreLoop()
        {
            this.StopVoiceIgnoreLoop(this.DefaultVoiceFadeTime);
        }

        public void StopVoiceIgnoreLoop(float fadeTime)
        {
            this.System.StopGroupIgnoreLoop("Voice", fadeTime);
        }

        private bool TryChangeFloat(ref float volume, float value)
        {
            if (Mathf.Approximately(volume, value))
            {
                return false;
            }
            volume = value;
            return true;
        }

        public float AmbienceVolume
        {
            get
            {
                return this.System.GetMasterVolume("Ambience");
            }
            set
            {
                this.System.SetMasterVolume("Ambience", value);
            }
        }

        public float BgmVolume
        {
            get
            {
                return this.System.GetMasterVolume("Bgm");
            }
            set
            {
                this.System.SetMasterVolume("Bgm", value);
            }
        }

        public string CurrentVoiceCharacterLabel { get; set; }

        public float DefaultFadeTime
        {
            get
            {
                return this.defaultFadeTime;
            }
            set
            {
                this.TryChangeFloat(ref this.defaultFadeTime, value);
            }
        }

        public float DefaultVoiceFadeTime
        {
            get
            {
                return this.defaultVoiceFadeTime;
            }
            set
            {
                this.TryChangeFloat(ref this.defaultVoiceFadeTime, value);
            }
        }

        public float DefaultVolume
        {
            get
            {
                return this.defaultVolume;
            }
            set
            {
                this.TryChangeFloat(ref this.defaultVolume, value);
            }
        }

        public float DuckFadeTime
        {
            get
            {
                return this.duckFadeTime;
            }
            set
            {
                this.duckFadeTime = value;
            }
        }

        public float DuckVolume
        {
            get
            {
                return this.duckVolume;
            }
            set
            {
                this.duckVolume = value;
            }
        }

        public bool IsLoading
        {
            get
            {
                return this.System.IsLoading;
            }
        }

        public float MasterVolume
        {
            get
            {
                return this.masterVolume;
            }
            set
            {
                this.masterVolume = value;
            }
        }

        public bool MultiVoice
        {
            get
            {
                return this.System.IsMultiPlay("Voice");
            }
            set
            {
                this.System.SetMultiPlay("Voice", value);
            }
        }

        public SoundManagerEvent OnCreateSoundSystem
        {
            get
            {
                return this.onCreateSoundSystem;
            }
            set
            {
                this.onCreateSoundSystem = value;
            }
        }

        public string SaveKey
        {
            get
            {
                return "SoundManager";
            }
        }

        public float SeVolume
        {
            get
            {
                return this.System.GetMasterVolume("Se");
            }
            set
            {
                this.System.SetMasterVolume("Se", value);
            }
        }

        public SoundManagerSystemInterface System
        {
            get
            {
                if (this.system == null)
                {
                    this.OnCreateSoundSystem.Invoke(this);
                    if (this.system == null)
                    {
                        this.system = new SoundManagerSystem();
                    }
                    string[] collection = new string[] { "Bgm", "Ambience" };
                    List<string> saveStreamNameList = new List<string>(collection);
                    this.system.Init(this, saveStreamNameList);
                }
                return this.system;
            }
            set
            {
                this.system = value;
            }
        }

        public List<TaggedMasterVolume> TaggedMasterVolumes
        {
            get
            {
                return this.taggedMasterVolumes;
            }
        }

        public SoundPlayMode VoicePlayMode
        {
            get
            {
                return this.voicePlayMode;
            }
            set
            {
                this.voicePlayMode = value;
            }
        }

        public float VoiceVolume
        {
            get
            {
                return this.System.GetMasterVolume("Voice");
            }
            set
            {
                this.System.SetMasterVolume("Voice", value);
            }
        }

        [CompilerGenerated]
        private sealed class <PlayVoice>c__AnonStorey1
        {
            internal string characterLabel;

            internal bool <>m__0(SoundManager.TaggedMasterVolume x)
            {
                return (x.Tag == this.characterLabel);
            }
        }

        [CompilerGenerated]
        private sealed class <SetTaggedMasterVolume>c__AnonStorey0
        {
            internal string tag;

            internal bool <>m__0(SoundManager.TaggedMasterVolume x)
            {
                return (x.Tag == this.tag);
            }
        }

        [Serializable]
        public class SoundManagerEvent : UnityEvent<SoundManager>
        {
        }

        [Serializable]
        public class TaggedMasterVolume
        {
            [SerializeField]
            private string tag;
            [Range(0f, 1f), SerializeField]
            private float volume = 1f;

            public string Tag
            {
                get
                {
                    return this.tag;
                }
                set
                {
                    this.tag = value;
                }
            }

            public float Volume
            {
                get
                {
                    return this.volume;
                }
                set
                {
                    this.volume = value;
                }
            }
        }
    }
}

