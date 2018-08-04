namespace Utage
{
    using System;
    using UnityEngine;

    [ExecuteInEditMode, RequireComponent(typeof(Camera)), AddComponentMenu("Utage/Lib/Image Effects/Bloom and Glow/Bloom")]
    public class Bloom : ImageEffectSingelShaderBase
    {
        [Range(1f, 4f)]
        public int blurIterations = 1;
        [Range(0.25f, 5.5f)]
        public float blurSize = 1f;
        public BlurType blurType;
        [Range(0f, 2.5f)]
        public float intensity = 0.75f;
        private Resolution resolution;
        [Range(0f, 1.5f)]
        public float threshold = 0.25f;

        protected override void RenderImage(RenderTexture source, RenderTexture destination)
        {
            int num = (this.resolution != Resolution.Low) ? 2 : 4;
            float num2 = (this.resolution != Resolution.Low) ? 1f : 0.5f;
            base.Material.SetVector("_Parameter", new Vector4(this.blurSize * num2, 0f, this.threshold, this.intensity));
            source.set_filterMode(1);
            int num3 = source.get_width() / num;
            int num4 = source.get_height() / num;
            RenderTexture texture = RenderTexture.GetTemporary(num3, num4, 0, source.get_format());
            texture.set_filterMode(1);
            Graphics.Blit(source, texture, base.Material, 1);
            int num5 = (this.blurType != BlurType.Standard) ? 2 : 0;
            for (int i = 0; i < this.blurIterations; i++)
            {
                base.Material.SetVector("_Parameter", new Vector4((this.blurSize * num2) + (i * 1f), 0f, this.threshold, this.intensity));
                RenderTexture texture2 = RenderTexture.GetTemporary(num3, num4, 0, source.get_format());
                texture2.set_filterMode(1);
                Graphics.Blit(texture, texture2, base.Material, 2 + num5);
                RenderTexture.ReleaseTemporary(texture);
                texture = texture2;
                texture2 = RenderTexture.GetTemporary(num3, num4, 0, source.get_format());
                texture2.set_filterMode(1);
                Graphics.Blit(texture, texture2, base.Material, 3 + num5);
                RenderTexture.ReleaseTemporary(texture);
                texture = texture2;
            }
            base.Material.SetTexture("_Bloom", texture);
            Graphics.Blit(source, destination, base.Material, 0);
            RenderTexture.ReleaseTemporary(texture);
        }

        protected override bool NeedRenderTexture
        {
            get
            {
                return true;
            }
        }

        public enum BlurType
        {
            Standard,
            Sgx
        }

        public enum Resolution
        {
            Low,
            High
        }
    }
}

