namespace Utage
{
    using System;
    using UnityEngine;

    public static class LanguageSystemText
    {
        private const string LanguageDataName = "SystemText";

        public static string LocalizeText(SystemText type)
        {
            LanguageManagerBase instance = LanguageManagerBase.Instance;
            if (instance != null)
            {
                return instance.LocalizeText("SystemText", type.ToString());
            }
            Debug.LogWarning("LanguageManager is NULL");
            return type.ToString();
        }
    }
}

