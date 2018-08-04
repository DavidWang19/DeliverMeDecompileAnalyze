namespace Utage
{
    using System;

    public interface IAssetFileSoundSettingData : IAssetFileSettingData
    {
        float IntroTime { get; }

        float Volume { get; }
    }
}

