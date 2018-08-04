namespace Utage
{
    using System;
    using UnityEngine;

    [ExecuteInEditMode, AddComponentMenu("Utage/Lib/Image Effects/Displacement/Vortex")]
    public class Vortex : ImageEffectSingelShaderBase
    {
        public float angle = 50f;
        public Vector2 center = new Vector2(0.5f, 0.5f);
        public Vector2 radius = new Vector2(0.4f, 0.4f);

        protected override void RenderImage(RenderTexture source, RenderTexture destination)
        {
            ImageEffectUtil.RenderDistortion(base.Material, source, destination, this.angle, this.center, this.radius);
        }
    }
}

