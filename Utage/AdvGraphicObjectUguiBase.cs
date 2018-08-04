namespace Utage
{
    using System;
    using UnityEngine;
    using UtageExtensions;

    public abstract class AdvGraphicObjectUguiBase : AdvGraphicBase
    {
        protected AdvGraphicObjectUguiBase()
        {
        }

        protected abstract void AddGraphicComponentOnInit();
        internal override void Alignment(Utage.Alignment alignment, AdvGraphicInfo graphic)
        {
            RectTransform t = base.get_transform() as RectTransform;
            t.set_pivot(graphic.Pivot);
            if (alignment == Utage.Alignment.None)
            {
                t.set_anchoredPosition(graphic.Position);
            }
            else
            {
                Vector2 alignmentValue = AlignmentUtil.GetAlignmentValue(alignment);
                Vector2 vector2 = alignmentValue;
                t.set_anchorMax(vector2);
                t.set_anchorMin(vector2);
                Vector3 vector3 = t.get_pivot() - alignmentValue;
                vector3.Scale(t.GetSizeScaled());
                t.set_anchoredPosition(graphic.Position + vector3);
            }
        }

        public override void Init(AdvGraphicObject parentObject)
        {
            base.Init(parentObject);
            this.AddGraphicComponentOnInit();
            if (base.GetComponent<IAdvClickEvent>() == null)
            {
                base.get_gameObject().AddComponent<AdvClickEvent>();
            }
        }

        internal override void Scale(AdvGraphicInfo graphic)
        {
            (base.get_transform() as RectTransform).set_localScale(graphic.Scale);
        }

        protected abstract UnityEngine.Material Material { get; set; }
    }
}

