namespace Utage
{
    using System;
    using UnityEngine;
    using UnityEngine.EventSystems;

    [AddComponentMenu("Utage/Lib/UI/NovelTextEventTrigger"), RequireComponent(typeof(UguiNovelText))]
    public class UguiNovelTextEventTrigger : MonoBehaviour, ICanvasRaycastFilter, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerClickHandler, IEventSystemHandler
    {
        private RectTransform cachedRectTransform;
        private UguiNovelTextHitArea currentTarget;
        private UguiNovelTextGenerator generator;
        public Color hoverColor = ColorUtil.Red;
        private bool isEntered;
        private UguiNovelText novelText;
        public OnClickLinkEvent OnClick = new OnClickLinkEvent();

        private void ChangeCurrentTarget(UguiNovelTextHitArea target)
        {
            if (this.currentTarget != target)
            {
                if (this.currentTarget != null)
                {
                    this.currentTarget.ResetEffectColor();
                }
                this.currentTarget = target;
                if (this.currentTarget != null)
                {
                    this.currentTarget.ChangeEffectColor(this.hoverColor);
                }
            }
        }

        private UguiNovelTextHitArea HitTest(PointerEventData eventData)
        {
            return this.HitTest(eventData.get_position(), eventData.get_pressEventCamera());
        }

        private UguiNovelTextHitArea HitTest(Vector2 screenPoint, Camera cam)
        {
            Vector2 vector;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(this.CachedRectTransform, screenPoint, cam, ref vector);
            foreach (UguiNovelTextHitArea area in this.Generator.HitGroupLists)
            {
                if (area.HitTest(vector))
                {
                    return area;
                }
            }
            return null;
        }

        public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
        {
            UguiNovelTextHitArea target = this.HitTest(sp, eventCamera);
            if (this.isEntered)
            {
                this.ChangeCurrentTarget(target);
            }
            return (target != null);
        }

        private void OnDrawGizmos()
        {
            foreach (UguiNovelTextHitArea area in this.Generator.HitGroupLists)
            {
                foreach (Rect rect in area.HitAreaList)
                {
                    Gizmos.set_color(Color.get_yellow());
                    Vector3 vector = rect.get_center();
                    Vector3 vector2 = rect.get_size();
                    vector = this.CachedRectTransform.TransformPoint(vector);
                    vector2 = this.CachedRectTransform.TransformVector(vector2);
                    Gizmos.DrawWireCube(vector, vector2);
                }
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            UguiNovelTextHitArea area = this.HitTest(eventData);
            if (area != null)
            {
                this.OnClick.Invoke(area);
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            this.isEntered = true;
            this.ChangeCurrentTarget(this.HitTest(eventData));
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            this.isEntered = false;
            this.ChangeCurrentTarget(null);
        }

        public RectTransform CachedRectTransform
        {
            get
            {
                if (this.cachedRectTransform == null)
                {
                    this.cachedRectTransform = base.GetComponent<RectTransform>();
                }
                return this.cachedRectTransform;
            }
        }

        public UguiNovelTextGenerator Generator
        {
            get
            {
                if (this.generator == null)
                {
                }
                return (this.generator = base.GetComponent<UguiNovelTextGenerator>());
            }
        }

        public UguiNovelText NovelText
        {
            get
            {
                if (this.novelText == null)
                {
                }
                return (this.novelText = base.GetComponent<UguiNovelText>());
            }
        }
    }
}

