namespace Utage
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.Events;

    [AddComponentMenu("Utage/Lib/System UI/SystemUi")]
    public class SystemUi : MonoBehaviour
    {
        [SerializeField]
        private SystemUiDialog1Button dialog1Button;
        [SerializeField]
        private SystemUiDialog2Button dialog2Button;
        [SerializeField]
        private SystemUiDialog3Button dialog3Button;
        [SerializeField]
        private SystemUiDialog2Button dialogGameExit;
        [SerializeField]
        private IndicatorIcon indicator;
        private static SystemUi instance;
        [SerializeField]
        private bool isEnableInputEscape = true;
        private bool isEnableTitleAniamtion = true;

        private void Awake()
        {
            if (null == instance)
            {
                instance = this;
            }
            else
            {
                Debug.LogError(LanguageErrorMsg.LocalizeTextFormat(ErrorMsg.SingletonError, new object[0]));
                Object.Destroy(this);
            }
        }

        [DebuggerHidden]
        private IEnumerator CoGameExit()
        {
            return new <CoGameExit>c__Iterator0();
        }

        public static SystemUi GetInstance()
        {
            return instance;
        }

        public void OnDialogExitGameNo()
        {
            this.IsEnableInputEscape = true;
        }

        private void OnDialogExitGameYes()
        {
            this.IsEnableInputEscape = true;
            base.StartCoroutine(this.CoGameExit());
        }

        public void OnOpenDialogExitGame()
        {
            this.IsEnableInputEscape = false;
            this.dialogGameExit.Open(LanguageSystemText.LocalizeText(SystemText.QuitGame), LanguageSystemText.LocalizeText(SystemText.Yes), LanguageSystemText.LocalizeText(SystemText.No), new UnityAction(this, (IntPtr) this.OnDialogExitGameYes), new UnityAction(this, (IntPtr) this.OnDialogExitGameNo));
        }

        public void OpenDialog(string text, List<ButtonEventInfo> buttons)
        {
            switch (buttons.Count)
            {
                case 1:
                    this.OpenDialog1Button(text, buttons[0]);
                    break;

                case 2:
                    this.OpenDialog2Button(text, buttons[0], buttons[1]);
                    break;

                case 3:
                    this.OpenDialog3Button(text, buttons[0], buttons[1], buttons[2]);
                    break;

                default:
                    Debug.LogError(" Dilog Button Count over = " + buttons.Count);
                    break;
            }
        }

        public void OpenDialog1Button(string text, ButtonEventInfo button1)
        {
            this.OpenDialog1Button(text, button1.text, button1.callBackClicked);
        }

        public void OpenDialog1Button(string text, string buttonText1, UnityAction callbackOnClickButton1)
        {
            this.dialog1Button.Open(text, buttonText1, callbackOnClickButton1);
        }

        public void OpenDialog2Button(string text, ButtonEventInfo button1, ButtonEventInfo button2)
        {
            this.OpenDialog2Button(text, button1.text, button2.text, button1.callBackClicked, button2.callBackClicked);
        }

        public void OpenDialog2Button(string text, string buttonText1, string buttonText2, UnityAction callbackOnClickButton1, UnityAction callbackOnClickButton2)
        {
            this.dialog2Button.Open(text, buttonText1, buttonText2, callbackOnClickButton1, callbackOnClickButton2);
        }

        public void OpenDialog3Button(string text, ButtonEventInfo button1, ButtonEventInfo button2, ButtonEventInfo button3)
        {
            this.OpenDialog3Button(text, button1.text, button2.text, button3.text, button1.callBackClicked, button2.callBackClicked, button3.callBackClicked);
        }

        public void OpenDialog3Button(string text, string buttonText1, string buttonText2, string buttonText3, UnityAction callbackOnClickButton1, UnityAction callbackOnClickButton2, UnityAction callbackOnClickButton3)
        {
            this.dialog3Button.Open(text, buttonText1, buttonText2, buttonText3, callbackOnClickButton1, callbackOnClickButton2, callbackOnClickButton3);
        }

        public void OpenDialogYesNo(string text, UnityAction callbackOnClickYes, UnityAction callbackOnClickNo)
        {
            this.OpenDialog2Button(text, LanguageSystemText.LocalizeText(SystemText.Yes), LanguageSystemText.LocalizeText(SystemText.No), callbackOnClickYes, callbackOnClickNo);
        }

        public void StartIndicator(Object obj)
        {
            if (this.indicator != null)
            {
                this.indicator.StartIndicator(obj);
            }
        }

        public void StopIndicator(Object obj)
        {
            if (this.indicator != null)
            {
                this.indicator.StopIndicator(obj);
            }
        }

        private void Update()
        {
            if ((this.IsEnableInputEscape && ((WrapperMoviePlayer.GetInstance() == null) || !WrapperMoviePlayer.IsPlaying())) && Input.GetKeyDown(0x1b))
            {
                this.OnOpenDialogExitGame();
            }
        }

        public bool IsEnableInputEscape
        {
            get
            {
                return this.isEnableInputEscape;
            }
            set
            {
                this.isEnableInputEscape = value;
            }
        }

        public bool IsEnableTitleAniamtion
        {
            get
            {
                return this.isEnableTitleAniamtion;
            }
            set
            {
                this.isEnableTitleAniamtion = value;
            }
        }

        [CompilerGenerated]
        private sealed class <CoGameExit>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal bool $disposing;
            internal int $PC;

            [DebuggerHidden]
            public void Dispose()
            {
            }

            public bool MoveNext()
            {
                this.$PC = -1;
                if (this.$PC == 0)
                {
                    Application.Quit();
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
    }
}

