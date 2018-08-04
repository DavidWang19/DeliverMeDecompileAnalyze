namespace Utage
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UtageExtensions;

    public class UguiNovelTextCharacter
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private float <BmpFontScale>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <CustomSpace>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <IsVisible>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private float <RubySpaceSize>k__BackingField;
        public CharData charData;
        public CharacterInfo charInfo;
        private int defaultFontSize;
        private Color effectColor;
        private int fontSize;
        private UnityEngine.FontStyle fontStyle;
        public bool isAutoLineBreak;
        private bool isError;
        public bool isKinsokuBurasage;
        private bool isSprite;
        private Vector2 position;
        private UIVertex[] verts;
        private float width;

        public UguiNovelTextCharacter(CharData charData, UguiNovelTextGenerator generator)
        {
            this.effectColor = Color.get_white();
            if (charData.CustomInfo.IsDash)
            {
                charData.Char = generator.DashChar;
            }
            int bmpFontSize = generator.BmpFontSize;
            this.Init(charData, generator.NovelText.get_font(), generator.NovelText.get_fontSize(), bmpFontSize, generator.NovelText.get_fontStyle(), generator.Space);
            if (charData.CustomInfo.IsSuperOrSubScript)
            {
                this.fontSize = Mathf.FloorToInt(generator.SupOrSubSizeScale * this.fontSize);
                if (!generator.NovelText.get_font().get_dynamic())
                {
                    this.BmpFontScale = (1f * this.fontSize) / ((float) bmpFontSize);
                }
            }
            if (charData.CustomInfo.IsSpace)
            {
                this.width = charData.CustomInfo.SpaceSize;
                this.CustomSpace = true;
            }
            if (generator.EmojiData != null)
            {
                if (this.CustomInfo.IsEmoji || generator.EmojiData.Contains(this.Char))
                {
                    this.IsSprite = true;
                }
                if (this.IsSprite)
                {
                    Sprite sprite = this.FindSprite(generator);
                    if (sprite != null)
                    {
                        float num2 = sprite.get_rect().get_width() / ((float) generator.EmojiData.Size);
                        this.width = num2 * this.fontSize;
                    }
                    else
                    {
                        Debug.LogError(string.Concat(new object[] { "Not found Emoji[", this.Char, "]:", (int) this.Char }));
                    }
                }
            }
        }

        public UguiNovelTextCharacter(CharData charData, Font font, int fontSize, int bmpFontSize, UnityEngine.FontStyle fontStyle)
        {
            this.effectColor = Color.get_white();
            this.Init(charData, font, fontSize, bmpFontSize, fontStyle, -1f);
        }

        internal RectTransform AddGraphicObject(RectTransform parent, UguiNovelTextGenerator generator)
        {
            if (this.IsSprite)
            {
                Sprite sprite = this.FindSprite(generator);
                if (sprite != null)
                {
                    RectTransform transform = parent.AddChildGameObjectComponent<RectTransform>(sprite.get_name());
                    transform.get_gameObject().set_hideFlags(0x34);
                    transform.get_gameObject().AddComponent<Image>().set_sprite(sprite);
                    float num2 = 1f;
                    float num = num2 = (1f * this.FontSize) / ((float) generator.EmojiData.Size);
                    float num3 = sprite.get_rect().get_width() * num;
                    float num4 = sprite.get_rect().get_height() * num2;
                    transform.set_sizeDelta(new Vector2(num3, num4));
                    transform.set_localPosition(new Vector3(this.PositionX + (num3 / 2f), this.PositionY + (num4 / 2f), 0f));
                    return transform;
                }
            }
            return null;
        }

        public void ApplyOffsetX(float offsetX)
        {
            this.PositionX += offsetX;
        }

        public void ApplyOffsetY(float offsetY)
        {
            this.PositionY += offsetY;
        }

        internal void ChangeEffectColor(Color effectColor)
        {
            this.effectColor = effectColor;
        }

        private Sprite FindSprite(UguiNovelTextGenerator generator)
        {
            if (generator.EmojiData == null)
            {
                return null;
            }
            Sprite sprite = generator.EmojiData.GetSprite(this.Char);
            if ((sprite == null) && this.CustomInfo.IsEmoji)
            {
                sprite = generator.EmojiData.GetSprite(this.charData.CustomInfo.EmojiKey);
            }
            return sprite;
        }

        private void Init(CharData charData, Font font, int fontSize, int bmpFontSize, UnityEngine.FontStyle fontStyle, float spacing)
        {
            this.charData = charData;
            this.fontSize = this.defaultFontSize = charData.CustomInfo.GetCustomedSize(fontSize);
            this.fontStyle = charData.CustomInfo.GetCustomedStyle(fontStyle);
            if (charData.IsBr)
            {
                this.width = 0f;
            }
            else if (char.IsWhiteSpace(charData.Char) && (spacing >= 0f))
            {
                this.CustomSpace = true;
                this.width = spacing;
            }
            if (font.get_dynamic())
            {
                this.BmpFontScale = 1f;
            }
            else
            {
                this.BmpFontScale = (1f * this.fontSize) / ((float) bmpFontSize);
            }
        }

        internal void InitPositionX(float x, float currentSpace)
        {
            this.PositionX = x;
        }

        public void MakeVerts(Color defaultColor, UguiNovelTextGenerator generator)
        {
            if (!this.IsNoFontData)
            {
                this.verts = new UIVertex[4];
                Color color = this.charData.CustomInfo.GetCustomedColor(defaultColor) * this.effectColor;
                for (int i = 0; i < 4; i++)
                {
                    this.verts[i] = UIVertex.simpleVert;
                    this.verts[i].color = color;
                }
                WrapperUnityVersion.SetCharacterInfoToVertex(this.verts, this, generator.NovelText.get_font());
                if (!generator.NovelText.get_font().get_dynamic() && !generator.IsUnicodeFont)
                {
                    float fontSize = this.fontSize;
                    for (int j = 0; j < 4; j++)
                    {
                        this.verts[j].position.y += fontSize;
                    }
                }
                if (this.CustomInfo.IsSuperScript)
                {
                    float num4 = (1f - generator.SupOrSubSizeScale) * this.DefaultFontSize;
                    for (int k = 0; k < 4; k++)
                    {
                        this.verts[k].position.y += num4;
                    }
                }
                if (this.CustomInfo.IsDash)
                {
                    float num6 = Mathf.Abs(this.verts[2].position.y - this.verts[0].position.y);
                    float num7 = this.PositionY + (this.FontSize / 2);
                    this.verts[0].position.y = this.verts[1].position.y = num7 - (num6 / 2f);
                    this.verts[2].position.y = this.verts[3].position.y = num7 + (num6 / 2f);
                    UIVertex[] vertex = new UIVertex[12];
                    for (int m = 0; m < 12; m++)
                    {
                        vertex[m] = this.verts[m % 4];
                    }
                    float x = this.verts[0].position.x;
                    float num11 = this.verts[1].position.x - this.verts[0].position.x;
                    float xMax = x + this.Width;
                    float num13 = x + (num11 / 3f);
                    float num14 = xMax - (num11 / 3f);
                    this.SetVertexX(vertex, 0, x, num13);
                    this.SetVertexX(vertex, 4, num13, num14);
                    this.SetVertexX(vertex, 8, num14, xMax);
                    Vector2 uvBottomLeft = this.verts[0].uv0;
                    Vector2 uvBottomRight = this.verts[1].uv0;
                    Vector2 uvTopRight = this.verts[2].uv0;
                    Vector2 uvTopLeft = this.verts[3].uv0;
                    Vector2 vector5 = ((Vector2) (((uvBottomRight - uvBottomLeft) * 1f) / 3f)) + uvBottomLeft;
                    Vector2 vector6 = ((Vector2) (((uvBottomRight - uvBottomLeft) * 2f) / 3f)) + uvBottomLeft;
                    Vector2 vector7 = ((Vector2) (((uvTopRight - uvTopLeft) * 2f) / 3f)) + uvTopLeft;
                    Vector2 vector8 = ((Vector2) (((uvTopRight - uvTopLeft) * 1f) / 3f)) + uvTopLeft;
                    this.SetVertexUV(vertex, 0, uvBottomLeft, vector5, vector8, uvTopLeft);
                    this.SetVertexUV(vertex, 4, vector5, vector6, vector7, vector8);
                    this.SetVertexUV(vertex, 8, vector6, uvBottomRight, uvTopRight, vector7);
                    this.verts = vertex;
                }
            }
        }

        internal void ResetEffectColor()
        {
            this.effectColor = Color.get_white();
        }

        public void SetCharacterInfo(Font font)
        {
            if (!this.TrySetCharacterInfo(font))
            {
                this.isError = true;
                this.width = this.fontSize;
            }
        }

        private void SetVertexUV(UIVertex[] vertex, int index, Vector2 uvBottomLeft, Vector2 uvBottomRight, Vector2 uvTopRight, Vector2 uvTopLeft)
        {
            vertex[index].uv0 = uvBottomLeft;
            vertex[index + 1].uv0 = uvBottomRight;
            vertex[index + 2].uv0 = uvTopRight;
            vertex[index + 3].uv0 = uvTopLeft;
        }

        private void SetVertexX(UIVertex[] vertex, int index, float xMin, float xMax)
        {
            vertex[index].position.x = vertex[index + 3].position.x = xMin;
            vertex[index + 1].position.x = vertex[index + 2].position.x = xMax;
        }

        public bool TrySetCharacterInfo(Font font)
        {
            if (!this.IsNoFontData)
            {
                if (!font.get_dynamic())
                {
                    if (!font.GetCharacterInfo(this.charData.Char, ref this.charInfo))
                    {
                        return false;
                    }
                }
                else if (!font.GetCharacterInfo(this.charData.Char, ref this.charInfo, this.FontSize, this.FontStyle))
                {
                    return false;
                }
                this.width = WrapperUnityVersion.GetCharacterInfoWidth(ref this.charInfo);
                this.width *= this.BmpFontScale;
                if (this.CustomInfo.IsDash)
                {
                    this.width *= this.CustomInfo.DashSize;
                }
            }
            return true;
        }

        public float BeginPositionX
        {
            get
            {
                return (this.PositionX - this.RubySpaceSize);
            }
        }

        public float BmpFontScale { get; private set; }

        public char Char
        {
            get
            {
                return this.charData.Char;
            }
        }

        public CharData.CustomCharaInfo CustomInfo
        {
            get
            {
                return this.charData.CustomInfo;
            }
        }

        private bool CustomSpace { get; set; }

        public int DefaultFontSize
        {
            get
            {
                return this.defaultFontSize;
            }
        }

        public float EndPositionX
        {
            get
            {
                return ((this.PositionX + this.Width) + this.RubySpaceSize);
            }
        }

        public int FontSize
        {
            get
            {
                return this.fontSize;
            }
        }

        public UnityEngine.FontStyle FontStyle
        {
            get
            {
                return this.fontStyle;
            }
        }

        public bool IsBlank
        {
            get
            {
                return (this.IsCustomBlank || char.IsWhiteSpace(this.charData.Char));
            }
        }

        public bool IsBr
        {
            get
            {
                return this.charData.IsBr;
            }
        }

        public bool IsBrOrAutoBr
        {
            get
            {
                return (this.isAutoLineBreak || this.charData.IsBr);
            }
        }

        private bool IsCustomBlank
        {
            get
            {
                return ((this.isError || this.CustomSpace) || this.charData.IsBr);
            }
        }

        public bool IsDisableAutoLineBreak
        {
            get
            {
                return ((this.CustomInfo.IsRuby && !this.CustomInfo.IsRubyTop) || (this.CustomInfo.IsGroup && !this.CustomInfo.IsGroupTop));
            }
        }

        public bool IsNoFontData
        {
            get
            {
                return (this.IsCustomBlank || this.IsSprite);
            }
        }

        public bool IsSprite
        {
            get
            {
                return this.isSprite;
            }
            set
            {
                this.isSprite = value;
            }
        }

        public bool IsVisible { get; set; }

        public float PositionX
        {
            get
            {
                return this.position.x;
            }
            set
            {
                this.position.x = value;
            }
        }

        public float PositionY
        {
            get
            {
                return this.position.y;
            }
            set
            {
                this.position.y = value;
            }
        }

        public float RubySpaceSize { get; set; }

        public UIVertex[] Verts
        {
            get
            {
                return this.verts;
            }
        }

        public float Width
        {
            get
            {
                return this.width;
            }
            set
            {
                this.width = value;
            }
        }
    }
}

