namespace Utage
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class MinMaxAttribute : PropertyAttribute
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private float <Max>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <MaxPropertyName>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private float <Min>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <MinPropertyName>k__BackingField;

        public MinMaxAttribute(float min, float max, string minPropertyName = "min", string maxPropertyName = "max")
        {
            this.Min = min;
            this.Max = max;
            this.MinPropertyName = minPropertyName;
            this.MaxPropertyName = maxPropertyName;
        }

        public float Max { get; private set; }

        public string MaxPropertyName { get; private set; }

        public float Min { get; private set; }

        public string MinPropertyName { get; private set; }
    }
}

