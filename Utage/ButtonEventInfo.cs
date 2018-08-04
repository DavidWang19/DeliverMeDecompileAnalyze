namespace Utage
{
    using System;
    using UnityEngine.Events;

    public class ButtonEventInfo
    {
        public UnityAction callBackClicked;
        public string text;

        public ButtonEventInfo(string text, UnityAction callBackClicked)
        {
            this.text = text;
            this.callBackClicked = callBackClicked;
        }
    }
}

