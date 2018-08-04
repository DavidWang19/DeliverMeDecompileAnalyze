using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Utage;

[AddComponentMenu("Utage/TemplateUI/MainGame")]
public class UtageUguiMainGame : UguiView
{
    private BootType bootType;
    public GameObject buttons;
    public Toggle checkAuto;
    public Toggle checkSkip;
    public UtageUguiConfig config;
    [SerializeField]
    private AdvEngine engine;
    public UtageUguiGallery gallery;
    private bool isInit;
    private float lastSpeed;
    [SerializeField]
    private Utage.LetterBoxCamera letterBoxCamera;
    private AdvSaveData loadData;
    public UtageUguiSaveLoad saveLoad;
    private string scenarioLabel;
    public UtageUguiTitle title;

    private void Awake()
    {
        this.Engine.Page.OnEndText.AddListener(new UnityAction<AdvPage>(this, (IntPtr) this.<Awake>m__0));
    }

    private Texture2D CaptureScreen()
    {
        Rect rect = this.LetterBoxCamera.CachedCamera.get_rect();
        int num = Mathf.CeilToInt(rect.get_x() * Screen.get_width());
        int num2 = Mathf.CeilToInt(rect.get_y() * Screen.get_height());
        int num3 = Mathf.FloorToInt(rect.get_width() * Screen.get_width());
        int num4 = Mathf.FloorToInt(rect.get_height() * Screen.get_height());
        return UtageToolKit.CaptureScreen(new Rect((float) num, (float) num2, (float) num3, (float) num4));
    }

    private void CaptureScreenOnSavePoint(AdvPage page)
    {
        if ((this.Engine.SaveManager.Type == AdvSaveManager.SaveType.SavePoint) && page.IsSavePoint)
        {
            Debug.Log("Capture");
            base.StartCoroutine(this.CoCaptureScreen());
        }
    }

    private void ClearBootData()
    {
        this.bootType = BootType.Default;
        this.isInit = false;
        this.loadData = null;
    }

    public override void Close()
    {
        base.Close();
        this.Engine.UiManager.Close();
        this.Engine.Config.IsSkip = false;
    }

    [DebuggerHidden]
    private IEnumerator CoCaptureScreen()
    {
        return new <CoCaptureScreen>c__Iterator1 { $this = this };
    }

    [DebuggerHidden]
    private IEnumerator CoQSave()
    {
        return new <CoQSave>c__Iterator3 { $this = this };
    }

    [DebuggerHidden]
    private IEnumerator CoSave()
    {
        return new <CoSave>c__Iterator2 { $this = this };
    }

    [DebuggerHidden]
    private IEnumerator CoWaitOpen()
    {
        return new <CoWaitOpen>c__Iterator0 { $this = this };
    }

    private void LateUpdate()
    {
        bool flag = this.Engine.UiManager.IsShowingMenuButton && (this.Engine.UiManager.Status == AdvUiManager.UiStatus.Menu);
        float num = !flag ? -1f : 1f;
        int num2 = !flag ? 1 : 0;
        if (this.lastSpeed != num)
        {
            this.buttons.GetComponent<Animator>().Play("MainMenuMovement", 0, (float) num2);
            this.buttons.GetComponent<Animator>().SetFloat("speed", num);
            this.lastSpeed = num;
        }
        if ((this.checkSkip != null) && (this.checkSkip.get_isOn() != this.Engine.Config.IsSkip))
        {
            this.checkSkip.set_isOn(this.Engine.Config.IsSkip);
        }
        if ((this.checkAuto != null) && (this.checkAuto.get_isOn() != this.Engine.Config.IsAutoBrPage))
        {
            this.checkAuto.set_isOn(this.Engine.Config.IsAutoBrPage);
        }
    }

    private void OnOpen()
    {
        if (this.Engine.SaveManager.Type != AdvSaveManager.SaveType.SavePoint)
        {
            this.Engine.SaveManager.ClearCaptureTexture();
        }
        base.StartCoroutine(this.CoWaitOpen());
    }

    public void OnTapAuto(bool isOn)
    {
        this.Engine.Config.IsAutoBrPage = isOn;
    }

    public void OnTapConfig()
    {
        this.Close();
        this.config.Open(this);
    }

    public void OnTapLoad()
    {
        if (!this.Engine.IsSceneGallery)
        {
            this.Close();
            this.saveLoad.OpenLoad(this);
        }
    }

    public void OnTapQLoad()
    {
        if (!this.Engine.IsSceneGallery)
        {
            this.Engine.Config.IsSkip = false;
            this.Engine.QuickLoad();
        }
    }

    public void OnTapQSave()
    {
        if (!this.Engine.IsSceneGallery)
        {
            this.Engine.Config.IsSkip = false;
            base.StartCoroutine(this.CoQSave());
        }
    }

    public void OnTapSave()
    {
        if (!this.Engine.IsSceneGallery)
        {
            base.StartCoroutine(this.CoSave());
        }
    }

    public void OnTapSkip(bool isOn)
    {
        this.Engine.Config.IsSkip = isOn;
    }

    public void OpenLoadGame(AdvSaveData loadData)
    {
        this.ClearBootData();
        this.bootType = BootType.Load;
        this.loadData = loadData;
        this.Open();
    }

    public void OpenSceneGallery(string scenarioLabel)
    {
        this.ClearBootData();
        this.bootType = BootType.SceneGallery;
        this.scenarioLabel = scenarioLabel;
        this.Open();
    }

    public void OpenStartGame()
    {
        this.ClearBootData();
        this.bootType = BootType.Start;
        this.Open();
    }

    public void OpenStartLabel(string label)
    {
        this.ClearBootData();
        this.bootType = BootType.StartLabel;
        this.scenarioLabel = label;
        this.Open();
    }

    private void Update()
    {
        if (this.isInit)
        {
            if (SystemUi.GetInstance() != null)
            {
                if (this.Engine.IsLoading)
                {
                    SystemUi.GetInstance().StartIndicator(this);
                }
                else
                {
                    SystemUi.GetInstance().StopIndicator(this);
                }
            }
            if (this.Engine.IsEndScenario)
            {
                this.Close();
                if (this.Engine.IsSceneGallery)
                {
                    this.gallery.Open();
                }
                else
                {
                    this.title.Open(this);
                }
            }
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

    public Utage.LetterBoxCamera LetterBoxCamera
    {
        get
        {
            if (this.letterBoxCamera == null)
            {
            }
            return (this.letterBoxCamera = Object.FindObjectOfType<Utage.LetterBoxCamera>());
        }
    }

    [CompilerGenerated]
    private sealed class <CoCaptureScreen>c__Iterator1 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal bool $disposing;
        internal int $PC;
        internal UtageUguiMainGame $this;

        [DebuggerHidden]
        public void Dispose()
        {
            this.$disposing = true;
            this.$PC = -1;
        }

        public bool MoveNext()
        {
            uint num = (uint) this.$PC;
            this.$PC = -1;
            switch (num)
            {
                case 0:
                    this.$current = new WaitForEndOfFrame();
                    if (!this.$disposing)
                    {
                        this.$PC = 1;
                    }
                    return true;

                case 1:
                    this.$this.Engine.SaveManager.CaptureTexture = this.$this.CaptureScreen();
                    this.$PC = -1;
                    break;
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

    [CompilerGenerated]
    private sealed class <CoQSave>c__Iterator3 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal bool $disposing;
        internal int $PC;
        internal UtageUguiMainGame $this;

        [DebuggerHidden]
        public void Dispose()
        {
            this.$disposing = true;
            this.$PC = -1;
        }

        public bool MoveNext()
        {
            uint num = (uint) this.$PC;
            this.$PC = -1;
            switch (num)
            {
                case 0:
                    if (this.$this.Engine.SaveManager.Type == AdvSaveManager.SaveType.SavePoint)
                    {
                        break;
                    }
                    this.$current = new WaitForEndOfFrame();
                    if (!this.$disposing)
                    {
                        this.$PC = 1;
                    }
                    return true;

                case 1:
                    this.$this.Engine.SaveManager.CaptureTexture = this.$this.CaptureScreen();
                    break;

                default:
                    goto Label_00D8;
            }
            this.$this.Engine.QuickSave();
            this.$this.Engine.UiManager.Status = AdvUiManager.UiStatus.Default;
            if (this.$this.Engine.SaveManager.Type != AdvSaveManager.SaveType.SavePoint)
            {
                this.$this.Engine.SaveManager.ClearCaptureTexture();
            }
            this.$PC = -1;
        Label_00D8:
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

    [CompilerGenerated]
    private sealed class <CoSave>c__Iterator2 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal bool $disposing;
        internal int $PC;
        internal UtageUguiMainGame $this;

        [DebuggerHidden]
        public void Dispose()
        {
            this.$disposing = true;
            this.$PC = -1;
        }

        public bool MoveNext()
        {
            uint num = (uint) this.$PC;
            this.$PC = -1;
            switch (num)
            {
                case 0:
                    if (this.$this.Engine.SaveManager.Type == AdvSaveManager.SaveType.SavePoint)
                    {
                        break;
                    }
                    this.$current = new WaitForEndOfFrame();
                    if (!this.$disposing)
                    {
                        this.$PC = 1;
                    }
                    return true;

                case 1:
                    this.$this.Engine.SaveManager.CaptureTexture = this.$this.CaptureScreen();
                    break;

                default:
                    goto Label_00A3;
            }
            this.$this.Close();
            this.$this.saveLoad.OpenSave(this.$this);
            this.$PC = -1;
        Label_00A3:
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

    [CompilerGenerated]
    private sealed class <CoWaitOpen>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal bool $disposing;
        internal int $PC;
        internal UtageUguiMainGame $this;

        [DebuggerHidden]
        public void Dispose()
        {
            this.$disposing = true;
            this.$PC = -1;
        }

        public bool MoveNext()
        {
            uint num = (uint) this.$PC;
            this.$PC = -1;
            switch (num)
            {
                case 0:
                case 1:
                    if (this.$this.Engine.IsWaitBootLoading)
                    {
                        this.$current = null;
                        if (!this.$disposing)
                        {
                            this.$PC = 1;
                        }
                        return true;
                    }
                    switch (this.$this.bootType)
                    {
                        case UtageUguiMainGame.BootType.Default:
                            this.$this.Engine.UiManager.Open();
                            goto Label_0110;

                        case UtageUguiMainGame.BootType.Start:
                            this.$this.Engine.StartGame();
                            goto Label_0110;

                        case UtageUguiMainGame.BootType.Load:
                            this.$this.Engine.OpenLoadGame(this.$this.loadData);
                            goto Label_0110;

                        case UtageUguiMainGame.BootType.SceneGallery:
                            this.$this.Engine.StartSceneGallery(this.$this.scenarioLabel);
                            goto Label_0110;

                        case UtageUguiMainGame.BootType.StartLabel:
                            this.$this.Engine.StartGame(this.$this.scenarioLabel);
                            goto Label_0110;
                    }
                    break;

                default:
                    goto Label_0150;
            }
        Label_0110:
            this.$this.ClearBootData();
            this.$this.loadData = null;
            this.$this.Engine.Config.IsSkip = false;
            this.$this.isInit = true;
            this.$PC = -1;
        Label_0150:
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

    private enum BootType
    {
        Default,
        Start,
        Load,
        SceneGallery,
        StartLabel
    }
}

