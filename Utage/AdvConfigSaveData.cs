namespace Utage
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;

    [Serializable]
    public class AdvConfigSaveData
    {
        public float ambienceVolume = 0.5f;
        public float autoBrPageSpeed = 0.5f;
        public float bgmVolume = 0.5f;
        public bool hideMessageWindowOnPlayingVoice;
        public bool isAutoBrPage;
        public bool isEffect = true;
        public bool isFullScreen;
        public bool isMouseWheelSendMessage = true;
        public bool isPlayingTextSound;
        public bool isSkipUnread;
        public bool isStopSkipInSelection = true;
        public float messageSpeed = 0.5f;
        public float messageSpeedRead = 0.5f;
        public float messageWindowTransparency = 0.1f;
        public float seVolume = 0.5f;
        public float soundMasterVolume = 1f;
        public List<TaggedMasterVolume> taggedMasterVolumeList;
        private const int VERSION = 1;
        private const int VERSION0 = 0;
        public VoiceStopType voiceStopType;
        public float voiceVolume = 0.75f;

        public AdvConfigSaveData()
        {
            List<TaggedMasterVolume> list = new List<TaggedMasterVolume>();
            TaggedMasterVolume item = new TaggedMasterVolume {
                tag = "Others",
                volume = 1f
            };
            list.Add(item);
            this.taggedMasterVolumeList = list;
        }

        public void Read(BinaryReader reader)
        {
            int num = reader.ReadInt32();
            if (num <= 1)
            {
                this.isFullScreen = reader.ReadBoolean();
                this.isMouseWheelSendMessage = reader.ReadBoolean();
                this.isEffect = reader.ReadBoolean();
                this.isSkipUnread = reader.ReadBoolean();
                this.isStopSkipInSelection = reader.ReadBoolean();
                this.messageSpeed = reader.ReadSingle();
                this.autoBrPageSpeed = reader.ReadSingle();
                this.messageWindowTransparency = reader.ReadSingle();
                this.soundMasterVolume = reader.ReadSingle();
                this.bgmVolume = reader.ReadSingle();
                this.seVolume = reader.ReadSingle();
                this.ambienceVolume = reader.ReadSingle();
                this.voiceVolume = reader.ReadSingle();
                this.voiceStopType = (VoiceStopType) reader.ReadInt32();
                int num2 = reader.ReadInt32();
                for (int i = 0; i < num2; i++)
                {
                    reader.ReadBoolean();
                }
                this.isAutoBrPage = reader.ReadBoolean();
                if (num > 0)
                {
                    this.messageSpeedRead = reader.ReadSingle();
                    this.hideMessageWindowOnPlayingVoice = reader.ReadBoolean();
                    this.isPlayingTextSound = reader.ReadBoolean();
                    int num4 = reader.ReadInt32();
                    this.taggedMasterVolumeList.Clear();
                    for (int j = 0; j < num4; j++)
                    {
                        TaggedMasterVolume item = new TaggedMasterVolume {
                            tag = reader.ReadString(),
                            volume = reader.ReadSingle()
                        };
                        this.taggedMasterVolumeList.Add(item);
                    }
                }
            }
            else
            {
                object[] args = new object[] { num };
                Debug.LogError(LanguageErrorMsg.LocalizeTextFormat(ErrorMsg.UnknownVersion, args));
            }
        }

        public void SetTaggedMasterVolume(string tag, float volume)
        {
            <SetTaggedMasterVolume>c__AnonStorey0 storey = new <SetTaggedMasterVolume>c__AnonStorey0 {
                tag = tag
            };
            TaggedMasterVolume item = this.taggedMasterVolumeList.Find(new Predicate<TaggedMasterVolume>(storey.<>m__0));
            if (item == null)
            {
                item = new TaggedMasterVolume {
                    tag = storey.tag
                };
                this.taggedMasterVolumeList.Add(item);
            }
            item.volume = volume;
        }

        public bool TryGetTaggedMasterVolume(string tag, out float volume)
        {
            <TryGetTaggedMasterVolume>c__AnonStorey1 storey = new <TryGetTaggedMasterVolume>c__AnonStorey1 {
                tag = tag
            };
            TaggedMasterVolume volume2 = this.taggedMasterVolumeList.Find(new Predicate<TaggedMasterVolume>(storey.<>m__0));
            if (volume2 == null)
            {
                volume = 0f;
                return false;
            }
            volume = volume2.volume;
            return true;
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write(1);
            writer.Write(this.isFullScreen);
            writer.Write(this.isMouseWheelSendMessage);
            writer.Write(this.isEffect);
            writer.Write(this.isSkipUnread);
            writer.Write(this.isStopSkipInSelection);
            writer.Write(this.messageSpeed);
            writer.Write(this.autoBrPageSpeed);
            writer.Write(this.messageWindowTransparency);
            writer.Write(this.soundMasterVolume);
            writer.Write(this.bgmVolume);
            writer.Write(this.seVolume);
            writer.Write(this.ambienceVolume);
            writer.Write(this.voiceVolume);
            writer.Write((int) this.voiceStopType);
            writer.Write(0);
            writer.Write(this.isAutoBrPage);
            writer.Write(this.messageSpeedRead);
            writer.Write(this.hideMessageWindowOnPlayingVoice);
            writer.Write(this.isPlayingTextSound);
            writer.Write(this.taggedMasterVolumeList.Count);
            foreach (TaggedMasterVolume volume in this.taggedMasterVolumeList)
            {
                writer.Write(volume.tag);
                writer.Write(volume.volume);
            }
        }

        [CompilerGenerated]
        private sealed class <SetTaggedMasterVolume>c__AnonStorey0
        {
            internal string tag;

            internal bool <>m__0(AdvConfigSaveData.TaggedMasterVolume x)
            {
                return (x.tag == this.tag);
            }
        }

        [CompilerGenerated]
        private sealed class <TryGetTaggedMasterVolume>c__AnonStorey1
        {
            internal string tag;

            internal bool <>m__0(AdvConfigSaveData.TaggedMasterVolume x)
            {
                return (x.tag == this.tag);
            }
        }

        [Serializable]
        public class TaggedMasterVolume
        {
            public string tag;
            public float volume;
        }
    }
}

