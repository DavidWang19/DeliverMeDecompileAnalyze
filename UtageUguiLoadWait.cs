using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Utage;

[AddComponentMenu("Utage/TemplateUI/LoadWait")]
public class UtageUguiLoadWait : UguiView
{
    [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private State <CurrentState>k__BackingField;
    [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private Type <DownloadType>k__BackingField;
    public string bootSceneName;
    public GameObject buttonBack;
    public GameObject buttonDownload;
    public GameObject buttonSkip;
    [SerializeField]
    private AdvEngine engine;
    public bool isAutoCacheFileLoad;
    public Image loadingBar;
    public GameObject loadingBarRoot;
    [SerializeField]
    private OpenDialogEvent onOpenDialog;
    [SerializeField]
    private AdvEngineStarter starter;
    public Text textCount;
    public Text textMain;
    public UtageUguiTitle title;

    private void ChangeState(State state)
    {
        this.CurrentState = state;
        if (state != State.Start)
        {
            if (state == State.Downloding)
            {
                this.buttonDownload.SetActive(false);
                base.StartCoroutine(this.CoUpdateLoading());
            }
            else if (state == State.DownlodFinished)
            {
                this.OnFinished();
            }
        }
        else
        {
            this.buttonDownload.SetActive(true);
            this.loadingBarRoot.SetActive(false);
            this.textMain.set_text(string.Empty);
            this.textCount.set_text(string.Empty);
            this.StartLoadEngine();
        }
    }

    [DebuggerHidden]
    private IEnumerator CoUpdateLoading()
    {
        return new <CoUpdateLoading>c__Iterator0 { $this = this };
    }

    private bool IsMobileOffLine()
    {
        switch (Application.get_internetReachability())
        {
            case 0:
                return true;
        }
        return false;
    }

    private void OnClose()
    {
        this.DownloadType = Type.Default;
    }

    private void OnFailedLoadEngine()
    {
        if (this.isAutoCacheFileLoad)
        {
            this.Starter.LoadEngineAsyncFromCacheManifest(new Action(this.OnFailedLoadEngine));
        }
        else
        {
            string str = LanguageSystemText.LocalizeText(SystemText.WarningNotOnline);
            List<ButtonEventInfo> list = new List<ButtonEventInfo> {
                new ButtonEventInfo(LanguageSystemText.LocalizeText(0), new UnityAction(this, (IntPtr) this.<OnFailedLoadEngine>m__0)),
                new ButtonEventInfo(LanguageSystemText.LocalizeText(6), new UnityAction(this, (IntPtr) this.<OnFailedLoadEngine>m__1))
            };
            this.OnOpenDialog.Invoke(str, list);
        }
    }

    private void OnFinished()
    {
        switch (this.DownloadType)
        {
            case Type.Boot:
                this.Close();
                this.title.Open();
                break;

            case Type.Default:
                this.buttonDownload.SetActive(false);
                this.loadingBarRoot.SetActive(false);
                this.textMain.set_text(LanguageSystemText.LocalizeText(SystemText.DownloadFinished));
                this.textCount.set_text(string.Empty);
                break;

            case Type.ChapterDownload:
                this.Close();
                break;
        }
    }

    private void OnOpen()
    {
        switch (this.DownloadType)
        {
            case Type.Boot:
                if (this.buttonBack != null)
                {
                    this.buttonBack.SetActive(false);
                }
                if (this.buttonSkip != null)
                {
                    this.buttonSkip.SetActive(true);
                }
                if (this.buttonDownload != null)
                {
                    this.buttonDownload.SetActive(true);
                }
                break;

            case Type.Default:
                if (this.buttonBack != null)
                {
                    this.buttonBack.SetActive(true);
                }
                if (this.buttonSkip != null)
                {
                    this.buttonSkip.SetActive(false);
                }
                if (this.buttonDownload != null)
                {
                    this.buttonDownload.SetActive(true);
                }
                break;

            case Type.ChapterDownload:
                if (this.buttonBack != null)
                {
                    this.buttonBack.SetActive(false);
                }
                if (this.buttonSkip != null)
                {
                    this.buttonSkip.SetActive(false);
                }
                if (this.buttonDownload != null)
                {
                    this.buttonDownload.SetActive(false);
                }
                break;
        }
        if (!this.Starter.IsLoadStart)
        {
            this.ChangeState(State.Start);
        }
        else
        {
            this.ChangeState(State.Downloding);
        }
    }

    public void OnTapReDownload()
    {
        AssetFileManager.GetInstance().AssetBundleInfoManager.DeleteAllCache();
        if (string.IsNullOrEmpty(this.bootSceneName))
        {
            WrapperUnityVersion.LoadScene(0);
        }
        else
        {
            WrapperUnityVersion.LoadScene(this.bootSceneName);
        }
    }

    public void OnTapSkip()
    {
        this.Close();
        this.title.Open();
    }

    public void OpenOnBoot()
    {
        this.DownloadType = Type.Boot;
        this.Open();
    }

    public void OpenOnChapter()
    {
        this.DownloadType = Type.ChapterDownload;
        this.Open();
    }

    private void StartLoadEngine()
    {
        base.StartCoroutine(this.Starter.LoadEngineAsync(new Action(this.OnFailedLoadEngine)));
        this.ChangeState(State.Downloding);
    }

    private State CurrentState { get; set; }

    private Type DownloadType { get; set; }

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

    public OpenDialogEvent OnOpenDialog
    {
        get
        {
            if ((this.onOpenDialog.GetPersistentEventCount() == 0) && (SystemUi.GetInstance() != null))
            {
                this.onOpenDialog.RemoveAllListeners();
                this.onOpenDialog.AddListener(new UnityAction<string, List<ButtonEventInfo>>(SystemUi.GetInstance(), (IntPtr) this.OpenDialog));
            }
            return this.onOpenDialog;
        }
        set
        {
            this.onOpenDialog = value;
        }
    }

    public AdvEngineStarter Starter
    {
        get
        {
            if (this.starter == null)
            {
            }
            return (this.starter = Object.FindObjectOfType<AdvEngineStarter>());
        }
    }

    [CompilerGenerated]
    private sealed class <CoUpdateLoading>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal bool $disposing;
        internal int $PC;
        internal UtageUguiLoadWait $this;
        internal int <countDownLoaded>__1;
        internal int <countDownLoading>__0;
        internal int <maxCountDownLoad>__0;

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
                    this.<maxCountDownLoad>__0 = 0;
                    this.<countDownLoading>__0 = 0;
                    this.$this.loadingBarRoot.SetActive(true);
                    this.$this.loadingBar.set_fillAmount(0f);
                    this.$this.textMain.set_text(LanguageSystemText.LocalizeText(SystemText.Downloading));
                    this.$this.textCount.set_text(string.Format(LanguageSystemText.LocalizeText(SystemText.DownloadCount), 0, 1));
                    break;

                case 1:
                    break;

                case 2:
                    goto Label_01AB;

                case 3:
                    this.<countDownLoading>__0 = AssetFileManager.CountDownloading();
                    this.<maxCountDownLoad>__0 = Mathf.Max(this.<maxCountDownLoad>__0, this.<countDownLoading>__0);
                    this.<countDownLoaded>__1 = this.<maxCountDownLoad>__0 - this.<countDownLoading>__0;
                    this.$this.textCount.set_text(string.Format(LanguageSystemText.LocalizeText(SystemText.DownloadCount), this.<countDownLoaded>__1, this.<maxCountDownLoad>__0));
                    if (this.<maxCountDownLoad>__0 > 0)
                    {
                        this.$this.loadingBar.set_fillAmount((1f * (this.<maxCountDownLoad>__0 - this.<countDownLoading>__0)) / ((float) this.<maxCountDownLoad>__0));
                    }
                    goto Label_01AB;

                default:
                    goto Label_01DE;
            }
            if (this.$this.Engine.IsWaitBootLoading)
            {
                this.$current = null;
                if (!this.$disposing)
                {
                    this.$PC = 1;
                }
            }
            else
            {
                this.$current = null;
                if (!this.$disposing)
                {
                    this.$PC = 2;
                }
            }
            goto Label_01E0;
        Label_01AB:
            while (!AssetFileManager.IsDownloadEnd())
            {
                this.$current = null;
                if (!this.$disposing)
                {
                    this.$PC = 3;
                }
                goto Label_01E0;
            }
            this.$this.loadingBarRoot.get_gameObject().SetActive(false);
            this.$this.ChangeState(UtageUguiLoadWait.State.DownlodFinished);
            this.$PC = -1;
        Label_01DE:
            return false;
        Label_01E0:
            return true;
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

    private enum State
    {
        Start,
        Downloding,
        DownlodFinished
    }

    private enum Type
    {
        Default,
        Boot,
        ChapterDownload
    }
}

