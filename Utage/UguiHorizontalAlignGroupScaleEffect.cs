namespace Utage
{
    using System;
    using UnityEngine;

    [ExecuteInEditMode, AddComponentMenu("Utage/Lib/UI/HolizontalAlignGroupScaleEffect")]
    public class UguiHorizontalAlignGroupScaleEffect : UguiHorizontalAlignGroup
    {
        public bool ignoreLocalPositionToScaleEffectRage = true;
        public float maxScale = 1f;
        public float minScale = 0.5f;
        public float scaleRangeLeft = -100f;
        public float scaleRangeWidth = 200f;

        protected override void CustomChild(RectTransform child, float offset)
        {
            this.tracker.Add(this, child, 0xe0);
            float minScale = this.minScale;
            float num2 = child.get_rect().get_width() * minScale;
            float scaleEffectChildLocalPointLeft = this.ScaleEffectChildLocalPointLeft;
            float scaleEffectChildLocalPointRight = this.ScaleEffectChildLocalPointRight;
            if (base.direction == UguiHorizontalAlignGroup.AlignDirection.LeftToRight)
            {
                scaleEffectChildLocalPointLeft -= num2;
                if ((scaleEffectChildLocalPointLeft < offset) && (offset < scaleEffectChildLocalPointRight))
                {
                    float num5 = (offset - scaleEffectChildLocalPointLeft) / (scaleEffectChildLocalPointRight - scaleEffectChildLocalPointLeft);
                    if (num5 > 0.5f)
                    {
                        num5 = 1f - num5;
                    }
                    minScale = Mathf.Lerp(this.minScale, this.maxScale, num5);
                }
            }
            else
            {
                scaleEffectChildLocalPointRight += num2;
                if ((scaleEffectChildLocalPointLeft < offset) && (offset < scaleEffectChildLocalPointRight))
                {
                    float num6 = Mathf.Sin((3.141593f * (offset - scaleEffectChildLocalPointLeft)) / (scaleEffectChildLocalPointRight - scaleEffectChildLocalPointLeft));
                    minScale = Mathf.Lerp(this.minScale, this.maxScale, num6);
                }
            }
            child.set_localScale((Vector3) (Vector3.get_one() * minScale));
        }

        protected override void CustomLayoutRectTransform()
        {
            DrivenTransformProperties properties = 0;
            properties |= 0x4500;
            this.tracker.Add(this, base.CachedRectTransform, properties);
            if (base.direction == UguiHorizontalAlignGroup.AlignDirection.LeftToRight)
            {
                base.CachedRectTransform.set_anchorMin(new Vector2(0f, base.CachedRectTransform.get_anchorMin().y));
                base.CachedRectTransform.set_anchorMax(new Vector2(0f, base.CachedRectTransform.get_anchorMax().y));
                base.CachedRectTransform.set_pivot(new Vector2(0f, base.CachedRectTransform.get_pivot().y));
            }
            else
            {
                base.CachedRectTransform.set_anchorMin(new Vector2(1f, base.CachedRectTransform.get_anchorMin().y));
                base.CachedRectTransform.set_anchorMax(new Vector2(1f, base.CachedRectTransform.get_anchorMax().y));
                base.CachedRectTransform.set_pivot(new Vector2(1f, base.CachedRectTransform.get_pivot().y));
            }
        }

        private void OnDrawGizmos()
        {
            Vector3 scaleEffectWorldPointLeft = this.ScaleEffectWorldPointLeft;
            Vector3 scaleEffectWorldPointRight = this.ScaleEffectWorldPointRight;
            Gizmos.DrawLine(scaleEffectWorldPointLeft, scaleEffectWorldPointRight);
        }

        private float ScaleEffectChildLocalPointLeft
        {
            get
            {
                Vector3 scaleEffectWorldPointLeft = this.ScaleEffectWorldPointLeft;
                return base.CachedRectTransform.InverseTransformPoint(scaleEffectWorldPointLeft).x;
            }
        }

        private float ScaleEffectChildLocalPointRight
        {
            get
            {
                Vector3 scaleEffectWorldPointRight = this.ScaleEffectWorldPointRight;
                return base.CachedRectTransform.InverseTransformPoint(scaleEffectWorldPointRight).x;
            }
        }

        private Vector3 ScaleEffectWorldPointLeft
        {
            get
            {
                Vector3 vector = new Vector3(this.scaleRangeLeft, 0f, 0f);
                if (this.ignoreLocalPositionToScaleEffectRage)
                {
                    vector -= base.CachedRectTransform.get_localPosition();
                }
                return base.CachedRectTransform.TransformPoint(vector);
            }
        }

        private Vector3 ScaleEffectWorldPointRight
        {
            get
            {
                Vector3 vector = new Vector3(this.scaleRangeLeft + this.scaleRangeWidth, 0f, 0f);
                if (this.ignoreLocalPositionToScaleEffectRage)
                {
                    vector -= base.CachedRectTransform.get_localPosition();
                }
                return base.CachedRectTransform.TransformPoint(vector);
            }
        }
    }
}

