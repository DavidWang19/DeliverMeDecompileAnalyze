namespace Utage
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    [CreateAssetMenu(menuName="Utage/AvatarData")]
    public class AvatarData : ScriptableObject
    {
        [NotEditable]
        public List<Category> categories = new List<Category>();
        [SerializeField]
        private string optionTag = "accessory";
        [SerializeField, NotEditable]
        private Vector2 size;

        public List<string> GetAllOptionPatterns()
        {
            HashSet<string> collection = new HashSet<string>();
            foreach (Category category in this.categories)
            {
                if (category.Tag == this.OptionTag)
                {
                    collection.UnionWith(category.GetAllPatternNames());
                }
            }
            return new List<string>(collection);
        }

        public List<Sprite> MakeSortedSprites(AvatarPattern pattern)
        {
            List<Sprite> list = new List<Sprite>();
            foreach (Category category in this.categories)
            {
                if (category.Tag != this.optionTag)
                {
                    foreach (AvatarPattern.PartternData data in pattern.DataList)
                    {
                        if (category.Tag == data.tag)
                        {
                            list.Add(category.GetSprite(data.patternName));
                        }
                    }
                }
                else
                {
                    using (List<string>.Enumerator enumerator3 = pattern.OptionPatternNameList.GetEnumerator())
                    {
                        while (enumerator3.MoveNext())
                        {
                            <MakeSortedSprites>c__AnonStorey0 storey = new <MakeSortedSprites>c__AnonStorey0 {
                                optionPattern = enumerator3.Current
                            };
                            list.AddRange(category.Sprites.FindAll(new Predicate<Sprite>(storey.<>m__0)));
                        }
                    }
                }
            }
            return list;
        }

        private static string ToPatternName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return string.Empty;
            }
            char[] separator = new char[] { '_' };
            return name.Split(separator)[0];
        }

        private static string ToPatternName(Sprite sprite)
        {
            if (sprite == null)
            {
                return string.Empty;
            }
            return ToPatternName(sprite.get_name());
        }

        public string OptionTag
        {
            get
            {
                return this.optionTag;
            }
        }

        public Vector2 Size
        {
            get
            {
                return this.size;
            }
            internal set
            {
                this.size = value;
            }
        }

        [CompilerGenerated]
        private sealed class <MakeSortedSprites>c__AnonStorey0
        {
            internal string optionPattern;

            internal bool <>m__0(Sprite x)
            {
                return (AvatarData.ToPatternName(x) == this.optionPattern);
            }
        }

        [Serializable]
        public class Category
        {
            [SerializeField]
            private string name;
            [SerializeField]
            private int sortOrder;
            [SerializeField]
            private List<Sprite> sprites = new List<Sprite>();
            [SerializeField]
            private string tag;

            public HashSet<string> GetAllPatternNames()
            {
                <GetAllPatternNames>c__AnonStorey0 storey = new <GetAllPatternNames>c__AnonStorey0 {
                    set = new HashSet<string>()
                };
                this.Sprites.ForEach(new Action<Sprite>(storey.<>m__0));
                return storey.set;
            }

            public Sprite GetSprite(string pattern)
            {
                <GetSprite>c__AnonStorey1 storey = new <GetSprite>c__AnonStorey1 {
                    pattern = pattern
                };
                Sprite sprite = this.Sprites.Find(new Predicate<Sprite>(storey.<>m__0));
                if (sprite == null)
                {
                    sprite = this.Sprites.Find(new Predicate<Sprite>(storey.<>m__1));
                }
                if (sprite == null)
                {
                    sprite = this.Sprites.Find(new Predicate<Sprite>(storey.<>m__2));
                }
                return sprite;
            }

            public string Name
            {
                get
                {
                    return this.name;
                }
                set
                {
                    this.name = value;
                }
            }

            public int SortOrder
            {
                get
                {
                    return this.sortOrder;
                }
                set
                {
                    this.sortOrder = value;
                }
            }

            public List<Sprite> Sprites
            {
                get
                {
                    return this.sprites;
                }
                set
                {
                    this.sprites = value;
                }
            }

            public string Tag
            {
                get
                {
                    return this.tag;
                }
                set
                {
                    this.tag = value;
                }
            }

            [CompilerGenerated]
            private sealed class <GetAllPatternNames>c__AnonStorey0
            {
                internal HashSet<string> set;

                internal void <>m__0(Sprite x)
                {
                    this.set.Add(AvatarData.ToPatternName(x));
                }
            }

            [CompilerGenerated]
            private sealed class <GetSprite>c__AnonStorey1
            {
                internal string pattern;

                internal bool <>m__0(Sprite x)
                {
                    return (AvatarData.ToPatternName(x) == this.pattern);
                }

                internal bool <>m__1(Sprite x)
                {
                    return ((x != null) && (x.get_name() == this.pattern));
                }

                internal bool <>m__2(Sprite x)
                {
                    return ((x != null) && (x.get_name() == AvatarData.ToPatternName(this.pattern)));
                }
            }
        }
    }
}

