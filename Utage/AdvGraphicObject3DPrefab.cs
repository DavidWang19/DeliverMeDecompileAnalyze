namespace Utage
{
    using System;
    using UnityEngine;

    [AddComponentMenu("Utage/ADV/Internal/GraphicObject/3DModel")]
    public class AdvGraphicObject3DPrefab : AdvGraphicObjectPrefabBase
    {
        protected override void ChangeResourceOnDrawSub(AdvGraphicInfo grapic)
        {
        }

        public override void Init(AdvGraphicObject parentObject)
        {
            base.Init(parentObject);
            base.get_transform().set_localEulerAngles((Vector3) (Vector3.get_up() * 180f));
        }

        internal override void OnEffectColorsChange(AdvEffectColor color)
        {
            if (base.currentObject != null)
            {
                Color mulColor = color.MulColor;
                mulColor.a = 1f;
                foreach (Renderer renderer in base.currentObject.GetComponentsInChildren<Renderer>())
                {
                    renderer.get_material().set_color(mulColor);
                }
            }
        }
    }
}

