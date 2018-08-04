namespace Utage
{
    using System;
    using System.Collections;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public abstract class LipSynch2d : LipSynchBase
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private float <LipSyncVolume>k__BackingField;
        [SerializeField]
        private MiniAnimationData animationData = new MiniAnimationData();
        protected Coroutine coLypSync;
        [SerializeField]
        private float duration = 0.2f;
        [SerializeField]
        private float interval = 0.2f;
        [SerializeField]
        private string lipTag = "lip";
        [SerializeField]
        private float scaleVoiceVolume = 1f;
        [SerializeField]
        private GameObject target;

        protected LipSynch2d()
        {
        }

        protected abstract IEnumerator CoUpdateLipSync();
        protected override void OnStartLipSync()
        {
            if (this.coLypSync == null)
            {
                this.coLypSync = base.StartCoroutine(this.CoUpdateLipSync());
            }
        }

        protected override void OnStopLipSync()
        {
            if (this.coLypSync != null)
            {
                base.StopCoroutine(this.coLypSync);
                this.coLypSync = null;
            }
        }

        protected override void OnUpdateLipSync()
        {
            if (base.CheckVoiceLipSync())
            {
                this.LipSyncVolume = SoundManager.GetInstance().GetVoiceSamplesVolume(base.CharacterLabel) * this.ScaleVoiceVolume;
            }
            else
            {
                this.LipSyncVolume = -1f;
            }
        }

        public MiniAnimationData AnimationData
        {
            get
            {
                return this.animationData;
            }
            set
            {
                this.animationData = value;
            }
        }

        public float Duration
        {
            get
            {
                return this.duration;
            }
            set
            {
                this.duration = value;
            }
        }

        public float Interval
        {
            get
            {
                return this.interval;
            }
            set
            {
                this.interval = value;
            }
        }

        public float LipSyncVolume { get; set; }

        public string LipTag
        {
            get
            {
                return this.lipTag;
            }
            set
            {
                this.lipTag = value;
            }
        }

        public float ScaleVoiceVolume
        {
            get
            {
                return this.scaleVoiceVolume;
            }
            set
            {
                this.scaleVoiceVolume = value;
            }
        }

        public GameObject Target
        {
            get
            {
                if (this.target == null)
                {
                    this.target = base.get_gameObject();
                }
                return this.target;
            }
            set
            {
                this.target = value;
            }
        }
    }
}

