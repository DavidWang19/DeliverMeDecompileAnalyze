using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Utage;

[AddComponentMenu("Utage/TemplateUI/SaveLoadItem"), RequireComponent(typeof(Button))]
public class UtageUguiSaveLoadItem : MonoBehaviour
{
    public Texture2D autoSaveIcon;
    private Button button;
    public RawImage captureImage;
    private AdvSaveData data;
    public Text date;
    private Color defaultColor;
    private int index;
    public Text no;
    public Text text;
    public string textEmpty = "Empty";

    public void Init(AdvSaveData data, Action<UtageUguiSaveLoadItem> ButtonClickedEvent, int index, bool isSave)
    {
        <Init>c__AnonStorey0 storey = new <Init>c__AnonStorey0 {
            ButtonClickedEvent = ButtonClickedEvent,
            $this = this
        };
        this.data = data;
        this.index = index;
        this.button = base.GetComponent<Button>();
        this.button.get_onClick().AddListener(new UnityAction(storey, (IntPtr) this.<>m__0));
        this.Refresh(isSave);
    }

    private void OnDestroy()
    {
        if ((this.captureImage != null) && (this.captureImage.get_texture() != null))
        {
            this.captureImage.set_texture(null);
        }
    }

    public void Refresh(bool isSave)
    {
        this.no.set_text(string.Format("No.{0,3}", this.index));
        if (this.data.IsSaved)
        {
            if ((this.data.Type == AdvSaveData.SaveDataType.Auto) || (this.data.Texture == null))
            {
                if ((this.data.Type == AdvSaveData.SaveDataType.Auto) && (this.autoSaveIcon != null))
                {
                    this.captureImage.set_texture(this.autoSaveIcon);
                    this.captureImage.set_color(Color.get_white());
                }
                else
                {
                    this.captureImage.set_texture(null);
                    this.captureImage.set_color(Color.get_black());
                }
            }
            else
            {
                this.captureImage.set_texture(this.data.Texture);
                this.captureImage.set_color(Color.get_white());
            }
            this.text.set_text(this.data.Title);
            this.date.set_text(UtageToolKit.DateToStringJp(this.data.Date));
            this.button.set_interactable(true);
        }
        else
        {
            this.text.set_text(this.textEmpty);
            this.date.set_text(string.Empty);
            this.button.set_interactable(isSave);
        }
        if (this.data.Type == AdvSaveData.SaveDataType.Auto)
        {
            this.no.set_text("Auto");
            if (isSave)
            {
                this.button.set_interactable(false);
            }
        }
    }

    public AdvSaveData Data
    {
        get
        {
            return this.data;
        }
    }

    public int Index
    {
        get
        {
            return this.index;
        }
    }

    [CompilerGenerated]
    private sealed class <Init>c__AnonStorey0
    {
        internal UtageUguiSaveLoadItem $this;
        internal Action<UtageUguiSaveLoadItem> ButtonClickedEvent;

        internal void <>m__0()
        {
            this.ButtonClickedEvent(this.$this);
        }
    }
}

