namespace Utage
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    [AddComponentMenu("Utage/Lib/UI/LocalizeRectTransform")]
    public class UguiLocalizeRectTransform : UguiLocalizeBase
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <HasChanged>k__BackingField;
        private RectTransform cachedRectTransform;
        [NonSerialized]
        private Setting defaultSetting;
        [SerializeField]
        private List<Setting> settingList = new List<Setting>();

        protected override void InitDefault()
        {
        }

        private void InitDefaultSetting()
        {
            this.defaultSetting = new Setting();
            this.defaultSetting.anchoredPosition = this.CachedRectTransform.get_anchoredPosition();
            this.defaultSetting.size = this.CachedRectTransform.get_rect().get_size();
        }

        protected override void RefreshSub()
        {
            this.HasChanged = true;
        }

        public override void ResetDefault()
        {
            if (this.defaultSetting != null)
            {
                this.CachedRectTransform.set_anchoredPosition(this.defaultSetting.anchoredPosition);
                this.CachedRectTransform.SetSizeWithCurrentAnchors(0, this.defaultSetting.size.x);
                this.CachedRectTransform.SetSizeWithCurrentAnchors(1, this.defaultSetting.size.y);
            }
        }

        private void Update()
        {
            if (this.defaultSetting == null)
            {
                this.InitDefaultSetting();
            }
            if (this.HasChanged)
            {
                Setting defaultSetting = this.settingList.Find(x => x.language == base.currentLanguage);
                if (defaultSetting == null)
                {
                    defaultSetting = this.defaultSetting;
                }
                if (defaultSetting != null)
                {
                    this.CachedRectTransform.set_anchoredPosition(defaultSetting.anchoredPosition);
                    this.CachedRectTransform.SetSizeWithCurrentAnchors(0, defaultSetting.size.x);
                    this.CachedRectTransform.SetSizeWithCurrentAnchors(1, defaultSetting.size.y);
                }
            }
        }

        private RectTransform CachedRectTransform
        {
            get
            {
                if (null == this.cachedRectTransform)
                {
                    this.cachedRectTransform = base.GetComponent<RectTransform>();
                }
                return this.cachedRectTransform;
            }
        }

        private bool HasChanged { get; set; }

        [Serializable]
        public class Setting
        {
            public Vector2 anchoredPosition;
            public string language;
            public Vector2 size = new Vector2(100f, 100f);
        }
    }
}

