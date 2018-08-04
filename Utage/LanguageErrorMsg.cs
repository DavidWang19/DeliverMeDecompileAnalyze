namespace Utage
{
    using System;
    using UnityEngine;

    public static class LanguageErrorMsg
    {
        private const string LanguageDataName = "ErrorMsg";

        public static string LocalizeText(ErrorMsg type)
        {
            LanguageManagerBase instance = LanguageManagerBase.Instance;
            if (instance != null)
            {
                return instance.LocalizeText("ErrorMsg", type.ToString());
            }
            Debug.LogWarning("LanguageManager is NULL");
            return type.ToString();
        }

        public static string LocalizeTextFormat(ErrorMsg type, params object[] args)
        {
            return string.Format(LocalizeText(type), args);
        }
    }
}

