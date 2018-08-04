namespace Utage
{
    using System;
    using UnityEngine;

    [ExecuteInEditMode, AddComponentMenu("Utage/Lib/Image Effects/Color Adjustments/Grayscale")]
    public class Grayscale : ImageEffectSingelShaderBase, IImageEffectStrength
    {
        [Range(-1f, 1f)]
        public float rampOffset;
        [Range(0f, 1f)]
        public float strength = 1f;
        public Texture textureRamp;
        private Texture tmpTextureRamp;

        protected override void RenderImage(RenderTexture source, RenderTexture destination)
        {
            base.Material.SetFloat("_Strength", this.strength);
            base.Material.SetTexture("_RampTex", this.textureRamp);
            base.Material.SetFloat("_RampOffset", this.rampOffset);
            Graphics.Blit(source, destination, base.Material);
        }

        protected override void RestoreObjectsOnJsonRead()
        {
            base.RestoreObjectsOnJsonRead();
            this.textureRamp = this.tmpTextureRamp;
        }

        protected override void StoreObjectsOnJsonRead()
        {
            base.StoreObjectsOnJsonRead();
            this.tmpTextureRamp = this.textureRamp;
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

