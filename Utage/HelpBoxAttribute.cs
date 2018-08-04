namespace Utage
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class HelpBoxAttribute : PropertyAttribute
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <Message>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Type <MessageType>k__BackingField;

        public HelpBoxAttribute(string message, Type type = 0, int order = 0)
        {
            this.Message = message;
            this.MessageType = type;
            base.set_order(order);
        }

        public string Message { get; set; }

        public Type MessageType { get; set; }

        public enum Type
        {
            None,
            Info,
            Warning,
            Error
        }
    }
}

