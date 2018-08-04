namespace Utage
{
    using System;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.UI;

    [AddComponentMenu("Utage/Lib/System UI/Dialog2Button")]
    public class SystemUiDialog2Button : SystemUiDialog1Button
    {
        [SerializeField]
        protected Text button2Text;
        [SerializeField]
        protected UnityEvent OnClickButton2;

        public void OnClickButton2Sub()
        {
            this.OnClickButton2.Invoke();
            base.Close();
        }

        public void Open(string text, string buttonText1, string buttonText2, UnityAction callbackOnClickButton1, UnityAction callbackOnClickButton2)
        {
            this.button2Text.set_text(buttonText2);
            this.OnClickButton2.RemoveAllListeners();
            this.OnClickButton2.AddListener(callbackOnClickButton2);
            base.Open(text, buttonText1, callbackOnClickButton1);
        }
    }
}

