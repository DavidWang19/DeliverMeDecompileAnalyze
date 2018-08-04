namespace Utage
{
    using System;
    using UnityEngine;
    using UnityEngine.Events;

    internal interface IAdvClickEvent
    {
        void AddClickEvent(bool isPolygon, StringGridRow row, UnityAction<BaseEventData> action);
        void RemoveClickEvent();

        GameObject gameObject { get; }
    }
}

