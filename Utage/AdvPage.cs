namespace Utage
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Text;
    using UnityEngine;

    [AddComponentMenu("Utage/ADV/Internal/MessageWindow")]
    public class AdvPage : MonoBehaviour
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private AdvCharacterInfo <CharacterInfo>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private AdvScenarioPageData <CurrentData>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private AdvCommandText <CurrentTextDataInPage>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <CurrentTextLength>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <CurrentTextLengthMax>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <IsWaitingInputCommand>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <LastInputSendMessage>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <SaveDataTitle>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Utage.TextData <TextData>k__BackingField;
        private AdvPageController contoller = new AdvPageController();
        private float deltaTimeSendMessage;
        private AdvEngine engine;
        private bool isInputSendMessage;
        [SerializeField]
        private AdvPageEvent onBeginPage = new AdvPageEvent();
        [SerializeField]
        private AdvPageEvent onBeginText = new AdvPageEvent();
        [SerializeField]
        private AdvPageEvent onChangeStatus = new AdvPageEvent();
        [SerializeField]
        private AdvPageEvent onChangeText = new AdvPageEvent();
        [SerializeField]
        private AdvPageEvent onEndPage = new AdvPageEvent();
        [SerializeField]
        private AdvPageEvent onEndText = new AdvPageEvent();
        [SerializeField]
        private AdvPageEvent onTrigInput = new AdvPageEvent();
        [SerializeField]
        private AdvPageEvent onTrigWaitInputBrPage = new AdvPageEvent();
        [SerializeField]
        private AdvPageEvent onTrigWaitInputInPage = new AdvPageEvent();
        [SerializeField]
        private AdvPageEvent onUpdateSendChar = new AdvPageEvent();
        private PageStatus status;
        private List<AdvCommandText> textDataList = new List<AdvCommandText>();
        private float waitingTimeInput;

        public void BeginPage(AdvScenarioPageData currentPageData)
        {
            this.LastInputSendMessage = false;
            this.CurrentData = currentPageData;
            this.CurrentTextLength = 0;
            this.CurrentTextLengthMax = 0;
            this.deltaTimeSendMessage = 0f;
            this.Contoller.Clear();
            this.TextData = new Utage.TextData(string.Empty);
            this.TextDataList.Clear();
            this.SaveDataTitle = this.CurrentData.ScenarioLabelData.SaveTitle;
            if (string.IsNullOrEmpty(this.SaveDataTitle))
            {
                this.SaveDataTitle = this.TextData.NoneMetaString;
            }
            this.UpdateText();
            this.OnBeginPage.Invoke(this);
            this.Engine.UiManager.OnBeginPage();
            this.Engine.MessageWindowManager.ChangeCurrentWindow(currentPageData.MessageWindowName);
            if (!currentPageData.IsEmptyText)
            {
                this.Engine.BacklogManager.AddPage();
            }
        }

        public bool CheckReadPage()
        {
            return this.Engine.SystemSaveData.ReadData.CheckReadPage(this.ScenarioLabel, this.PageNo);
        }

        public bool CheckSkip()
        {
            return this.Engine.Config.CheckSkip(this.Engine.SystemSaveData.ReadData.CheckReadPage(this.ScenarioLabel, this.PageNo));
        }

        public void Clear()
        {
            this.Status = PageStatus.None;
            this.CurrentData = null;
            this.CurrentTextLength = 0;
            this.CurrentTextLengthMax = 0;
            this.deltaTimeSendMessage = 0f;
            this.Contoller.Clear();
        }

        public bool EnableSkip()
        {
            return (this.Engine.Config.IsSkipUnread || this.CheckReadPage());
        }

        public void EndPage()
        {
            this.Status = PageStatus.None;
            if ((this.Engine.Config.VoiceStopType == VoiceStopType.OnClick) && !this.CurrentData.IsEmptyText)
            {
                this.Engine.SoundManager.StopVoiceIgnoreLoop();
            }
            this.Engine.SystemSaveData.ReadData.AddReadPage(this.ScenarioLabel, this.PageNo);
            this.Engine.UiManager.OnEndPage();
            this.OnEndPage.Invoke(this);
            this.CurrentData = null;
            this.CurrentTextLength = 0;
            this.CurrentTextLengthMax = 0;
            this.deltaTimeSendMessage = 0f;
            this.Contoller.Clear();
        }

        private void EndSendChar()
        {
            this.OnEndText.Invoke(this);
            this.CurrentTextLength = this.CurrentTextLengthMax;
            if (this.CurrentTextDataInPage.IsPageEnd && this.Engine.SelectionManager.TryStartWaitInputIfShowing())
            {
                this.ToNextCommand();
            }
            else if (this.Contoller.IsWaitInput)
            {
                if (this.CurrentTextDataInPage.IsPageEnd)
                {
                    if (this.Engine.ScenarioPlayer.MainThread.WaitManager.IsWaitingPageEndEffect)
                    {
                        this.Status = PageStatus.WaitEffectOnEndPage;
                    }
                    else
                    {
                        this.Status = PageStatus.WaitInputBrPage;
                    }
                }
                else if (this.Engine.ScenarioPlayer.MainThread.WaitManager.IsWaitingInputEffect)
                {
                    this.Status = PageStatus.WaitEffectOnInputInPage;
                }
                else
                {
                    this.Status = PageStatus.WaitInputInPage;
                }
                this.waitingTimeInput = 0f;
            }
            else
            {
                this.ToNextCommand();
            }
        }

        public void InputSendMessage()
        {
            this.isInputSendMessage = true;
        }

        private bool IsInputSendMessage()
        {
            return (this.isInputSendMessage || this.CheckSkip());
        }

        public void OnChangeLanguage()
        {
            if (Application.get_isPlaying())
            {
                this.RemakeText();
            }
        }

        public void RemakeText()
        {
            if (this.CurrentData != null)
            {
                this.RemakeTextData();
                this.Status = PageStatus.SendChar;
                if (this.CurrentTextLength == 0)
                {
                    this.OnBeginText.Invoke(this);
                }
                if ((this.IsNoWaitAllText || this.CheckSkip()) || this.LastInputSendMessage)
                {
                    this.EndSendChar();
                }
                this.OnChangeText.Invoke(this);
                this.Engine.MessageWindowManager.OnPageTextChange(this);
                this.Engine.OnPageTextChange.Invoke(this.Engine);
            }
        }

        private void RemakeTextData()
        {
            StringBuilder builder = new StringBuilder();
            foreach (AdvCommandText text in this.TextDataList)
            {
                builder.Append(text.ParseCellLocalizedText());
                if (text.IsNextBr)
                {
                    builder.Append("\n");
                }
            }
            this.CurrentTextLengthMax = new Utage.TextData(builder.ToString()).Length;
            int num = 0;
            for (int i = 0; i < this.CurrentData.TextDataList.Count; i++)
            {
                AdvCommandText item = this.CurrentData.TextDataList[i];
                if (!this.TextDataList.Contains(item))
                {
                    num = i;
                }
            }
            for (int j = num; j < this.CurrentData.TextDataList.Count; j++)
            {
                AdvCommandText text3 = this.CurrentData.TextDataList[j];
                builder.Append(text3.ParseCellLocalizedText());
                if (text3.IsNextBr)
                {
                    builder.Append("\n");
                }
            }
            this.TextData = new Utage.TextData(builder.ToString());
        }

        private void SendChar(float timeCharSend)
        {
            if (timeCharSend <= 0f)
            {
                if (this.IsNoWaitAllText)
                {
                    this.CurrentTextLength = this.CurrentTextLengthMax;
                    return;
                }
                timeCharSend = 0f;
            }
            this.deltaTimeSendMessage += Time.get_deltaTime();
            while (this.deltaTimeSendMessage >= 0f)
            {
                this.CurrentTextLength++;
                this.deltaTimeSendMessage -= timeCharSend;
                if (this.CurrentTextLength > this.CurrentTextLengthMax)
                {
                    this.CurrentTextLength = this.CurrentTextLengthMax;
                    break;
                }
                if (this.CurrentCharData.CustomInfo.IsInterval || this.CurrentCharData.CustomInfo.IsSpeed)
                {
                    break;
                }
            }
        }

        private void ToNextCommand()
        {
            this.CurrentTextLength = this.CurrentTextLengthMax;
            if (this.CurrentTextDataInPage.IsPageEnd)
            {
                this.Status = PageStatus.None;
            }
            else
            {
                this.Status = PageStatus.OtherCommandInPage;
            }
        }

        public float ToSkippedTime(float time)
        {
            return (!this.CheckSkip() ? time : (time / this.Engine.Config.SkipSpped));
        }

        public void UpdatePageTextData(AdvCommandText text)
        {
            bool isBr = this.Contoller.IsBr;
            this.CurrentTextDataInPage = text;
            this.TextDataList.Add(text);
            this.Contoller.Update(this.CurrentTextDataInPage.PageCtrlType);
            if (isBr)
            {
                this.CurrentTextLengthMax++;
            }
            this.RemakeText();
            this.Engine.UiManager.ShowMessageWindow();
            this.Engine.BacklogManager.AddCurrentPageLog(this.CurrentTextDataInPage, this.CharacterInfo);
        }

        public void UpdatePageTextData(AdvPageControllerType pageCtrlType)
        {
            bool isBr = this.Contoller.IsBr;
            this.Contoller.Update(pageCtrlType);
            if (isBr)
            {
                this.CurrentTextLengthMax++;
            }
            if (!this.Engine.SelectionManager.TryStartWaitInputIfShowing())
            {
                this.Engine.UiManager.ShowMessageWindow();
            }
        }

        private void UpdateSendChar()
        {
            this.OnUpdateSendChar.Invoke(this);
            if (this.IsInputSendMessage() && !this.CurrentCharData.CustomInfo.IsSpeed)
            {
                this.EndSendChar();
            }
            else
            {
                float timeSendChar = this.Engine.Config.GetTimeSendChar(this.CheckReadPage());
                if (this.CurrentCharData.CustomInfo.IsSpeed && (this.CurrentCharData.CustomInfo.speed >= 0f))
                {
                    timeSendChar = this.CurrentCharData.CustomInfo.speed;
                }
                if (this.CurrentCharData.CustomInfo.IsInterval)
                {
                    timeSendChar = this.CurrentCharData.CustomInfo.Interval;
                }
                this.SendChar(timeSendChar);
                if (this.CurrentTextLength >= this.CurrentTextLengthMax)
                {
                    this.EndSendChar();
                }
            }
        }

        public void UpdateText()
        {
            this.LastInputSendMessage = false;
            switch (this.Status)
            {
                case PageStatus.SendChar:
                    this.UpdateSendChar();
                    this.LastInputSendMessage = this.isInputSendMessage;
                    break;

                case PageStatus.WaitEffectOnInputInPage:
                    this.UpdateWaitEffectOnInput();
                    break;

                case PageStatus.WaitInputInPage:
                case PageStatus.WaitInputBrPage:
                    this.UpdateWaitInput();
                    break;

                case PageStatus.WaitEffectOnEndPage:
                    this.UpdateWaitEffectOnEndPage();
                    break;
            }
            this.isInputSendMessage = false;
        }

        private void UpdateWaitEffectOnEndPage()
        {
            if (!this.Engine.ScenarioPlayer.MainThread.WaitManager.IsWaitingPageEndEffect)
            {
                this.Status = PageStatus.WaitInputBrPage;
            }
        }

        private void UpdateWaitEffectOnInput()
        {
            if (!this.Engine.ScenarioPlayer.MainThread.WaitManager.IsWaitingInputEffect)
            {
                this.Status = PageStatus.WaitInputInPage;
            }
        }

        private void UpdateWaitInput()
        {
            if ((this.Engine.Config.IsAutoBrPage && !this.Engine.SoundManager.IsPlayingVoice()) && (this.waitingTimeInput >= this.Engine.Config.AutoPageWaitTime))
            {
                this.ToNextCommand();
            }
            else if (this.IsInputSendMessage())
            {
                if (this.isInputSendMessage)
                {
                    this.OnTrigInput.Invoke(this);
                }
                if (this.Engine.Config.VoiceStopType == VoiceStopType.OnClick)
                {
                    this.Engine.SoundManager.StopVoiceIgnoreLoop();
                }
                this.ToNextCommand();
            }
            else if (!this.Engine.Config.IsAutoBrPage || !this.Engine.SoundManager.IsPlayingVoice())
            {
                this.waitingTimeInput += Time.get_deltaTime();
            }
        }

        public AdvCharacterInfo CharacterInfo { get; set; }

        public string CharacterLabel
        {
            get
            {
                return ((this.CharacterInfo != null) ? this.CharacterInfo.Label : string.Empty);
            }
        }

        public AdvPageController Contoller
        {
            get
            {
                return this.contoller;
            }
        }

        public char CurrenLipiSyncWord
        {
            get
            {
                return this.CurrentCharData.Char;
            }
        }

        public CharData CurrentCharData
        {
            get
            {
                int num = Mathf.Clamp(this.CurrentTextLength, 0, this.TextData.ParsedText.CharList.Count - 1);
                return this.TextData.ParsedText.CharList[num];
            }
        }

        public AdvScenarioPageData CurrentData { get; private set; }

        public AdvCommandText CurrentTextDataInPage { get; private set; }

        public int CurrentTextLength { get; protected set; }

        public int CurrentTextLengthMax { get; private set; }

        public AdvEngine Engine
        {
            get
            {
                if (this.engine == null)
                {
                }
                return (this.engine = base.GetComponent<AdvEngine>());
            }
        }

        private bool IsNoWaitAllText
        {
            get
            {
                if (this.TextData.IsNoWaitAll)
                {
                    return true;
                }
                if (this.TextData.ContainsSpeedTag)
                {
                    return false;
                }
                return (this.Engine.Config.GetTimeSendChar(this.CheckReadPage()) <= 0f);
            }
        }

        public bool IsSavePoint
        {
            get
            {
                return ((this.CurrentData != null) ? ((this.CurrentData.PageNo == 0) && this.CurrentData.ScenarioLabelData.IsSavePoint) : false);
            }
        }

        public bool IsSendChar
        {
            get
            {
                return (this.Status == PageStatus.SendChar);
            }
        }

        [Obsolete]
        public bool IsShowingText
        {
            get
            {
                return this.Engine.UiManager.IsShowingMessageWindow;
            }
        }

        public bool IsWaitBrPage
        {
            get
            {
                return (this.Status == PageStatus.WaitInputBrPage);
            }
        }

        public bool IsWaitingInputCommand { get; set; }

        [Obsolete("Use IsWaitingInputCommand instead")]
        public bool IsWaitingIntputCommand
        {
            get
            {
                return this.IsWaitingInputCommand;
            }
        }

        public bool IsWaitInputInPage
        {
            get
            {
                return ((this.Status == PageStatus.WaitInputInPage) || this.IsWaitingInputCommand);
            }
        }

        [Obsolete("Use IsWaitInputInPage instead")]
        public bool IsWaitIntputInPage
        {
            get
            {
                return this.IsWaitInputInPage;
            }
        }

        [Obsolete]
        public bool IsWaitPage
        {
            get
            {
                return (this.Engine.UiManager.IsShowingMessageWindow || this.Engine.SelectionManager.IsWaitInput);
            }
        }

        public bool IsWaitTextCommand
        {
            get
            {
                if (this.Engine.SelectionManager.IsWaitInput)
                {
                    return true;
                }
                switch (this.Status)
                {
                    case PageStatus.SendChar:
                    case PageStatus.WaitEffectOnInputInPage:
                    case PageStatus.WaitInputInPage:
                    case PageStatus.WaitEffectOnEndPage:
                    case PageStatus.WaitInputBrPage:
                        return true;
                }
                return false;
            }
        }

        private bool LastInputSendMessage { get; set; }

        private AdvIfManager MainThreadIfManager
        {
            get
            {
                return this.Engine.ScenarioPlayer.MainThread.IfManager;
            }
        }

        public string NameText
        {
            get
            {
                return ((this.CharacterInfo != null) ? this.CharacterInfo.LocalizeNameText : string.Empty);
            }
        }

        public AdvPageEvent OnBeginPage
        {
            get
            {
                return this.onBeginPage;
            }
        }

        public AdvPageEvent OnBeginText
        {
            get
            {
                return this.onBeginText;
            }
        }

        public AdvPageEvent OnChangeStatus
        {
            get
            {
                return this.onChangeStatus;
            }
        }

        public AdvPageEvent OnChangeText
        {
            get
            {
                return this.onChangeText;
            }
        }

        public AdvPageEvent OnEndPage
        {
            get
            {
                return this.onEndPage;
            }
        }

        public AdvPageEvent OnEndText
        {
            get
            {
                return this.onEndText;
            }
        }

        public AdvPageEvent OnTrigInput
        {
            get
            {
                return this.onTrigInput;
            }
        }

        public AdvPageEvent OnTrigWaitInputBrPage
        {
            get
            {
                return this.onTrigWaitInputBrPage;
            }
        }

        public AdvPageEvent OnTrigWaitInputInPage
        {
            get
            {
                return this.onTrigWaitInputInPage;
            }
        }

        public AdvPageEvent OnUpdateSendChar
        {
            get
            {
                return this.onUpdateSendChar;
            }
        }

        public int PageNo
        {
            get
            {
                return ((this.CurrentData != null) ? this.CurrentData.PageNo : 0);
            }
        }

        public string SaveDataTitle { get; private set; }

        public string ScenarioLabel
        {
            get
            {
                return ((this.CurrentData != null) ? this.CurrentData.ScenarioLabelData.ScenarioLabel : string.Empty);
            }
        }

        public float SkippedSpeed
        {
            get
            {
                return (!this.CheckSkip() ? 1f : this.Engine.Config.SkipSpped);
            }
        }

        public PageStatus Status
        {
            get
            {
                return this.status;
            }
            set
            {
                if (this.status != value)
                {
                    this.status = value;
                    this.OnChangeStatus.Invoke(this);
                    switch (this.Status)
                    {
                        case PageStatus.WaitInputInPage:
                            this.OnTrigWaitInputInPage.Invoke(this);
                            break;

                        case PageStatus.WaitInputBrPage:
                            this.OnTrigWaitInputBrPage.Invoke(this);
                            break;
                    }
                }
            }
        }

        public Utage.TextData TextData { get; private set; }

        private List<AdvCommandText> TextDataList
        {
            get
            {
                return this.textDataList;
            }
        }

        public enum PageStatus
        {
            None,
            SendChar,
            WaitEffectOnInputInPage,
            WaitInputInPage,
            OtherCommandInPage,
            WaitEffectOnEndPage,
            WaitInputBrPage
        }
    }
}

