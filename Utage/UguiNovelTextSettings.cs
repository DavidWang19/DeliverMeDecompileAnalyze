namespace Utage
{
    using System;
    using UnityEngine;

    public class UguiNovelTextSettings : ScriptableObject
    {
        [SerializeField]
        private string autoIndentation = string.Empty;
        [SerializeField]
        private bool forceIgonreJapaneseKinsoku;
        [SerializeField]
        private string ignoreLetterSpace = "…‒–—―⁓〜〰";
        [SerializeField]
        private string kinsokuEnd = "([{〔〈《「『【〘〖〝‘“｟\x00ab（［｛";
        [SerializeField]
        private string kinsokuTop = ",)]}、〕〉》」』】〙〗〟’”｠\x00bbゝゞーァィゥェォッャュョヮヵヶぁぃぅぇぉっゃゅょゎゕゖㇰㇱㇲㇳㇴㇵㇶㇷㇸㇹㇷ゚ㇺㇻㇼㇽㇾㇿ々〻‐゠–〜～?!‼⁇⁈⁉・:;/。.，）］｝＝？！：；／";
        [SerializeField]
        private string wordWrapSeparator = "!#%&(),-.:<=>?@[]{}";

        private bool CheckKinsokuBurasage(UguiNovelTextCharacter c)
        {
            return false;
        }

        private bool CheckKinsokuEnd(UguiNovelTextCharacter character)
        {
            return (this.kinsokuEnd.IndexOf(character.Char) >= 0);
        }

        private bool CheckKinsokuTop(UguiNovelTextCharacter character)
        {
            return (this.kinsokuTop.IndexOf(character.Char) >= 0);
        }

        internal bool CheckWordWrap(UguiNovelTextGenerator generator, UguiNovelTextCharacter current, UguiNovelTextCharacter prev)
        {
            if (!this.IsIgonreLetterSpace(prev, current))
            {
                bool flag = (generator.WordWrapType & UguiNovelTextGenerator.WordWrap.Default) == UguiNovelTextGenerator.WordWrap.Default;
                bool flag2 = !this.forceIgonreJapaneseKinsoku && ((generator.WordWrapType & UguiNovelTextGenerator.WordWrap.JapaneseKinsoku) == UguiNovelTextGenerator.WordWrap.JapaneseKinsoku);
                if (flag)
                {
                    if (flag2)
                    {
                        if (this.CheckWordWrapDefaultEnd(prev) && this.CheckWordWrapDefaultTop(current))
                        {
                            return true;
                        }
                    }
                    else if (!char.IsSeparator(prev.Char) && !char.IsSeparator(current.Char))
                    {
                        return true;
                    }
                }
                if (!flag2 || (!this.CheckKinsokuEnd(prev) && !this.CheckKinsokuTop(current)))
                {
                    return false;
                }
            }
            return true;
        }

        private bool CheckWordWrapDefaultEnd(UguiNovelTextCharacter character)
        {
            char c = character.Char;
            return ((UtageToolKit.IsHankaku(c) && !char.IsSeparator(c)) && (this.wordWrapSeparator.IndexOf(c) < 0));
        }

        private bool CheckWordWrapDefaultTop(UguiNovelTextCharacter character)
        {
            char c = character.Char;
            return (UtageToolKit.IsHankaku(c) && !char.IsSeparator(c));
        }

        internal bool IsAutoIndentation(char character)
        {
            return (this.autoIndentation.IndexOf(character) >= 0);
        }

        internal bool IsIgonreLetterSpace(UguiNovelTextCharacter current, UguiNovelTextCharacter next)
        {
            if ((current == null) || (next == null))
            {
                return false;
            }
            return ((current.Char == next.Char) && (this.IgnoreLetterSpace.IndexOf(current.Char) >= 0));
        }

        internal string AutoIndentation
        {
            get
            {
                return this.autoIndentation;
            }
        }

        internal string IgnoreLetterSpace
        {
            get
            {
                return this.ignoreLetterSpace;
            }
        }

        internal string KinsokuEnd
        {
            get
            {
                return this.kinsokuEnd;
            }
        }

        internal string KinsokuTop
        {
            get
            {
                return this.kinsokuTop;
            }
        }

        internal string WordWrapSeparator
        {
            get
            {
                return this.wordWrapSeparator;
            }
        }
    }
}

