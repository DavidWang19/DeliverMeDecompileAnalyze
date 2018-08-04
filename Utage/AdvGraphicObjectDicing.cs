namespace Utage
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UtageExtensions;

    [AddComponentMenu("Utage/ADV/Internal/GraphicObject/Dicing")]
    public class AdvGraphicObjectDicing : AdvGraphicObjectUguiBase
    {
        [CompilerGenerated]
        private static Action <>f__am$cache0;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private AdvAnimationPlayer <Animation>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private DicingImage <Dicing>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private EyeBlinkDicing <EyeBlink>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private LipSynchDicing <LipSynch>k__BackingField;
        private AssetFileReference crossFadeReference;

        protected override void AddGraphicComponentOnInit()
        {
            this.Dicing = base.get_gameObject().AddComponent<DicingImage>();
            this.EyeBlink = base.get_gameObject().AddComponent<EyeBlinkDicing>();
            this.LipSynch = base.get_gameObject().AddComponent<LipSynchDicing>();
            this.Animation = base.get_gameObject().AddComponent<AdvAnimationPlayer>();
        }

        public override void ChangePattern(string pattern)
        {
            this.Dicing.ChangePattern(pattern);
        }

        internal override void ChangeResourceOnDraw(AdvGraphicInfo graphic, float fadeTime)
        {
            this.Dicing.set_material(graphic.RenderTextureSetting.GetRenderMaterialIfEnable(this.Dicing.get_material()));
            bool flag = this.TryCreateCrossFadeImage(fadeTime, graphic);
            if (!flag)
            {
                this.ReleaseCrossFadeReference();
                this.RemoveComponent<UguiCrossFadeDicing>();
            }
            DicingTextures unityObject = graphic.File.UnityObject as DicingTextures;
            string subFileName = graphic.SubFileName;
            this.Dicing.DicingData = unityObject;
            this.Dicing.ChangePattern(subFileName);
            this.Dicing.SetNativeSize();
            this.SetEyeBlinkSync(graphic.EyeBlinkData);
            this.SetLipSynch(graphic.LipSynchData);
            this.SetAnimation(graphic.AnimationData);
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

        private bool EnableCrossFade(AdvGraphicInfo graphic)
        {
            DicingTextures unityObject = graphic.File.UnityObject as DicingTextures;
            string subFileName = graphic.SubFileName;
            DicingTextureData textureData = this.Dicing.DicingData.GetTextureData(subFileName);
            if (textureData == null)
            {
                return false;
            }
            return ((((this.Dicing.DicingData == unityObject) && (this.Dicing.get_rectTransform().get_pivot() == graphic.Pivot)) && (this.Dicing.PatternData.Width == textureData.Width)) && (this.Dicing.PatternData.Height == textureData.Height));
        }

        private void ReleaseCrossFadeReference()
        {
            if (this.crossFadeReference != null)
            {
                Object.Destroy(this.crossFadeReference);
                this.crossFadeReference = null;
            }
        }

        protected void SetAnimation(AdvAnimationData data)
        {
            this.Animation.Cancel();
            if (data != null)
            {
                this.Animation.Play(data.Clip, base.Engine.Page.SkippedSpeed, null);
            }
        }

        protected void SetEyeBlinkSync(AdvEyeBlinkData data)
        {
            if (data == null)
            {
                this.EyeBlink.AnimationData = new MiniAnimationData();
            }
            else
            {
                this.EyeBlink.IntervalTime.Min = data.IntervalMin;
                this.EyeBlink.IntervalTime.Max = data.IntervalMax;
                this.EyeBlink.RandomDoubleEyeBlink = data.RandomDoubleEyeBlink;
                this.EyeBlink.EyeTag = data.Tag;
                this.EyeBlink.AnimationData = data.AnimationData;
            }
        }

        protected void SetLipSynch(AdvLipSynchData data)
        {
            this.LipSynch.Cancel();
            if (data == null)
            {
                this.LipSynch.AnimationData = new MiniAnimationData();
            }
            else
            {
                this.LipSynch.Type = data.Type;
                this.LipSynch.Interval = data.Interval;
                this.LipSynch.ScaleVoiceVolume = data.ScaleVoiceVolume;
                this.LipSynch.LipTag = data.Tag;
                this.LipSynch.AnimationData = data.AnimationData;
                this.LipSynch.Play();
            }
        }

        protected bool TryCreateCrossFadeImage(float time, AdvGraphicInfo graphic)
        {
            if ((base.LastResource != null) && this.EnableCrossFade(graphic))
            {
                <TryCreateCrossFadeImage>c__AnonStorey0 storey = new <TryCreateCrossFadeImage>c__AnonStorey0();
                this.ReleaseCrossFadeReference();
                this.crossFadeReference = base.get_gameObject().AddComponent<AssetFileReference>();
                this.crossFadeReference.Init(base.LastResource.File);
                storey.crossFade = base.get_gameObject().GetComponentCreateIfMissing<UguiCrossFadeDicing>();
                storey.crossFade.CrossFade(this.Dicing.PatternData, this.Dicing.get_mainTexture(), time, new Action(storey.<>m__0));
                return true;
            }
            return false;
        }

        protected AdvAnimationPlayer Animation { get; set; }

        protected DicingImage Dicing { get; set; }

        protected EyeBlinkDicing EyeBlink { get; set; }

        protected LipSynchDicing LipSynch { get; set; }

        protected override UnityEngine.Material Material
        {
            get
            {
                return this.Dicing.get_material();
            }
            set
            {
                this.Dicing.set_material(value);
            }
        }

        [CompilerGenerated]
        private sealed class <TryCreateCrossFadeImage>c__AnonStorey0
        {
            internal UguiCrossFadeDicing crossFade;

            internal void <>m__0()
            {
                Object.Destroy(this.crossFade);
            }
        }
    }
}

