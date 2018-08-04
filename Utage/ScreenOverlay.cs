namespace Utage
{
    using System;
    using UnityEngine;

    [ExecuteInEditMode, RequireComponent(typeof(Camera)), AddComponentMenu("Utage/Lib/Image Effects/Other/Screen Overlay")]
    public class ScreenOverlay : ImageEffectSingelShaderBase
    {
        public OverlayBlendMode blendMode = OverlayBlendMode.Overlay;
        public float intensity = 1f;
        public Texture2D texture;

        protected override void RenderImage(RenderTexture source, RenderTexture destination)
        {
            Vector4 vector = new Vector4(1f, 0f, 0f, 1f);
            base.Material.SetVector("_UV_Transform", vector);
            base.Material.SetFloat("_Intensity", this.intensity);
            base.Material.SetTexture("_Overlay", this.texture);
            Graphics.Blit(source, destination, base.Material, (int) this.blendMode);
        }

        public enum OverlayBlendMode
        {
            Additive,
            ScreenBlend,
            Multiply,
            Overlay,
            AlphaBlend
        }
    }
}

