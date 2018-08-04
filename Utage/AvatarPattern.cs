namespace Utage
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    [Serializable]
    public class AvatarPattern
    {
        [SerializeField]
        private List<PartternData> avatarPatternDataList = new List<PartternData>();
        [SerializeField]
        private List<string> optionPatternNameList = new List<string>();

        public string GetPatternName(string tag)
        {
            <GetPatternName>c__AnonStorey1 storey = new <GetPatternName>c__AnonStorey1 {
                tag = tag
            };
            PartternData data = this.DataList.Find(new Predicate<PartternData>(storey.<>m__0));
            return ((data != null) ? data.patternName : string.Empty);
        }

        internal bool Rebuild(AvatarData data)
        {
            if (data == null)
            {
                return false;
            }
            bool flag = false;
            using (List<AvatarData.Category>.Enumerator enumerator = data.categories.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    <Rebuild>c__AnonStorey3 storey = new <Rebuild>c__AnonStorey3 {
                        category = enumerator.Current
                    };
                    if (this.DataList.Find(new Predicate<PartternData>(storey.<>m__0)) == null)
                    {
                        PartternData item = new PartternData {
                            tag = storey.category.Tag
                        };
                        this.DataList.Add(item);
                        flag = true;
                    }
                }
            }
            return flag;
        }

        internal void SetPattern(StringGridRow rowData)
        {
            using (Dictionary<string, int>.Enumerator enumerator = rowData.Grid.ColumnIndexTbl.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    <SetPattern>c__AnonStorey2 storey = new <SetPattern>c__AnonStorey2 {
                        keyValue = enumerator.Current
                    };
                    PartternData data = this.DataList.Find(new Predicate<PartternData>(storey.<>m__0));
                    if (data != null)
                    {
                        data.patternName = rowData.Strings[storey.keyValue.Value];
                    }
                }
            }
        }

        public void SetPatternName(string tag, string patternName)
        {
            <SetPatternName>c__AnonStorey0 storey = new <SetPatternName>c__AnonStorey0 {
                tag = tag
            };
            PartternData data = this.DataList.Find(new Predicate<PartternData>(storey.<>m__0));
            if (data == null)
            {
                Debug.LogError(string.Format("Unknown Pattern [{0}], tag[{1}] ", patternName, storey.tag));
            }
            else
            {
                data.patternName = patternName;
            }
        }

        public List<PartternData> DataList
        {
            get
            {
                return this.avatarPatternDataList;
            }
        }

        public List<string> OptionPatternNameList
        {
            get
            {
                return this.optionPatternNameList;
            }
        }

        [CompilerGenerated]
        private sealed class <GetPatternName>c__AnonStorey1
        {
            internal string tag;

            internal bool <>m__0(AvatarPattern.PartternData x)
            {
                return (x.tag == this.tag);
            }
        }

        [CompilerGenerated]
        private sealed class <Rebuild>c__AnonStorey3
        {
            internal AvatarData.Category category;

            internal bool <>m__0(AvatarPattern.PartternData x)
            {
                return (x.tag == this.category.Tag);
            }
        }

        [CompilerGenerated]
        private sealed class <SetPattern>c__AnonStorey2
        {
            internal KeyValuePair<string, int> keyValue;

            internal bool <>m__0(AvatarPattern.PartternData x)
            {
                return (x.tag == this.keyValue.Key);
            }
        }

        [CompilerGenerated]
        private sealed class <SetPatternName>c__AnonStorey0
        {
            internal string tag;

            internal bool <>m__0(AvatarPattern.PartternData x)
            {
                return (x.tag == this.tag);
            }
        }

        [Serializable]
        public class PartternData
        {
            public string patternName;
            public string tag;
        }
    }
}

