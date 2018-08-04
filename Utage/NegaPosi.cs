namespace Utage
{
    using System;
    using UnityEngine;

    [ExecuteInEditMode, AddComponentMenu("Utage/Lib/Image Effects/Color Adjustments/NegaPosi")]
    public class NegaPosi : ImageEffectSingelShaderBase
    {
        protected override void RenderImage(RenderTexture source, RenderTexture destination)
        {
            Graphics.Blit(source, destination, base.Material);
        }
    }
}

