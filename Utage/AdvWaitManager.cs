namespace Utage
{
    using System;
    using System.Collections.Generic;

    internal class AdvWaitManager
    {
        private List<AdvCommandWaitBase> commandList = new List<AdvCommandWaitBase>();

        internal void Clear()
        {
            this.commandList.Clear();
        }

        internal void CompleteCommand(AdvCommandWaitBase command)
        {
            if (command.WaitType != AdvCommandWaitType.NoWait)
            {
                this.commandList.Remove(command);
            }
        }

        internal void StartCommand(AdvCommandWaitBase command)
        {
            if (command.WaitType != AdvCommandWaitType.NoWait)
            {
                this.commandList.Add(command);
            }
        }

        internal bool IsWaiting
        {
            get
            {
                return (this.commandList.Count > 0);
            }
        }

        internal bool IsWaitingAdd
        {
            get
            {
                foreach (AdvCommandWaitBase base2 in this.commandList)
                {
                    switch (base2.WaitType)
                    {
                        case AdvCommandWaitType.ThisAndAdd:
                        case AdvCommandWaitType.Add:
                            return true;
                    }
                }
                return false;
            }
        }

        internal bool IsWaitingInputEffect
        {
            get
            {
                foreach (AdvCommandWaitBase base2 in this.commandList)
                {
                    switch (base2.WaitType)
                    {
                        case AdvCommandWaitType.Add:
                        case AdvCommandWaitType.InputWait:
                            return true;
                    }
                }
                return false;
            }
        }

        internal bool IsWaitingPageEndEffect
        {
            get
            {
                foreach (AdvCommandWaitBase base2 in this.commandList)
                {
                    switch (base2.WaitType)
                    {
                        case AdvCommandWaitType.Add:
                        case AdvCommandWaitType.InputWait:
                        case AdvCommandWaitType.PageWait:
                            return true;
                    }
                }
                return false;
            }
        }
    }
}

