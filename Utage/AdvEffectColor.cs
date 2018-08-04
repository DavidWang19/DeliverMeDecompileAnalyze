namespace Utage
{
    using System;
    using System.IO;
    using UnityEngine;
    using UtageExtensions;

    [AddComponentMenu("Utage/ADV/Internal/EffectColor")]
    public class AdvEffectColor : MonoBehaviour
    {
        [SerializeField]
        private Color animationColor = Color.get_white();
        [SerializeField]
        private Color customColor = Color.get_white();
        [SerializeField]
        private float fadeAlpha = 1f;
        private Color lastColor = Color.get_white();
        public EventEffectColor OnValueChanged = new EventEffectColor();
        [SerializeField]
        private Color scriptColor = Color.get_white();
        [SerializeField]
        private Color tweenColor = Color.get_white();
        private const int Version = 0;

        private void ChangedValue()
        {
            Color mulColor = this.MulColor;
            this.OnValueChanged.Invoke(this);
            this.lastColor = mulColor;
        }

        private void OnValidate()
        {
            this.ChangedValue();
        }

        public void Read(BinaryReader reader)
        {
            int num = reader.ReadInt32();
            if ((num < 0) || (num > 0))
            {
                object[] args = new object[] { num };
                Debug.LogError(LanguageErrorMsg.LocalizeTextFormat(ErrorMsg.UnknownVersion, args));
            }
            else
            {
                this.animationColor = reader.ReadColor();
                this.tweenColor = reader.ReadColor();
                this.scriptColor = reader.ReadColor();
                this.customColor = reader.ReadColor();
                this.fadeAlpha = reader.ReadSingle();
                this.fadeAlpha = 1f;
                this.ChangedValue();
            }
        }

        private void Update()
        {
            if (this.lastColor != this.MulColor)
            {
                this.ChangedValue();
            }
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write(0);
            writer.Write(this.AnimationColor);
            writer.Write(this.TweenColor);
            writer.Write(this.ScriptColor);
            writer.Write(this.CustomColor);
            writer.Write(this.FadeAlpha);
        }

        public Color AnimationColor
        {
            get
            {
                return this.animationColor;
            }
            set
            {
                this.animationColor = value;
                this.ChangedValue();
            }
        }

        public Color CustomColor
        {
            get
            {
                return this.customColor;
            }
            set
            {
                this.customColor = value;
                this.ChangedValue();
            }
        }

        public float FadeAlpha
        {
            get
            {
                return this.fadeAlpha;
            }
            set
            {
                this.fadeAlpha = value;
                this.ChangedValue();
            }
        }

        public Color MulColor
        {
            get
            {
                Color color = ((this.AnimationColor * this.TweenColor) * this.ScriptColor) * this.CustomColor;
                color.a *= this.FadeAlpha;
                return color;
            }
        }

        public Color ScriptColor
        {
            get
            {
                return this.scriptColor;
            }
            set
            {
                this.scriptColor = value;
                this.ChangedValue();
            }
        }

        public Color TweenColor
        {
            get
            {
                return this.tweenColor;
            }
            set
            {
                this.tweenColor = value;
                this.ChangedValue();
            }
        }
    }
}

