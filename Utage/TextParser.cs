namespace Utage
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text;

    public class TextParser
    {
        [CompilerGenerated]
        private static Dictionary<string, int> <>f__switch$mapC;
        public static Func<string, object> CallbackCalcExpression;
        private List<CharData> charList = new List<CharData>();
        private int currentTextIndex;
        private CharData.CustomCharaInfo customInfo = new CharData.CustomCharaInfo();
        private string errorMsg;
        private bool isParseParamOnly;
        private string noneMetaString;
        private string originalText;
        public const string TagSound = "sound";
        public const string TagSpeed = "speed";
        public const string TagUnderLine = "u";

        public TextParser(string text, bool isParseParamOnly = false)
        {
            this.originalText = text;
            this.isParseParamOnly = isParseParamOnly;
            this.Parse();
        }

        private void AddChar(char c)
        {
            CharData item = new CharData(c, this.customInfo);
            this.charList.Add(item);
            this.customInfo.ClearOnNextChar();
        }

        private void AddDash(string arg)
        {
            int num;
            if (!int.TryParse(arg, out num))
            {
                num = 1;
            }
            CharData.CustomCharaInfo customInfo = this.customInfo;
            customInfo.IsDash = true;
            customInfo.DashSize = num;
            CharData item = new CharData('—', customInfo);
            this.charList.Add(item);
        }

        private void AddDoubleLineBreak()
        {
            CharData.CustomCharaInfo customInfo = this.customInfo;
            customInfo.IsDoubleWord = true;
            CharData item = new CharData('\n', customInfo);
            this.charList.Add(item);
        }

        private void AddErrorMsg(string msg)
        {
            if (string.IsNullOrEmpty(this.errorMsg))
            {
                this.errorMsg = string.Empty;
            }
            else
            {
                this.errorMsg = this.errorMsg + "\n";
            }
            this.errorMsg = this.errorMsg + msg;
        }

        private void AddStrng(string text)
        {
            foreach (char ch in text)
            {
                this.AddChar(ch);
            }
        }

        public static string AddTag(string text, string tag, string arg)
        {
            return string.Format("<{1}={2}>{0}</{1}>", text, tag, arg);
        }

        private string ExpressionToString(string exp)
        {
            if (CallbackCalcExpression == null)
            {
                this.AddErrorMsg(LanguageErrorMsg.LocalizeTextFormat(Utage.ErrorMsg.TextCallbackCalcExpression, new object[0]));
                return string.Empty;
            }
            object obj2 = CallbackCalcExpression(exp);
            if (obj2 == null)
            {
                this.AddErrorMsg(LanguageErrorMsg.LocalizeTextFormat(Utage.ErrorMsg.TextFailedCalcExpression, new object[0]));
                return string.Empty;
            }
            return obj2.ToString();
        }

        private string FormatExpressionToString(string format, string[] exps)
        {
            if (CallbackCalcExpression == null)
            {
                this.AddErrorMsg(LanguageErrorMsg.LocalizeTextFormat(Utage.ErrorMsg.TextCallbackCalcExpression, new object[0]));
                return string.Empty;
            }
            List<object> list = new List<object>();
            foreach (string str in exps)
            {
                list.Add(CallbackCalcExpression(str));
            }
            return string.Format(format, list.ToArray());
        }

        private void InitNoneMetaText()
        {
            if (string.IsNullOrEmpty(this.noneMetaString))
            {
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < this.CharList.Count; i++)
                {
                    builder.Append(this.CharList[i].Char);
                }
                this.noneMetaString = builder.ToString();
            }
        }

        public static string MakeLogText(string text)
        {
            return new TextParser(text, true).NoneMetaString;
        }

        private void Parse()
        {
            try
            {
                int length = this.OriginalText.Length;
                this.currentTextIndex = 0;
                while (this.currentTextIndex < length)
                {
                    if (!this.ParseEscapeSequence())
                    {
                        Func<string, string, bool> func;
                        if (this.isParseParamOnly)
                        {
                            func = new Func<string, string, bool>(this.ParseTagParamOnly);
                        }
                        else
                        {
                            func = new Func<string, string, bool>(this.ParseTag);
                        }
                        int num2 = ParserUtil.ParseTag(this.OriginalText, this.currentTextIndex, func);
                        if (this.currentTextIndex == num2)
                        {
                            this.AddChar(this.OriginalText[this.currentTextIndex]);
                            this.currentTextIndex++;
                        }
                        else
                        {
                            this.currentTextIndex = num2 + 1;
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                this.AddErrorMsg(exception.Message + exception.StackTrace);
            }
        }

        private bool ParseEscapeSequence()
        {
            if ((this.currentTextIndex + 1) < this.OriginalText.Length)
            {
                char ch = this.OriginalText[this.currentTextIndex];
                char ch2 = this.OriginalText[this.currentTextIndex + 1];
                if ((ch == '\\') && (ch2 == 'n'))
                {
                    this.AddDoubleLineBreak();
                    this.currentTextIndex += 2;
                    return true;
                }
                if ((ch == '\r') && (ch2 == '\n'))
                {
                    this.AddDoubleLineBreak();
                    this.currentTextIndex += 2;
                    return true;
                }
            }
            return false;
        }

        private bool ParseTag(string name, string arg)
        {
            if (name != null)
            {
                int num;
                if (<>f__switch$mapC == null)
                {
                    Dictionary<string, int> dictionary = new Dictionary<string, int>(0x24);
                    dictionary.Add("b", 0);
                    dictionary.Add("/b", 1);
                    dictionary.Add("i", 2);
                    dictionary.Add("/i", 3);
                    dictionary.Add("color", 4);
                    dictionary.Add("/color", 5);
                    dictionary.Add("size", 6);
                    dictionary.Add("/size", 7);
                    dictionary.Add("ruby", 8);
                    dictionary.Add("/ruby", 9);
                    dictionary.Add("em", 10);
                    dictionary.Add("/em", 11);
                    dictionary.Add("sup", 12);
                    dictionary.Add("/sup", 13);
                    dictionary.Add("sub", 14);
                    dictionary.Add("/sub", 15);
                    dictionary.Add("u", 0x10);
                    dictionary.Add("/u", 0x11);
                    dictionary.Add("strike", 0x12);
                    dictionary.Add("/strike", 0x13);
                    dictionary.Add("group", 20);
                    dictionary.Add("/group", 0x15);
                    dictionary.Add("emoji", 0x16);
                    dictionary.Add("dash", 0x17);
                    dictionary.Add("space", 0x18);
                    dictionary.Add("param", 0x19);
                    dictionary.Add("link", 0x1a);
                    dictionary.Add("/link", 0x1b);
                    dictionary.Add("tips", 0x1c);
                    dictionary.Add("/tips", 0x1d);
                    dictionary.Add("sound", 30);
                    dictionary.Add("/sound", 0x1f);
                    dictionary.Add("speed", 0x20);
                    dictionary.Add("/speed", 0x21);
                    dictionary.Add("interval", 0x22);
                    dictionary.Add("format", 0x23);
                    <>f__switch$mapC = dictionary;
                }
                if (<>f__switch$mapC.TryGetValue(name, out num))
                {
                    switch (num)
                    {
                        case 0:
                            return this.customInfo.TryParseBold(arg);

                        case 1:
                            this.customInfo.ResetBold();
                            return true;

                        case 2:
                            return this.customInfo.TryParseItalic(arg);

                        case 3:
                            this.customInfo.ResetItalic();
                            return true;

                        case 4:
                            return this.customInfo.TryParseColor(arg);

                        case 5:
                            this.customInfo.ResetColor();
                            return true;

                        case 6:
                            return this.customInfo.TryParseSize(arg);

                        case 7:
                            this.customInfo.ResetSize();
                            return true;

                        case 8:
                            return this.customInfo.TryParseRuby(arg);

                        case 9:
                            this.customInfo.ResetRuby();
                            return true;

                        case 10:
                            return this.customInfo.TryParseEmphasisMark(arg);

                        case 11:
                            this.customInfo.ResetEmphasisMark();
                            return true;

                        case 12:
                            return this.customInfo.TryParseSuperScript(arg);

                        case 13:
                            this.customInfo.ResetSuperScript();
                            return true;

                        case 14:
                            return this.customInfo.TryParseSubScript(arg);

                        case 15:
                            this.customInfo.ResetSubScript();
                            return true;

                        case 0x10:
                            return this.customInfo.TryParseUnderLine(arg);

                        case 0x11:
                            this.customInfo.ResetUnderLine();
                            return true;

                        case 0x12:
                            return this.customInfo.TryParseStrike(arg);

                        case 0x13:
                            this.customInfo.ResetStrike();
                            return true;

                        case 20:
                            return this.customInfo.TryParseGroup(arg);

                        case 0x15:
                            this.customInfo.ResetGroup();
                            return true;

                        case 0x16:
                            return this.TryAddEmoji(arg);

                        case 0x17:
                            this.AddDash(arg);
                            return true;

                        case 0x18:
                            return this.TryAddSpace(arg);

                        case 0x19:
                        {
                            string text = this.ExpressionToString(arg);
                            this.AddStrng(text);
                            return true;
                        }
                        case 0x1a:
                            return this.customInfo.TryParseLink(arg);

                        case 0x1b:
                            this.customInfo.ResetLink();
                            return true;

                        case 0x1c:
                            return this.customInfo.TryParseTips(arg);

                        case 0x1d:
                            this.customInfo.ResetTips();
                            return true;

                        case 30:
                            return this.customInfo.TryParseSound(arg);

                        case 0x1f:
                            this.customInfo.ResetSound();
                            return true;

                        case 0x20:
                            return this.customInfo.TryParseSpeed(arg);

                        case 0x21:
                            this.customInfo.ResetSpeed();
                            return true;

                        case 0x22:
                            return this.TryAddInterval(arg);

                        case 0x23:
                        {
                            char[] separator = new char[] { ':' };
                            string[] sourceArray = arg.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                            int length = sourceArray.Length - 1;
                            string[] destinationArray = new string[length];
                            Array.Copy(sourceArray, 1, destinationArray, 0, length);
                            string str2 = this.FormatExpressionToString(sourceArray[0], destinationArray);
                            this.AddStrng(str2);
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private bool ParseTagParamOnly(string name, string arg)
        {
            if (name != null)
            {
                if (name == "param")
                {
                    string text = this.ExpressionToString(arg);
                    this.AddStrng(text);
                    return true;
                }
                if (name == "format")
                {
                    char[] separator = new char[] { ':' };
                    string[] sourceArray = arg.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                    int length = sourceArray.Length - 1;
                    string[] destinationArray = new string[length];
                    Array.Copy(sourceArray, 1, destinationArray, 0, length);
                    string str2 = this.FormatExpressionToString(sourceArray[0], destinationArray);
                    this.AddStrng(str2);
                    return true;
                }
            }
            return false;
        }

        private bool TryAddEmoji(string arg)
        {
            if (string.IsNullOrEmpty(arg))
            {
                return false;
            }
            CharData.CustomCharaInfo customInfo = this.customInfo;
            customInfo.IsEmoji = true;
            customInfo.EmojiKey = arg;
            CharData item = new CharData('□', customInfo);
            this.charList.Add(item);
            return true;
        }

        private bool TryAddInterval(string arg)
        {
            if (this.charList.Count <= 0)
            {
                return false;
            }
            return this.charList[this.charList.Count - 1].TryParseInterval(arg);
        }

        private bool TryAddSpace(string arg)
        {
            int num;
            if (!int.TryParse(arg, out num))
            {
                return false;
            }
            CharData.CustomCharaInfo customInfo = this.customInfo;
            customInfo.IsSpace = true;
            customInfo.SpaceSize = num;
            CharData item = new CharData(' ', customInfo);
            this.charList.Add(item);
            return true;
        }

        public List<CharData> CharList
        {
            get
            {
                return this.charList;
            }
        }

        public string ErrorMsg
        {
            get
            {
                return this.errorMsg;
            }
        }

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
                this.InitNoneMetaText();
                return this.noneMetaString;
            }
        }

        public string OriginalText
        {
            get
            {
                return this.originalText;
            }
        }
    }
}

