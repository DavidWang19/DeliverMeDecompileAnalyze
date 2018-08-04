namespace Utage
{
    using System;
    using UnityEngine.Events;

    [Serializable]
    public class Open3ButtonDialogEvent : UnityEvent<string, ButtonEventInfo, ButtonEventInfo, ButtonEventInfo>
    {
    }
}

