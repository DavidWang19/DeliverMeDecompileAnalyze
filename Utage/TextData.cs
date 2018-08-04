namespace Utage
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class TextData
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <ContainsSpeedTag>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <IsNoWaitAll>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private TextParser <ParsedText>k__BackingField;
        private const string BoldEndTag = "</b>";
        private const string ColorEndTag = "</color>";
        private const string ItalicEndTag = "</i>";
        private const string SizeEndTag = "</size>";
        private string unityRitchText;

        public TextData(string text)
        {
            this.ParsedText = new TextParser(text, false);
            this.IsNoWaitAll = true;
            foreach (CharData data in this.ParsedText.CharList)
            {
                if (data.CustomInfo.IsSpeed)
                {
                    this.ContainsSpeedTag = true;
                }
                if (!data.CustomInfo.IsSpeed || (data.CustomInfo.speed != 0f))
                {
                    this.IsNoWaitAll = false;
                }
            }
        }

        public void InitUnityRitchText()
        {
            if (string.IsNullOrEmpty(this.unityRitchText))
            {
                this.unityRitchText = string.Empty;
                CharData.CustomCharaInfo lastCustomInfo = new CharData.CustomCharaInfo();
                Stack<string> stack = new Stack<string>();
                for (int i = 0; i < this.CharList.Count; i++)
                {
                    CharData data = this.CharList[i];
                    if (data.CustomInfo.IsEndBold(ref lastCustomInfo))
                    {
                        this.unityRitchText = this.unityRitchText + stack.Pop();
                    }
                    if (data.CustomInfo.IsEndItalic(ref lastCustomInfo))
                    {
                        this.unityRitchText = this.unityRitchText + stack.Pop();
                    }
                    if (data.CustomInfo.IsEndSize(ref lastCustomInfo))
                    {
                        this.unityRitchText = this.unityRitchText + stack.Pop();
                    }
                    if (data.CustomInfo.IsEndColor(ref lastCustomInfo))
                    {
                        this.unityRitchText = this.unityRitchText + stack.Pop();
                    }
                    if (data.CustomInfo.IsBeginColor(ref lastCustomInfo))
                    {
                        this.unityRitchText = this.unityRitchText + "<color=" + data.CustomInfo.colorStr + ">";
                        stack.Push("</color>");
                    }
                    if (data.CustomInfo.IsBeginSize(ref lastCustomInfo))
                    {
                        string unityRitchText = this.unityRitchText;
                        object[] objArray1 = new object[] { unityRitchText, "<size=", data.CustomInfo.size, ">" };
                        this.unityRitchText = string.Concat(objArray1);
                        stack.Push("</size>");
                    }
                    if (data.CustomInfo.IsBeginItalic(ref lastCustomInfo))
                    {
                        this.unityRitchText = this.unityRitchText + "<i>";
                        stack.Push("</i>");
                    }
                    if (data.CustomInfo.IsBeginBold(ref lastCustomInfo))
                    {
                        this.unityRitchText = this.unityRitchText + "<b>";
                        stack.Push("</b>");
                    }
                    data.UnityRitchTextIndex = this.unityRitchText.Length;
                    this.unityRitchText = this.unityRitchText + data.Char;
                    if (data.CustomInfo.IsDoubleWord)
                    {
                        this.unityRitchText = this.unityRitchText + " ";
                    }
                    lastCustomInfo = data.CustomInfo;
                }
                if (lastCustomInfo.IsBold)
                {
                    this.unityRitchText = this.unityRitchText + stack.Pop();
                }
                if (lastCustomInfo.IsItalic)
                {
                    this.unityRitchText = this.unityRitchText + stack.Pop();
                }
                if (lastCustomInfo.IsSize)
                {
                    this.unityRitchText = this.unityRitchText + stack.Pop();
                }
                if (lastCustomInfo.IsColor)
                {
                    this.unityRitchText = this.unityRitchText + stack.Pop();
                }
            }
        }

        public List<CharData> CharList
        {
            get
            {
                return this.ParsedText.CharList;
            }
        }

        public bool ContainsSpeedTag { get; protected set; }

        public string ErrorMsg
        {
            get
            {
                return this.ParsedText.ErrorMsg;
            }
        }

        public bool IsNoWaitAll { get; protected set; }

        public int Length
        {
            get
            {
                return this.CharList.Count;
            }
        }

        public string NoneMetaString
        {
            get
            {
                return this.ParsedText.NoneMetaString;
            }
        }

        public string OriginalText
        {
            get
            {
                return this.ParsedText.OriginalText;
            }
        }

        public TextParser ParsedText { get; private set; }

        public string UnityRitchText
        {
            get
            {
                this.InitUnityRitchText();
                return this.unityRitchText;
            }
        }
    }
}

