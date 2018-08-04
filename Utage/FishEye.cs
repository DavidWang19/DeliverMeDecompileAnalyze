namespace Utage
{
    using System;
    using UnityEngine;

    [ExecuteInEditMode, RequireComponent(typeof(Camera)), AddComponentMenu("Utage/Lib/Image Effects/Displacement/FishEye")]
    public class FishEye : ImageEffectSingelShaderBase
    {
        [Range(0f, 1.5f)]
        public float strengthX = 0.5f;
        [Range(0f, 1.5f)]
        public float strengthY = 0.5f;

        protected override void RenderImage(RenderTexture source, RenderTexture destination)
        {
            float num = 0.15625f;
            float num2 = (source.get_width() * 1f) / (source.get_height() * 1f);
            base.Material.SetVector("intensity", new Vector4((this.strengthX * num2) * num, this.strengthY * num, (this.strengthX * num2) * num, this.strengthY * num));
            Graphics.Blit(source, destination, base.Material);
        }
    }
}

