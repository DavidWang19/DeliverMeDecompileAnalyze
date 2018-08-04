namespace Utage
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class UguiNovelTextGeneratorAdditionalLine
    {
        private UguiNovelTextCharacter characteData;
        private List<UguiNovelTextCharacter> stringData;
        private UguiNovelTextLine textLine;
        private Type type;

        private UguiNovelTextGeneratorAdditionalLine(UguiNovelTextGeneratorAdditionalLine srcLine, int index, int count, UguiNovelTextGenerator generator)
        {
            this.stringData = new List<UguiNovelTextCharacter>();
            this.InitSub(srcLine.type, generator);
            for (int i = 0; i < count; i++)
            {
                this.stringData.Add(srcLine.stringData[index + i]);
            }
        }

        internal UguiNovelTextGeneratorAdditionalLine(Type type, List<UguiNovelTextCharacter> characters, int index, UguiNovelTextGenerator generator)
        {
            this.stringData = new List<UguiNovelTextCharacter>();
            this.InitSub(type, generator);
            this.stringData.Add(characters[index]);
            for (int i = index + 1; i < characters.Count; i++)
            {
                UguiNovelTextCharacter c = characters[i];
                if (this.IsLineEnd(c))
                {
                    break;
                }
                this.stringData.Add(c);
            }
        }

        internal List<UIVertex> GetDrawVertex(Vector2 endPosition, UguiNovelTextGenerator generator)
        {
            List<UIVertex> list = new List<UIVertex>();
            if (this.TopCharacter.IsVisible)
            {
                float positionX = this.TopCharacter.PositionX;
                float endPositionX = this.TopCharacter.EndPositionX;
                foreach (UguiNovelTextCharacter character in this.stringData)
                {
                    if (!character.IsVisible)
                    {
                        break;
                    }
                    endPositionX = character.EndPositionX;
                }
                Color defaultColor = Color.get_white();
                foreach (UguiNovelTextCharacter character2 in this.stringData)
                {
                    if (!character2.IsVisible)
                    {
                        break;
                    }
                    if (character2.Verts != null)
                    {
                        defaultColor = character2.Verts[0].color;
                    }
                }
                this.CharacteData.Width = endPositionX - positionX;
                this.CharacteData.MakeVerts(defaultColor, generator);
                list.AddRange(this.CharacteData.Verts);
            }
            return list;
        }

        internal void InitPosition(UguiNovelTextGenerator generator)
        {
            Vector2 vector = Vector2.get_zero();
            float maxFontSize = this.textLine.MaxFontSize;
            switch (this.LineType)
            {
                case Type.UnderLine:
                    vector.y -= maxFontSize / 2f;
                    break;
            }
            this.CharacteData.PositionX = this.TopCharacter.PositionX + vector.x;
            this.CharacteData.PositionY = this.TopCharacter.PositionY + vector.y;
        }

        private void InitSub(Type type, UguiNovelTextGenerator generator)
        {
            this.type = type;
            CharData.CustomCharaInfo customInfo = new CharData.CustomCharaInfo {
                IsDash = true,
                DashSize = 1
            };
            CharData charData = new CharData(generator.DashChar, customInfo);
            this.characteData = new UguiNovelTextCharacter(charData, generator);
        }

        private void InitTextLine(UguiNovelTextGenerator generator)
        {
            foreach (UguiNovelTextLine line in generator.LineDataList)
            {
                if (line.Characters.IndexOf(this.TopCharacter) >= 0)
                {
                    this.textLine = line;
                }
            }
        }

        private bool IsLineEnd(UguiNovelTextCharacter c)
        {
            Type lineType = this.LineType;
            if (lineType != Type.Strike)
            {
                return ((lineType == Type.UnderLine) && (!c.CustomInfo.IsUnderLine || c.CustomInfo.IsUnderLineTop));
            }
            return (!c.CustomInfo.IsStrike || c.CustomInfo.IsStrikeTop);
        }

        internal List<UguiNovelTextGeneratorAdditionalLine> MakeOtherLineInTextLine(UguiNovelTextGenerator generator)
        {
            this.InitTextLine(generator);
            return this.MakeOtherLineInTextLineSub(generator);
        }

        internal List<UguiNovelTextGeneratorAdditionalLine> MakeOtherLineInTextLineSub(UguiNovelTextGenerator generator)
        {
            List<UguiNovelTextGeneratorAdditionalLine> list = new List<UguiNovelTextGeneratorAdditionalLine>();
            int index = this.stringData.Count - 1;
            foreach (UguiNovelTextLine line in generator.LineDataList)
            {
                if (this.textLine != line)
                {
                    bool flag = false;
                    int num2 = 0;
                    int count = 0;
                    for (int i = 0; i < this.stringData.Count; i++)
                    {
                        if (line.Characters.IndexOf(this.stringData[i]) >= 0)
                        {
                            if (!flag)
                            {
                                num2 = i;
                                index = Mathf.Min(i, index);
                                flag = true;
                            }
                            count++;
                        }
                    }
                    if (flag)
                    {
                        UguiNovelTextGeneratorAdditionalLine item = new UguiNovelTextGeneratorAdditionalLine(this, num2, count, generator);
                        list.Add(item);
                        item.InitTextLine(generator);
                        if (!item.characteData.TrySetCharacterInfo(generator.NovelText.get_font()))
                        {
                            Debug.LogError("Line Font Missing");
                        }
                    }
                }
            }
            if (index < (this.stringData.Count - 1))
            {
                this.stringData.RemoveRange(index, this.stringData.Count - index);
            }
            return list;
        }

        public UguiNovelTextCharacter CharacteData
        {
            get
            {
                return this.characteData;
            }
        }

        public Type LineType
        {
            get
            {
                return this.type;
            }
        }

        internal UguiNovelTextCharacter TopCharacter
        {
            get
            {
                return this.stringData[0];
            }
        }

        public enum Type
        {
            UnderLine,
            Strike
        }
    }
}

