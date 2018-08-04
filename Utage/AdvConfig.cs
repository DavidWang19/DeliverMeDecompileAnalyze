namespace Utage
{
    using System;
    using System.IO;
    using System.Runtime.InteropServices;
    using UnityEngine;
    using UnityEngine.Serialization;

    [AddComponentMenu("Utage/ADV/Internal/Config")]
    public class AdvConfig : MonoBehaviour, IBinaryIO
    {
        [SerializeField]
        private float autoPageWaitSecMax = 2.5f;
        [SerializeField]
        private float autoPageWaitSecMin;
        [SerializeField]
        private float bgmVolumeFilterOfPlayingVoice = 0.5f;
        private AdvConfigSaveData current = new AdvConfigSaveData();
        [SerializeField]
        private AdvConfigSaveData defaultData;
        [SerializeField, FormerlySerializedAs("dontUseFullScreen")]
        private bool dontSaveFullScreen = true;
        [SerializeField]
        private bool dontUseSystemSaveData;
        [SerializeField]
        private bool forceSkipInputCtl = true;
        private bool isSkip;
        [SerializeField]
        private float sendCharWaitSecMax = 0.1f;
        [FormerlySerializedAs("skipSpped"), SerializeField]
        private float skipSpeed = 20f;
        [SerializeField]
        private bool skipVoiceAndSe;
        [SerializeField]
        private bool useMessageSpeedRead;

        public bool CheckSkip(bool isReadPage)
        {
            if (this.forceSkipInputCtl && InputUtil.IsInputControl())
            {
                return true;
            }
            if (!this.isSkip)
            {
                return false;
            }
            return (this.IsSkipUnread || isReadPage);
        }

        public float GetTimeSendChar(bool read)
        {
            if (read && this.useMessageSpeedRead)
            {
                return ((1f - this.MessageSpeedRead) * this.sendCharWaitSecMax);
            }
            return ((1f - this.MessageSpeed) * this.sendCharWaitSecMax);
        }

        public void InitDefault()
        {
            this.SetData(this.defaultData, false);
        }

        public void InitDefaultAll()
        {
            this.SetData(this.defaultData, true);
        }

        public void OnRead(BinaryReader reader)
        {
            AdvConfigSaveData data = new AdvConfigSaveData();
            data.Read(reader);
            if (!this.dontUseSystemSaveData)
            {
                this.SetData(data, false);
            }
            else
            {
                this.InitDefault();
            }
        }

        public void OnWrite(BinaryWriter writer)
        {
            this.current.Write(writer);
        }

        private void SetData(AdvConfigSaveData data, bool isSetDefault)
        {
            if (UtageToolKit.IsPlatformStandAloneOrEditor())
            {
                if (this.dontSaveFullScreen)
                {
                    this.IsFullScreen = Screen.get_fullScreen();
                }
                else
                {
                    this.IsFullScreen = data.isFullScreen;
                }
            }
            this.IsMouseWheelSendMessage = data.isMouseWheelSendMessage;
            this.IsEffect = data.isEffect;
            this.IsSkipUnread = data.isSkipUnread;
            this.IsStopSkipInSelection = data.isStopSkipInSelection;
            this.MessageSpeed = data.messageSpeed;
            this.AutoBrPageSpeed = data.autoBrPageSpeed;
            this.MessageWindowTransparency = data.messageWindowTransparency;
            this.SoundMasterVolume = data.soundMasterVolume;
            this.BgmVolume = data.bgmVolume;
            this.SeVolume = data.seVolume;
            this.AmbienceVolume = data.ambienceVolume;
            this.VoiceVolume = data.voiceVolume;
            this.VoiceStopType = data.voiceStopType;
            if (!isSetDefault)
            {
                this.IsAutoBrPage = data.isAutoBrPage;
            }
            this.MessageSpeedRead = data.messageSpeedRead;
            this.HideMessageWindowOnPlayingVoice = data.hideMessageWindowOnPlayingVoice;
            this.IsPlayingTextSound = data.isPlayingTextSound;
            this.current.taggedMasterVolumeList.Clear();
            int count = data.taggedMasterVolumeList.Count;
            for (int i = 0; i < count; i++)
            {
                this.SetTaggedMasterVolume(data.taggedMasterVolumeList[i].tag, data.taggedMasterVolumeList[i].volume);
            }
        }

        public void SetTaggedMasterVolume(string tag, float volume)
        {
            this.current.SetTaggedMasterVolume(tag, volume);
            SoundManager instance = SoundManager.GetInstance();
            if (instance != null)
            {
                instance.SetTaggedMasterVolume(tag, volume);
            }
        }

        public void StopSkipInSelection()
        {
            if (this.IsStopSkipInSelection && this.isSkip)
            {
                this.isSkip = false;
            }
        }

        public void ToggleAuto()
        {
            this.IsAutoBrPage = !this.IsAutoBrPage;
        }

        public void ToggleEffect()
        {
            this.IsEffect = !this.IsEffect;
        }

        public void ToggleFullScreen()
        {
            this.IsFullScreen = !this.IsFullScreen;
        }

        public void ToggleMouseWheelSendMessage()
        {
            this.IsMouseWheelSendMessage = !this.IsMouseWheelSendMessage;
        }

        public void TogglePlayingTextSound()
        {
            this.IsPlayingTextSound = !this.IsPlayingTextSound;
        }

        public void ToggleSkip()
        {
            this.isSkip = !this.isSkip;
        }

        public void ToggleSkipUnread()
        {
            this.IsSkipUnread = !this.IsSkipUnread;
        }

        public void ToggleStopSkipInSelection()
        {
            this.IsStopSkipInSelection = !this.IsStopSkipInSelection;
        }

        public bool TryGetTaggedMasterVolume(string tag, out float volume)
        {
            return this.current.TryGetTaggedMasterVolume(tag, out volume);
        }

        public float AmbienceVolume
        {
            get
            {
                return this.current.ambienceVolume;
            }
            set
            {
                this.current.ambienceVolume = value;
                SoundManager instance = SoundManager.GetInstance();
                if (instance != null)
                {
                    instance.AmbienceVolume = value;
                }
            }
        }

        public float AutoBrPageSpeed
        {
            get
            {
                return this.current.autoBrPageSpeed;
            }
            set
            {
                this.current.autoBrPageSpeed = value;
            }
        }

        public float AutoPageWaitTime
        {
            get
            {
                return (((1f - this.AutoBrPageSpeed) * (this.autoPageWaitSecMax - this.autoPageWaitSecMin)) + this.autoPageWaitSecMin);
            }
        }

        public float BgmVolume
        {
            get
            {
                return this.current.bgmVolume;
            }
            set
            {
                this.current.bgmVolume = value;
                SoundManager instance = SoundManager.GetInstance();
                if (instance != null)
                {
                    instance.BgmVolume = value;
                    instance.DuckVolume = this.bgmVolumeFilterOfPlayingVoice;
                }
            }
        }

        public bool HideMessageWindowOnPlayingVoice
        {
            get
            {
                return this.current.hideMessageWindowOnPlayingVoice;
            }
            set
            {
                this.current.hideMessageWindowOnPlayingVoice = value;
            }
        }

        public bool IsAutoBrPage
        {
            get
            {
                return this.current.isAutoBrPage;
            }
            set
            {
                this.current.isAutoBrPage = value;
            }
        }

        public bool IsEffect
        {
            get
            {
                return this.current.isEffect;
            }
            set
            {
                this.current.isEffect = value;
            }
        }

        public bool IsFullScreen
        {
            get
            {
                return this.current.isFullScreen;
            }
            set
            {
                if (UtageToolKit.IsPlatformStandAloneOrEditor())
                {
                    this.current.isFullScreen = value;
                    Screen.set_fullScreen(value);
                }
            }
        }

        public bool IsMouseWheelSendMessage
        {
            get
            {
                return this.current.isMouseWheelSendMessage;
            }
            set
            {
                this.current.isMouseWheelSendMessage = value;
            }
        }

        public bool IsPlayingTextSound
        {
            get
            {
                return this.current.isPlayingTextSound;
            }
            set
            {
                this.current.isPlayingTextSound = value;
            }
        }

        public bool IsSkip
        {
            get
            {
                return this.isSkip;
            }
            set
            {
                this.isSkip = value;
            }
        }

        public bool IsSkipUnread
        {
            get
            {
                return this.current.isSkipUnread;
            }
            set
            {
                this.current.isSkipUnread = value;
            }
        }

        public bool IsStopSkipInSelection
        {
            get
            {
                return this.current.isStopSkipInSelection;
            }
            set
            {
                this.current.isStopSkipInSelection = value;
            }
        }

        public float MessageSpeed
        {
            get
            {
                return this.current.messageSpeed;
            }
            set
            {
                this.current.messageSpeed = value;
            }
        }

        public float MessageSpeedRead
        {
            get
            {
                return this.current.messageSpeedRead;
            }
            set
            {
                this.current.messageSpeedRead = value;
            }
        }

        public float MessageWindowAlpha
        {
            get
            {
                return (1f - this.MessageWindowTransparency);
            }
        }

        public float MessageWindowTransparency
        {
            get
            {
                return this.current.messageWindowTransparency;
            }
            set
            {
                this.current.messageWindowTransparency = value;
            }
        }

        public string SaveKey
        {
            get
            {
                return "AdvConfig";
            }
        }

        public float SeVolume
        {
            get
            {
                return this.current.seVolume;
            }
            set
            {
                this.current.seVolume = value;
                SoundManager instance = SoundManager.GetInstance();
                if (instance != null)
                {
                    instance.SeVolume = value;
                }
            }
        }

        public float SkipSpped
        {
            get
            {
                return this.skipSpeed;
            }
        }

        public bool SkipVoiceAndSe
        {
            get
            {
                return this.skipVoiceAndSe;
            }
        }

        public float SoundMasterVolume
        {
            get
            {
                return this.current.soundMasterVolume;
            }
            set
            {
                this.current.soundMasterVolume = value;
                SoundManager instance = SoundManager.GetInstance();
                if (instance != null)
                {
                    instance.MasterVolume = value;
                }
            }
        }

        public Utage.VoiceStopType VoiceStopType
        {
            get
            {
                return this.current.voiceStopType;
            }
            set
            {
                this.current.voiceStopType = value;
            }
        }

        public float VoiceVolume
        {
            get
            {
                return this.current.voiceVolume;
            }
            set
            {
                this.current.voiceVolume = value;
                SoundManager instance = SoundManager.GetInstance();
                if (instance != null)
                {
                    instance.VoiceVolume = value;
                }
            }
        }
    }
}

