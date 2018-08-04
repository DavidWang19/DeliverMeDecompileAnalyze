namespace Utage
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class ButtonAttribute : PropertyAttribute
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <Function>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <Text>k__BackingField;

        public ButtonAttribute(string function, string text = "", int order = 0)
        {
            this.Function = function;
            this.Text = text;
            base.set_order(order);
        }

        public string Function { get; set; }

        public string Text { get; set; }
    }
}

