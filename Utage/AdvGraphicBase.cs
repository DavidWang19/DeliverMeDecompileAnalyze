namespace Utage
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.UI;

    public abstract class AdvGraphicBase : MonoBehaviour
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private AdvGraphicObject <ParentObject>k__BackingField;

        protected AdvGraphicBase()
        {
        }

        internal abstract void Alignment(Utage.Alignment alignment, AdvGraphicInfo graphic);
        public virtual void ChangePattern(string pattern)
        {
        }

        internal abstract void ChangeResourceOnDraw(AdvGraphicInfo graphic, float fadeTime);
        internal abstract bool CheckFailedCrossFade(AdvGraphicInfo graphic);
        internal virtual void Flip(bool flipX, bool flipY)
        {
            UguiFlip component = base.GetComponent<UguiFlip>();
            if (component == null)
            {
                if (!flipX && !flipY)
                {
                    return;
                }
                component = base.get_gameObject().AddComponent<UguiFlip>();
            }
            component.FlipX = flipX;
            component.FlipY = flipY;
        }

        public virtual void Init(AdvGraphicObject parentObject)
        {
            this.ParentObject = parentObject;
        }

        internal virtual void OnEffectColorsChange(AdvEffectColor color)
        {
            Graphic component = base.GetComponent<Graphic>();
            if (component != null)
            {
                component.set_color(color.MulColor);
            }
        }

        public virtual void Read(BinaryReader reader)
        {
        }

        public virtual void RuleFadeIn(AdvEngine engine, AdvTransitionArgs data, Action onComplete)
        {
            <RuleFadeIn>c__AnonStorey0 storey = new <RuleFadeIn>c__AnonStorey0 {
                onComplete = onComplete,
                transition = base.get_gameObject().AddComponent<UguiTransition>()
            };
            storey.transition.RuleFadeIn(engine.EffectManager.FindRuleTexture(data.TextureName), data.Vague, false, data.GetSkippedTime(engine), new Action(storey.<>m__0));
        }

        public virtual void RuleFadeOut(AdvEngine engine, AdvTransitionArgs data, Action onComplete)
        {
            <RuleFadeOut>c__AnonStorey1 storey = new <RuleFadeOut>c__AnonStorey1 {
                onComplete = onComplete,
                transition = base.get_gameObject().AddComponent<UguiTransition>()
            };
            storey.transition.RuleFadeOut(engine.EffectManager.FindRuleTexture(data.TextureName), data.Vague, false, data.GetSkippedTime(engine), new Action(storey.<>m__0));
        }

        internal abstract void Scale(AdvGraphicInfo graphic);
        internal virtual void SetCommandArg(AdvCommand command)
        {
        }

        public virtual void Write(BinaryWriter writer)
        {
        }

        internal AdvEngine Engine
        {
            get
            {
                return this.Layer.Manager.Engine;
            }
        }

        protected AdvGraphicInfo LastResource
        {
            get
            {
                return this.ParentObject.LastResource;
            }
        }

        internal AdvGraphicLayer Layer
        {
            get
            {
                return this.ParentObject.Layer;
            }
        }

        internal AdvGraphicObject ParentObject { get; set; }

        protected float PixelsToUnits
        {
            get
            {
                return this.Layer.Manager.PixelsToUnits;
            }
        }

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
    }
}

