namespace Utage
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.Video;
    using UtageExtensions;

    [AddComponentMenu("Utage/ADV/Internal/GraphicObject/Video")]
    public class AdvGraphicObjectVideo : AdvGraphicObjectUguiBase
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Timer <FadeTimer>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <Height>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private UnityEngine.UI.RawImage <RawImage>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private UnityEngine.RenderTexture <RenderTexture>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private UnityEngine.Video.VideoClip <VideoClip>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private UnityEngine.Video.VideoPlayer <VideoPlayer>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <Width>k__BackingField;
        private AssetFileReference crossFadeReference;

        protected override void AddGraphicComponentOnInit()
        {
            this.RawImage = ((Component) this).GetComponentCreateIfMissing<UnityEngine.UI.RawImage>();
            this.FadeTimer = base.get_gameObject().AddComponent<Timer>();
            this.FadeTimer.AutoDestroy = false;
            this.VideoPlayer = base.get_gameObject().AddComponent<UnityEngine.Video.VideoPlayer>();
        }

        internal override void ChangeResourceOnDraw(AdvGraphicInfo graphic, float fadeTime)
        {
            this.VideoClip = graphic.File.UnityObject as UnityEngine.Video.VideoClip;
            this.VideoPlayer.set_clip(this.VideoClip);
            this.VideoPlayer.set_isLooping(true);
            this.VideoPlayer.set_renderMode(2);
            this.ReleaseTexture();
            this.RenderTexture = new UnityEngine.RenderTexture((int) this.VideoClip.get_width(), (int) this.VideoClip.get_height(), 0x10, 0);
            this.VideoPlayer.set_targetTexture(this.RenderTexture);
            this.VideoPlayer.Play();
            this.RawImage.set_texture(this.RenderTexture);
            this.RawImage.SetNativeSize();
        }

        internal override bool CheckFailedCrossFade(AdvGraphicInfo graphic)
        {
            return true;
        }

        private void OnDestroy()
        {
            this.ReleaseTexture();
        }

        private void OnDisable()
        {
            this.VideoPlayer.Stop();
        }

        internal override void OnEffectColorsChange(AdvEffectColor color)
        {
        }

        private void ReleaseCrossFadeReference()
        {
            if (this.crossFadeReference != null)
            {
                Object.Destroy(this.crossFadeReference);
                this.crossFadeReference = null;
            }
        }

        private void ReleaseTexture()
        {
            if (this.RenderTexture != null)
            {
                this.RenderTexture.Release();
                Object.Destroy(this.RenderTexture);
            }
        }

        protected Timer FadeTimer { get; set; }

        private int Height { get; set; }

        protected override UnityEngine.Material Material
        {
            get
            {
                return this.RawImage.get_material();
            }
            set
            {
                this.RawImage.set_material(value);
            }
        }

        private UnityEngine.UI.RawImage RawImage { get; set; }

        private UnityEngine.RenderTexture RenderTexture { get; set; }

        private UnityEngine.Video.VideoClip VideoClip { get; set; }

        private UnityEngine.Video.VideoPlayer VideoPlayer { get; set; }

        private int Width { get; set; }
    }
}

