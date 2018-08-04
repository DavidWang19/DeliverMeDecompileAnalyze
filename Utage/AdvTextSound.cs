namespace Utage
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.Events;

    [AddComponentMenu("Utage/ADV/Extra/TextSound")]
    public class AdvTextSound : MonoBehaviour
    {
        public AudioClip defaultSound;
        [SerializeField]
        private bool disable;
        [SerializeField]
        protected AdvEngine engine;
        public int intervalCount = 3;
        public float intervalTime = 0.1f;
        private int lastCharacterCount;
        private float lastTime;
        public List<SoundInfo> soundInfoList = new List<SoundInfo>();
        public Type type;

        private void Awake()
        {
            this.Engine.Page.OnBeginPage.AddListener(new UnityAction<AdvPage>(this, (IntPtr) this.OnBeginPage));
            this.Engine.Page.OnUpdateSendChar.AddListener(new UnityAction<AdvPage>(this, (IntPtr) this.OnUpdateSendChar));
        }

        private bool CheckPlaySe(AdvPage page)
        {
            this.Disable = !this.engine.Config.IsPlayingTextSound;
            if (!this.Disable)
            {
                if (page.CurrentTextLength == this.lastCharacterCount)
                {
                    return false;
                }
                Type type = this.type;
                if (type != Type.Time)
                {
                    if ((type == Type.CharacterCount) && (page.CurrentTextLength >= (this.lastCharacterCount + this.intervalCount)))
                    {
                        return true;
                    }
                }
                else if ((Time.get_time() - this.lastTime) > this.intervalTime)
                {
                    return true;
                }
            }
            return false;
        }

        private AudioClip GetSe(AdvPage page)
        {
            <GetSe>c__AnonStorey0 storey = new <GetSe>c__AnonStorey0 {
                page = page
            };
            SoundInfo info = this.soundInfoList.Find(new Predicate<SoundInfo>(storey.<>m__0));
            return ((info == null) ? this.defaultSound : info.sound);
        }

        private void OnBeginPage(AdvPage page)
        {
            this.lastTime = 0f;
            this.lastCharacterCount = -this.intervalCount;
        }

        private void OnUpdateSendChar(AdvPage page)
        {
            if (this.CheckPlaySe(page))
            {
                AudioClip se = this.GetSe(page);
                if (se != null)
                {
                    SoundManager.GetInstance().PlaySe(se, string.Empty, SoundPlayMode.Add, false);
                    this.lastCharacterCount = page.CurrentTextLength;
                    this.lastTime = Time.get_time();
                }
            }
        }

        public bool Disable
        {
            get
            {
                return this.disable;
            }
            set
            {
                this.disable = value;
            }
        }

        public AdvEngine Engine
        {
            get
            {
                if (this.engine == null)
                {
                }
                return (this.engine = Object.FindObjectOfType<AdvEngine>());
            }
        }

        [CompilerGenerated]
        private sealed class <GetSe>c__AnonStorey0
        {
            internal AdvPage page;

            internal bool <>m__0(AdvTextSound.SoundInfo x)
            {
                return (x.key == this.page.CharacterLabel);
            }
        }

        [Serializable]
        public class SoundInfo
        {
            public string key;
            public AudioClip sound;
        }

        public enum Type
        {
            Time,
            CharacterCount
        }
    }
}

