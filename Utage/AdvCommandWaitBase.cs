namespace Utage
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public abstract class AdvCommandWaitBase : AdvCommand
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private AdvCommandWaitType <WaitType>k__BackingField;

        protected AdvCommandWaitBase(StringGridRow row) : base(row)
        {
        }

        public override void DoCommand(AdvEngine engine)
        {
            base.CurrentTread.WaitManager.StartCommand(this);
            this.OnStart(engine, base.CurrentTread);
        }

        internal virtual void OnComplete(AdvScenarioThread thread)
        {
            thread.WaitManager.CompleteCommand(this);
        }

        protected abstract void OnStart(AdvEngine engine, AdvScenarioThread thread);
        public override bool Wait(AdvEngine engine)
        {
            switch (this.WaitType)
            {
                case AdvCommandWaitType.ThisAndAdd:
                    return base.CurrentTread.WaitManager.IsWaitingAdd;
            }
            return false;
        }

        public AdvCommandWaitType WaitType { get; protected set; }
    }
}

