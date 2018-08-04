namespace Utage
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class AdvScenarioJumpData
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private StringGridRow <FromRow>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <ToLabel>k__BackingField;

        public AdvScenarioJumpData(string toLabel, StringGridRow fromRow)
        {
            this.ToLabel = toLabel;
            this.FromRow = fromRow;
        }

        public StringGridRow FromRow { get; private set; }

        public string ToLabel { get; private set; }
    }
}

