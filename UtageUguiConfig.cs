using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Utage;

[AddComponentMenu("Utage/TemplateUI/Config")]
public class UtageUguiConfig : UguiView
{
    [SerializeField]
    private Toggle checkFullscreen;
    [SerializeField]
    private Toggle checkHideMessageWindowOnPlyaingVoice;
    [SerializeField]
    private Toggle checkMessageWindowOnPlyaingTextSound;
    [SerializeField]
    private Toggle checkMouseWheel;
    [SerializeField]
    private Toggle checkSkipUnread;
    [SerializeField]
    private Toggle checkStopSkipInSelection;
    [SerializeField]
    private SystemUiDialog2Button dialogBackToTitle;
    [SerializeField]
    private AdvEngine engine;
    private bool isInit;
    [SerializeField]
    private UguiToggleGroupIndexed radioButtonsVoiceStopType;
    [SerializeField]
    private Slider sliderAmbienceVolume;
    [SerializeField]
    private Slider sliderAutoBrPageSpeed;
    [SerializeField]
    private Slider sliderBgmVolume;
    [SerializeField]
    private Slider sliderMessageSpeed;
    [SerializeField]
    private Slider sliderMessageSpeedRead;
    [SerializeField]
    private Slider sliderMessageWindowTransparency;
    [SerializeField]
    private Slider sliderSeVolume;
    [SerializeField]
    private Slider sliderSoundMasterVolume;
    [SerializeField]
    private Slider sliderVoiceVolume;
    [SerializeField]
    private List<TagedMasterVolumSliders> tagedMasterVolumSliders;
    [SerializeField]
    private UtageUguiTitle title;

    public override void Close()
    {
        this.Engine.WriteSystemData();
        base.Close();
    }

    [DebuggerHidden]
    private IEnumerator CoWaitOpen()
    {
        return new <CoWaitOpen>c__Iterator0 { $this = this };
    }

    private void LoadValues()
    {
        this.isInit = false;
        if (this.checkFullscreen != null)
        {
            this.checkFullscreen.set_isOn(this.Config.IsFullScreen);
        }
        if (this.checkMouseWheel != null)
        {
            this.checkMouseWheel.set_isOn(this.Config.IsMouseWheelSendMessage);
        }
        if (this.checkSkipUnread != null)
        {
            this.checkSkipUnread.set_isOn(this.Config.IsSkipUnread);
        }
        if (this.checkStopSkipInSelection != null)
        {
            this.checkStopSkipInSelection.set_isOn(this.Config.IsStopSkipInSelection);
        }
        if (this.checkHideMessageWindowOnPlyaingVoice != null)
        {
            this.checkHideMessageWindowOnPlyaingVoice.set_isOn(this.Config.HideMessageWindowOnPlayingVoice);
        }
        if (this.checkMessageWindowOnPlyaingTextSound != null)
        {
            this.checkMessageWindowOnPlyaingTextSound.set_isOn(this.Config.IsPlayingTextSound);
        }
        if (this.sliderMessageSpeed != null)
        {
            this.sliderMessageSpeed.set_value(this.Config.MessageSpeed);
        }
        if (this.sliderMessageSpeedRead != null)
        {
            this.sliderMessageSpeed.set_value(this.Config.MessageSpeed);
        }
        if (this.sliderAutoBrPageSpeed != null)
        {
            this.sliderAutoBrPageSpeed.set_value(this.Config.AutoBrPageSpeed);
        }
        if (this.sliderMessageWindowTransparency != null)
        {
            this.sliderMessageWindowTransparency.set_value(this.Config.MessageWindowTransparency);
        }
        if (this.sliderSoundMasterVolume != null)
        {
            this.sliderSoundMasterVolume.set_value(this.Config.SoundMasterVolume);
        }
        if (this.sliderBgmVolume != null)
        {
            this.sliderBgmVolume.set_value(this.Config.BgmVolume);
        }
        if (this.sliderSeVolume != null)
        {
            this.sliderSeVolume.set_value(this.Config.SeVolume);
        }
        if (this.sliderAmbienceVolume != null)
        {
            this.sliderAmbienceVolume.set_value(this.Config.AmbienceVolume);
        }
        if (this.sliderVoiceVolume != null)
        {
            this.sliderVoiceVolume.set_value(this.Config.VoiceVolume);
        }
        if (this.radioButtonsVoiceStopType != null)
        {
            this.radioButtonsVoiceStopType.CurrentIndex = (int) this.Config.VoiceStopType;
        }
        foreach (TagedMasterVolumSliders sliders in this.tagedMasterVolumSliders)
        {
            float num;
            if ((!string.IsNullOrEmpty(sliders.tag) && (sliders.volumeSlider != null)) && this.Config.TryGetTaggedMasterVolume(sliders.tag, out num))
            {
                sliders.volumeSlider.set_value(num);
            }
        }
        if (!UtageToolKit.IsPlatformStandAloneOrEditor())
        {
            if (this.checkFullscreen != null)
            {
                this.checkFullscreen.get_gameObject().SetActive(false);
            }
            if (this.checkMouseWheel != null)
            {
                this.checkMouseWheel.get_gameObject().SetActive(false);
            }
        }
        this.isInit = true;
    }

    private void OnOpen()
    {
        this.isInit = false;
        if (this.Engine.SaveManager.Type != AdvSaveManager.SaveType.SavePoint)
        {
            this.Engine.SaveManager.ClearCaptureTexture();
        }
        base.StartCoroutine(this.CoWaitOpen());
    }

    public void OnOpenDialogBackTitle()
    {
        SystemUi.GetInstance().IsEnableInputEscape = false;
        this.dialogBackToTitle.Open(LanguageSystemText.LocalizeText(SystemText.BackToTitleDesc), LanguageSystemText.LocalizeText(SystemText.Yes), LanguageSystemText.LocalizeText(SystemText.No), new UnityAction(this, (IntPtr) this.OnTapBackTitle), new UnityAction(SystemUi.GetInstance(), (IntPtr) this.OnDialogExitGameNo));
    }

    public void OnTapBackTitle()
    {
        SystemUi.GetInstance().IsEnableInputEscape = true;
        SystemUi.GetInstance().IsEnableTitleAniamtion = true;
        this.Engine.EndScenario();
        this.Close();
        this.title.Open();
    }

    public void OnTapInitDefaultAll()
    {
        if (this.IsInit)
        {
            this.Config.InitDefaultAll();
            this.LoadValues();
        }
    }

    public void OnTapRadioButtonVoiceStopType(int index)
    {
        if (this.IsInit)
        {
            this.Config.VoiceStopType = (VoiceStopType) index;
        }
    }

    public void OnValugeChangedTaggedMasterVolume(string tag, float value)
    {
        if (this.IsInit)
        {
            this.Config.SetTaggedMasterVolume(tag, value);
        }
    }

    private void Update()
    {
        if (this.isInit && InputUtil.IsMouseRightButtonDown())
        {
            this.Back();
        }
    }

    public float AmbienceVolume
    {
        set
        {
            if (this.IsInit)
            {
                this.Config.AmbienceVolume = value;
            }
        }
    }

    public float AutoBrPageSpeed
    {
        set
        {
            if (this.IsInit)
            {
                this.Config.AutoBrPageSpeed = value;
            }
        }
    }

    public float BgmVolume
    {
        set
        {
            if (this.IsInit)
            {
                this.Config.BgmVolume = value;
            }
        }
    }

    private AdvConfig Config
    {
        get
        {
            return this.Engine.Config;
        }
    }

    public AdvEngine Engine
    {
        get
        {
            if (this.engine == null)
            {
            }
            return (this.engine = Object.FindObjectOfType<AdvEngine>());
        }
    }

    public bool HideMessageWindowOnPlyaingVoice
    {
        set
        {
            if (this.IsInit)
            {
                this.Config.HideMessageWindowOnPlayingVoice = value;
            }
        }
    }

    public bool IsEffect
    {
        set
        {
            if (this.IsInit)
            {
                this.Config.IsEffect = value;
            }
        }
    }

    public bool IsFullScreen
    {
        set
        {
            if (this.IsInit)
            {
                this.Config.IsFullScreen = value;
            }
        }
    }

    public bool IsInit
    {
        get
        {
            return this.isInit;
        }
        set
        {
            this.isInit = value;
        }
    }

    public bool IsMouseWheel
    {
        set
        {
            if (this.IsInit)
            {
                this.Config.IsMouseWheelSendMessage = value;
            }
        }
    }

    public bool IsPlyaingTextSound
    {
        set
        {
            if (this.IsInit)
            {
                this.Config.IsPlayingTextSound = value;
            }
        }
    }

    public bool IsSkipUnread
    {
        set
        {
            if (this.IsInit)
            {
                this.Config.IsSkipUnread = value;
            }
        }
    }

    public bool IsStopSkipInSelection
    {
        set
        {
            if (this.IsInit)
            {
                this.Config.IsStopSkipInSelection = value;
            }
        }
    }

    public float MessageSpeed
    {
        set
        {
            if (this.IsInit)
            {
                this.Config.MessageSpeed = value;
            }
        }
    }

    public float MessageSpeedRead
    {
        set
        {
            if (this.IsInit)
            {
                this.Config.MessageSpeedRead = value;
            }
        }
    }

    public float MessageWindowTransparency
    {
        set
        {
            if (this.IsInit)
            {
                this.Config.MessageWindowTransparency = value;
            }
        }
    }

    public float SeVolume
    {
        set
        {
            if (this.IsInit)
            {
                this.Config.SeVolume = value;
            }
        }
    }

    public float SoundMasterVolume
    {
        set
        {
            if (this.IsInit)
            {
                this.Config.SoundMasterVolume = value;
            }
        }
    }

    public float VoiceVolume
    {
        set
        {
            if (this.IsInit)
            {
                this.Config.VoiceVolume = value;
            }
        }
    }

    [CompilerGenerated]
    private sealed class <CoWaitOpen>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal bool $disposing;
        internal int $PC;
        internal UtageUguiConfig $this;

        [DebuggerHidden]
        public void Dispose()
        {
        }

        public bool MoveNext()
        {
            this.$PC = -1;
            if ((this.$PC == 0) && !this.$this.Engine.IsWaitBootLoading)
            {
                this.$this.LoadValues();
            }
            return false;
        }

        [DebuggerHidden]
        public void Reset()
        {
            throw new NotSupportedException();
        }

        object IEnumerator<object>.Current
        {
            [DebuggerHidden]
            get
            {
                return this.$current;
            }
        }

        object IEnumerator.Current
        {
            [DebuggerHidden]
            get
            {
                return this.$current;
            }
        }
    }

    [Serializable]
    private class TagedMasterVolumSliders
    {
        public string tag = string.Empty;
        public Slider volumeSlider;
    }
}

