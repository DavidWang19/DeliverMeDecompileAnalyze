namespace Utage
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public abstract class LanguageManagerBase : ScriptableObject
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private LanguageData <Data>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Action <OnChangeLanugage>k__BackingField;
        private const string Auto = "Auto";
        private string currentLanguage;
        [SerializeField]
        protected string defaultLanguage = "Japanese";
        [SerializeField]
        private bool ignoreLocalizeUiText;
        [SerializeField]
        private bool ignoreLocalizeVoice = true;
        private static LanguageManagerBase instance;
        [SerializeField]
        protected string language = "Auto";
        [SerializeField]
        private List<TextAsset> languageData = new List<TextAsset>();
        [SerializeField]
        private List<string> voiceLanguages = new List<string>();

        protected LanguageManagerBase()
        {
        }

        private void Init()
        {
            this.Data = new LanguageData();
            foreach (TextAsset asset in this.languageData)
            {
                if (asset != null)
                {
                    this.Data.OverwriteData(asset);
                }
            }
            this.currentLanguage = (!string.IsNullOrEmpty(this.language) && !(this.language == "Auto")) ? this.language : Application.get_systemLanguage().ToString();
            this.RefreshCurrentLanguage();
        }

        public string LocalizeText(string key)
        {
            string text = key;
            this.TryLocalizeText(key, out text);
            return text;
        }

        public string LocalizeText(string dataName, string key)
        {
            string str;
            if (this.Data.ContainsKey(key) && this.Data.TryLocalizeText(out str, this.CurrentLanguage, this.DefaultLanguage, key, dataName))
            {
                return str;
            }
            Debug.LogError(key + " is not found in " + dataName);
            return key;
        }

        private void OnEnable()
        {
            this.Init();
        }

        protected abstract void OnRefreshCurrentLanguage();
        internal void OverwriteData(StringGrid grid)
        {
            this.Data.OverwriteData(grid);
            this.RefreshCurrentLanguage();
        }

        protected void RefreshCurrentLanguage()
        {
            if (Instance == this)
            {
                if (this.OnChangeLanugage != null)
                {
                    this.OnChangeLanugage();
                }
                this.OnRefreshCurrentLanguage();
            }
        }

        public bool TryLocalizeText(string key, out string text)
        {
            text = key;
            return (this.Data.ContainsKey(key) && this.Data.TryLocalizeText(out text, this.CurrentLanguage, this.DefaultLanguage, key, string.Empty));
        }

        public string CurrentLanguage
        {
            get
            {
                return this.currentLanguage;
            }
            set
            {
                if (this.currentLanguage != value)
                {
                    this.currentLanguage = value;
                    this.RefreshCurrentLanguage();
                }
            }
        }

        private LanguageData Data { get; set; }

        public string DefaultLanguage
        {
            get
            {
                return this.defaultLanguage;
            }
        }

        public bool IgnoreLocalizeUiText
        {
            get
            {
                return this.ignoreLocalizeUiText;
            }
        }

        public bool IgnoreLocalizeVoice
        {
            get
            {
                return this.ignoreLocalizeVoice;
            }
        }

        public static LanguageManagerBase Instance
        {
            get
            {
                if (instance == null)
                {
                    if (CustomProjectSetting.Instance != null)
                    {
                        instance = CustomProjectSetting.Instance.Language;
                    }
                    if (instance != null)
                    {
                        instance.Init();
                    }
                }
                return instance;
            }
        }

        public string Language
        {
            get
            {
                return this.language;
            }
        }

        public List<string> Languages
        {
            get
            {
                return this.Data.Languages;
            }
        }

        public Action OnChangeLanugage { get; set; }

        public List<string> VoiceLanguages
        {
            get
            {
                return this.voiceLanguages;
            }
        }
    }
}

