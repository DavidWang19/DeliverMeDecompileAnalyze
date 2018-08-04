namespace Utage
{
    using System;
    using System.Collections.Generic;

    public interface IAdvSetting
    {
        void BootInit(AdvSettingDataManager dataManager);
        void DownloadAll();
        void ParseGrid(StringGrid grid);

        List<StringGrid> GridList { get; }
    }
}

