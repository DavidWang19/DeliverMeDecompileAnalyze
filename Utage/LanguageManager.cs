namespace Utage
{
    using System;
    using UnityEngine;

    public class LanguageManager : LanguageManagerBase
    {
        protected override void OnRefreshCurrentLanguage()
        {
            if (!base.IgnoreLocalizeUiText)
            {
                foreach (UguiLocalizeBase base2 in Object.FindObjectsOfType<UguiLocalizeBase>())
                {
                    base2.OnLocalize();
                }
            }
        }
    }
}

