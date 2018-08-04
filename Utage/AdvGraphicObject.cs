namespace Utage
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.Events;
    using UtageExtensions;

    [AddComponentMenu("Utage/ADV/Internal/GraphicObject"), RequireComponent(typeof(RectTransform))]
    public class AdvGraphicObject : MonoBehaviour, IAdvFade
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Timer <FadeTimer>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private AdvGraphicInfo <LastResource>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private RectTransform <rectTransform>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private AdvGraphicBase <RenderObject>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private AdvRenderTextureSpace <RenderTextureSpace>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private AdvGraphicBase <TargetObject>k__BackingField;
        private AdvEffectColor effectColor;
        protected AdvGraphicLayer layer;
        private AdvGraphicLoader loader;
        private const int Version = 1;
        private const int Version0 = 0;

        public virtual void ChangePattern(string pattern)
        {
            if (this.TargetObject != null)
            {
                this.TargetObject.ChangePattern(pattern);
            }
        }

        public virtual void Clear()
        {
            this.RemoveFromLayer();
            base.get_gameObject().SetActive(false);
            Object.Destroy(base.get_gameObject());
        }

        public virtual void Draw(AdvGraphicOperaitonArg arg, float fadeTime)
        {
            this.DrawSub(arg.Graphic, fadeTime);
        }

        private void DrawSub(AdvGraphicInfo graphic, float fadeTime)
        {
            this.TargetObject.set_name(graphic.File.FileName);
            this.TargetObject.ChangeResourceOnDraw(graphic, fadeTime);
            if (this.RenderObject != this.TargetObject)
            {
                this.RenderObject.ChangeResourceOnDraw(graphic, fadeTime);
                if (graphic.IsUguiComponentType)
                {
                    this.RenderObject.Scale(graphic);
                }
            }
            else
            {
                this.TargetObject.Scale(graphic);
            }
            this.RenderObject.Alignment(this.Layer.SettingData.Alignment, graphic);
            this.RenderObject.Flip(this.Layer.SettingData.FlipX, this.Layer.SettingData.FlipY);
            this.LastResource = graphic;
        }

        public void FadeIn(float fadeTime, Action onComplete)
        {
            <FadeIn>c__AnonStorey0 storey = new <FadeIn>c__AnonStorey0 {
                onComplete = onComplete,
                $this = this,
                begin = 0f,
                end = 1f
            };
            this.FadeTimer.StartTimer(fadeTime, new Action<Timer>(storey.<>m__0), new Action<Timer>(storey.<>m__1), 0f);
        }

        public virtual void FadeOut(float time)
        {
            this.FadeOut(time, new Action(this.Clear));
        }

        public void FadeOut(float time, Action onComplete)
        {
            <FadeOut>c__AnonStorey1 storey = new <FadeOut>c__AnonStorey1 {
                onComplete = onComplete,
                $this = this
            };
            if (this.TargetObject == null)
            {
                if (storey.onComplete != null)
                {
                    storey.onComplete();
                }
            }
            else
            {
                storey.begin = this.EffectColor.FadeAlpha;
                storey.end = 0f;
                this.FadeTimer.StartTimer(time, new Action<Timer>(storey.<>m__0), new Action<Timer>(storey.<>m__1), 0f);
            }
        }

        public virtual void Init(AdvGraphicLayer layer, AdvGraphicInfo graphic)
        {
            this.layer = layer;
            this.rectTransform = base.get_transform() as RectTransform;
            this.rectTransform.SetStretch();
            if (graphic.RenderTextureSetting.EnableRenderTexture)
            {
                this.InitRenderTextureImage(graphic);
            }
            else
            {
                GameObject obj2 = base.get_transform().AddChildGameObject(graphic.Key);
                AdvGraphicBase base2 = obj2.AddComponent(graphic.GetComponentType()) as AdvGraphicBase;
                this.RenderObject = base2;
                this.TargetObject = base2;
                this.TargetObject.Init(this);
            }
            LipSynchBase componentInChildren = this.TargetObject.GetComponentInChildren<LipSynchBase>();
            if (componentInChildren != null)
            {
                componentInChildren.CharacterLabel = base.get_gameObject().get_name();
                componentInChildren.OnCheckTextLipSync.AddListener(new UnityAction<LipSynchBase>(this, (IntPtr) this.<Init>m__0));
            }
            this.FadeTimer = base.get_gameObject().AddComponent<Timer>();
            this.effectColor = ((Component) this).GetComponentCreateIfMissing<AdvEffectColor>();
            AdvGraphicBase renderObject = this.RenderObject;
            this.effectColor.OnValueChanged.AddListener(new UnityAction<AdvEffectColor>(renderObject, (IntPtr) renderObject.OnEffectColorsChange));
        }

        internal void InitCaptureImage(AdvGraphicInfo grapic, Camera cachedCamera)
        {
            this.LastResource = grapic;
            base.get_gameObject().GetComponentInChildren<AdvGraphicObjectRawImage>().CaptureCamera(cachedCamera);
        }

        private void InitRenderTextureImage(AdvGraphicInfo graphic)
        {
            AdvGraphicManager manager = this.Layer.Manager;
            this.RenderTextureSpace = manager.RenderTextureManager.CreateSpace();
            this.RenderTextureSpace.Init(graphic, manager.PixelsToUnits);
            AdvGraphicObjectRenderTextureImage image = base.get_transform().AddChildGameObject(graphic.Key).AddComponent<AdvGraphicObjectRenderTextureImage>();
            this.RenderObject = image;
            image.Init(this.RenderTextureSpace);
            this.RenderObject.Init(this);
            this.TargetObject = this.RenderTextureSpace.RenderRoot.get_transform().AddChildGameObject(graphic.Key).AddComponent(graphic.GetComponentType()) as AdvGraphicBase;
            this.TargetObject.Init(this);
        }

        protected virtual void OnDestroy()
        {
            this.RemoveFromLayer();
            if (this.RenderTextureSpace != null)
            {
                Object.Destroy(this.RenderTextureSpace.get_gameObject());
            }
        }

        private void Read(BinaryReader reader)
        {
            int num = reader.ReadInt32();
            if ((num < 0) || (num > 1))
            {
                object[] args = new object[] { num };
                Debug.LogError(LanguageErrorMsg.LocalizeTextFormat(ErrorMsg.UnknownVersion, args));
            }
            else
            {
                reader.ReadLocalTransform(base.get_transform());
                reader.ReadBuffer(new Action<BinaryReader>(this.EffectColor.Read));
                reader.ReadBuffer(x => AdvITweenPlayer.ReadSaveData(x, base.get_gameObject(), true, this.PixelsToUnits));
                reader.ReadBuffer(x => AdvAnimationPlayer.ReadSaveData(x, base.get_gameObject(), this.Engine));
                if (num > 0)
                {
                    reader.ReadBuffer(x => this.TargetObject.Read(x));
                }
            }
        }

        public void Read(byte[] buffer, AdvGraphicInfo graphic)
        {
            <Read>c__AnonStorey3 storey = new <Read>c__AnonStorey3 {
                graphic = graphic,
                buffer = buffer,
                $this = this
            };
            this.TargetObject.get_gameObject().SetActive(false);
            this.Loader.LoadGraphic(storey.graphic, new Action(storey.<>m__0));
        }

        public virtual void RemoveFromLayer()
        {
            if (this.Layer != null)
            {
                this.Layer.Remove(this);
            }
        }

        public void RuleFadeIn(AdvEngine engine, AdvTransitionArgs data, Action onComplete)
        {
            if (this.TargetObject == null)
            {
                if (onComplete != null)
                {
                    onComplete();
                }
            }
            else
            {
                this.RenderObject.RuleFadeIn(engine, data, onComplete);
            }
        }

        public void RuleFadeOut(AdvEngine engine, AdvTransitionArgs data, Action onComplete)
        {
            <RuleFadeOut>c__AnonStorey2 storey = new <RuleFadeOut>c__AnonStorey2 {
                onComplete = onComplete,
                $this = this
            };
            if (this.TargetObject == null)
            {
                if (storey.onComplete != null)
                {
                    storey.onComplete();
                }
                this.Clear();
            }
            else
            {
                this.RenderObject.RuleFadeOut(engine, data, new Action(storey.<>m__0));
            }
        }

        internal virtual void SetCommandPostion(AdvCommand command)
        {
            float num;
            float num2;
            bool flag = false;
            Vector3 vector = base.get_transform().get_localPosition();
            if (command.TryParseCell<float>(AdvColumnName.Arg4, out num))
            {
                vector.x = num;
                flag = true;
            }
            if (command.TryParseCell<float>(AdvColumnName.Arg5, out num2))
            {
                vector.y = num2;
                flag = true;
            }
            if (flag)
            {
                base.get_transform().set_localPosition(vector);
            }
        }

        private void SetGraphicOnSaveDataRead(AdvGraphicInfo graphic)
        {
            this.DrawSub(graphic, 0f);
        }

        public virtual bool TryFadeIn(float time)
        {
            if (this.TargetObject != null)
            {
                this.FadeIn(time, null);
                return true;
            }
            return false;
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write(1);
            writer.WriteLocalTransform(base.get_transform());
            writer.WriteBuffer(new Action<BinaryWriter>(this.EffectColor.Write));
            writer.WriteBuffer(x => AdvITweenPlayer.WriteSaveData(x, base.get_gameObject()));
            writer.WriteBuffer(x => AdvAnimationPlayer.WriteSaveData(x, base.get_gameObject()));
            writer.WriteBuffer(x => this.TargetObject.Write(x));
        }

        public AdvEffectColor EffectColor
        {
            get
            {
                return ((Component) this).GetComponentCacheCreateIfMissing<AdvEffectColor>(ref this.effectColor);
            }
        }

        public bool EnableRenderTexture
        {
            get
            {
                return ((this.LastResource != null) && this.LastResource.RenderTextureSetting.EnableRenderTexture);
            }
        }

        public AdvEngine Engine
        {
            get
            {
                return this.Layer.Manager.Engine;
            }
        }

        private Timer FadeTimer { get; set; }

        public AdvGraphicInfo LastResource { get; private set; }

        public AdvGraphicLayer Layer
        {
            get
            {
                return this.layer;
            }
        }

        public AdvGraphicLoader Loader
        {
            get
            {
                return ((Component) this).GetComponentCacheCreateIfMissing<AdvGraphicLoader>(ref this.loader);
            }
        }

        public float PixelsToUnits
        {
            get
            {
                return this.Layer.Manager.PixelsToUnits;
            }
        }

        public RectTransform rectTransform { get; private set; }

        public AdvGraphicBase RenderObject { get; private set; }

        public AdvRenderTextureSpace RenderTextureSpace { get; private set; }

        public AdvGraphicBase TargetObject { get; private set; }

        [CompilerGenerated]
        private sealed class <FadeIn>c__AnonStorey0
        {
            internal AdvGraphicObject $this;
            internal float begin;
            internal float end;
            internal Action onComplete;

            internal void <>m__0(Timer x)
            {
                this.$this.EffectColor.FadeAlpha = x.GetCurve(this.begin, this.end);
            }

            internal void <>m__1(Timer x)
            {
                if (this.onComplete != null)
                {
                    this.onComplete();
                }
            }
        }

        [CompilerGenerated]
        private sealed class <FadeOut>c__AnonStorey1
        {
            internal AdvGraphicObject $this;
            internal float begin;
            internal float end;
            internal Action onComplete;

            internal void <>m__0(Timer x)
            {
                this.$this.EffectColor.FadeAlpha = x.GetCurve(this.begin, this.end);
            }

            internal void <>m__1(Timer x)
            {
                if (this.onComplete != null)
                {
                    this.onComplete();
                }
            }
        }

        [CompilerGenerated]
        private sealed class <Read>c__AnonStorey3
        {
            internal AdvGraphicObject $this;
            internal byte[] buffer;
            internal AdvGraphicInfo graphic;

            internal void <>m__0()
            {
                this.$this.TargetObject.get_gameObject().SetActive(true);
                this.$this.SetGraphicOnSaveDataRead(this.graphic);
                BinaryUtil.BinaryRead(this.buffer, new Action<BinaryReader>(this.$this.Read));
            }
        }

        [CompilerGenerated]
        private sealed class <RuleFadeOut>c__AnonStorey2
        {
            internal AdvGraphicObject $this;
            internal Action onComplete;

            internal void <>m__0()
            {
                if (this.onComplete != null)
                {
                    this.onComplete();
                }
                this.$this.Clear();
            }
        }
    }
}

