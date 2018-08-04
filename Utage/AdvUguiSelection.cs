namespace Utage
{
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.UI;

    [AddComponentMenu("Utage/ADV/UiSelection")]
    public class AdvUguiSelection : MonoBehaviour
    {
        private AdvSelection data;
        public Text text;

        public void Init(AdvSelection data, Action<AdvUguiSelection> ButtonClickedEvent)
        {
            <Init>c__AnonStorey0 storey = new <Init>c__AnonStorey0 {
                ButtonClickedEvent = ButtonClickedEvent,
                $this = this
            };
            this.data = data;
            this.text.set_text(data.Text);
            base.GetComponent<Button>().get_onClick().AddListener(new UnityAction(storey, (IntPtr) this.<>m__0));
        }

        public void OnInitSelected(Color color)
        {
            this.text.set_color(color);
        }

        public AdvSelection Data
        {
            get
            {
                return this.data;
            }
        }

        [CompilerGenerated]
        private sealed class <Init>c__AnonStorey0
        {
            internal AdvUguiSelection $this;
            internal Action<AdvUguiSelection> ButtonClickedEvent;

            internal void <>m__0()
            {
                this.ButtonClickedEvent(this.$this);
            }
        }
    }
}

