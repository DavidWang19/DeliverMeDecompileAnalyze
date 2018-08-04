namespace Utage
{
    using System;
    using System.Collections;
    using UnityEngine;
    using UnityEngine.UI;

    [ExecuteInEditMode]
    public abstract class UguiLayoutControllerBase : MonoBehaviour
    {
        private RectTransform cachedRectTransform;
        protected DrivenRectTransformTracker tracker;

        protected UguiLayoutControllerBase()
        {
        }

        protected virtual void OnDisable()
        {
            this.tracker.Clear();
        }

        protected virtual void OnEnable()
        {
            this.SetDirty();
        }

        protected void SetDirty()
        {
            if (base.get_gameObject().get_activeInHierarchy())
            {
                LayoutRebuilder.MarkLayoutForRebuild(this.CachedRectTransform);
            }
        }

        protected virtual void Update()
        {
            bool flag = this.CachedRectTransform.get_hasChanged();
            if (!flag)
            {
                IEnumerator enumerator = this.CachedRectTransform.GetEnumerator();
                try
                {
                    while (enumerator.MoveNext())
                    {
                        RectTransform current = (RectTransform) enumerator.Current;
                        if (current.get_hasChanged())
                        {
                            flag = true;
                            goto Label_0065;
                        }
                    }
                }
                finally
                {
                    IDisposable disposable = enumerator as IDisposable;
                    if (disposable != null)
                    {
                        disposable.Dispose();
                    }
                }
            }
        Label_0065:
            if (flag)
            {
                this.SetDirty();
            }
        }

        public RectTransform CachedRectTransform
        {
            get
            {
                if (this.cachedRectTransform == null)
                {
                    this.cachedRectTransform = base.GetComponent<RectTransform>();
                }
                return this.cachedRectTransform;
            }
        }
    }
}

