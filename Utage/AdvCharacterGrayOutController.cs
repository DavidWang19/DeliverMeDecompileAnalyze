namespace Utage
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.Events;

    [AddComponentMenu("Utage/ADV/Extra/CharacterGrayOutContoller")]
    public class AdvCharacterGrayOutController : MonoBehaviour
    {
        [SerializeField]
        protected AdvEngine engine;
        [SerializeField]
        private float fadeTime = 0.2f;
        private bool isChanged;
        [SerializeField]
        private Color mainColor = Color.get_white();
        [SerializeField, EnumFlags]
        private LightingMask mask = LightingMask.Talking;
        [SerializeField]
        private List<string> noGrayoutCharacters = new List<string>();
        private List<AdvGraphicLayer> pageBeginLayer;
        [SerializeField]
        private Color subColor = Color.get_gray();

        private void Awake()
        {
            if (this.Engine != null)
            {
                this.Engine.Page.OnBeginPage.AddListener(new UnityAction<AdvPage>(this, (IntPtr) this.OnBeginPage));
                this.Engine.Page.OnChangeText.AddListener(new UnityAction<AdvPage>(this, (IntPtr) this.OnChangeText));
            }
        }

        private void ChangeColor(AdvGraphicLayer layer, Color color)
        {
            foreach (KeyValuePair<string, AdvGraphicObject> pair in layer.CurrentGraphics)
            {
                AdvEffectColor component = pair.Value.get_gameObject().GetComponent<AdvEffectColor>();
                if (component != null)
                {
                    if (this.FadeTime > 0f)
                    {
                        Color customColor = component.CustomColor;
                        base.StartCoroutine(this.FadeColor(component, customColor, color));
                    }
                    else
                    {
                        component.CustomColor = color;
                    }
                }
            }
        }

        [DebuggerHidden]
        private IEnumerator FadeColor(AdvEffectColor effect, Color from, Color to)
        {
            return new <FadeColor>c__Iterator0 { from = from, to = to, effect = effect, $this = this };
        }

        private bool IsLightingCharacter(AdvPage page, AdvGraphicLayer layer)
        {
            <IsLightingCharacter>c__AnonStorey1 storey = new <IsLightingCharacter>c__AnonStorey1 {
                layer = layer
            };
            return ((((this.Mask & LightingMask.Talking) == LightingMask.Talking) && (storey.layer.DefaultObject.get_name() == page.CharacterLabel)) || ((((this.Mask & LightingMask.NewCharacerInPage) == LightingMask.NewCharacerInPage) && (this.pageBeginLayer.Find(new Predicate<AdvGraphicLayer>(storey.<>m__0)) == null)) || this.NoGrayoutCharacters.Exists(new Predicate<string>(storey.<>m__1))));
        }

        private void OnBeginPage(AdvPage page)
        {
            this.pageBeginLayer = page.Engine.GraphicManager.CharacterManager.AllGraphicsLayers();
            if ((this.mask == 0) && this.isChanged)
            {
                foreach (AdvGraphicLayer layer in this.pageBeginLayer)
                {
                    this.ChangeColor(layer, this.MainColor);
                }
                this.isChanged = false;
            }
        }

        private void OnChangeText(AdvPage page)
        {
            if (this.mask != 0)
            {
                this.isChanged = true;
                AdvEngine engine = page.Engine;
                if (!string.IsNullOrEmpty(page.CharacterLabel) || ((this.Mask & LightingMask.NoChanageIfTextOnly) != LightingMask.NoChanageIfTextOnly))
                {
                    foreach (AdvGraphicLayer layer in engine.GraphicManager.CharacterManager.AllGraphicsLayers())
                    {
                        this.ChangeColor(layer, !this.IsLightingCharacter(page, layer) ? this.SubColor : this.MainColor);
                    }
                }
            }
        }

        public AdvEngine Engine
        {
            get
            {
                if (this.engine == null)
                {
                }
                return (this.engine = Object.FindObjectOfType<AdvEngine>());
            }
        }

        public float FadeTime
        {
            get
            {
                return this.fadeTime;
            }
            set
            {
                this.fadeTime = value;
            }
        }

        public Color MainColor
        {
            get
            {
                return this.mainColor;
            }
            set
            {
                this.mainColor = value;
            }
        }

        public LightingMask Mask
        {
            get
            {
                return this.mask;
            }
            set
            {
                this.mask = value;
            }
        }

        public List<string> NoGrayoutCharacters
        {
            get
            {
                return this.noGrayoutCharacters;
            }
            set
            {
                this.noGrayoutCharacters = value;
            }
        }

        public Color SubColor
        {
            get
            {
                return this.subColor;
            }
            set
            {
                this.subColor = value;
            }
        }

        [CompilerGenerated]
        private sealed class <FadeColor>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal bool $disposing;
            internal int $PC;
            internal AdvCharacterGrayOutController $this;
            internal float <elapsed>__0;
            internal AdvEffectColor effect;
            internal Color from;
            internal Color to;

            [DebuggerHidden]
            public void Dispose()
            {
                this.$disposing = true;
                this.$PC = -1;
            }

            public bool MoveNext()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                switch (num)
                {
                    case 0:
                        this.<elapsed>__0 = 0f;
                        break;

                    case 1:
                        this.<elapsed>__0 += Time.get_deltaTime();
                        if (this.<elapsed>__0 >= this.$this.fadeTime)
                        {
                            this.<elapsed>__0 = this.$this.fadeTime;
                        }
                        this.effect.CustomColor = Color.Lerp(this.from, this.to, this.<elapsed>__0 / this.$this.FadeTime);
                        if (this.<elapsed>__0 < this.$this.fadeTime)
                        {
                            break;
                        }
                        goto Label_00D9;

                    default:
                        goto Label_00D9;
                }
                this.$current = new WaitForEndOfFrame();
                if (!this.$disposing)
                {
                    this.$PC = 1;
                }
                return true;
                this.$PC = -1;
            Label_00D9:
                return false;
            }

            [DebuggerHidden]
            public void Reset()
            {
                throw new NotSupportedException();
            }

            object IEnumerator<object>.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.$current;
                }
            }

            object IEnumerator.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.$current;
                }
            }
        }

        [CompilerGenerated]
        private sealed class <IsLightingCharacter>c__AnonStorey1
        {
            internal AdvGraphicLayer layer;

            internal bool <>m__0(AdvGraphicLayer x)
            {
                return ((x != null) && (x.DefaultObject.get_name() == this.layer.DefaultObject.get_name()));
            }

            internal bool <>m__1(string x)
            {
                return (x == this.layer.DefaultObject.get_name());
            }
        }

        [Flags]
        public enum LightingMask
        {
            NewCharacerInPage = 2,
            NoChanageIfTextOnly = 4,
            Talking = 1
        }
    }
}

