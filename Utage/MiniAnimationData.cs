namespace Utage
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    [Serializable]
    public class MiniAnimationData
    {
        [SerializeField]
        private List<Data> dataList = new List<Data>();

        internal bool TryParse(StringGridRow row, int index)
        {
            try
            {
                this.DataList.Clear();
                while ((index + 1) < row.Strings.Length)
                {
                    if (row.IsEmptyCell(index) && row.IsEmptyCell((int) (index + 1)))
                    {
                        break;
                    }
                    string name = row.ParseCell<string>(index++);
                    float duration = row.ParseCell<float>(index++);
                    this.DataList.Add(new Data(duration, name));
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<Data> DataList
        {
            get
            {
                return this.dataList;
            }
        }

        [Serializable]
        public class Data
        {
            [SerializeField]
            private float duration;
            [SerializeField]
            private string name;

            public Data(float duration, string name)
            {
                this.duration = duration;
                this.name = name;
            }

            public string ComvertName(string originalName)
            {
                NamingType type = this.ParseNamigType();
                if (type != NamingType.Suffix)
                {
                    if (type == NamingType.Swap)
                    {
                        return this.GetSwapName(originalName);
                    }
                    return this.name;
                }
                return (originalName + this.GetSffixName());
            }

            public string ComvertNameSimple()
            {
                if (this.ParseNamigType() != NamingType.Suffix)
                {
                    return this.name;
                }
                return this.GetSffixName();
            }

            public string GetSffixName()
            {
                return this.name.Substring(1);
            }

            public string GetSwapName(string originalName)
            {
                if (originalName.Length < 2)
                {
                    return originalName;
                }
                int index = originalName.IndexOf('(');
                if (index < 0)
                {
                    return originalName;
                }
                return (originalName.Substring(0, index) + this.name.Substring(1));
            }

            private NamingType ParseNamigType()
            {
                if (this.name.Length < 2)
                {
                    return NamingType.Default;
                }
                if (this.name[0] != '*')
                {
                    return NamingType.Default;
                }
                if ((this.name[1] == '(') && (this.name[this.name.Length - 1] == ')'))
                {
                    return NamingType.Swap;
                }
                return NamingType.Suffix;
            }

            public float Duration
            {
                get
                {
                    return this.duration;
                }
            }

            private enum NamingType
            {
                Default,
                Suffix,
                Swap
            }
        }
    }
}

