namespace Utage
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class PathDialogAttribute : PropertyAttribute
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <Extention>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private DialogType <Type>k__BackingField;

        public PathDialogAttribute(DialogType type)
        {
            this.Type = type;
            this.Extention = string.Empty;
        }

        public PathDialogAttribute(DialogType type, string extention)
        {
            this.Type = type;
            this.Extention = extention;
        }

        public string Extention { get; private set; }

        public DialogType Type { get; private set; }

        public enum DialogType
        {
            Directory,
            File
        }
    }
}

