namespace Utage
{
    using System;
    using System.Runtime.CompilerServices;

    public class AdvVideoSettingData : AdvSettingDictinoayItemBase
    {
        private AdvGraphicInfo graphic;

        public void BootInit(AdvSettingDataManager dataManager)
        {
            <BootInit>c__AnonStorey0 storey = new <BootInit>c__AnonStorey0 {
                dataManager = dataManager,
                $this = this
            };
            this.Graphic.BootInit(new Func<string, string, string>(storey.<>m__0), storey.dataManager);
        }

        private string FileNameToPath(string fileName, string fileType, AdvBootSetting settingData)
        {
            return settingData.VideoDirInfo.FileNameToPath(fileName);
        }

        public override bool InitFromStringGridRow(StringGridRow row)
        {
            if (row.IsEmptyOrCommantOut)
            {
                return false;
            }
            base.RowData = row;
            string str = AdvParser.ParseCell<string>(row, AdvColumnName.Label);
            if (string.IsNullOrEmpty(str))
            {
                return false;
            }
            base.InitKey(str);
            this.graphic = new AdvGraphicInfo("Particle", 0, str, row, this);
            return true;
        }

        public AdvGraphicInfo Graphic
        {
            get
            {
                return this.graphic;
            }
        }

        [CompilerGenerated]
        private sealed class <BootInit>c__AnonStorey0
        {
            internal AdvVideoSettingData $this;
            internal AdvSettingDataManager dataManager;

            internal string <>m__0(string fileName, string fileType)
            {
                return this.$this.FileNameToPath(fileName, fileType, this.dataManager.BootSetting);
            }
        }
    }
}

