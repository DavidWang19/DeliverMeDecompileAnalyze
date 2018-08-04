namespace Utage
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class NotEditableAttribute : PropertyAttribute
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <EnablePropertyPath>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <IsEnableProperty>k__BackingField;

        public NotEditableAttribute()
        {
        }

        public NotEditableAttribute(string enablePropertyPath, bool isEnableProperty = true)
        {
            this.EnablePropertyPath = enablePropertyPath;
            this.IsEnableProperty = isEnableProperty;
        }

        public string EnablePropertyPath { get; private set; }

        public bool IsEnableProperty { get; private set; }
    }
}

