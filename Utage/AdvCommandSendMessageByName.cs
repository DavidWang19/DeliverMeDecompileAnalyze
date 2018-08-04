namespace Utage
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UtageExtensions;

    public class AdvCommandSendMessageByName : AdvCommand
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private AdvEngine <Engine>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <IsWait>k__BackingField;

        public AdvCommandSendMessageByName(StringGridRow row) : base(row)
        {
        }

        public override void DoCommand(AdvEngine engine)
        {
            this.Engine = engine;
            string str = base.ParseCell<string>(AdvColumnName.Arg1);
            string functionName = base.ParseCell<string>(AdvColumnName.Arg2);
            GameObject go = GameObject.Find(str);
            if (go == null)
            {
                Debug.LogError(str + " is not found in current scene");
            }
            else
            {
                go.SafeSendMessage(functionName, this, false);
            }
        }

        public override bool Wait(AdvEngine engine)
        {
            return this.IsWait;
        }

        public AdvEngine Engine { get; private set; }

        public bool IsWait { get; set; }
    }
}

