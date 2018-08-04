namespace Utage
{
    using System;
    using UnityEngine.Events;

    [Serializable]
    public class Open2ButtonDialogEvent : UnityEvent<string, ButtonEventInfo, ButtonEventInfo>
    {
    }
}

