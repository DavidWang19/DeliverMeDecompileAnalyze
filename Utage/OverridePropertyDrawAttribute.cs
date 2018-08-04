namespace Utage
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class OverridePropertyDrawAttribute : PropertyAttribute
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <Function>k__BackingField;

        public OverridePropertyDrawAttribute(string function)
        {
            this.Function = function;
        }

        public string Function { get; set; }
    }
}

