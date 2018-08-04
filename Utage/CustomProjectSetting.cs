namespace Utage
{
    using System;
    using UnityEngine;

    public class CustomProjectSetting : ScriptableObject
    {
        private static CustomProjectSetting instance;
        [SerializeField]
        private LanguageManager language;

        public static CustomProjectSetting Instance
        {
            get
            {
                if (instance == null)
                {
                    BootCustomProjectSetting setting = Object.FindObjectOfType<BootCustomProjectSetting>();
                    if (setting != null)
                    {
                        instance = setting.CustomProjectSetting;
                        if (instance == null)
                        {
                            Debug.LogError("CustomProjectSetting is NONE", setting);
                        }
                    }
                }
                return instance;
            }
        }

        public LanguageManager Language
        {
            get
            {
                return this.language;
            }
            set
            {
                this.language = value;
            }
        }
    }
}

