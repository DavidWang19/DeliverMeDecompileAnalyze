namespace Utage
{
    using System;
    using UnityEngine;

    public abstract class UguiLocalizeBase : MonoBehaviour
    {
        [NonSerialized]
        protected string currentLanguage;
        [NonSerialized]
        protected bool isInit;

        protected UguiLocalizeBase()
        {
        }

        public virtual void EditorRefresh()
        {
            LanguageManagerBase instance = LanguageManagerBase.Instance;
            if (instance != null)
            {
                this.currentLanguage = instance.CurrentLanguage;
                if (!this.isInit)
                {
                    this.isInit = true;
                    this.InitDefault();
                }
                this.RefreshSub();
            }
        }

        protected virtual void ForceRefresh()
        {
            this.currentLanguage = string.Empty;
            this.Refresh();
        }

        protected abstract void InitDefault();
        protected virtual bool IsEnable()
        {
            if (!Application.get_isPlaying())
            {
                return false;
            }
            if (!base.get_gameObject().get_activeInHierarchy())
            {
                return false;
            }
            return true;
        }

        protected virtual void OnEnable()
        {
            if (!this.isInit)
            {
                this.isInit = true;
                this.InitDefault();
            }
            this.ForceRefresh();
        }

        public virtual void OnLocalize()
        {
            this.ForceRefresh();
        }

        protected virtual void OnValidate()
        {
            this.ForceRefresh();
        }

        protected virtual void Refresh()
        {
            LanguageManagerBase instance = LanguageManagerBase.Instance;
            if (((instance != null) && (this.currentLanguage != instance.CurrentLanguage)) && this.IsEnable())
            {
                this.currentLanguage = instance.CurrentLanguage;
                this.RefreshSub();
            }
        }

        protected abstract void RefreshSub();
        public abstract void ResetDefault();
    }
}

