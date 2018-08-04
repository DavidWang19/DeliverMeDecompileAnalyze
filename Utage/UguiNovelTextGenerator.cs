namespace Utage
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.UI;

    [AddComponentMenu("Utage/Lib/UI/Internal/NovelTextGenerator")]
    public class UguiNovelTextGenerator : MonoBehaviour
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private ChagneType <CurrentChangeType>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <IsRebuidFont>k__BackingField;
        private UguiNovelTextGeneratorAdditional additional;
        [SerializeField]
        private int bmpFontSize = 1;
        private RectTransform cachedRectTransform;
        [SerializeField]
        private char dashChar = '—';
        [SerializeField]
        private UguiNovelTextEmojiData emojiData;
        private Vector3 endPosition;
        private List<GraphicObject> graphicObjectList;
        private float height;
        private List<UguiNovelTextHitArea> hitGroupLists = new List<UguiNovelTextHitArea>();
        private bool isDebugLog;
        private bool isInitGraphicObjectList;
        private bool isRequestingCharactersInTexture;
        [SerializeField]
        private bool isUnicodeFont;
        [SerializeField]
        private int lengthOfView = -1;
        [SerializeField]
        private float letterSpaceSize = 1f;
        private List<UguiNovelTextLine> lineDataList = new List<UguiNovelTextLine>();
        private float maxHeight;
        private float maxWidth;
        private UguiNovelText novelText;
        private float preferredHeight;
        [SerializeField]
        private float rubySizeScale = 0.5f;
        [SerializeField]
        private float space = -1f;
        [SerializeField]
        private float supOrSubSizeScale = 0.5f;
        private Utage.TextData textData;
        [SerializeField]
        private UguiNovelTextSettings textSettings;
        [SerializeField, EnumFlags]
        private WordWrap wordWrap = (WordWrap.JapaneseKinsoku | WordWrap.Default);

        private void AddToRequestCharactersInfoList(UguiNovelTextCharacter characterData, List<RequestCharactersInfo> infoList)
        {
            if (!characterData.IsNoFontData)
            {
                foreach (RequestCharactersInfo info in infoList)
                {
                    if (info.TryAddData(characterData))
                    {
                        return;
                    }
                }
                infoList.Add(new RequestCharactersInfo(characterData));
            }
        }

        private void ApplyOffset(List<UguiNovelTextLine> lineList)
        {
            Vector2 vector = this.CachedRectTransform.get_pivot();
            Vector2 offset = new Vector2(-vector.x * this.MaxWidth, (1f - vector.y) * this.MaxHeight);
            foreach (UguiNovelTextLine line in lineList)
            {
                line.ApplyOffset(offset);
            }
        }

        private void ApplyTextAnchor(List<UguiNovelTextLine> lineList, TextAnchor anchor)
        {
            Vector2 textAnchorPivot = Text.GetTextAnchorPivot(anchor);
            foreach (UguiNovelTextLine line in lineList)
            {
                line.ApplyTextAnchorX(textAnchorPivot.x, this.MaxWidth);
            }
            if (textAnchorPivot.y != 1f)
            {
                float offsetY = (this.MaxHeight - this.Height) * (textAnchorPivot.y - 1f);
                foreach (UguiNovelTextLine line2 in lineList)
                {
                    line2.ApplyTextAnchorY(offsetY);
                }
            }
        }

        private void AutoLineBreak(List<UguiNovelTextCharacter> characterDataList)
        {
            float num = 0f;
            int index = 0;
            while (index < characterDataList.Count)
            {
                int beginIndex = index;
                float currentSpace = 0f;
                float x = 0f;
                while (index < characterDataList.Count)
                {
                    UguiNovelTextCharacter lineTopCharacter = characterDataList[index];
                    if (x == 0f)
                    {
                        currentSpace = this.Additional.GetTopLetterSpace(lineTopCharacter, this);
                        x += num;
                        if ((index == 0) && this.IsAutoIndentation(lineTopCharacter.Char))
                        {
                            num = lineTopCharacter.Width + this.LetterSpaceSize;
                        }
                    }
                    if (lineTopCharacter.CustomInfo.IsRuby)
                    {
                        currentSpace += lineTopCharacter.RubySpaceSize;
                    }
                    x += currentSpace;
                    if (!lineTopCharacter.IsBlank && this.IsOverMaxWidth(x, this.Additional.GetMaxWidth(lineTopCharacter)))
                    {
                        index = this.GetAutoLineBreakIndex(characterDataList, beginIndex, index);
                        lineTopCharacter = characterDataList[index];
                        lineTopCharacter.isAutoLineBreak = true;
                    }
                    index++;
                    if (!lineTopCharacter.IsBrOrAutoBr)
                    {
                        lineTopCharacter.InitPositionX(x, currentSpace);
                        x += lineTopCharacter.Width;
                        if (lineTopCharacter.RubySpaceSize != 0f)
                        {
                            currentSpace = lineTopCharacter.RubySpaceSize;
                        }
                        else
                        {
                            currentSpace = this.LetterSpaceSize;
                            if (((this.TextSettings != null) && (index < characterDataList.Count)) && this.TextSettings.IsIgonreLetterSpace(lineTopCharacter, characterDataList[index]))
                            {
                                currentSpace = 0f;
                            }
                        }
                    }
                }
            }
        }

        public void ChangeAll()
        {
            this.CurrentChangeType = ChagneType.All;
        }

        private bool CheckWordWrap(UguiNovelTextCharacter current, UguiNovelTextCharacter prev)
        {
            return (current.IsDisableAutoLineBreak || ((this.TextSettings != null) && this.TextSettings.CheckWordWrap(this, current, prev)));
        }

        private void ClearCurrentChangeType()
        {
            this.CurrentChangeType = ChagneType.None;
        }

        private void ClearGraphicObjectList()
        {
            if (this.graphicObjectList != null)
            {
                foreach (GraphicObject obj2 in this.graphicObjectList)
                {
                    if (Application.get_isPlaying())
                    {
                        Object.Destroy(obj2.graphic.get_gameObject());
                    }
                    else
                    {
                        Object.DestroyImmediate(obj2.graphic.get_gameObject());
                    }
                }
                this.graphicObjectList.Clear();
                this.graphicObjectList = null;
                this.isInitGraphicObjectList = false;
            }
        }

        private List<UguiNovelTextCharacter> CreateCharacterDataList()
        {
            List<UguiNovelTextCharacter> list = new List<UguiNovelTextCharacter>();
            if (this.textData != null)
            {
                for (int i = 0; i < this.textData.Length; i++)
                {
                    UguiNovelTextCharacter item = new UguiNovelTextCharacter(this.textData.CharList[i], this);
                    list.Add(item);
                }
            }
            return list;
        }

        private List<UguiNovelTextLine> CreateLineList(List<UguiNovelTextCharacter> characterDataList)
        {
            List<UguiNovelTextLine> list = new List<UguiNovelTextLine>();
            UguiNovelTextLine item = new UguiNovelTextLine();
            foreach (UguiNovelTextCharacter character in characterDataList)
            {
                item.AddCharaData(character);
                if (character.IsBrOrAutoBr)
                {
                    item.EndCharaData(this);
                    list.Add(item);
                    item = new UguiNovelTextLine();
                }
            }
            if (item.Characters.Count > 0)
            {
                item.EndCharaData(this);
                list.Add(item);
            }
            if (list.Count > 0)
            {
                float y = 0f;
                for (int i = 0; i < list.Count; i++)
                {
                    UguiNovelTextLine line2 = list[i];
                    float num3 = y;
                    y -= line2.MaxFontSize;
                    line2.IsOver = this.IsOverMaxHeight(-y);
                    if (!line2.IsOver)
                    {
                        this.height = -y;
                    }
                    this.PreferredHeight = -y;
                    line2.SetLineY(y, this);
                    y = num3 - line2.TotalHeight;
                }
            }
            return list;
        }

        public List<UIVertex> CreateVertex()
        {
            if (this.IsChangedAll || this.IsRebuidFont)
            {
                if (this.isDebugLog && this.IsRebuidFont)
                {
                    Debug.LogError("Refresh on CreateVertex");
                }
                this.Refresh();
            }
            else
            {
                this.UpdateGraphicObjectList(this.lineDataList);
            }
            this.ClearCurrentChangeType();
            this.MakeVerts(this.lineDataList);
            List<UIVertex> list = this.CreateVertexList(this.lineDataList, this.CurrentLengthOfView);
            this.RefreshHitArea();
            return list;
        }

        private List<UIVertex> CreateVertexList(List<UguiNovelTextLine> lineList, int max)
        {
            List<UIVertex> verts = new List<UIVertex>();
            if ((lineList != null) && ((max > 0) || (lineList.Count > 0)))
            {
                int num = 0;
                UguiNovelTextCharacter character = null;
                foreach (UguiNovelTextLine line in lineList)
                {
                    if (line.IsOver)
                    {
                        break;
                    }
                    for (int i = 0; i < line.Characters.Count; i++)
                    {
                        UguiNovelTextCharacter character2 = line.Characters[i];
                        character2.IsVisible = num < max;
                        num++;
                        if (!character2.IsBr && character2.IsVisible)
                        {
                            character = character2;
                            if (!character2.IsNoFontData)
                            {
                                verts.AddRange(character2.Verts);
                            }
                            this.endPosition = new Vector3(character.EndPositionX, line.Y0, 0f);
                        }
                    }
                }
                this.Additional.AddVerts(verts, this.endPosition, this);
            }
            return verts;
        }

        private void FontTextureRebuildCallback()
        {
            if (this.isDebugLog)
            {
                Debug.LogError("FontTextureRebuildCallback");
            }
        }

        private void FontTextureRebuildCallback(Font font)
        {
            this.FontTextureRebuildCallback();
        }

        private void ForceUpdate()
        {
            if (this.IsChangedAll)
            {
                this.ClearGraphicObjectList();
                this.Refresh();
                this.ClearCurrentChangeType();
            }
            this.UpdateGraphicObjectList(this.lineDataList);
        }

        private int GetAutoLineBreakIndex(List<UguiNovelTextCharacter> characterList, int beginIndex, int index)
        {
            if (index <= beginIndex)
            {
                return index;
            }
            UguiNovelTextCharacter current = characterList[index];
            UguiNovelTextCharacter prev = characterList[index - 1];
            if (prev.IsBrOrAutoBr)
            {
                return index;
            }
            if (this.CheckWordWrap(current, prev))
            {
                int num = this.ParseWordWrap(characterList, beginIndex, index - 1);
                if (num != beginIndex)
                {
                    return num;
                }
            }
            return --index;
        }

        private void InitFontCharactes(Font font, List<UguiNovelTextCharacter> characterDataList)
        {
            bool flag = false;
            int num = 5;
            for (int i = 0; i < num; i++)
            {
                if (this.TryeSetFontCharcters(font, characterDataList))
                {
                    flag = true;
                    break;
                }
                this.RequestCharactersInTexture(font, characterDataList);
                if (i == (num - 1))
                {
                    this.SetFontCharcters(font, characterDataList);
                }
            }
            if (this.isDebugLog && !flag)
            {
                Debug.LogError("InitFontCharactes Error");
                this.TryeSetFontCharcters(font, characterDataList);
            }
        }

        private bool IsAutoIndentation(char character)
        {
            return ((this.TextSettings != null) && this.TextSettings.IsAutoIndentation(character));
        }

        private bool IsOverMaxHeight(float height)
        {
            return (height > this.MaxHeight);
        }

        private bool IsOverMaxWidth(float x, float width)
        {
            return ((x > 0f) && ((x + width) > this.MaxWidth));
        }

        private void LateUpdate()
        {
            this.ForceUpdate();
        }

        private void MakeHitGroups(List<UguiNovelTextCharacter> characterDataList)
        {
            this.hitGroupLists = new List<UguiNovelTextHitArea>();
            int num = 0;
            while (num < characterDataList.Count)
            {
                UguiNovelTextCharacter character = characterDataList[num];
                if (character.charData.CustomInfo.IsHitEventTop)
                {
                    CharData.HitEventType hitEventType = character.CustomInfo.HitEventType;
                    string hitEventArg = character.CustomInfo.HitEventArg;
                    List<UguiNovelTextCharacter> characters = new List<UguiNovelTextCharacter> {
                        character
                    };
                    num++;
                    while (num < characterDataList.Count)
                    {
                        UguiNovelTextCharacter item = characterDataList[num];
                        if (!item.CustomInfo.IsHitEvent || item.CustomInfo.IsHitEventTop)
                        {
                            break;
                        }
                        characters.Add(item);
                        num++;
                    }
                    this.hitGroupLists.Add(new UguiNovelTextHitArea(this.NovelText, hitEventType, hitEventArg, characters));
                    continue;
                }
                num++;
            }
        }

        private List<RequestCharactersInfo> MakeRequestCharactersInfoList(List<UguiNovelTextCharacter> characterDataList)
        {
            List<RequestCharactersInfo> infoList = new List<RequestCharactersInfo>();
            foreach (UguiNovelTextCharacter character in characterDataList)
            {
                this.AddToRequestCharactersInfoList(character, infoList);
            }
            foreach (UguiNovelTextCharacter character2 in this.Additional.MakeCharacterList())
            {
                this.AddToRequestCharactersInfoList(character2, infoList);
            }
            return infoList;
        }

        private void MakeVerts(List<UguiNovelTextLine> lineList)
        {
            Color defaultColor = this.NovelText.get_color();
            foreach (UguiNovelTextLine line in lineList)
            {
                foreach (UguiNovelTextCharacter character in line.Characters)
                {
                    character.MakeVerts(defaultColor, this);
                }
            }
            this.Additional.MakeVerts(defaultColor, this);
        }

        private void OnEnable()
        {
            this.ForceUpdate();
        }

        private int ParseWordWrap(List<UguiNovelTextCharacter> infoList, int beginIndex, int index)
        {
            if (index <= beginIndex)
            {
                return beginIndex;
            }
            UguiNovelTextCharacter current = infoList[index];
            UguiNovelTextCharacter prev = infoList[index - 1];
            if (this.CheckWordWrap(current, prev))
            {
                return this.ParseWordWrap(infoList, beginIndex, index - 1);
            }
            return (index - 1);
        }

        private void Refresh()
        {
            if (this.isRequestingCharactersInTexture)
            {
                if (this.isDebugLog)
                {
                    Debug.LogError("RequestingCharactersInTexture on Refresh");
                }
            }
            else
            {
                this.textData = new Utage.TextData(this.NovelText.get_text());
                if (this.isDebugLog)
                {
                    Debug.Log(this.textData.ParsedText.OriginalText);
                }
                Rect rect = this.CachedRectTransform.get_rect();
                this.maxWidth = Mathf.Abs(rect.get_width());
                this.maxHeight = Mathf.Abs(rect.get_height());
                List<UguiNovelTextCharacter> characters = this.CreateCharacterDataList();
                this.additional = new UguiNovelTextGeneratorAdditional(characters, this);
                this.InitFontCharactes(this.NovelText.get_font(), characters);
                this.Additional.InitAfterCharacterInfo(this);
                this.AutoLineBreak(characters);
                this.lineDataList = this.CreateLineList(characters);
                this.ApplyTextAnchor(this.lineDataList, this.NovelText.get_alignment());
                this.ApplyOffset(this.lineDataList);
                this.Additional.InitPosition(this);
                this.MakeHitGroups(characters);
                this.isInitGraphicObjectList = false;
                this.IsRebuidFont = false;
            }
        }

        internal void RefreshEndPosition()
        {
            int currentLengthOfView = this.CurrentLengthOfView;
            if ((this.LineDataList != null) && ((currentLengthOfView > 0) || (this.LineDataList.Count > 0)))
            {
                int num2 = 0;
                UguiNovelTextCharacter character = null;
                foreach (UguiNovelTextLine line in this.LineDataList)
                {
                    if (line.IsOver)
                    {
                        break;
                    }
                    for (int i = 0; i < line.Characters.Count; i++)
                    {
                        UguiNovelTextCharacter character2 = line.Characters[i];
                        character2.IsVisible = num2 < currentLengthOfView;
                        num2++;
                        if (!character2.IsBr && character2.IsVisible)
                        {
                            character = character2;
                            this.endPosition = new Vector3(character.EndPositionX, line.Y0, 0f);
                        }
                    }
                }
            }
        }

        private void RefreshHitArea()
        {
            foreach (UguiNovelTextHitArea area in this.hitGroupLists)
            {
                area.RefreshHitAreaList();
            }
        }

        private void RequestCharactersInTexture(Font font, List<UguiNovelTextCharacter> characterDataList)
        {
            List<RequestCharactersInfo> list = this.MakeRequestCharactersInfoList(characterDataList);
            this.isRequestingCharactersInTexture = true;
            Font.add_textureRebuilt(new Action<Font>(this.FontTextureRebuildCallback));
            foreach (RequestCharactersInfo info in list)
            {
                font.RequestCharactersInTexture(info.characters, info.size, info.style);
            }
            Font.remove_textureRebuilt(new Action<Font>(this.FontTextureRebuildCallback));
            this.isRequestingCharactersInTexture = false;
        }

        public void SetAllDirty()
        {
            this.NovelText.SetAllDirty();
        }

        private void SetFontCharcters(Font font, List<UguiNovelTextCharacter> characterDataList)
        {
            foreach (UguiNovelTextCharacter character in characterDataList)
            {
                character.SetCharacterInfo(font);
            }
        }

        public void SetVerticesOnlyDirty()
        {
            ChagneType currentChangeType = this.CurrentChangeType;
            this.NovelText.SetVerticesDirty();
            if (currentChangeType != ChagneType.All)
            {
                this.CurrentChangeType = ChagneType.VertexOnly;
            }
        }

        private bool TryeSetFontCharcters(Font font, List<UguiNovelTextCharacter> characterDataList)
        {
            foreach (UguiNovelTextCharacter character in characterDataList)
            {
                if (!character.TrySetCharacterInfo(font))
                {
                    return false;
                }
            }
            return this.Additional.TrySetFontCharcters(font);
        }

        private void UpdateGraphicObjectList(List<UguiNovelTextLine> lineList)
        {
            if (!this.isInitGraphicObjectList)
            {
                this.ClearGraphicObjectList();
                this.graphicObjectList = new List<GraphicObject>();
                foreach (UguiNovelTextLine line in lineList)
                {
                    foreach (UguiNovelTextCharacter character in line.Characters)
                    {
                        RectTransform graphic = character.AddGraphicObject(this.CachedRectTransform, this);
                        if (graphic != null)
                        {
                            this.graphicObjectList.Add(new GraphicObject(character, graphic));
                        }
                    }
                }
                this.isInitGraphicObjectList = true;
            }
            foreach (GraphicObject obj2 in this.graphicObjectList)
            {
                obj2.graphic.get_gameObject().SetActive(obj2.character.IsVisible);
            }
        }

        public UguiNovelTextGeneratorAdditional Additional
        {
            get
            {
                return this.additional;
            }
        }

        public int BmpFontSize
        {
            get
            {
                if (((this.NovelText.get_font() != null) && !this.NovelText.get_font().get_dynamic()) && (this.bmpFontSize <= 0))
                {
                    Debug.LogError("bmpFontSize is zero", this);
                    return 1;
                }
                return this.bmpFontSize;
            }
        }

        public RectTransform CachedRectTransform
        {
            get
            {
                if (this.cachedRectTransform == null)
                {
                    this.cachedRectTransform = base.GetComponent<RectTransform>();
                }
                return this.cachedRectTransform;
            }
        }

        private ChagneType CurrentChangeType { get; set; }

        private int CurrentLengthOfView
        {
            get
            {
                return ((this.LengthOfView >= 0) ? this.LengthOfView : this.textData.Length);
            }
        }

        public char DashChar
        {
            get
            {
                return ((this.dashChar != '\0') ? this.dashChar : '—');
            }
        }

        public UguiNovelTextEmojiData EmojiData
        {
            get
            {
                return this.emojiData;
            }
            set
            {
                this.emojiData = value;
                this.SetAllDirty();
            }
        }

        public Vector3 EndPosition
        {
            get
            {
                return this.endPosition;
            }
        }

        private float Height
        {
            get
            {
                return this.height;
            }
        }

        public List<UguiNovelTextHitArea> HitGroupLists
        {
            get
            {
                return this.hitGroupLists;
            }
        }

        private bool IsChangedAll
        {
            get
            {
                return (this.CurrentChangeType == ChagneType.All);
            }
        }

        private bool IsChangedVertexOnly
        {
            get
            {
                return (this.CurrentChangeType == ChagneType.VertexOnly);
            }
        }

        public bool IsRebuidFont { get; set; }

        public bool IsRequestingCharactersInTexture
        {
            get
            {
                return this.isRequestingCharactersInTexture;
            }
        }

        public bool IsUnicodeFont
        {
            get
            {
                return this.isUnicodeFont;
            }
        }

        public int LengthOfView
        {
            get
            {
                return this.lengthOfView;
            }
            set
            {
                if (this.lengthOfView != value)
                {
                    this.lengthOfView = value;
                    this.NovelText.SetVerticesOnlyDirty();
                }
            }
        }

        public float LetterSpaceSize
        {
            get
            {
                return this.letterSpaceSize;
            }
            set
            {
                this.letterSpaceSize = value;
                this.SetAllDirty();
            }
        }

        public List<UguiNovelTextLine> LineDataList
        {
            get
            {
                return this.lineDataList;
            }
        }

        public float MaxHeight
        {
            get
            {
                return this.maxHeight;
            }
        }

        public float MaxWidth
        {
            get
            {
                return this.maxWidth;
            }
        }

        public UguiNovelText NovelText
        {
            get
            {
                if (this.novelText == null)
                {
                }
                return (this.novelText = base.GetComponent<UguiNovelText>());
            }
        }

        public float PreferredHeight
        {
            get
            {
                this.ForceUpdate();
                return this.preferredHeight;
            }
            private set
            {
                this.preferredHeight = value;
            }
        }

        public float RubySizeScale
        {
            get
            {
                return this.rubySizeScale;
            }
            set
            {
                this.rubySizeScale = value;
                this.SetAllDirty();
            }
        }

        public float Space
        {
            get
            {
                return this.space;
            }
            set
            {
                this.space = value;
                this.SetAllDirty();
            }
        }

        public float SupOrSubSizeScale
        {
            get
            {
                return this.supOrSubSizeScale;
            }
            set
            {
                this.supOrSubSizeScale = value;
                this.SetAllDirty();
            }
        }

        public Utage.TextData TextData
        {
            get
            {
                return this.textData;
            }
        }

        public UguiNovelTextSettings TextSettings
        {
            get
            {
                return this.textSettings;
            }
            set
            {
                this.textSettings = value;
                this.SetAllDirty();
            }
        }

        public WordWrap WordWrapType
        {
            get
            {
                return this.wordWrap;
            }
            set
            {
                this.wordWrap = value;
                this.SetAllDirty();
            }
        }

        private enum ChagneType
        {
            None,
            VertexOnly,
            All
        }

        private class GraphicObject
        {
            public UguiNovelTextCharacter character;
            public RectTransform graphic;

            public GraphicObject(UguiNovelTextCharacter character, RectTransform graphic)
            {
                this.character = character;
                this.graphic = graphic;
            }
        }

        internal class RequestCharactersInfo
        {
            public string characters;
            public readonly int size;
            public readonly FontStyle style;

            public RequestCharactersInfo(UguiNovelTextCharacter data)
            {
                this.characters = string.Empty + data.Char;
                this.size = data.FontSize;
                this.style = data.FontStyle;
            }

            public bool TryAddData(UguiNovelTextCharacter data)
            {
                if ((this.size == data.FontSize) && (this.style == data.FontStyle))
                {
                    this.characters = this.characters + data.Char;
                    return true;
                }
                return false;
            }
        }

        [Flags]
        public enum WordWrap
        {
            Default = 1,
            JapaneseKinsoku = 2
        }
    }
}

