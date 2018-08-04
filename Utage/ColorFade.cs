namespace Utage
{
    using System;
    using UnityEngine;

    [ExecuteInEditMode, AddComponentMenu("Utage/Lib/Image Effects/Color Adjustments/ColorFade")]
    public class ColorFade : ImageEffectSingelShaderBase, IImageEffectStrength
    {
        public Color color = new Color(0f, 0f, 0f, 0f);
        [Range(0f, 1f)]
        public float strength = 1f;

        protected override void RenderImage(RenderTexture source, RenderTexture destination)
        {
            base.Material.SetFloat("_Strength", this.Strength);
            base.Material.set_color(this.color);
            Graphics.Blit(source, destination, base.Material);
        }

        public float Strength
        {
            get
            {
                return this.strength;
            }
            set
            {
                this.strength = value;
            }
        }
    }
}

