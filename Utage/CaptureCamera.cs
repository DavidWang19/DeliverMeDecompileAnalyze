namespace Utage
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UtageExtensions;

    [RequireComponent(typeof(Camera)), AddComponentMenu("Utage/Lib/Camera/CaptureCamera")]
    public class CaptureCamera : MonoBehaviour
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private RenderTexture <CaptureImage>k__BackingField;
        public CaptureCameraEvent OnCaptured = new CaptureCameraEvent();

        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            if (base.get_enabled())
            {
                if (this.CaptureImage != null)
                {
                    RenderTexture.ReleaseTemporary(this.CaptureImage);
                }
                this.CaptureImage = source.CreateCopyTemporary();
                Graphics.Blit(source, destination);
                this.OnCaptured.Invoke(this);
            }
        }

        public RenderTexture CaptureImage { get; set; }
    }
}

