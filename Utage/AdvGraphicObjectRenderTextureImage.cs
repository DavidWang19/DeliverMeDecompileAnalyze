namespace Utage
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.UI;
    using UtageExtensions;

    [AddComponentMenu("Utage/ADV/Internal/GraphicObject/RenderTextureImage")]
    public class AdvGraphicObjectRenderTextureImage : AdvGraphicObjectUguiBase
    {
        [CompilerGenerated]
        private static Action <>f__am$cache0;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private UnityEngine.UI.RawImage <RawImage>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private AdvRenderTextureSpace <RenderTextureSpace>k__BackingField;
        private RenderTexture copyTemporary;

        protected override void AddGraphicComponentOnInit()
        {
        }

        internal override void ChangeResourceOnDraw(AdvGraphicInfo graphic, float fadeTime)
        {
            bool flag = this.TryCreateCrossFadeImage(fadeTime, graphic);
            if (!flag)
            {
                this.RemoveComponent<UguiCrossFadeRawImage>();
                this.ReleaseTemporary();
            }
            this.RawImage.set_texture(this.RenderTextureSpace.RenderTexture);
            this.RawImage.SetNativeSize();
            if (!flag && (base.LastResource == null))
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
            return false;
        }

        internal void Init(AdvRenderTextureSpace renderTextureSpace)
        {
            this.RenderTextureSpace = renderTextureSpace;
            this.RawImage = base.get_gameObject().GetComponentCreateIfMissing<UnityEngine.UI.RawImage>();
            if (renderTextureSpace.RenderTextureType == AdvRenderTextureMode.Image)
            {
                this.Material = new UnityEngine.Material(ShaderManager.DrawByRenderTexture);
            }
            this.RawImage.set_texture(this.RenderTextureSpace.RenderTexture);
            this.RawImage.SetNativeSize();
            this.RawImage.get_rectTransform().set_localScale(Vector3.get_one());
        }

        private void OnDestroy()
        {
            this.ReleaseTemporary();
        }

        private void ReleaseTemporary()
        {
            if (this.copyTemporary != null)
            {
                RenderTexture.ReleaseTemporary(this.copyTemporary);
                this.copyTemporary = null;
            }
        }

        public override void RuleFadeIn(AdvEngine engine, AdvTransitionArgs data, Action onComplete)
        {
            <RuleFadeIn>c__AnonStorey0 storey = new <RuleFadeIn>c__AnonStorey0 {
                onComplete = onComplete,
                transition = base.get_gameObject().AddComponent<UguiTransition>()
            };
            storey.transition.RuleFadeIn(engine.EffectManager.FindRuleTexture(data.TextureName), data.Vague, this.RenderTextureSpace.RenderTextureType == AdvRenderTextureMode.Image, data.GetSkippedTime(engine), new Action(storey.<>m__0));
        }

        public override void RuleFadeOut(AdvEngine engine, AdvTransitionArgs data, Action onComplete)
        {
            <RuleFadeOut>c__AnonStorey1 storey = new <RuleFadeOut>c__AnonStorey1 {
                onComplete = onComplete,
                $this = this,
                transition = base.get_gameObject().AddComponent<UguiTransition>()
            };
            storey.transition.RuleFadeOut(engine.EffectManager.FindRuleTexture(data.TextureName), data.Vague, this.RenderTextureSpace.RenderTextureType == AdvRenderTextureMode.Image, data.GetSkippedTime(engine), new Action(storey.<>m__0));
        }

        protected bool TryCreateCrossFadeImage(float time, AdvGraphicInfo graphic)
        {
            <TryCreateCrossFadeImage>c__AnonStorey2 storey = new <TryCreateCrossFadeImage>c__AnonStorey2 {
                $this = this
            };
            if (base.LastResource == null)
            {
                return false;
            }
            if (this.RawImage.get_texture() == null)
            {
                return false;
            }
            this.ReleaseTemporary();
            UnityEngine.Material material = this.Material;
            this.copyTemporary = this.RenderTextureSpace.RenderTexture.CreateCopyTemporary(0);
            storey.crossFade = base.get_gameObject().AddComponent<UguiCrossFadeRawImage>();
            storey.crossFade.Material = material;
            storey.crossFade.CrossFade(this.copyTemporary, time, new Action(storey.<>m__0));
            return true;
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

        public AdvRenderTextureSpace RenderTextureSpace { get; private set; }

        [CompilerGenerated]
        private sealed class <RuleFadeIn>c__AnonStorey0
        {
            internal Action onComplete;
            internal UguiTransition transition;

            internal void <>m__0()
            {
                Object.Destroy(this.transition);
                if (this.onComplete != null)
                {
                    this.onComplete();
                }
            }
        }

        [CompilerGenerated]
        private sealed class <RuleFadeOut>c__AnonStorey1
        {
            internal AdvGraphicObjectRenderTextureImage $this;
            internal Action onComplete;
            internal UguiTransition transition;

            internal void <>m__0()
            {
                Object.Destroy(this.transition);
                this.$this.RawImage.SetAlpha(0f);
                if (this.onComplete != null)
                {
                    this.onComplete();
                }
            }
        }

        [CompilerGenerated]
        private sealed class <TryCreateCrossFadeImage>c__AnonStorey2
        {
            internal AdvGraphicObjectRenderTextureImage $this;
            internal UguiCrossFadeRawImage crossFade;

            internal void <>m__0()
            {
                this.$this.ReleaseTemporary();
                Object.Destroy(this.crossFade);
            }
        }
    }
}

