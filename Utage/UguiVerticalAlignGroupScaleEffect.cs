namespace Utage
{
    using System;
    using UnityEngine;

    [ExecuteInEditMode, AddComponentMenu("Utage/Lib/UI/VerticalAlignGroupScaleEffect")]
    public class UguiVerticalAlignGroupScaleEffect : UguiVerticalAlignGroup
    {
        public bool ignoreLocalPositionToScaleEffectRage = true;
        public float maxScale = 1f;
        public float minScale = 0.5f;
        public float scaleRangeHeight = 200f;
        public float scaleRangeTop = -100f;

        protected override void CustomChild(RectTransform child, float offset)
        {
            this.tracker.Add(this, child, 0xe0);
            float minScale = this.minScale;
            float num2 = child.get_rect().get_height() * minScale;
            float scaleEffectChildLocalPointTop = this.ScaleEffectChildLocalPointTop;
            float scaleEffectChildLocalPointBottom = this.ScaleEffectChildLocalPointBottom;
            if (base.direction == UguiVerticalAlignGroup.AlignDirection.BottomToTop)
            {
                scaleEffectChildLocalPointBottom -= num2;
                if ((scaleEffectChildLocalPointBottom < offset) && (offset < scaleEffectChildLocalPointTop))
                {
                    float num5 = (offset - scaleEffectChildLocalPointBottom) / (scaleEffectChildLocalPointTop - scaleEffectChildLocalPointBottom);
                    if (num5 > 0.5f)
                    {
                        num5 = 1f - num5;
                    }
                    minScale = Mathf.Lerp(this.minScale, this.maxScale, num5);
                }
            }
            else
            {
                scaleEffectChildLocalPointTop += num2;
                if ((scaleEffectChildLocalPointBottom < offset) && (offset < scaleEffectChildLocalPointTop))
                {
                    float num6 = Mathf.Sin((3.141593f * (offset - scaleEffectChildLocalPointBottom)) / (scaleEffectChildLocalPointTop - scaleEffectChildLocalPointBottom));
                    minScale = Mathf.Lerp(this.minScale, this.maxScale, num6);
                }
            }
            child.set_localScale((Vector3) (Vector3.get_one() * minScale));
        }

        protected override void CustomLayoutRectTransform()
        {
            DrivenTransformProperties properties = 0;
            properties |= 0x8a00;
            this.tracker.Add(this, base.CachedRectTransform, properties);
            if (base.direction == UguiVerticalAlignGroup.AlignDirection.BottomToTop)
            {
                base.CachedRectTransform.set_anchorMin(new Vector2(base.CachedRectTransform.get_anchorMin().x, 0f));
                base.CachedRectTransform.set_anchorMax(new Vector2(base.CachedRectTransform.get_anchorMax().x, 0f));
                base.CachedRectTransform.set_pivot(new Vector2(base.CachedRectTransform.get_pivot().x, 0f));
            }
            else
            {
                base.CachedRectTransform.set_anchorMin(new Vector2(base.CachedRectTransform.get_anchorMin().x, 1f));
                base.CachedRectTransform.set_anchorMax(new Vector2(base.CachedRectTransform.get_anchorMax().x, 1f));
                base.CachedRectTransform.set_pivot(new Vector2(base.CachedRectTransform.get_pivot().x, 1f));
            }
        }

        private void OnDrawGizmos()
        {
            Vector3 scaleEffectWorldPointTop = this.ScaleEffectWorldPointTop;
            Vector3 scaleEffectWorldPointBottom = this.ScaleEffectWorldPointBottom;
            Gizmos.DrawLine(scaleEffectWorldPointTop, scaleEffectWorldPointBottom);
        }

        private float ScaleEffectChildLocalPointBottom
        {
            get
            {
                Vector3 scaleEffectWorldPointBottom = this.ScaleEffectWorldPointBottom;
                return base.CachedRectTransform.InverseTransformPoint(scaleEffectWorldPointBottom).y;
            }
        }

        private float ScaleEffectChildLocalPointTop
        {
            get
            {
                Vector3 scaleEffectWorldPointTop = this.ScaleEffectWorldPointTop;
                return base.CachedRectTransform.InverseTransformPoint(scaleEffectWorldPointTop).y;
            }
        }

        private Vector3 ScaleEffectWorldPointBottom
        {
            get
            {
                Vector3 vector = new Vector3(0f, this.scaleRangeTop - this.scaleRangeHeight, 0f);
                if (this.ignoreLocalPositionToScaleEffectRage)
                {
                    vector -= base.CachedRectTransform.get_localPosition();
                }
                return base.CachedRectTransform.TransformPoint(vector);
            }
        }

        private Vector3 ScaleEffectWorldPointTop
        {
            get
            {
                Vector3 vector = new Vector3(0f, this.scaleRangeTop, 0f);
                if (this.ignoreLocalPositionToScaleEffectRage)
                {
                    vector -= base.CachedRectTransform.get_localPosition();
                }
                return base.CachedRectTransform.TransformPoint(vector);
            }
        }
    }
}

