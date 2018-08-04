namespace Utage
{
    using System;
    using System.Collections.Generic;

    public abstract class AdvSettingBase : IAdvSetting
    {
        private List<StringGrid> gridList = new List<StringGrid>();

        protected AdvSettingBase()
        {
        }

        public virtual void BootInit(AdvSettingDataManager dataManager)
        {
        }

        public virtual void DownloadAll()
        {
        }

        protected abstract void OnParseGrid(StringGrid grid);
        public virtual void ParseGrid(StringGrid grid)
        {
            this.GridList.Add(grid);
            grid.InitLink();
            this.OnParseGrid(grid);
        }

        public List<StringGrid> GridList
        {
            get
            {
                return this.gridList;
            }
        }
    }
}

