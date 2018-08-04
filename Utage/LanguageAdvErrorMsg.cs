namespace Utage
{
    using System;
    using UnityEngine;

    public static class LanguageAdvErrorMsg
    {
        private const string LanguageDataName = "AdvErrorMsg";

        public static string LocalizeText(AdvErrorMsg type)
        {
            LanguageManagerBase instance = LanguageManagerBase.Instance;
            if (instance != null)
            {
                return instance.LocalizeText("AdvErrorMsg", type.ToString());
            }
            Debug.LogWarning("LanguageManager is NULL");
            return type.ToString();
        }

        public static string LocalizeTextFormat(AdvErrorMsg type, params object[] args)
        {
            return string.Format(LocalizeText(type), args);
        }
    }
}

