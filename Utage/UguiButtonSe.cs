namespace Utage
{
    using System;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    [AddComponentMenu("Utage/Lib/UI/ButtonSe")]
    public class UguiButtonSe : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, ISubmitHandler, IMoveHandler, IEventSystemHandler
    {
        public AudioClip clicked;
        public SoundPlayMode clickedPlayMode;
        public AudioClip highlited;
        public SoundPlayMode highlitedPlayMode;
        private UnityEngine.UI.Selectable selectable;

        public void OnMove(AxisEventData eventData)
        {
            if (eventData.get_selectedObject() != base.get_gameObject())
            {
                this.PlayeSe(this.highlitedPlayMode, this.highlited);
            }
        }

        public void OnPointerClick(PointerEventData data)
        {
            this.PlayeSe(this.clickedPlayMode, this.clicked);
        }

        public void OnPointerEnter(PointerEventData data)
        {
            this.PlayeSe(this.highlitedPlayMode, this.highlited);
        }

        public void OnSubmit(BaseEventData eventData)
        {
            this.PlayeSe(this.clickedPlayMode, this.clicked);
        }

        private void PlayeSe(SoundPlayMode playMode, AudioClip clip)
        {
            if (((this.Selectable != null) && this.Selectable.get_interactable()) && (clip != null))
            {
                SoundManager instance = SoundManager.GetInstance();
                if (instance != null)
                {
                    instance.PlaySe(clip, clip.get_name(), playMode, false);
                }
                else
                {
                    AudioSource.PlayClipAtPoint(clip, Vector3.get_zero());
                }
            }
        }

        private UnityEngine.UI.Selectable Selectable
        {
            get
            {
                if (this.selectable == null)
                {
                }
                return (this.selectable = base.GetComponent<UnityEngine.UI.Selectable>());
            }
        }
    }
}

