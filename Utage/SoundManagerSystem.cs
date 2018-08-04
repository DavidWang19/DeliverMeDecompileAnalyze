namespace Utage
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UtageExtensions;

    public class SoundManagerSystem : SoundManagerSystemInterface
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Utage.SoundManager <SoundManager>k__BackingField;
        private const string GameObjectNameSe = "One shot audio";
        private Dictionary<string, SoundGroup> groups = new Dictionary<string, SoundGroup>();
        private const int Version = 0;

        public AudioSource GetAudioSource(string groupName, string label)
        {
            SoundGroup group = this.GetGroup(groupName);
            if (group == null)
            {
                return null;
            }
            return group.GetAudioSource(label);
        }

        public SoundGroup GetGroup(string name)
        {
            SoundGroup group;
            if (!this.Groups.TryGetValue(name, out group))
            {
                return null;
            }
            return group;
        }

        private SoundGroup GetGroupAndCreateIfMissing(string name)
        {
            SoundGroup group = this.GetGroup(name);
            if (group == null)
            {
                group = this.SoundManager.get_transform().Find<SoundGroup>(name);
                if (group == null)
                {
                    group = this.SoundManager.get_transform().AddChildGameObjectComponent<SoundGroup>(name);
                    if (name != null)
                    {
                        if (!(name == "Bgm"))
                        {
                            if (name == "Ambience")
                            {
                                group.DuckGroups.Add(this.GetGroupAndCreateIfMissing("Voice"));
                            }
                            else if (name == "Voice")
                            {
                                group.AutoDestoryPlayer = true;
                            }
                            else if (name == "Se")
                            {
                                group.AutoDestoryPlayer = true;
                                group.MultiPlay = true;
                            }
                        }
                        else
                        {
                            group.DuckGroups.Add(this.GetGroupAndCreateIfMissing("Voice"));
                        }
                    }
                }
                group.Init(this);
                this.Groups.Add(name, group);
            }
            return group;
        }

        public float GetGroupVolume(string groupName)
        {
            SoundGroup group = this.GetGroup(groupName);
            if (group == null)
            {
                Debug.LogError(groupName + " is not created");
                return 1f;
            }
            return group.GroupVolume;
        }

        public float GetMasterVolume(string groupName)
        {
            SoundGroup group = this.GetGroup(groupName);
            if (group == null)
            {
                Debug.LogError(groupName + " is not created");
                return 1f;
            }
            return group.MasterVolume;
        }

        public float GetSamplesVolume(string groupName, string label)
        {
            SoundGroup group = this.GetGroup(groupName);
            if (group == null)
            {
                return 0f;
            }
            return group.GetSamplesVolume(label);
        }

        public void Init(Utage.SoundManager soundManager, List<string> saveStreamNameList)
        {
            this.SoundManager = soundManager;
        }

        public bool IsMultiPlay(string groupName)
        {
            SoundGroup group = this.GetGroup(groupName);
            if (group == null)
            {
                Debug.LogError(groupName + " is not created");
                return false;
            }
            return group.MultiPlay;
        }

        public bool IsPlaying(string groupName, string label)
        {
            SoundGroup group = this.GetGroup(groupName);
            if (group == null)
            {
                return false;
            }
            return group.IsPlaying(label);
        }

        public void Play(string groupName, string label, SoundData data, float fadeInTime, float fadeOutTime)
        {
            this.GetGroupAndCreateIfMissing(groupName).Play(label, data, fadeInTime, fadeOutTime);
        }

        public void ReadSaveDataBuffer(BinaryReader reader)
        {
            int num = reader.ReadInt32();
            if (num <= 0)
            {
                int num2 = reader.ReadInt32();
                List<SoundGroup> list = new List<SoundGroup>();
                for (int i = 0; i < num2; i++)
                {
                    string name = reader.ReadString();
                    list.Add(this.GetGroupAndCreateIfMissing(name));
                }
                for (int j = 0; j < num2; j++)
                {
                    reader.ReadBuffer(new Action<BinaryReader>(list[j].Read));
                }
            }
            else
            {
                object[] args = new object[] { num };
                Debug.LogError(LanguageErrorMsg.LocalizeTextFormat(ErrorMsg.UnknownVersion, args));
            }
        }

        public void SetGroupVolume(string groupName, float volume)
        {
            this.GetGroupAndCreateIfMissing(groupName).GroupVolume = volume;
        }

        public void SetMasterVolume(string groupName, float volume)
        {
            this.GetGroupAndCreateIfMissing(groupName).MasterVolume = volume;
        }

        public void SetMultiPlay(string groupName, bool multiPlay)
        {
            this.GetGroupAndCreateIfMissing(groupName).MultiPlay = multiPlay;
        }

        public void Stop(string groupName, string label, float fadeTime)
        {
            SoundGroup group = this.GetGroup(groupName);
            if (group != null)
            {
                group.Stop(label, fadeTime);
            }
        }

        public void StopAll(float fadeTime)
        {
            foreach (KeyValuePair<string, SoundGroup> pair in this.Groups)
            {
                pair.Value.StopAll(fadeTime);
            }
        }

        public void StopGroup(string groupName, float fadeTime)
        {
            SoundGroup group = this.GetGroup(groupName);
            if (group != null)
            {
                group.StopAll(fadeTime);
            }
        }

        public void StopGroupIgnoreLoop(string groupName, float fadeTime)
        {
            SoundGroup group = this.GetGroup(groupName);
            if (group != null)
            {
                group.StopAllIgnoreLoop(fadeTime);
            }
        }

        public void WriteSaveData(BinaryWriter writer)
        {
            writer.Write(0);
            writer.Write(this.Groups.Count);
            foreach (KeyValuePair<string, SoundGroup> pair in this.Groups)
            {
                writer.Write(pair.Key);
            }
            foreach (KeyValuePair<string, SoundGroup> pair2 in this.Groups)
            {
                writer.WriteBuffer(new Action<BinaryWriter>(pair2.Value.Write));
            }
        }

        private Dictionary<string, SoundGroup> Groups
        {
            get
            {
                return this.groups;
            }
        }

        public bool IsLoading
        {
            get
            {
                foreach (KeyValuePair<string, SoundGroup> pair in this.Groups)
                {
                    if (pair.Value.IsLoading)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        internal Utage.SoundManager SoundManager { get; private set; }
    }
}

