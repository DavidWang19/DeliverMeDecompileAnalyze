namespace Utage
{
    using System;
    using UnityEngine;

    [ExecuteInEditMode, RequireComponent(typeof(Camera)), AddComponentMenu("Utage/Lib/Image Effects/Blur/Blur")]
    public class Blur : ImageEffectSingelShaderBase
    {
        [Range(1f, 4f)]
        public int blurIterations = 2;
        [Range(0f, 10f)]
        public float blurSize = 3f;
        public BlurType blurType;
        [Range(0f, 2f)]
        public int downsample = 1;

        protected override void RenderImage(RenderTexture source, RenderTexture destination)
        {
            float num = 1f / (1f * (((int) 1) << this.downsample));
            base.Material.SetVector("_Parameter", new Vector4(this.blurSize * num, -this.blurSize * num, 0f, 0f));
            source.set_filterMode(1);
            int num2 = source.get_width() >> this.downsample;
            int num3 = source.get_height() >> this.downsample;
            RenderTexture texture = RenderTexture.GetTemporary(num2, num3, 0, source.get_format());
            texture.set_filterMode(1);
            Graphics.Blit(source, texture, base.Material, 0);
            int num4 = (this.blurType != BlurType.StandardGauss) ? 2 : 0;
            for (int i = 0; i < this.blurIterations; i++)
            {
                float num6 = i * 1f;
                base.Material.SetVector("_Parameter", new Vector4((this.blurSize * num) + num6, (-this.blurSize * num) - num6, 0f, 0f));
                RenderTexture texture2 = RenderTexture.GetTemporary(num2, num3, 0, source.get_format());
                texture2.set_filterMode(1);
                Graphics.Blit(texture, texture2, base.Material, 1 + num4);
                RenderTexture.ReleaseTemporary(texture);
                texture = texture2;
                texture2 = RenderTexture.GetTemporary(num2, num3, 0, source.get_format());
                texture2.set_filterMode(1);
                Graphics.Blit(texture, texture2, base.Material, 2 + num4);
                RenderTexture.ReleaseTemporary(texture);
                texture = texture2;
            }
            Graphics.Blit(texture, destination);
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
            StandardGauss,
            SgxGauss
        }
    }
}

