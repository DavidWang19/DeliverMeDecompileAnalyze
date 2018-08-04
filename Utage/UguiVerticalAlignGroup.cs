namespace Utage
{
    using System;
    using System.Collections;
    using UnityEngine;

    [ExecuteInEditMode, AddComponentMenu("Utage/Lib/UI/VerticalAlignGroup")]
    public class UguiVerticalAlignGroup : UguiAlignGroup
    {
        public AlignDirection direction;
        public float paddingBottom;
        public float paddingTop;

        protected virtual float AlignChild(RectTransform child, ref float offset)
        {
            float num = (this.direction != AlignDirection.BottomToTop) ? ((float) (-1)) : ((float) 1);
            float num2 = (this.direction != AlignDirection.BottomToTop) ? ((float) 1) : ((float) 0);
            DrivenTransformProperties properties = 0xa04;
            this.tracker.Add(this, child, properties);
            child.set_anchorMin(new Vector2(child.get_anchorMin().x, num2));
            child.set_anchorMax(new Vector2(child.get_anchorMax().x, num2));
            this.CustomChild(child, offset);
            float num3 = child.get_rect().get_height() * Mathf.Abs(child.get_localScale().y);
            offset += num * (num3 * child.get_pivot().y);
            child.set_anchoredPosition(new Vector2(child.get_anchoredPosition().x, offset));
            offset += num * ((num3 * (1f - child.get_pivot().y)) + base.space);
            return num3;
        }

        protected virtual void CustomChild(RectTransform child, float offset)
        {
        }

        protected virtual void CustomLayoutRectTransform()
        {
        }

        protected virtual void LayoutRectTransorm(float totalSize)
        {
            if (base.isAutoResize)
            {
                this.tracker.Add(this, base.CachedRectTransform, 0x2000);
                base.CachedRectTransform.set_sizeDelta(new Vector2(base.CachedRectTransform.get_sizeDelta().x, totalSize));
            }
            this.CustomLayoutRectTransform();
        }

        public override void Reposition()
        {
            if (base.CachedRectTransform.get_childCount() > 0)
            {
                float offset = (this.direction != AlignDirection.BottomToTop) ? -this.paddingTop : this.paddingBottom;
                float totalSize = 0f;
                IEnumerator enumerator = base.CachedRectTransform.GetEnumerator();
                try
                {
                    while (enumerator.MoveNext())
                    {
                        RectTransform current = (RectTransform) enumerator.Current;
                        float num3 = this.AlignChild(current, ref offset);
                        totalSize += num3 + base.space;
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
                totalSize += (this.paddingBottom + this.paddingTop) - base.space;
                this.LayoutRectTransorm(totalSize);
            }
        }

        public enum AlignDirection
        {
            TopToBottom,
            BottomToTop
        }
    }
}

