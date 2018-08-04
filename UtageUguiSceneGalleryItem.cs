using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Utage;

[AddComponentMenu("Utage/TemplateUI/SceneGalleryItem")]
public class UtageUguiSceneGalleryItem : MonoBehaviour
{
    private AdvSceneGallerySettingData data;
    public AdvUguiLoadGraphicFile texture;
    public Text title;

    public void Init(AdvSceneGallerySettingData data, Action<UtageUguiSceneGalleryItem> ButtonClickedEvent, AdvSystemSaveData saveData)
    {
        <Init>c__AnonStorey0 storey = new <Init>c__AnonStorey0 {
            ButtonClickedEvent = ButtonClickedEvent,
            $this = this
        };
        this.data = data;
        Button component = base.GetComponent<Button>();
        component.get_onClick().AddListener(new UnityAction(storey, (IntPtr) this.<>m__0));
        bool flag = saveData.GalleryData.CheckSceneLabels(data.ScenarioLabel);
        component.set_interactable(flag);
        if (!flag)
        {
            this.texture.get_gameObject().SetActive(false);
            if (this.title != null)
            {
                this.title.set_text(string.Empty);
            }
        }
        else
        {
            this.texture.get_gameObject().SetActive(true);
            this.texture.LoadTextureFile(data.ThumbnailPath);
            if (this.title != null)
            {
                this.title.set_text(data.Title);
            }
        }
    }

    public AdvSceneGallerySettingData Data
    {
        get
        {
            return this.data;
        }
    }

    [CompilerGenerated]
    private sealed class <Init>c__AnonStorey0
    {
        internal UtageUguiSceneGalleryItem $this;
        internal Action<UtageUguiSceneGalleryItem> ButtonClickedEvent;

        internal void <>m__0()
        {
            this.ButtonClickedEvent(this.$this);
        }
    }
}

