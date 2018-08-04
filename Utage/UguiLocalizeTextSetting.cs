namespace Utage
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    [AddComponentMenu("Utage/Lib/UI/Localize/TextSetting")]
    public class UguiLocalizeTextSetting : UguiLocalizeBase
    {
        private Text cachedText;
        [NonSerialized]
        private Setting defaultSetting;
        [SerializeField]
        private List<Setting> settingList = new List<Setting>();

        protected override void InitDefault()
        {
            this.defaultSetting = new Setting();
            this.defaultSetting.font = this.CachedText.get_font();
            this.defaultSetting.fontSize = this.CachedText.get_fontSize();
            this.defaultSetting.lineSpacing = this.CachedText.get_lineSpacing();
        }

        protected override void RefreshSub()
        {
            if (this.CachedText != null)
            {
                Setting defaultSetting = this.settingList.Find(x => x.language == base.currentLanguage);
                if (defaultSetting == null)
                {
                    defaultSetting = this.defaultSetting;
                }
                if (defaultSetting != null)
                {
                    this.CachedText.set_font((defaultSetting.font == null) ? this.defaultSetting.font : defaultSetting.font);
                    this.CachedText.set_fontSize(defaultSetting.fontSize);
                    this.CachedText.set_lineSpacing(defaultSetting.lineSpacing);
                }
            }
        }

        public override void ResetDefault()
        {
            this.CachedText.set_font(this.defaultSetting.font);
            this.CachedText.set_fontSize(this.defaultSetting.fontSize);
            this.CachedText.set_lineSpacing(this.defaultSetting.lineSpacing);
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

        [Serializable]
        public class Setting
        {
            public Font font;
            public int fontSize = 20;
            public string language;
            public float lineSpacing = 1f;
        }
    }
}

