namespace Utage
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    internal class AdvCommandScenarioLabel : AdvCommand
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <ScenarioLabel>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <Title>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private ScenarioLabelType <Type>k__BackingField;

        public AdvCommandScenarioLabel(StringGridRow row) : base(row)
        {
            this.ScenarioLabel = base.ParseScenarioLabel(AdvColumnName.Command);
            this.Type = base.ParseCellOptional<ScenarioLabelType>(AdvColumnName.Arg1, ScenarioLabelType.None);
            this.Title = base.ParseCellOptional<string>(AdvColumnName.Arg2, string.Empty);
        }

        public override void DoCommand(AdvEngine engine)
        {
        }

        public string ScenarioLabel { get; protected set; }

        public string Title { get; protected set; }

        public ScenarioLabelType Type { get; protected set; }

        public enum ScenarioLabelType
        {
            None,
            SavePoint
        }
    }
}

