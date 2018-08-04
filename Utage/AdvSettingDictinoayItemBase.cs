namespace Utage
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public abstract class AdvSettingDictinoayItemBase : IAdvSettingData
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private StringGridRow <RowData>k__BackingField;
        private string key;

        protected AdvSettingDictinoayItemBase()
        {
        }

        public abstract bool InitFromStringGridRow(StringGridRow row);
        internal bool InitFromStringGridRowMain(StringGridRow row)
        {
            this.RowData = row;
            return this.InitFromStringGridRow(row);
        }

        internal void InitKey(string key)
        {
            this.key = key;
        }

        public string Key
        {
            get
            {
                return this.key;
            }
        }

        public StringGridRow RowData { get; protected set; }
    }
}

