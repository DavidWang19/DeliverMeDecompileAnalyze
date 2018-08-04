using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Utage;

[AddComponentMenu("Utage/TemplateUI/SoundRoomItem")]
public class UtageUguiSoundRoomItem : MonoBehaviour
{
    private AdvSoundSettingData data;
    public Text title;

    public void Init(AdvSoundSettingData data, Action<UtageUguiSoundRoomItem> ButtonClickedEvent, int index)
    {
        <Init>c__AnonStorey0 storey = new <Init>c__AnonStorey0 {
            ButtonClickedEvent = ButtonClickedEvent,
            $this = this
        };
        this.data = data;
        this.title.set_text(data.Title);
        base.GetComponent<Button>().get_onClick().AddListener(new UnityAction(storey, (IntPtr) this.<>m__0));
    }

    public AdvSoundSettingData Data
    {
        get
        {
            return this.data;
        }
    }

    [CompilerGenerated]
    private sealed class <Init>c__AnonStorey0
    {
        internal UtageUguiSoundRoomItem $this;
        internal Action<UtageUguiSoundRoomItem> ButtonClickedEvent;

        internal void <>m__0()
        {
            this.ButtonClickedEvent(this.$this);
        }
    }
}

