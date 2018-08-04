namespace Utage
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UtageExtensions;

    [AddComponentMenu("Utage/Lib/Sound/Group")]
    public class SoundGroup : MonoBehaviour
    {
        [CompilerGenerated]
        private static Predicate<SoundGroup> <>f__am$cache0;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private float <DuckVolume>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Utage.SoundManagerSystem <SoundManagerSystem>k__BackingField;
        [SerializeField]
        private bool autoDestoryPlayer;
        [SerializeField]
        private List<SoundGroup> duckGroups = new List<SoundGroup>();
        private float duckVelocity = 1f;
        [Range(0f, 1f), SerializeField]
        private float groupVolume = 1f;
        [Range(0f, 1f), SerializeField]
        private float masterVolume = 1f;
        [SerializeField]
        private bool multiPlay;
        private Dictionary<string, SoundAudioPlayer> playerList = new Dictionary<string, SoundAudioPlayer>();
        private const int Version = 1;
        private const int Version0 = 0;

        internal AudioSource GetAudioSource(string label)
        {
            SoundAudioPlayer player = this.GetPlayer(label);
            if (player == null)
            {
                return null;
            }
            return player.Audio.AudioSource;
        }

        private SoundAudioPlayer GetOnlyOnePlayer(string label, float fadeOutTime)
        {
            SoundAudioPlayer playerOrCreateIfMissing = this.GetPlayerOrCreateIfMissing(label);
            if (this.PlayerList.Count > 1)
            {
                foreach (KeyValuePair<string, SoundAudioPlayer> pair in this.PlayerList)
                {
                    if (pair.Value != playerOrCreateIfMissing)
                    {
                        pair.Value.Stop(fadeOutTime);
                    }
                }
            }
            return playerOrCreateIfMissing;
        }

        private SoundAudioPlayer GetPlayer(string label)
        {
            SoundAudioPlayer player;
            if (this.PlayerList.TryGetValue(label, out player))
            {
                return player;
            }
            return null;
        }

        private SoundAudioPlayer GetPlayerOrCreateIfMissing(string label)
        {
            SoundAudioPlayer player = this.GetPlayer(label);
            if (player == null)
            {
                player = base.get_transform().AddChildGameObjectComponent<SoundAudioPlayer>(label);
                player.Init(label, this);
                this.PlayerList.Add(label, player);
            }
            return player;
        }

        internal float GetSamplesVolume(string label)
        {
            SoundAudioPlayer player = this.GetPlayer(label);
            if (player == null)
            {
                return 0f;
            }
            return player.GetSamplesVolume();
        }

        internal float GetVolume(string tag)
        {
            float num = (this.GroupVolume * this.MasterVolume) * this.SoundManager.MasterVolume;
            foreach (Utage.SoundManager.TaggedMasterVolume volume in this.SoundManager.TaggedMasterVolumes)
            {
                if (volume.Tag == tag)
                {
                    num *= volume.Volume;
                }
            }
            return (num * this.DuckVolume);
        }

        internal void Init(Utage.SoundManagerSystem soundManagerSystem)
        {
            this.SoundManagerSystem = soundManagerSystem;
            this.DuckVolume = 1f;
            this.duckVelocity = 1f;
        }

        internal bool IsPlaying()
        {
            foreach (KeyValuePair<string, SoundAudioPlayer> pair in this.PlayerList)
            {
                if (pair.Value.IsPlaying())
                {
                    return true;
                }
            }
            return false;
        }

        internal bool IsPlaying(string label)
        {
            SoundAudioPlayer player = this.GetPlayer(label);
            if (player == null)
            {
                return false;
            }
            return player.IsPlaying();
        }

        internal bool IsPlayingLoop(string label)
        {
            SoundAudioPlayer player = this.GetPlayer(label);
            if (player == null)
            {
                return false;
            }
            return player.IsPlayingLoop();
        }

        internal void Play(string label, SoundData data, float fadeInTime, float fadeOutTime)
        {
            (!this.MultiPlay ? this.GetOnlyOnePlayer(label, fadeOutTime) : this.GetPlayerOrCreateIfMissing(label)).Play(data, fadeInTime, fadeOutTime);
        }

        internal void Read(BinaryReader reader)
        {
            int num = reader.ReadInt32();
            if (num <= 1)
            {
                if (num > 0)
                {
                    this.GroupVolume = reader.ReadSingle();
                }
                int num2 = reader.ReadInt32();
                for (int i = 0; i < num2; i++)
                {
                    string label = reader.ReadString();
                    SoundAudioPlayer playerOrCreateIfMissing = this.GetPlayerOrCreateIfMissing(label);
                    reader.ReadBuffer(new Action<BinaryReader>(playerOrCreateIfMissing.Read));
                }
            }
            else
            {
                object[] args = new object[] { num };
                Debug.LogError(LanguageErrorMsg.LocalizeTextFormat(ErrorMsg.UnknownVersion, args));
            }
        }

        internal void Remove(string label)
        {
            this.PlayerList.Remove(label);
        }

        internal void Stop(string label, float fadeTime)
        {
            SoundAudioPlayer player = this.GetPlayer(label);
            if (player != null)
            {
                player.Stop(fadeTime);
            }
        }

        internal void StopAll(float fadeTime)
        {
            foreach (KeyValuePair<string, SoundAudioPlayer> pair in this.PlayerList)
            {
                pair.Value.Stop(fadeTime);
            }
        }

        internal void StopAllIgnoreLoop(float fadeTime)
        {
            foreach (KeyValuePair<string, SoundAudioPlayer> pair in this.PlayerList)
            {
                if (!pair.Value.IsPlayingLoop())
                {
                    pair.Value.Stop(fadeTime);
                }
            }
        }

        private void Update()
        {
            if (Mathf.Approximately(1f, this.SoundManager.DuckVolume))
            {
                this.DuckVolume = 1f;
            }
            else if (this.DuckGroups.Count <= 0)
            {
                this.DuckVolume = 1f;
            }
            else
            {
                if (<>f__am$cache0 == null)
                {
                    <>f__am$cache0 = x => x.IsPlaying();
                }
                float num = !this.DuckGroups.Exists(<>f__am$cache0) ? 1f : this.SoundManager.DuckVolume;
                if (Mathf.Abs(num - this.DuckVolume) < 0.001f)
                {
                    this.DuckVolume = num;
                    this.duckVelocity = 0f;
                }
                else
                {
                    this.DuckVolume = Mathf.SmoothDamp(this.DuckVolume, num, ref this.duckVelocity, this.SoundManager.DuckFadeTime);
                }
            }
        }

        internal void Write(BinaryWriter writer)
        {
            writer.Write(1);
            writer.Write(this.GroupVolume);
            writer.Write(this.PlayerList.Count);
            foreach (KeyValuePair<string, SoundAudioPlayer> pair in this.PlayerList)
            {
                writer.Write(pair.Key);
                writer.WriteBuffer(new Action<BinaryWriter>(pair.Value.Write));
            }
        }

        public bool AutoDestoryPlayer
        {
            get
            {
                return this.autoDestoryPlayer;
            }
            set
            {
                this.autoDestoryPlayer = value;
            }
        }

        public List<SoundGroup> DuckGroups
        {
            get
            {
                return this.duckGroups;
            }
        }

        private float DuckVolume { get; set; }

        public string GroupName
        {
            get
            {
                return base.get_gameObject().get_name();
            }
        }

        public float GroupVolume
        {
            get
            {
                return this.groupVolume;
            }
            set
            {
                this.groupVolume = value;
            }
        }

        public bool IsLoading
        {
            get
            {
                foreach (KeyValuePair<string, SoundAudioPlayer> pair in this.PlayerList)
                {
                    if (pair.Value.IsLoading)
                    {
                        return true;
                    }
                }
                return false;
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

        public bool MultiPlay
        {
            get
            {
                return this.multiPlay;
            }
            set
            {
                this.multiPlay = value;
            }
        }

        internal Dictionary<string, SoundAudioPlayer> PlayerList
        {
            get
            {
                return this.playerList;
            }
        }

        internal Utage.SoundManager SoundManager
        {
            get
            {
                return this.SoundManagerSystem.SoundManager;
            }
        }

        internal Utage.SoundManagerSystem SoundManagerSystem { get; private set; }
    }
}

