namespace Utage
{
    using System;
    using UnityEngine.Events;

    [Serializable]
    public class OpenDialogEvent : UnityEvent<string, List<ButtonEventInfo>>
    {
    }
}

