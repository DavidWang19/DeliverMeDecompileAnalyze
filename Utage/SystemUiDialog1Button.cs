namespace Utage
{
    using System;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.UI;

    [AddComponentMenu("Utage/Lib/System UI/Dialog1Button")]
    public class SystemUiDialog1Button : MonoBehaviour
    {
        [SerializeField]
        protected Text button1Text;
        [SerializeField]
        protected UnityEvent OnClickButton1;
        [SerializeField]
        protected Text titleText;

        public void Close()
        {
            base.get_gameObject().SetActive(false);
        }

        public void OnClickButton1Sub()
        {
            this.OnClickButton1.Invoke();
            this.Close();
        }

        public void Open()
        {
            base.get_gameObject().SetActive(true);
        }

        public void Open(string text, string buttonText1, UnityAction callbackOnClickButton1)
        {
            this.titleText.set_text(text);
            this.button1Text.set_text(buttonText1);
            this.OnClickButton1.RemoveAllListeners();
            this.OnClickButton1.AddListener(callbackOnClickButton1);
            this.Open();
        }
    }
}

