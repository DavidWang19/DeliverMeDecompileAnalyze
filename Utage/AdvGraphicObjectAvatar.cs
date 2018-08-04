namespace Utage
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.UI;

    [AddComponentMenu("Utage/ADV/Internal/GraphicObject/Avatar")]
    public class AdvGraphicObjectAvatar : AdvGraphicObjectUguiBase
    {
        [CompilerGenerated]
        private static Action <>f__am$cache0;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private AdvAnimationPlayer <Animation>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private AvatarImage <Avatar>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private EyeBlinkAvatar <EyeBlink>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Timer <FadeTimer>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private CanvasGroup <Group>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private LipSynchAvatar <LipSynch>k__BackingField;

        protected override void AddGraphicComponentOnInit()
        {
            this.Avatar = base.get_gameObject().AddComponent<AvatarImage>();
            this.Avatar.OnPostRefresh.AddListener(new UnityAction(this, (IntPtr) this.OnPostRefresh));
            this.EyeBlink = base.get_gameObject().AddComponent<EyeBlinkAvatar>();
            this.LipSynch = base.get_gameObject().AddComponent<LipSynchAvatar>();
            this.Animation = base.get_gameObject().AddComponent<AdvAnimationPlayer>();
            this.Group = base.get_gameObject().AddComponent<CanvasGroup>();
            this.FadeTimer = base.get_gameObject().AddComponent<Timer>();
            this.FadeTimer.AutoDestroy = false;
        }

        internal override void ChangeResourceOnDraw(AdvGraphicInfo graphic, float fadeTime)
        {
            this.Avatar.Material = graphic.RenderTextureSetting.GetRenderMaterialIfEnable(this.Avatar.Material);
            AvatarData unityObject = graphic.File.UnityObject as AvatarData;
            this.Avatar.AvatarData = unityObject;
            this.Avatar.CachedRectTransform.set_sizeDelta(unityObject.Size);
            this.Avatar.AvatarPattern.SetPattern(graphic.RowData);
            this.SetEyeBlinkSync(graphic.EyeBlinkData);
            this.SetLipSynch(graphic.LipSynchData);
            this.SetAnimation(graphic.AnimationData);
            if (base.LastResource == null)
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
            AvatarData unityObject = graphic.File.UnityObject as AvatarData;
            return (this.Avatar.AvatarData != unityObject);
        }

        internal override void Flip(bool flipX, bool flipY)
        {
            this.Avatar.Flip(flipX, flipY);
        }

        internal override void OnEffectColorsChange(AdvEffectColor color)
        {
            foreach (Graphic graphic in base.GetComponentsInChildren<Graphic>())
            {
                if (graphic != null)
                {
                    graphic.set_color(color.MulColor);
                }
            }
        }

        private void OnPostRefresh()
        {
            if (!base.LastResource.RenderTextureSetting.EnableRenderTexture)
            {
                this.OnEffectColorsChange(base.ParentObject.EffectColor);
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

        protected AdvAnimationPlayer Animation { get; set; }

        protected AvatarImage Avatar { get; set; }

        protected EyeBlinkAvatar EyeBlink { get; set; }

        protected Timer FadeTimer { get; set; }

        protected CanvasGroup Group { get; set; }

        protected LipSynchAvatar LipSynch { get; set; }

        protected override UnityEngine.Material Material
        {
            get
            {
                return this.Avatar.Material;
            }
            set
            {
                this.Avatar.Material = value;
            }
        }
    }
}

