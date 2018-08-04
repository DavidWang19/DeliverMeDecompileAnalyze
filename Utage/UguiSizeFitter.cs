namespace Utage
{
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    [ExecuteInEditMode, AddComponentMenu("Utage/Lib/UI/SizeFitter")]
    public class UguiSizeFitter : UguiLayoutControllerBase, ILayoutSelfController, ILayoutController
    {
        public RectTransform target;

        public void SetLayoutHorizontal()
        {
            this.tracker.Clear();
            if (this.target != null)
            {
                this.tracker.Add(this, base.CachedRectTransform, 0x3000);
                base.CachedRectTransform.set_sizeDelta(this.target.get_sizeDelta());
            }
        }

        public void SetLayoutVertical()
        {
        }

        protected override void Update()
        {
            if ((this.target != null) && (this.target.get_rect().get_size() != base.CachedRectTransform.get_rect().get_size()))
            {
                base.SetDirty();
            }
        }
    }
}

