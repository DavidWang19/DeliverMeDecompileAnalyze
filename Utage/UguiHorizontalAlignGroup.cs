namespace Utage
{
    using System;
    using System.Collections;
    using UnityEngine;

    [ExecuteInEditMode, AddComponentMenu("Utage/Lib/UI/HorizontalAlignGroup")]
    public class UguiHorizontalAlignGroup : UguiAlignGroup
    {
        public AlignDirection direction;
        public float paddingLeft;
        public float paddingRight;

        protected virtual float AlignChild(RectTransform child, ref float offset)
        {
            float num = (this.direction != AlignDirection.LeftToRight) ? ((float) (-1)) : ((float) 1);
            float num2 = (this.direction != AlignDirection.LeftToRight) ? ((float) 1) : ((float) 0);
            DrivenTransformProperties properties = 0x502;
            this.tracker.Add(this, child, properties);
            child.set_anchorMin(new Vector2(num2, child.get_anchorMin().y));
            child.set_anchorMax(new Vector2(num2, child.get_anchorMax().y));
            this.CustomChild(child, offset);
            float num3 = child.get_rect().get_width() * Mathf.Abs(child.get_localScale().x);
            offset += num * (num3 * child.get_pivot().x);
            child.set_anchoredPosition(new Vector2(offset, child.get_anchoredPosition().y));
            offset += num * ((num3 * (1f - child.get_pivot().x)) + base.space);
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
                this.tracker.Add(this, base.CachedRectTransform, 0x1000);
                base.CachedRectTransform.set_sizeDelta(new Vector2(totalSize, base.CachedRectTransform.get_sizeDelta().y));
            }
            this.CustomLayoutRectTransform();
        }

        public override void Reposition()
        {
            if (base.CachedRectTransform.get_childCount() > 0)
            {
                float offset = (this.direction != AlignDirection.LeftToRight) ? -this.paddingRight : this.paddingLeft;
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
                totalSize += (this.paddingLeft + this.paddingRight) - base.space;
                this.LayoutRectTransorm(totalSize);
            }
        }

        public enum AlignDirection
        {
            LeftToRight,
            RightToLeft
        }
    }
}

