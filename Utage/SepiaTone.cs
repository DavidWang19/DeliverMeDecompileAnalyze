namespace Utage
{
    using System;
    using UnityEngine;

    [ExecuteInEditMode, AddComponentMenu("Utage/Lib/Image Effects/Color Adjustments/Sepia Tone")]
    public class SepiaTone : ImageEffectSingelShaderBase, IImageEffectStrength
    {
        [Range(0f, 1f)]
        public float strength = 1f;

        protected override void RenderImage(RenderTexture source, RenderTexture destination)
        {
            base.Material.SetFloat("_Strength", this.strength);
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

