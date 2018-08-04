namespace Utage
{
    using System;

    internal class AdvLocalVideoFile : AssetFileUtage
    {
        public AdvLocalVideoFile(AdvVideoLoadPathChanger pathChanger, AssetFileManager assetFileManager, AssetFileInfo fileInfo, IAssetFileSettingData settingData) : base(assetFileManager, fileInfo, settingData)
        {
            fileInfo.StrageType = AssetFileStrageType.Resources;
            if (settingData is AdvCommandSetting)
            {
                AdvCommandSetting setting = settingData as AdvCommandSetting;
                string str = setting.Command.ParseCell<string>(AdvColumnName.Arg1);
                string[] args = new string[] { pathChanger.RootPath, str };
                base.LoadPath = FilePathUtil.Combine(args);
            }
            else
            {
                AdvGraphicInfo info = settingData as AdvGraphicInfo;
                string fileName = info.FileName;
                string[] textArray2 = new string[] { pathChanger.RootPath, fileName };
                base.LoadPath = FilePathUtil.Combine(textArray2);
            }
        }
    }
}

