namespace Utage
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class AdvVoiceSetting : IAssetFileSoundSettingData, IAssetFileSettingData
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private StringGridRow <RowData>k__BackingField;

        public AdvVoiceSetting(StringGridRow row)
        {
            this.RowData = row;
        }

        public float IntroTime
        {
            get
            {
                return 0f;
            }
        }

        public StringGridRow RowData { get; private set; }

        public float Volume
        {
            get
            {
                return 1f;
            }
        }
    }
}

