namespace Utage
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class MinAttribute : PropertyAttribute
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private float <Min>k__BackingField;

        public MinAttribute(float min)
        {
            this.Min = min;
        }

        public float Min { get; private set; }
    }
}

