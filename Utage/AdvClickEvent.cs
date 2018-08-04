namespace Utage
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.EventSystems;
    using UtageExtensions;

    [AddComponentMenu("Utage/ADV/Internal/ClickEvent")]
    internal class AdvClickEvent : MonoBehaviour, IPointerClickHandler, IAdvClickEvent, IEventSystemHandler
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private UnityAction<BaseEventData> <action>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private StringGridRow <Row>k__BackingField;
        private AdvGraphicBase advGraphic;

        public virtual void AddClickEvent(bool isPolygon, StringGridRow row, UnityAction<BaseEventData> action)
        {
            this.Row = row;
            this.action = action;
            this.SetEnableCanvasRaycaster(true);
        }

        private void Awake()
        {
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (this.action != null)
            {
                this.action.Invoke(eventData);
            }
        }

        public virtual void RemoveClickEvent()
        {
            this.Row = null;
            this.action = null;
            this.SetEnableCanvasRaycaster(false);
        }

        private void SetEnableCanvasRaycaster(bool enable)
        {
            Canvas componentInParent = base.GetComponentInParent<Canvas>();
            if (componentInParent != null)
            {
                ((Component) componentInParent).GetComponentCreateIfMissing<GraphicRaycaster>().set_enabled(enable);
            }
        }

        GameObject IAdvClickEvent.get_gameObject()
        {
            return base.get_gameObject();
        }

        private UnityAction<BaseEventData> action { get; set; }

        private AdvGraphicBase AdvGraphic
        {
            get
            {
                return ((Component) this).GetComponentCache<AdvGraphicBase>(ref this.advGraphic);
            }
        }

        private StringGridRow Row { get; set; }
    }
}

