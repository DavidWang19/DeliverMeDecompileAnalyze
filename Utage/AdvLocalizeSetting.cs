namespace Utage
{
    using System;

    public class AdvLocalizeSetting : AdvSettingBase
    {
        protected override void OnParseGrid(StringGrid grid)
        {
            LanguageManagerBase instance = LanguageManagerBase.Instance;
            if (instance != null)
            {
                instance.OverwriteData(grid);
            }
        }
    }
}

