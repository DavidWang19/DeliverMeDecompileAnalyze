namespace Utage
{
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    [ExecuteInEditMode, AddComponentMenu("Utage/Lib/UI/Localize")]
    public class UguiLocalize : UguiLocalizeBase
    {
        private Text cachedText;
        [NonSerialized]
        private string defaultText;
        [SerializeField]
        private string key;

        protected override void InitDefault()
        {
            Text cachedText = this.CachedText;
            if (cachedText != null)
            {
                this.defaultText = cachedText.get_text();
            }
        }

        protected override void RefreshSub()
        {
            Text cachedText = this.CachedText;
            if ((cachedText != null) && !LanguageManagerBase.Instance.IgnoreLocalizeUiText)
            {
                string str;
                if (LanguageManagerBase.Instance.TryLocalizeText(this.key, out str))
                {
                    cachedText.set_text(str);
                }
                else
                {
                    Debug.LogError(this.key + " is not found in localize key", this);
                }
            }
        }

        public override void ResetDefault()
        {
            Text cachedText = this.CachedText;
            if (cachedText != null)
            {
                cachedText.set_text(this.defaultText);
            }
        }

        private Text CachedText
        {
            get
            {
                if (null == this.cachedText)
                {
                    this.cachedText = base.GetComponent<Text>();
                }
                return this.cachedText;
            }
        }

        public string Key
        {
            get
            {
                return this.key;
            }
            set
            {
                this.key = value;
                this.ForceRefresh();
            }
        }
    }
}

