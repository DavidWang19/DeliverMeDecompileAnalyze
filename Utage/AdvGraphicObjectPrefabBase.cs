namespace Utage
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UtageExtensions;

    public abstract class AdvGraphicObjectPrefabBase : AdvGraphicBase
    {
        [CompilerGenerated]
        private static Action <>f__am$cache0;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <AnimationStateName>k__BackingField;
        private Animator animator;
        protected GameObject currentObject;
        private const int Version = 1;

        protected AdvGraphicObjectPrefabBase()
        {
        }

        internal override void Alignment(Utage.Alignment alignment, AdvGraphicInfo graphic)
        {
            base.get_transform().set_localPosition(graphic.Position);
        }

        private void ChangeAnimationState(string name, float fadeTime)
        {
            this.AnimationStateName = name;
            if (!string.IsNullOrEmpty(this.AnimationStateName))
            {
                if (this.animator != null)
                {
                    this.animator.CrossFade(this.AnimationStateName, fadeTime);
                }
                else
                {
                    Animation componentInChildren = base.GetComponentInChildren<Animation>();
                    if (componentInChildren != null)
                    {
                        componentInChildren.CrossFade(this.AnimationStateName, fadeTime);
                    }
                }
            }
        }

        internal override void ChangeResourceOnDraw(AdvGraphicInfo grapic, float fadeTime)
        {
            if (base.LastResource != grapic)
            {
                this.currentObject = Object.Instantiate(grapic.File.UnityObject) as GameObject;
                Vector3 vector = this.currentObject.get_transform().get_localPosition();
                Vector3 vector2 = this.currentObject.get_transform().get_localEulerAngles();
                Vector3 vector3 = this.currentObject.get_transform().get_localScale();
                this.currentObject.get_transform().SetParent(base.get_transform());
                this.currentObject.get_transform().set_localPosition(vector);
                this.currentObject.get_transform().set_localScale(vector3);
                this.currentObject.get_transform().set_localEulerAngles(vector2);
                this.currentObject.ChangeLayerDeep(base.get_gameObject().get_layer());
                this.currentObject.get_gameObject().SetActive(true);
                this.animator = base.GetComponentInChildren<Animator>();
                this.ChangeResourceOnDrawSub(grapic);
            }
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

        protected abstract void ChangeResourceOnDrawSub(AdvGraphicInfo grapic);
        internal override bool CheckFailedCrossFade(AdvGraphicInfo grapic)
        {
            return (base.LastResource != grapic);
        }

        internal override void Flip(bool flipX, bool flipY)
        {
        }

        public override void Init(AdvGraphicObject parentObject)
        {
            this.AnimationStateName = string.Empty;
            base.Init(parentObject);
        }

        public override void Read(BinaryReader reader)
        {
            int num = reader.ReadInt32();
            if ((num < 0) || (num > 1))
            {
                object[] args = new object[] { num };
                Debug.LogError(LanguageErrorMsg.LocalizeTextFormat(ErrorMsg.UnknownVersion, args));
            }
            else
            {
                string str = reader.ReadString();
                switch (((SaveType) Enum.Parse(typeof(SaveType), str)))
                {
                    case SaveType.Animator:
                    {
                        int num2 = reader.ReadInt32();
                        for (int i = 0; i < num2; i++)
                        {
                            int num4 = reader.ReadInt32();
                            int num5 = i;
                            float num6 = reader.ReadSingle();
                            this.animator.Play(num4, num5, num6);
                        }
                        return;
                    }
                }
                string name = reader.ReadString();
                this.ChangeAnimationState(name, 0f);
            }
        }

        public override void RuleFadeIn(AdvEngine engine, AdvTransitionArgs data, Action onComplete)
        {
            Debug.LogError(base.get_gameObject().get_name() + " is not support RuleFadeIn", base.get_gameObject());
            if (onComplete != null)
            {
                onComplete();
            }
        }

        public override void RuleFadeOut(AdvEngine engine, AdvTransitionArgs data, Action onComplete)
        {
            Debug.LogError(base.get_gameObject().get_name() + " is not support RuleFadeOut", base.get_gameObject());
            if (onComplete != null)
            {
                onComplete();
            }
        }

        internal override void Scale(AdvGraphicInfo graphic)
        {
            base.get_transform().set_localScale((Vector3) (graphic.Scale * base.Layer.Manager.PixelsToUnits));
        }

        internal override void SetCommandArg(AdvCommand command)
        {
            string name = command.ParseCellOptional<string>(AdvColumnName.Arg2, string.Empty);
            float fadeTime = command.ParseCellOptional<float>(AdvColumnName.Arg6, 0.2f);
            this.ChangeAnimationState(name, fadeTime);
        }

        public override void Write(BinaryWriter writer)
        {
            writer.Write(1);
            if (this.animator != null)
            {
                writer.Write(SaveType.Animator.ToString());
                int num = this.animator.get_layerCount();
                writer.Write(num);
                for (int i = 0; i < num; i++)
                {
                    AnimatorStateInfo info = !this.animator.IsInTransition(i) ? this.animator.GetCurrentAnimatorStateInfo(i) : this.animator.GetNextAnimatorStateInfo(i);
                    writer.Write(info.get_fullPathHash());
                    writer.Write(info.get_normalizedTime());
                }
            }
            else
            {
                writer.Write(SaveType.Other.ToString());
                writer.Write(this.AnimationStateName);
            }
        }

        private string AnimationStateName { get; set; }

        private enum SaveType
        {
            Animator,
            Other
        }
    }
}

