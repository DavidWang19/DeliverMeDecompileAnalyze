namespace Utage
{
    using System;
    using UnityEngine;

    [AddComponentMenu("Utage/ADV/Internal/GraphicObject/Custom")]
    public class AdvGraphicObjectCustom : AdvGraphicObjectPrefabBase
    {
        protected SpriteRenderer sprite;

        protected override void ChangeResourceOnDrawSub(AdvGraphicInfo graphic)
        {
            foreach (IAdvGraphicObjectCustom custom in base.GetComponentsInChildren<IAdvGraphicObjectCustom>())
            {
                custom.ChangeResourceOnDrawSub(graphic);
            }
        }

        internal override void OnEffectColorsChange(AdvEffectColor color)
        {
            foreach (IAdvGraphicObjectCustom custom in base.GetComponentsInChildren<IAdvGraphicObjectCustom>())
            {
                custom.OnEffectColorsChange(color);
            }
        }
    }
}

