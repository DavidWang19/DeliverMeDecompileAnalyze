namespace Utage
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class CharData
    {
        private char c;
        private CustomCharaInfo customInfo;
        public const char Dash = '—';
        private int unityRitchTextIndex = -1;

        public CharData(char c, CustomCharaInfo customInfo)
        {
            this.c = c;
            this.customInfo = customInfo;
        }

        internal bool TryParseInterval(string arg)
        {
            return this.customInfo.TryParseInterval(arg);
        }

        public char Char
        {
            get
            {
                return this.c;
            }
            set
            {
                this.c = value;
            }
        }

        public CustomCharaInfo CustomInfo
        {
            get
            {
                return this.customInfo;
            }
        }

        public bool IsBr
        {
            get
            {
                return ((this.Char == '\n') || (this.Char == '\r'));
            }
        }

        public int UnityRitchTextIndex
        {
            get
            {
                return this.unityRitchTextIndex;
            }
            set
            {
                this.unityRitchTextIndex = value;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct CustomCharaInfo
        {
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private bool <IsColor>k__BackingField;
            public Color color;
            public string colorStr;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private bool <IsSize>k__BackingField;
            public int size;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private bool <IsBold>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private bool <IsItalic>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private bool <IsDoubleWord>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private bool <IsRubyTop>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private bool <IsRuby>k__BackingField;
            public string rubyStr;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private bool <IsEmphasisMark>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private bool <IsSuperScript>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private bool <IsSubScript>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private bool <IsUnderLineTop>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private bool <IsUnderLine>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private bool <IsStrikeTop>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private bool <IsStrike>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private bool <IsGroupTop>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private bool <IsGroup>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private bool <IsEmoji>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private string <EmojiKey>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private bool <IsDash>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private int <DashSize>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private bool <IsSpace>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private int <SpaceSize>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private bool <IsSpeed>k__BackingField;
            public float speed;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private bool <IsInterval>k__BackingField;
            public float Interval;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private bool <IsHitEventTop>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private bool <IsHitEvent>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private string <HitEventArg>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private Utage.CharData.HitEventType <HitEventType>k__BackingField;
            public bool IsColor { get; set; }
            public bool IsSize { get; set; }
            public bool IsBold { get; set; }
            public bool IsItalic { get; set; }
            public bool IsDoubleWord { get; set; }
            public bool IsRubyTop { get; set; }
            public bool IsRuby { get; set; }
            public bool IsEmphasisMark { get; set; }
            public bool IsSuperScript { get; set; }
            public bool IsSubScript { get; set; }
            public bool IsSuperOrSubScript
            {
                get
                {
                    return (this.IsSuperScript || this.IsSubScript);
                }
            }
            public bool IsUnderLineTop { get; set; }
            public bool IsUnderLine { get; set; }
            public bool IsStrikeTop { get; set; }
            public bool IsStrike { get; set; }
            public bool IsGroupTop { get; set; }
            public bool IsGroup { get; set; }
            public bool IsEmoji { get; set; }
            public string EmojiKey { get; set; }
            public bool IsDash { get; set; }
            public int DashSize { get; set; }
            public bool IsSpace { get; set; }
            public int SpaceSize { get; set; }
            public bool IsSpeed { get; set; }
            public bool IsInterval { get; set; }
            public bool IsHitEventTop { get; set; }
            public bool IsHitEvent { get; set; }
            public string HitEventArg { get; set; }
            public Utage.CharData.HitEventType HitEventType { get; set; }
            public bool TryParseBold(string arg)
            {
                bool flag = true;
                this.IsBold = flag;
                return flag;
            }

            public void ResetBold()
            {
                this.IsBold = false;
            }

            public bool TryParseItalic(string arg)
            {
                bool flag = true;
                this.IsItalic = flag;
                return flag;
            }

            public void ResetItalic()
            {
                this.IsItalic = false;
            }

            public bool TryParseSize(string arg)
            {
                bool flag = int.TryParse(arg, out this.size);
                this.IsSize = flag;
                return flag;
            }

            public void ResetSize()
            {
                this.IsSize = false;
                this.size = 0;
            }

            public bool TryParseColor(string arg)
            {
                this.IsColor = ColorUtil.TryParseColor(arg, ref this.color);
                if (this.IsColor)
                {
                    this.colorStr = arg;
                }
                return this.IsColor;
            }

            public void ResetColor()
            {
                this.IsColor = false;
                this.color = Color.get_white();
            }

            public bool TryParseRuby(string arg)
            {
                if (string.IsNullOrEmpty(arg))
                {
                    return false;
                }
                bool flag = true;
                this.IsRuby = flag;
                this.IsRubyTop = flag;
                this.rubyStr = arg;
                return true;
            }

            public void ResetRuby()
            {
                this.IsRuby = false;
                this.rubyStr = string.Empty;
            }

            public bool TryParseEmphasisMark(string arg)
            {
                if (string.IsNullOrEmpty(arg))
                {
                    return false;
                }
                this.rubyStr = arg;
                if (this.rubyStr.Length != 1)
                {
                    return false;
                }
                bool flag = true;
                this.IsEmphasisMark = flag;
                this.IsRuby = flag;
                this.IsRubyTop = flag;
                return true;
            }

            public void ResetEmphasisMark()
            {
                bool flag = false;
                this.IsEmphasisMark = flag;
                this.IsRuby = flag;
                this.rubyStr = string.Empty;
            }

            public bool TryParseSuperScript(string arg)
            {
                this.IsSuperScript = true;
                return true;
            }

            public void ResetSuperScript()
            {
                this.IsSuperScript = false;
            }

            public bool TryParseSubScript(string arg)
            {
                this.IsSubScript = true;
                return true;
            }

            public void ResetSubScript()
            {
                this.IsSubScript = false;
            }

            public bool TryParseUnderLine(string arg)
            {
                bool flag = true;
                this.IsUnderLine = flag;
                this.IsUnderLineTop = flag;
                return true;
            }

            public void ResetUnderLine()
            {
                this.IsUnderLine = false;
            }

            public bool TryParseStrike(string arg)
            {
                bool flag = true;
                this.IsStrike = flag;
                this.IsStrikeTop = flag;
                return true;
            }

            public void ResetStrike()
            {
                this.IsStrike = false;
            }

            public bool TryParseGroup(string arg)
            {
                bool flag = true;
                this.IsGroup = flag;
                this.IsGroupTop = flag;
                return true;
            }

            public void ResetGroup()
            {
                this.IsGroup = false;
            }

            public bool TryParseLink(string arg)
            {
                bool flag = true;
                this.IsHitEvent = flag;
                this.IsHitEventTop = flag;
                this.HitEventArg = arg;
                this.HitEventType = Utage.CharData.HitEventType.Link;
                return true;
            }

            public void ResetLink()
            {
                this.IsHitEvent = false;
            }

            public bool TryParseTips(string arg)
            {
                bool flag = true;
                this.IsHitEvent = flag;
                this.IsHitEventTop = flag;
                this.HitEventArg = arg;
                this.HitEventType = Utage.CharData.HitEventType.Tips;
                return true;
            }

            public void ResetTips()
            {
                this.IsHitEvent = false;
            }

            internal bool TryParseSound(string arg)
            {
                bool flag = true;
                this.IsHitEvent = flag;
                this.IsHitEventTop = flag;
                this.HitEventArg = arg;
                this.HitEventType = Utage.CharData.HitEventType.Sound;
                return true;
            }

            internal void ResetSound()
            {
                this.IsHitEvent = false;
            }

            internal bool TryParseSpeed(string arg)
            {
                bool flag = float.TryParse(arg, out this.speed);
                this.IsSpeed = flag;
                return flag;
            }

            internal void ResetSpeed()
            {
                this.IsSpeed = false;
                this.speed = 0f;
            }

            internal bool TryParseInterval(string arg)
            {
                bool flag = float.TryParse(arg, out this.Interval);
                this.IsInterval = flag;
                return flag;
            }

            public bool IsEndBold(ref CharData.CustomCharaInfo lastCustomInfo)
            {
                if (!lastCustomInfo.IsBold)
                {
                    return false;
                }
                return !this.IsBold;
            }

            public bool IsBeginBold(ref CharData.CustomCharaInfo lastCustomInfo)
            {
                if (!this.IsBold)
                {
                    return false;
                }
                return !lastCustomInfo.IsBold;
            }

            public bool IsEndItalic(ref CharData.CustomCharaInfo lastCustomInfo)
            {
                if (!lastCustomInfo.IsItalic)
                {
                    return false;
                }
                return !this.IsItalic;
            }

            public bool IsBeginItalic(ref CharData.CustomCharaInfo lastCustomInfo)
            {
                if (!this.IsItalic)
                {
                    return false;
                }
                return !lastCustomInfo.IsItalic;
            }

            public bool IsEndSize(ref CharData.CustomCharaInfo lastCustomInfo)
            {
                if (!lastCustomInfo.IsSize)
                {
                    return false;
                }
                return (!this.IsSize || (lastCustomInfo.size != this.size));
            }

            public bool IsBeginSize(ref CharData.CustomCharaInfo lastCustomInfo)
            {
                if (!this.IsSize)
                {
                    return false;
                }
                return (!lastCustomInfo.IsSize || (lastCustomInfo.size != this.size));
            }

            public bool IsEndColor(ref CharData.CustomCharaInfo lastCustomInfo)
            {
                if (!lastCustomInfo.IsColor)
                {
                    return false;
                }
                return (!this.IsColor || (lastCustomInfo.color != this.color));
            }

            public bool IsBeginColor(ref CharData.CustomCharaInfo lastCustomInfo)
            {
                if (!this.IsColor)
                {
                    return false;
                }
                return (!lastCustomInfo.IsColor || (lastCustomInfo.color != this.color));
            }

            public int GetCustomedSize(int defaultSize)
            {
                return (!this.IsSize ? defaultSize : this.size);
            }

            public FontStyle GetCustomedStyle(FontStyle defaultFontStyle)
            {
                if (this.IsItalic && this.IsBold)
                {
                    return 3;
                }
                if (this.IsItalic)
                {
                    return 2;
                }
                if (this.IsBold)
                {
                    return 1;
                }
                return defaultFontStyle;
            }

            public Color GetCustomedColor(Color defaultColor)
            {
                return (!this.IsColor ? defaultColor : this.color);
            }

            public void ClearOnNextChar()
            {
                this.IsRubyTop = false;
                this.IsUnderLineTop = false;
                this.IsStrikeTop = false;
                this.IsHitEventTop = false;
                this.IsGroupTop = false;
                this.rubyStr = string.Empty;
            }
        }

        public enum HitEventType
        {
            Sound,
            Link,
            Tips
        }
    }
}

