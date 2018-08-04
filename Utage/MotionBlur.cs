namespace Utage
{
    using System;
    using UnityEngine;

    [ExecuteInEditMode, AddComponentMenu("Utage/Lib/Image Effects/Blur/Motion Blur"), RequireComponent(typeof(Camera))]
    public class MotionBlur : ImageEffectSingelShaderBase
    {
        private RenderTexture accumTexture;
        [Range(0f, 0.92f)]
        public float blurAmount = 0.8f;
        public bool extraBlur;

        private void OnDisable()
        {
            Object.DestroyImmediate(this.accumTexture);
        }

        protected override void RenderImage(RenderTexture source, RenderTexture destination)
        {
            if (((this.accumTexture == null) || (this.accumTexture.get_width() != source.get_width())) || (this.accumTexture.get_height() != source.get_height()))
            {
                Object.DestroyImmediate(this.accumTexture);
                this.accumTexture = new RenderTexture(source.get_width(), source.get_height(), 0);
                this.accumTexture.set_hideFlags(0x3d);
                Graphics.Blit(source, this.accumTexture);
            }
            if (this.extraBlur)
            {
                RenderTexture texture = RenderTexture.GetTemporary(source.get_width() / 4, source.get_height() / 4, 0);
                this.accumTexture.MarkRestoreExpected();
                Graphics.Blit(this.accumTexture, texture);
                Graphics.Blit(texture, this.accumTexture);
                RenderTexture.ReleaseTemporary(texture);
            }
            this.blurAmount = Mathf.Clamp(this.blurAmount, 0f, 0.92f);
            base.Material.SetTexture("_MainTex", this.accumTexture);
            base.Material.SetFloat("_AccumOrig", 1f - this.blurAmount);
            this.accumTexture.MarkRestoreExpected();
            Graphics.Blit(source, this.accumTexture, base.Material);
            Graphics.Blit(this.accumTexture, destination);
        }

        protected override bool NeedRenderTexture
        {
            get
            {
                return true;
            }
        }
    }
}

