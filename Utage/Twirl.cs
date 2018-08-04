namespace Utage
{
    using System;
    using UnityEngine;

    [ExecuteInEditMode, AddComponentMenu("Utage/Lib/Image Effects/Displacement/Twirl")]
    public class Twirl : ImageEffectSingelShaderBase
    {
        [Range(0f, 360f)]
        public float angle = 50f;
        public Vector2 center = new Vector2(0.5f, 0.5f);
        public Vector2 radius = new Vector2(0.3f, 0.3f);

        protected override void RenderImage(RenderTexture source, RenderTexture destination)
        {
            ImageEffectUtil.RenderDistortion(base.Material, source, destination, this.angle, this.center, this.radius);
        }
    }
}

