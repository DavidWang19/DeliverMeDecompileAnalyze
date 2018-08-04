namespace Utage
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.UI;
    using UtageExtensions;

    [AddComponentMenu("Utage/ADV/Internal/GraphicObject/RawImage")]
    public class AdvGraphicObjectRawImage : AdvGraphicObjectUguiBase
    {
        [CompilerGenerated]
        private static Action <>f__am$cache0;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private UnityEngine.UI.RawImage <RawImage>k__BackingField;
        private AssetFileReference crossFadeReference;

        protected override void AddGraphicComponentOnInit()
        {
            this.RawImage = ((Component) this).GetComponentCreateIfMissing<UnityEngine.UI.RawImage>();
        }

        internal void CaptureCamera(Camera camera)
        {
            this.RawImage.set_enabled(false);
            Utage.CaptureCamera componentCreateIfMissing = ((Component) camera).GetComponentCreateIfMissing<Utage.CaptureCamera>();
            componentCreateIfMissing.set_enabled(true);
            componentCreateIfMissing.OnCaptured.AddListener(new UnityAction<Utage.CaptureCamera>(this, (IntPtr) this.OnCaptured));
        }

        internal override void ChangeResourceOnDraw(AdvGraphicInfo graphic, float fadeTime)
        {
            this.Material = graphic.RenderTextureSetting.GetRenderMaterialIfEnable(this.Material);
            bool flag = this.TryCreateCrossFadeImage(fadeTime, graphic);
            if (!flag)
            {
                this.ReleaseCrossFadeReference();
                this.RemoveComponent<UguiCrossFadeRawImage>();
            }
            this.RawImage.set_texture(graphic.File.Texture);
            this.RawImage.SetNativeSize();
            if (!flag)
            {
                if (<>f__am$cache0 == null)
                {
                    <>f__am$cache0 = delegate {
                    };
                }
                base.ParentObject.FadeIn(fadeTime, <>f__am$cache0);
            }
        }

        internal override bool CheckFailedCrossFade(AdvGraphicInfo graphic)
        {
            return !this.EnableCrossFade(graphic);
        }

        protected bool EnableCrossFade(AdvGraphicInfo graphic)
        {
            Texture texture = graphic.File.Texture;
            if (texture == null)
            {
                return false;
            }
            if (this.RawImage.get_texture() == null)
            {
                return false;
            }
            return (((this.RawImage.get_rectTransform().get_pivot() == graphic.Pivot) && (this.RawImage.get_texture().get_width() == texture.get_width())) && (this.RawImage.get_texture().get_height() == texture.get_height()));
        }

        private void OnCaptured(Utage.CaptureCamera captureCamera)
        {
            this.RawImage.set_enabled(true);
            this.RawImage.set_texture(captureCamera.CaptureImage);
            LetterBoxCamera component = captureCamera.GetComponent<LetterBoxCamera>();
            if (component != null)
            {
                this.RawImage.get_rectTransform().SetSize(component.CurrentSize);
                if (component.Zoom2D != 1f)
                {
                    int num = ((int) 1) << base.get_gameObject().get_layer();
                    if ((component.CachedCamera.get_cullingMask() & num) != 0)
                    {
                        Vector2 vector = component.Zoom2DCenter;
                        vector.x /= component.CurrentSize.x;
                        vector.y /= component.CurrentSize.y;
                        vector = -vector + ((Vector2) (Vector2.get_one() * 0.5f));
                        this.RawImage.get_rectTransform().set_pivot(vector);
                        this.RawImage.get_rectTransform().set_localScale((Vector3) (Vector3.get_one() / component.Zoom2D));
                    }
                }
            }
            else
            {
                this.RawImage.get_rectTransform().SetSize((float) Screen.get_width(), (float) Screen.get_height());
            }
            captureCamera.OnCaptured.RemoveListener(new UnityAction<Utage.CaptureCamera>(this, (IntPtr) this.OnCaptured));
            captureCamera.set_enabled(false);
        }

        private void ReleaseCrossFadeReference()
        {
            if (this.crossFadeReference != null)
            {
                Object.Destroy(this.crossFadeReference);
                this.crossFadeReference = null;
            }
        }

        internal void StartCrossFadeImage(float time)
        {
            <StartCrossFadeImage>c__AnonStorey0 storey = new <StartCrossFadeImage>c__AnonStorey0 {
                $this = this
            };
            Texture fadeTexture = base.LastResource.File.Texture;
            this.ReleaseCrossFadeReference();
            this.crossFadeReference = base.get_gameObject().AddComponent<AssetFileReference>();
            this.crossFadeReference.Init(base.LastResource.File);
            storey.crossFade = base.get_gameObject().GetComponentCreateIfMissing<UguiCrossFadeRawImage>();
            storey.crossFade.CrossFade(fadeTexture, time, new Action(storey.<>m__0));
        }

        protected bool TryCreateCrossFadeImage(float fadeTime, AdvGraphicInfo graphic)
        {
            if ((base.LastResource != null) && this.EnableCrossFade(graphic))
            {
                this.StartCrossFadeImage(fadeTime);
                return true;
            }
            return false;
        }

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

        [CompilerGenerated]
        private sealed class <StartCrossFadeImage>c__AnonStorey0
        {
            internal AdvGraphicObjectRawImage $this;
            internal UguiCrossFadeRawImage crossFade;

            internal void <>m__0()
            {
                this.$this.ReleaseCrossFadeReference();
                Object.Destroy(this.crossFade);
            }
        }
    }
}

