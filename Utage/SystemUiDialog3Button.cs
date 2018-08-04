namespace Utage
{
    using System;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.UI;

    [AddComponentMenu("Utage/Lib/System UI/Dialog3Button")]
    public class SystemUiDialog3Button : SystemUiDialog2Button
    {
        [SerializeField]
        protected Text button3Text;
        [SerializeField]
        protected UnityEvent OnClickButton3;

        public void OnClickButton3Sub()
        {
            this.OnClickButton3.Invoke();
            base.Close();
        }

        public void Open(string text, string buttonText1, string buttonText2, string buttonText3, UnityAction callbackOnClickButton1, UnityAction callbackOnClickButton2, UnityAction callbackOnClickButton3)
        {
            this.button3Text.set_text(buttonText3);
            this.OnClickButton3.RemoveAllListeners();
            this.OnClickButton3.AddListener(callbackOnClickButton3);
            base.Open(text, buttonText1, buttonText2, callbackOnClickButton1, callbackOnClickButton2);
        }
    }
}

