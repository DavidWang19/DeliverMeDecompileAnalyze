namespace Utage
{
    using System;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    public abstract class UguiAnimation : CurveAnimation, IBeginDragHandler, ICancelHandler, IDeselectHandler, IDragHandler, IDropHandler, IEndDragHandler, IInitializePotentialDragHandler, IMoveHandler, IPointerClickHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler, IScrollHandler, ISelectHandler, ISubmitHandler, IUpdateSelectedHandler, IEventSystemHandler
    {
        [SerializeField]
        private AnimationType animationType;
        [SerializeField, EnumFlags]
        private UiEventMask eventMask = UiEventMask.PointerClick;
        [SerializeField]
        private Graphic targetGraphic;

        protected UguiAnimation()
        {
        }

        protected virtual bool CheckEventMask(UiEventMask mask)
        {
            return ((this.EventMask & mask) == mask);
        }

        public virtual void OnBeginDrag(PointerEventData eventData)
        {
            this.PlayOnEvent(UiEventMask.BeginDrag);
        }

        public virtual void OnCancel(BaseEventData eventData)
        {
            this.PlayOnEvent(UiEventMask.Cancel);
        }

        public virtual void OnDeselect(BaseEventData eventData)
        {
            this.PlayOnEvent(UiEventMask.Deselect);
        }

        public virtual void OnDrag(PointerEventData eventData)
        {
            this.PlayOnEvent(UiEventMask.Drag);
        }

        public virtual void OnDrop(PointerEventData eventData)
        {
            this.PlayOnEvent(UiEventMask.Drop);
        }

        public virtual void OnEndDrag(PointerEventData eventData)
        {
            this.PlayOnEvent(UiEventMask.EndDrag);
        }

        public virtual void OnInitializePotentialDrag(PointerEventData eventData)
        {
            this.PlayOnEvent(UiEventMask.InitializePotentialDrag);
        }

        public virtual void OnMove(AxisEventData eventData)
        {
            this.PlayOnEvent(UiEventMask.Move);
        }

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            this.PlayOnEvent(UiEventMask.PointerClick);
        }

        public virtual void OnPointerDown(PointerEventData eventData)
        {
            this.PlayOnEvent(UiEventMask.PointerDown);
        }

        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            this.PlayOnEvent(UiEventMask.PointerEnter);
        }

        public virtual void OnPointerExit(PointerEventData eventData)
        {
            this.PlayOnEvent(UiEventMask.PointerExit);
        }

        public virtual void OnPointerUp(PointerEventData eventData)
        {
            this.PlayOnEvent(UiEventMask.PointerUp);
        }

        public virtual void OnScroll(PointerEventData eventData)
        {
            this.PlayOnEvent(UiEventMask.Scroll);
        }

        public virtual void OnSelect(BaseEventData eventData)
        {
            this.PlayOnEvent(UiEventMask.Select);
        }

        public virtual void OnSubmit(BaseEventData eventData)
        {
            this.PlayOnEvent(UiEventMask.Submit);
        }

        public virtual void OnUpdateSelected(BaseEventData eventData)
        {
            this.PlayOnEvent(UiEventMask.UpdateSelected);
        }

        public void Play()
        {
            this.Play(null);
        }

        public void Play(Action onComplete)
        {
            this.StartAnimation();
            base.PlayAnimation(new Action<float>(this.UpdateAnimation), onComplete);
        }

        protected virtual void PlayOnEvent(UiEventMask mask)
        {
            if (this.CheckEventMask(mask))
            {
                this.Play();
            }
        }

        protected void Reset()
        {
            this.targetGraphic = base.GetComponent<Graphic>();
        }

        protected abstract void StartAnimation();
        protected abstract void UpdateAnimation(float value);

        public UiEventMask EventMask
        {
            get
            {
                return this.eventMask;
            }
            set
            {
                this.eventMask = value;
            }
        }

        public Graphic TargetGraphic
        {
            get
            {
                return this.targetGraphic;
            }
            set
            {
                this.targetGraphic = value;
            }
        }

        public AnimationType Type
        {
            get
            {
                return this.animationType;
            }
            set
            {
                this.animationType = value;
            }
        }

        public enum AnimationType
        {
            To,
            From,
            FromTo,
            By
        }
    }
}

