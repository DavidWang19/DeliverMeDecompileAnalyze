namespace Utage
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class UguiNovelTextLine
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private float <Y0>k__BackingField;
        private List<UguiNovelTextCharacter> characters = new List<UguiNovelTextCharacter>();
        private bool isOver;
        private int maxFontSize;
        private List<UguiNovelTextCharacter> rubyCharacters = new List<UguiNovelTextCharacter>();
        private float totalHeight;
        private float width;

        public void AddCharaData(UguiNovelTextCharacter charaData)
        {
            this.characters.Add(charaData);
        }

        public void ApplyOffset(Vector2 offset)
        {
            this.Y0 += offset.y;
            foreach (UguiNovelTextCharacter character in this.characters)
            {
                character.ApplyOffsetX(offset.x);
                character.ApplyOffsetY(offset.y);
            }
        }

        public void ApplyTextAnchorX(float pivotX, float maxWidth)
        {
            if (pivotX != 0f)
            {
                float offsetX = (maxWidth - this.Width) * pivotX;
                foreach (UguiNovelTextCharacter character in this.characters)
                {
                    character.ApplyOffsetX(offsetX);
                }
            }
        }

        public void ApplyTextAnchorY(float offsetY)
        {
            this.Y0 += offsetY;
            foreach (UguiNovelTextCharacter character in this.characters)
            {
                character.ApplyOffsetY(offsetY);
            }
        }

        public void EndCharaData(UguiNovelTextGenerator generator)
        {
            this.maxFontSize = 0;
            float num = 0f;
            for (int i = 0; i < this.characters.Count; i++)
            {
                UguiNovelTextCharacter character = this.characters[i];
                this.maxFontSize = Mathf.Max(this.maxFontSize, character.DefaultFontSize);
                if (i == 0)
                {
                    num = character.PositionX - character.RubySpaceSize;
                }
            }
            float num3 = 0f;
            for (int j = this.characters.Count - 1; j >= 0; j--)
            {
                UguiNovelTextCharacter character2 = this.characters[j];
                if (!character2.IsBr)
                {
                    num3 = (character2.PositionX + character2.Width) + character2.RubySpaceSize;
                    break;
                }
            }
            this.width = Mathf.Abs(num3 - num);
            this.totalHeight = generator.NovelText.GetTotalLineHeight(this.MaxFontSize);
        }

        public void SetLineY(float y, UguiNovelTextGenerator generator)
        {
            this.Y0 = y;
            foreach (UguiNovelTextCharacter character in this.characters)
            {
                character.PositionY = this.Y0;
            }
        }

        public List<UguiNovelTextCharacter> Characters
        {
            get
            {
                return this.characters;
            }
        }

        public bool IsOver
        {
            get
            {
                return this.isOver;
            }
            set
            {
                this.isOver = value;
            }
        }

        public int MaxFontSize
        {
            get
            {
                return this.maxFontSize;
            }
        }

        public List<UguiNovelTextCharacter> RubyCharacters
        {
            get
            {
                return this.rubyCharacters;
            }
        }

        public float TotalHeight
        {
            get
            {
                return this.totalHeight;
            }
        }

        public float Width
        {
            get
            {
                return this.width;
            }
        }

        public float Y0 { get; set; }
    }
}

