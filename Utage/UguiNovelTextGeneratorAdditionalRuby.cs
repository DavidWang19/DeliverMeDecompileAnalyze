namespace Utage
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class UguiNovelTextGeneratorAdditionalRuby
    {
        private List<UguiNovelTextCharacter> rubyList = new List<UguiNovelTextCharacter>();
        private float rubyWidth;
        private List<UguiNovelTextCharacter> stringData = new List<UguiNovelTextCharacter>();
        private float stringWidth;
        private UguiNovelTextLine textLine;

        internal UguiNovelTextGeneratorAdditionalRuby(List<UguiNovelTextCharacter> characters, int index, UguiNovelTextGenerator generator)
        {
            Font font = generator.NovelText.get_font();
            float rubySizeScale = generator.RubySizeScale;
            UguiNovelTextCharacter item = characters[index];
            int fontSize = Mathf.CeilToInt(rubySizeScale * item.FontSize);
            this.stringData.Add(item);
            for (int i = index + 1; i < characters.Count; i++)
            {
                UguiNovelTextCharacter character2 = characters[i];
                if (!character2.CustomInfo.IsRuby || character2.CustomInfo.IsRubyTop)
                {
                    break;
                }
                this.stringData.Add(character2);
            }
            CharData.CustomCharaInfo customInfo = new CharData.CustomCharaInfo {
                IsColor = item.charData.CustomInfo.IsColor,
                color = item.charData.CustomInfo.color
            };
            if (item.charData.CustomInfo.IsEmphasisMark)
            {
                for (int j = 0; j < this.stringData.Count; j++)
                {
                    CharData charData = new CharData(item.charData.CustomInfo.rubyStr[0], customInfo);
                    this.rubyList.Add(new UguiNovelTextCharacter(charData, font, fontSize, generator.BmpFontSize, item.FontStyle));
                }
            }
            else
            {
                foreach (char ch in item.charData.CustomInfo.rubyStr)
                {
                    CharData data2 = new CharData(ch, customInfo);
                    this.rubyList.Add(new UguiNovelTextCharacter(data2, font, fontSize, generator.BmpFontSize, item.FontStyle));
                }
            }
        }

        internal List<UIVertex> GetDrawVertex(Vector2 endPosition)
        {
            List<UIVertex> list = new List<UIVertex>();
            if (!this.TopCharacter.IsVisible)
            {
                return list;
            }
            try
            {
                foreach (UguiNovelTextCharacter character in this.rubyList)
                {
                    if (((this.textLine.Y0 > endPosition.y) || ((character.PositionX + (character.Width / 2f)) < endPosition.x)) && (character.Verts != null))
                    {
                        list.AddRange(character.Verts);
                    }
                }
                return list;
            }
            catch (Exception exception)
            {
                Debug.LogError(exception.Message);
                return list;
            }
        }

        internal void InitAfterCharacterInfo(UguiNovelTextGenerator generator)
        {
            float num = 0f;
            foreach (UguiNovelTextCharacter character in this.rubyList)
            {
                num += character.Width;
            }
            this.rubyWidth = num;
            num = 0f;
            foreach (UguiNovelTextCharacter character2 in this.stringData)
            {
                num += character2.Width + generator.LetterSpaceSize;
            }
            this.stringWidth = num;
            if (this.IsWideType)
            {
                float num2 = this.RubyWidth - (this.StringWidth - (this.stringData.Count * generator.LetterSpaceSize));
                float num3 = (num2 / ((float) this.stringData.Count)) / 2f;
                foreach (UguiNovelTextCharacter character3 in this.stringData)
                {
                    character3.RubySpaceSize = num3;
                }
            }
        }

        internal void InitPosition(UguiNovelTextGenerator generator)
        {
            Vector2 vector;
            foreach (UguiNovelTextLine line in generator.LineDataList)
            {
                if (line.Characters.IndexOf(this.TopCharacter) >= 0)
                {
                    this.textLine = line;
                }
            }
            float letterSpaceSize = generator.LetterSpaceSize;
            float num2 = 0f;
            if (this.IsWideType)
            {
                vector.x = -this.TopCharacter.RubySpaceSize;
            }
            else
            {
                num2 = (this.StringWidth - this.RubyWidth) / ((float) this.rubyList.Count);
                vector.x = (-letterSpaceSize / 2f) + (num2 / 2f);
            }
            vector.y = this.textLine.MaxFontSize;
            float num3 = vector.x + this.TopCharacter.PositionX;
            float num4 = vector.y + this.TopCharacter.PositionY;
            foreach (UguiNovelTextCharacter character in this.rubyList)
            {
                character.PositionX = num3;
                character.PositionY = num4;
                num3 += character.Width + num2;
            }
        }

        public bool IsWideType
        {
            get
            {
                return (this.RubyWidth > this.StringWidth);
            }
        }

        public List<UguiNovelTextCharacter> RubyList
        {
            get
            {
                return this.rubyList;
            }
        }

        public float RubyWidth
        {
            get
            {
                return this.rubyWidth;
            }
        }

        public float StringWidth
        {
            get
            {
                return this.stringWidth;
            }
        }

        internal UguiNovelTextCharacter TopCharacter
        {
            get
            {
                return this.stringData[0];
            }
        }
    }
}

