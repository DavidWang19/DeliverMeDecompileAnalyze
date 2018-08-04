namespace Utage
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class AdvGraphicOperaitonArg
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private AdvCommand <Command>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private float <FadeTime>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private AdvGraphicInfo <Graphic>k__BackingField;

        internal AdvGraphicOperaitonArg(AdvCommand command, AdvGraphicInfo graphic, float fadeTime)
        {
            this.Command = command;
            this.Graphic = graphic;
            this.FadeTime = fadeTime;
        }

        public float GetSkippedFadeTime(AdvEngine engine)
        {
            return engine.Page.ToSkippedTime(this.FadeTime);
        }

        private AdvCommand Command { get; set; }

        private float FadeTime { get; set; }

        public AdvGraphicInfo Graphic { get; private set; }
    }
}

