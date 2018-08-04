namespace Utage
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class AdvPageController
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <IsBr>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <IsKeepText>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <IsWaitInput>k__BackingField;

        public void Clear()
        {
            this.IsKeepText = false;
            this.IsWaitInput = false;
            this.IsBr = false;
        }

        public static bool IsBrType(AdvPageControllerType type)
        {
            if ((type != AdvPageControllerType.Br) && (type != AdvPageControllerType.InputBr))
            {
                return false;
            }
            return true;
        }

        public static bool IsPageEndType(AdvPageControllerType type)
        {
            if ((type != AdvPageControllerType.InputBrPage) && (type != AdvPageControllerType.BrPage))
            {
                return false;
            }
            return true;
        }

        public static bool IsWaitInputType(AdvPageControllerType type)
        {
            switch (type)
            {
                case AdvPageControllerType.InputBrPage:
                case AdvPageControllerType.Input:
                case AdvPageControllerType.InputBr:
                    return true;
            }
            return false;
        }

        public void Update(AdvPageControllerType type)
        {
            this.IsKeepText = !IsPageEndType(type);
            this.IsWaitInput = IsWaitInputType(type);
            this.IsBr = IsBrType(type);
        }

        public bool IsBr { get; private set; }

        public bool IsKeepText { get; private set; }

        public bool IsWaitInput { get; private set; }
    }
}

