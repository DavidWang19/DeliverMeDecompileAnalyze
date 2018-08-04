namespace Utage
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class UguiNovelTextHitArea
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <Arg>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Utage.CharData.HitEventType <HitEventType>k__BackingField;
        private List<UguiNovelTextCharacter> characters = new List<UguiNovelTextCharacter>();
        private List<Rect> hitAreaList = new List<Rect>();
        private UguiNovelText novelText;

        public UguiNovelTextHitArea(UguiNovelText novelText, Utage.CharData.HitEventType type, string arg, List<UguiNovelTextCharacter> characters)
        {
            this.novelText = novelText;
            this.HitEventType = type;
            this.Arg = arg;
            this.characters = characters;
        }

        internal void ChangeEffectColor(Color effectColor)
        {
            foreach (UguiNovelTextCharacter character in this.characters)
            {
                character.ChangeEffectColor(effectColor);
            }
            this.novelText.SetVerticesOnlyDirty();
        }

        internal bool HitTest(Vector2 localPosition)
        {
            foreach (Rect rect in this.hitAreaList)
            {
                if (rect.Contains(localPosition))
                {
                    return true;
                }
            }
            return false;
        }

        private Rect MakeHitArea(List<UguiNovelTextCharacter> lineCharacters)
        {
            UguiNovelTextCharacter character = lineCharacters[0];
            float beginPositionX = character.BeginPositionX;
            float endPositionX = character.EndPositionX;
            int fontSize = 0;
            foreach (UguiNovelTextCharacter character2 in lineCharacters)
            {
                endPositionX = Mathf.Max(endPositionX, character2.EndPositionX);
                fontSize = Mathf.Max(fontSize, character2.FontSize);
            }
            int totalLineHeight = this.novelText.GetTotalLineHeight(fontSize);
            return new Rect(beginPositionX, character.PositionY - (((float) (totalLineHeight - fontSize)) / 2f), endPositionX - beginPositionX, (float) totalLineHeight);
        }

        public void RefreshHitAreaList()
        {
            this.hitAreaList.Clear();
            List<UguiNovelTextCharacter> lineCharacters = new List<UguiNovelTextCharacter>();
            foreach (UguiNovelTextCharacter character in this.characters)
            {
                if (!character.IsBr && character.IsVisible)
                {
                    lineCharacters.Add(character);
                }
                if (character.IsBrOrAutoBr)
                {
                    if (lineCharacters.Count > 0)
                    {
                        this.hitAreaList.Add(this.MakeHitArea(lineCharacters));
                    }
                    lineCharacters.Clear();
                }
            }
            if (lineCharacters.Count > 0)
            {
                this.hitAreaList.Add(this.MakeHitArea(lineCharacters));
            }
        }

        internal void ResetEffectColor()
        {
            foreach (UguiNovelTextCharacter character in this.characters)
            {
                character.ResetEffectColor();
            }
            this.novelText.SetVerticesOnlyDirty();
        }

        public string Arg { get; private set; }

        public List<Rect> HitAreaList
        {
            get
            {
                return this.hitAreaList;
            }
        }

        public Utage.CharData.HitEventType HitEventType { get; private set; }

        public enum Type
        {
            Link,
            Sound
        }
    }
}

