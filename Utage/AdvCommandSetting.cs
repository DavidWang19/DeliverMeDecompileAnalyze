namespace Utage
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class AdvCommandSetting : IAssetFileSettingData
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private AdvCommand <Command>k__BackingField;

        public AdvCommandSetting(AdvCommand command)
        {
            this.Command = command;
        }

        public AdvCommand Command { get; private set; }

        public StringGridRow RowData
        {
            get
            {
                return this.Command.RowData;
            }
        }
    }
}

