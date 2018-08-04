namespace Utage
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class AdvTransitionArgs
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <TextureName>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private float <Time>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private float <Vague>k__BackingField;

        internal AdvTransitionArgs(string textureName, float vague, float time)
        {
            this.TextureName = textureName;
            this.Vague = vague;
            this.Time = time;
        }

        internal float GetSkippedTime(AdvEngine engine)
        {
            return engine.Page.ToSkippedTime(this.Time);
        }

        internal string TextureName { get; private set; }

        private float Time { get; set; }

        internal float Vague { get; private set; }
    }
}

