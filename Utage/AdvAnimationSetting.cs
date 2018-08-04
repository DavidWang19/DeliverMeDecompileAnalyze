namespace Utage
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class AdvAnimationSetting : AdvSettingBase
    {
        private List<AdvAnimationData> DataList = new List<AdvAnimationData>();

        public AdvAnimationData Find(string name)
        {
            <Find>c__AnonStorey0 storey = new <Find>c__AnonStorey0 {
                name = name
            };
            return this.DataList.Find(new Predicate<AdvAnimationData>(storey.<>m__0));
        }

        protected override void OnParseGrid(StringGrid grid)
        {
            int index = 0;
            while (index < grid.Rows.Count)
            {
                if (grid.Rows[index].IsEmpty)
                {
                    index++;
                }
                else
                {
                    AdvAnimationData item = new AdvAnimationData(grid, ref index, true);
                    this.DataList.Add(item);
                }
            }
        }

        [CompilerGenerated]
        private sealed class <Find>c__AnonStorey0
        {
            internal string name;

            internal bool <>m__0(AdvAnimationData x)
            {
                return (x.Clip.get_name() == this.name);
            }
        }
    }
}

