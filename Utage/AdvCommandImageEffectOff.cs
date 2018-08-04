namespace Utage
{
    using System;

    internal class AdvCommandImageEffectOff : AdvCommandImageEffectBase
    {
        public AdvCommandImageEffectOff(StringGridRow row, AdvSettingDataManager dataManager) : base(row, dataManager, true)
        {
        }
    }
}

