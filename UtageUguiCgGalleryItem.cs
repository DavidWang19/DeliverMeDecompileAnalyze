using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Utage;

[AddComponentMenu("Utage/TemplateUI/CgGalleryItem")]
public class UtageUguiCgGalleryItem : MonoBehaviour
{
    public Text count;
    private AdvCgGalleryData data;
    public AdvUguiLoadGraphicFile texture;

    public void Init(AdvCgGalleryData data, Action<UtageUguiCgGalleryItem> ButtonClickedEvent)
    {
        <Init>c__AnonStorey0 storey = new <Init>c__AnonStorey0 {
            ButtonClickedEvent = ButtonClickedEvent,
            $this = this
        };
        this.data = data;
        Button component = base.GetComponent<Button>();
        component.get_onClick().AddListener(new UnityAction(storey, (IntPtr) this.<>m__0));
        bool flag = data.NumOpen > 0;
        component.set_interactable(flag);
        if (flag)
        {
            this.texture.get_gameObject().SetActive(true);
            this.texture.LoadTextureFile(data.ThumbnailPath);
            this.count.set_text(string.Format("{0,2}/{1,2}", data.NumOpen, data.NumTotal));
        }
        else
        {
            this.texture.get_gameObject().SetActive(false);
            this.count.set_text(string.Empty);
        }
    }

    public AdvCgGalleryData Data
    {
        get
        {
            return this.data;
        }
    }

    [CompilerGenerated]
    private sealed class <Init>c__AnonStorey0
    {
        internal UtageUguiCgGalleryItem $this;
        internal Action<UtageUguiCgGalleryItem> ButtonClickedEvent;

        internal void <>m__0()
        {
            this.ButtonClickedEvent(this.$this);
        }
    }
}

