namespace Utage
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public abstract class LipSynchBase : MonoBehaviour
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <EnableTextLipSync>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <IsEnable>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <IsPlaying>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Utage.LipSynchMode <LipSynchMode>k__BackingField;
        private string characterLabel;
        public LipSynchEvent OnCheckTextLipSync = new LipSynchEvent();
        [SerializeField]
        private LipSynchType type = LipSynchType.TextAndVoice;

        protected LipSynchBase()
        {
        }

        public void Cancel()
        {
            this.IsEnable = false;
            this.IsPlaying = false;
            this.OnStopLipSync();
        }

        protected bool CheckTextLipSync()
        {
            switch (this.Type)
            {
                case LipSynchType.Text:
                case LipSynchType.TextAndVoice:
                    this.OnCheckTextLipSync.Invoke(this);
                    return this.EnableTextLipSync;
            }
            return false;
        }

        protected bool CheckVoiceLipSync()
        {
            switch (this.Type)
            {
                case LipSynchType.Voice:
                case LipSynchType.TextAndVoice:
                {
                    SoundManager instance = SoundManager.GetInstance();
                    if ((instance != null) && instance.IsPlayingVoice(this.CharacterLabel))
                    {
                        return true;
                    }
                    break;
                }
            }
            return false;
        }

        protected abstract void OnStartLipSync();
        protected abstract void OnStopLipSync();
        protected abstract void OnUpdateLipSync();
        public void Play()
        {
            this.IsEnable = true;
        }

        protected virtual void Update()
        {
            bool flag = this.CheckVoiceLipSync();
            bool flag2 = this.CheckTextLipSync();
            this.LipSynchMode = !flag ? Utage.LipSynchMode.Text : Utage.LipSynchMode.Voice;
            if (this.IsEnable && (flag || flag2))
            {
                if (!this.IsPlaying)
                {
                    this.IsPlaying = true;
                    this.OnStartLipSync();
                }
                this.OnUpdateLipSync();
            }
            else if (this.IsPlaying)
            {
                this.IsPlaying = false;
                this.OnStopLipSync();
            }
        }

        public string CharacterLabel
        {
            get
            {
                if (string.IsNullOrEmpty(this.characterLabel))
                {
                    return base.get_gameObject().get_name();
                }
                return this.characterLabel;
            }
            set
            {
                this.characterLabel = value;
            }
        }

        public bool EnableTextLipSync { get; set; }

        public bool IsEnable { get; set; }

        public bool IsPlaying { get; set; }

        public Utage.LipSynchMode LipSynchMode { get; set; }

        public LipSynchType Type
        {
            get
            {
                return this.type;
            }
            set
            {
                this.type = value;
            }
        }
    }
}

