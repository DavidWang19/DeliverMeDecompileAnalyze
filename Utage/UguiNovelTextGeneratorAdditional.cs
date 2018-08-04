namespace Utage
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class UguiNovelTextGeneratorAdditional
    {
        private List<UguiNovelTextGeneratorAdditionalLine> lineList = new List<UguiNovelTextGeneratorAdditionalLine>();
        private List<UguiNovelTextGeneratorAdditionalRuby> rubyList = new List<UguiNovelTextGeneratorAdditionalRuby>();

        internal UguiNovelTextGeneratorAdditional(List<UguiNovelTextCharacter> characters, UguiNovelTextGenerator generataor)
        {
            for (int i = 0; i < characters.Count; i++)
            {
                UguiNovelTextCharacter character = characters[i];
                if (character.CustomInfo.IsStrikeTop)
                {
                    this.lineList.Add(new UguiNovelTextGeneratorAdditionalLine(UguiNovelTextGeneratorAdditionalLine.Type.Strike, characters, i, generataor));
                }
                if (character.CustomInfo.IsUnderLineTop)
                {
                    this.lineList.Add(new UguiNovelTextGeneratorAdditionalLine(UguiNovelTextGeneratorAdditionalLine.Type.UnderLine, characters, i, generataor));
                }
                if (character.CustomInfo.IsRubyTop)
                {
                    this.rubyList.Add(new UguiNovelTextGeneratorAdditionalRuby(characters, i, generataor));
                }
            }
        }

        internal void AddVerts(List<UIVertex> verts, Vector2 endPosition, UguiNovelTextGenerator generator)
        {
            foreach (UguiNovelTextGeneratorAdditionalLine line in this.lineList)
            {
                verts.AddRange(line.GetDrawVertex(endPosition, generator));
            }
            foreach (UguiNovelTextGeneratorAdditionalRuby ruby in this.RubyList)
            {
                verts.AddRange(ruby.GetDrawVertex(endPosition));
            }
        }

        internal float GetMaxWidth(UguiNovelTextCharacter currentData)
        {
            if (currentData.CustomInfo.IsRubyTop)
            {
                foreach (UguiNovelTextGeneratorAdditionalRuby ruby in this.RubyList)
                {
                    if (ruby.TopCharacter == currentData)
                    {
                        return Mathf.Max(ruby.StringWidth, ruby.RubyWidth);
                    }
                }
            }
            return currentData.Width;
        }

        internal float GetTopLetterSpace(UguiNovelTextCharacter lineTopCharacter, UguiNovelTextGenerator generator)
        {
            foreach (UguiNovelTextGeneratorAdditionalRuby ruby in this.RubyList)
            {
                if (!ruby.IsWideType && (ruby.TopCharacter == lineTopCharacter))
                {
                    return (generator.LetterSpaceSize / 2f);
                }
            }
            return 0f;
        }

        internal void InitAfterCharacterInfo(UguiNovelTextGenerator generator)
        {
            foreach (UguiNovelTextGeneratorAdditionalRuby ruby in this.RubyList)
            {
                ruby.InitAfterCharacterInfo(generator);
            }
        }

        internal void InitPosition(UguiNovelTextGenerator generator)
        {
            List<UguiNovelTextGeneratorAdditionalLine> collection = new List<UguiNovelTextGeneratorAdditionalLine>();
            foreach (UguiNovelTextGeneratorAdditionalLine line in this.lineList)
            {
                collection.AddRange(line.MakeOtherLineInTextLine(generator));
            }
            this.lineList.AddRange(collection);
            foreach (UguiNovelTextGeneratorAdditionalLine line2 in this.lineList)
            {
                line2.InitPosition(generator);
            }
            foreach (UguiNovelTextGeneratorAdditionalRuby ruby in this.RubyList)
            {
                ruby.InitPosition(generator);
            }
        }

        internal List<UguiNovelTextCharacter> MakeCharacterList()
        {
            List<UguiNovelTextCharacter> list = new List<UguiNovelTextCharacter>();
            foreach (UguiNovelTextGeneratorAdditionalLine line in this.lineList)
            {
                list.Add(line.CharacteData);
            }
            foreach (UguiNovelTextGeneratorAdditionalRuby ruby in this.rubyList)
            {
                foreach (UguiNovelTextCharacter character in ruby.RubyList)
                {
                    list.Add(character);
                }
            }
            return list;
        }

        internal void MakeVerts(Color color, UguiNovelTextGenerator generator)
        {
            foreach (UguiNovelTextGeneratorAdditionalLine line in this.lineList)
            {
                line.CharacteData.MakeVerts(color, generator);
            }
            foreach (UguiNovelTextGeneratorAdditionalRuby ruby in this.RubyList)
            {
                foreach (UguiNovelTextCharacter character in ruby.RubyList)
                {
                    character.MakeVerts(color, generator);
                }
            }
        }

        internal bool TrySetFontCharcters(Font font)
        {
            foreach (UguiNovelTextGeneratorAdditionalLine line in this.lineList)
            {
                if (!line.CharacteData.TrySetCharacterInfo(font))
                {
                    return false;
                }
            }
            foreach (UguiNovelTextGeneratorAdditionalRuby ruby in this.rubyList)
            {
                foreach (UguiNovelTextCharacter character in ruby.RubyList)
                {
                    if (!character.TrySetCharacterInfo(font))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public List<UguiNovelTextGeneratorAdditionalLine> LineList
        {
            get
            {
                return this.lineList;
            }
        }

        public List<UguiNovelTextGeneratorAdditionalRuby> RubyList
        {
            get
            {
                return this.rubyList;
            }
        }
    }
}

