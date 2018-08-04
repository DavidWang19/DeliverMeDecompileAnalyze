namespace Utage
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class LimitEnumAttribute : PropertyAttribute
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string[] <Args>k__BackingField;

        public LimitEnumAttribute(params string[] args)
        {
            this.Args = args;
        }

        public string[] Args { get; private set; }
    }
}

