namespace Utage
{
    using System;
    using UnityEngine;

    [AddComponentMenu("Utage/ADV/Internal/GraphicObject/2DPrefab")]
    public class AdvGraphicObject2DPrefab : AdvGraphicObjectPrefabBase
    {
        protected SpriteRenderer sprite;

        protected override void ChangeResourceOnDrawSub(AdvGraphicInfo graphic)
        {
            this.sprite = base.currentObject.GetComponent<SpriteRenderer>();
        }

        internal override void OnEffectColorsChange(AdvEffectColor color)
        {
            if (this.sprite != null)
            {
                this.sprite.set_color(color.MulColor);
            }
        }
    }
}

