namespace Utage
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.UI;

    [AddComponentMenu("Utage/Lib/UI/DicingImage"), ExecuteInEditMode, RequireComponent(typeof(RectTransform))]
    public class DicingImage : MaskableGraphic, ICanvasRaycastFilter
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <MainPattern>k__BackingField;
        [SerializeField]
        private DicingTextures dicingData;
        private Texture m_Texture;
        [SerializeField, StringPopupFunction("GetPattenNameList")]
        private string pattern;
        private DicingTextureData patternData;
        private Dictionary<string, string> patternOption = new Dictionary<string, string>();
        [SerializeField]
        private bool skipTransParentCell = true;
        [SerializeField]
        private Rect uvRect = new Rect(0f, 0f, 1f, 1f);

        public void ChangePattern(string pattern)
        {
            this.MainPattern = pattern;
            this.patternOption.Clear();
            this.Pattern = pattern;
        }

        protected void ForeachVertexList(Action<Rect, Rect> function)
        {
            Rect pixelAdjustedRect = base.GetPixelAdjustedRect();
            this.PatternData.ForeachVertexList(pixelAdjustedRect, this.uvRect, this.skipTransParentCell, this.DicingData, function);
        }

        private Vector2 GetNaitiveSize()
        {
            return new Vector2(this.uvRect.get_width() * this.PatternData.Width, this.uvRect.get_height() * this.PatternData.Height);
        }

        private List<string> GetPattenNameList()
        {
            if (this.dicingData == null)
            {
                return null;
            }
            return this.dicingData.GetPattenNameList();
        }

        internal List<DicingTextureData.QuadVerts> GetVerts(DicingTextureData patternData)
        {
            return this.DicingData.GetVerts(patternData);
        }

        public bool HitTest(Vector2 localPosition)
        {
            <HitTest>c__AnonStorey1 storey = new <HitTest>c__AnonStorey1 {
                localPosition = localPosition
            };
            if (!base.GetPixelAdjustedRect().Contains(storey.localPosition))
            {
                return false;
            }
            if (this.PatternData == null)
            {
                return false;
            }
            storey.isHit = false;
            this.ForeachVertexList(new Action<Rect, Rect>(storey.<>m__0));
            return storey.isHit;
        }

        public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
        {
            Vector2 vector;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(base.get_rectTransform(), sp, eventCamera, ref vector);
            return this.HitTest(vector);
        }

        public string MakePatternWithOption()
        {
            string mainPattern = this.MainPattern;
            SortedDictionary<string, string> dictionary = new SortedDictionary<string, string>(this.patternOption);
            foreach (KeyValuePair<string, string> pair in dictionary)
            {
                mainPattern = mainPattern + pair.Value;
            }
            return mainPattern;
        }

        private void OnChangePattern()
        {
            if ((this.DicingData == null) || string.IsNullOrEmpty(this.pattern))
            {
                this.m_Texture = Graphic.s_WhiteTexture;
            }
            else
            {
                this.patternData = this.DicingData.GetTextureData(this.Pattern);
                if (this.patternData == null)
                {
                    Debug.LogError(this.Pattern + " is not find in " + this.DicingData.get_name());
                }
                else
                {
                    this.m_Texture = this.DicingData.GetTexture(this.patternData.AtlasName);
                }
            }
        }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            <OnPopulateMesh>c__AnonStorey0 storey = new <OnPopulateMesh>c__AnonStorey0 {
                vh = vh
            };
            this.OnChangePattern();
            if (this.PatternData != null)
            {
                storey.color32 = this.get_color();
                storey.vh.Clear();
                storey.index = 0;
                this.ForeachVertexList(new Action<Rect, Rect>(storey.<>m__0));
            }
        }

        public override void SetNativeSize()
        {
            if (this.PatternData != null)
            {
                base.get_rectTransform().set_anchorMax(base.get_rectTransform().get_anchorMin());
                base.get_rectTransform().set_sizeDelta(this.GetNaitiveSize());
            }
        }

        public bool TryChangePatternWithOption(string mainPattern, string optionTag, string option)
        {
            this.MainPattern = mainPattern;
            this.patternOption[optionTag] = option;
            string pattern = this.MakePatternWithOption();
            if (this.DicingData.Exists(pattern))
            {
                this.Pattern = pattern;
                return true;
            }
            if (this.DicingData.Exists(option))
            {
                this.Pattern = option;
                return true;
            }
            this.Pattern = this.MainPattern;
            return false;
        }

        public DicingTextures DicingData
        {
            get
            {
                return this.dicingData;
            }
            set
            {
                this.dicingData = value;
                this.pattern = string.Empty;
                this.OnChangePattern();
                this.SetAllDirty();
            }
        }

        public string MainPattern { get; private set; }

        public override Texture mainTexture
        {
            get
            {
                if (this.m_Texture != null)
                {
                    return this.m_Texture;
                }
                if ((this.get_material() != null) && (this.get_material().get_mainTexture() != null))
                {
                    return this.get_material().get_mainTexture();
                }
                return Graphic.s_WhiteTexture;
            }
        }

        private string Pattern
        {
            get
            {
                return this.pattern;
            }
            set
            {
                if (!this.DicingData.Exists(value))
                {
                    Debug.LogError(value + " is not find in " + this.DicingData.get_name());
                }
                else
                {
                    this.pattern = value;
                    this.OnChangePattern();
                    this.SetAllDirty();
                }
            }
        }

        public DicingTextureData PatternData
        {
            get
            {
                return this.patternData;
            }
        }

        public bool SkipTransParentCell
        {
            get
            {
                return this.skipTransParentCell;
            }
            set
            {
                this.skipTransParentCell = value;
            }
        }

        public Rect UvRect
        {
            get
            {
                return this.uvRect;
            }
            set
            {
                this.uvRect = value;
                this.SetAllDirty();
            }
        }

        [CompilerGenerated]
        private sealed class <HitTest>c__AnonStorey1
        {
            internal bool isHit;
            internal Vector2 localPosition;

            internal void <>m__0(Rect r, Rect uv)
            {
                this.isHit |= r.Contains(this.localPosition);
            }
        }

        [CompilerGenerated]
        private sealed class <OnPopulateMesh>c__AnonStorey0
        {
            internal Color color32;
            internal int index;
            internal VertexHelper vh;

            internal void <>m__0(Rect r, Rect uv)
            {
                this.vh.AddVert(new Vector3(r.get_xMin(), r.get_yMin()), this.color32, new Vector2(uv.get_xMin(), uv.get_yMin()));
                this.vh.AddVert(new Vector3(r.get_xMin(), r.get_yMax()), this.color32, new Vector2(uv.get_xMin(), uv.get_yMax()));
                this.vh.AddVert(new Vector3(r.get_xMax(), r.get_yMax()), this.color32, new Vector2(uv.get_xMax(), uv.get_yMax()));
                this.vh.AddVert(new Vector3(r.get_xMax(), r.get_yMin()), this.color32, new Vector2(uv.get_xMax(), uv.get_yMin()));
                this.vh.AddTriangle(this.index, this.index + 1, this.index + 2);
                this.vh.AddTriangle(this.index + 2, this.index + 3, this.index);
                this.index += 4;
            }
        }
    }
}

