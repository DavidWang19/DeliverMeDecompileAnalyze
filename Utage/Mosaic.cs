namespace Utage
{
    using System;
    using UnityEngine;
    using UtageExtensions;

    [ExecuteInEditMode, AddComponentMenu("Utage/Lib/Image Effects/Color Adjustments/Mosaic")]
    public class Mosaic : ImageEffectSingelShaderBase
    {
        private Utage.LetterBoxCamera letterBoxCamera;
        [Min(1f)]
        public float size = 8f;

        protected override void RenderImage(RenderTexture source, RenderTexture destination)
        {
            float num = 1f;
            if (this.LetterBoxCamera != null)
            {
                num = Mathf.Min(((float) source.get_width()) / this.LetterBoxCamera.CurrentSize.x, ((float) source.get_height()) / this.LetterBoxCamera.CurrentSize.y);
            }
            if (this.size <= 1f)
            {
                Graphics.Blit(source, destination);
            }
            else
            {
                base.Material.SetFloat("_Size", (float) Mathf.CeilToInt(this.size * num));
                Graphics.Blit(source, destination, base.Material);
            }
        }

        private Utage.LetterBoxCamera LetterBoxCamera
        {
            get
            {
                return ((Component) this).GetComponentCache<Utage.LetterBoxCamera>(ref this.letterBoxCamera);
            }
        }
    }
}

