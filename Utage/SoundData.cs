namespace Utage
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class SoundData
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private AssetFile <File>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private float <IntroTime>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <IsLoop>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private SoundPlayMode <PlayMode>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private float <PlayVolume>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private float <ResourceVolume>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <Tag>k__BackingField;
        private AudioClip clip;
        private const int Version = 0;

        public SoundData()
        {
        }

        public SoundData(AudioClip clip, SoundPlayMode playmode, float playVolume, bool isLoop)
        {
            this.clip = clip;
            this.PlayMode = playmode;
            this.PlayVolume = playVolume;
            this.IsLoop = isLoop;
            this.ResourceVolume = 1f;
            this.Tag = string.Empty;
        }

        public SoundData(AssetFile file, SoundPlayMode playmode, float playVolume, bool isLoop)
        {
            this.File = file;
            this.PlayMode = playmode;
            this.PlayVolume = playVolume;
            this.IsLoop = isLoop;
            if (file.SettingData is IAssetFileSoundSettingData)
            {
                IAssetFileSoundSettingData settingData = file.SettingData as IAssetFileSoundSettingData;
                this.IntroTime = settingData.IntroTime;
                this.ResourceVolume = settingData.Volume;
            }
            else
            {
                this.IntroTime = 0f;
                this.ResourceVolume = 1f;
            }
            this.Tag = string.Empty;
        }

        internal void Read(BinaryReader reader)
        {
            int num = reader.ReadInt32();
            if (num <= 0)
            {
                this.PlayMode = (SoundPlayMode) reader.ReadInt32();
                this.IsLoop = reader.ReadBoolean();
                this.PlayVolume = reader.ReadSingle();
                this.ResourceVolume = reader.ReadSingle();
                this.IntroTime = reader.ReadSingle();
                this.Tag = reader.ReadString();
                this.File = AssetFileManager.GetFileCreateIfMissing(reader.ReadString(), null);
            }
            else
            {
                object[] args = new object[] { num };
                Debug.LogError(LanguageErrorMsg.LocalizeTextFormat(ErrorMsg.UnknownVersion, args));
            }
        }

        internal void Write(BinaryWriter writer)
        {
            writer.Write(0);
            writer.Write((int) this.PlayMode);
            writer.Write(this.IsLoop);
            writer.Write(this.PlayVolume);
            writer.Write(this.ResourceVolume);
            writer.Write(this.IntroTime);
            writer.Write(this.Tag);
            writer.Write(this.File.FileName);
        }

        public AudioClip Clip
        {
            get
            {
                if (this.clip == null)
                {
                    this.clip = this.File.Sound;
                }
                return this.clip;
            }
        }

        public bool EnableIntroLoop
        {
            get
            {
                return (this.IsLoop && (this.IntroTime > 0f));
            }
        }

        internal bool EnableSave
        {
            get
            {
                return ((this.File != null) && this.IsLoop);
            }
        }

        public AssetFile File { get; private set; }

        public float IntroTime { get; set; }

        public bool IsLoop { get; set; }

        public string Name
        {
            get
            {
                return ((this.File == null) ? this.Clip.get_name() : this.File.FileName);
            }
        }

        public SoundPlayMode PlayMode { get; private set; }

        public float PlayVolume { get; set; }

        public float ResourceVolume { get; set; }

        public string Tag { get; set; }

        public float Volume
        {
            get
            {
                return (this.ResourceVolume * this.PlayVolume);
            }
        }
    }
}

