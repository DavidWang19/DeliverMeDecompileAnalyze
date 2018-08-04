namespace Utage
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.UI;

    [AddComponentMenu("Utage/Lib/UI/ToggledGroupIndexd")]
    public class UguiToggleGroupIndexed : MonoBehaviour
    {
        public bool autoToggleInteractiveOff = true;
        private int currentIndex = -1;
        public int firstIndexOnAwake;
        public bool ignoreValueChangeOnAwake = true;
        private bool isIgnoreValueChange;
        public bool isLoopShift = true;
        public Button jumpLeftEdgeButton;
        public Button jumpRightEdgeButton;
        public UguiTabButtonGroupEvent OnValueChanged;
        public Button shiftLeftButton;
        public Button shiftRightButton;
        [SerializeField]
        private List<Toggle> toggles;

        public void Add(Toggle toggle)
        {
            <Add>c__AnonStorey2 storey = new <Add>c__AnonStorey2 {
                toggle = toggle,
                $this = this
            };
            this.toggles.Add(storey.toggle);
            storey.toggle.onValueChanged.AddListener(new UnityAction<bool>(storey, (IntPtr) this.<>m__0));
        }

        private void Awake()
        {
            for (int i = 0; i < this.toggles.Count; i++)
            {
                <Awake>c__AnonStorey0 storey = new <Awake>c__AnonStorey0 {
                    $this = this,
                    toggle = this.toggles[i]
                };
                storey.toggle.onValueChanged.AddListener(new UnityAction<bool>(storey, (IntPtr) this.<>m__0));
            }
            if (this.ignoreValueChangeOnAwake)
            {
                this.currentIndex = this.firstIndexOnAwake;
            }
            this.CurrentIndex = this.firstIndexOnAwake;
            if (this.shiftLeftButton != null)
            {
                this.shiftLeftButton.get_onClick().AddListener(new UnityAction(this, (IntPtr) this.ShiftLeft));
            }
            if (this.shiftRightButton != null)
            {
                this.shiftRightButton.get_onClick().AddListener(new UnityAction(this, (IntPtr) this.ShiftRight));
            }
            if (this.jumpLeftEdgeButton != null)
            {
                this.jumpLeftEdgeButton.get_onClick().AddListener(new UnityAction(this, (IntPtr) this.JumpLeftEdge));
            }
            if (this.jumpRightEdgeButton != null)
            {
                this.jumpRightEdgeButton.get_onClick().AddListener(new UnityAction(this, (IntPtr) this.JumpRightEdge));
            }
        }

        public void ClearToggles()
        {
            this.toggles.Clear();
        }

        public void JumpLeftEdge()
        {
            if (this.Count > 0)
            {
                this.CurrentIndex = 0;
            }
        }

        public void JumpRightEdge()
        {
            if (this.Count > 0)
            {
                this.CurrentIndex = this.Count - 1;
            }
        }

        private void OnToggleValueChanged(Toggle toggle)
        {
            <OnToggleValueChanged>c__AnonStorey1 storey = new <OnToggleValueChanged>c__AnonStorey1 {
                toggle = toggle
            };
            if (!this.isIgnoreValueChange)
            {
                this.isIgnoreValueChange = true;
                this.CurrentIndex = this.toggles.FindIndex(new Predicate<Toggle>(storey.<>m__0));
                this.isIgnoreValueChange = false;
            }
        }

        public void SetActiveLRButtons(bool isActive)
        {
            if (this.shiftLeftButton != null)
            {
                this.shiftLeftButton.get_gameObject().SetActive(isActive);
            }
            if (this.shiftRightButton != null)
            {
                this.shiftRightButton.get_gameObject().SetActive(isActive);
            }
            if (this.jumpLeftEdgeButton != null)
            {
                this.jumpLeftEdgeButton.get_gameObject().SetActive(isActive);
            }
            if (this.jumpRightEdgeButton != null)
            {
                this.jumpRightEdgeButton.get_gameObject().SetActive(isActive);
            }
        }

        public void ShiftLeft()
        {
            if (this.Count > 0)
            {
                int num = this.CurrentIndex - 1;
                if (num < 0)
                {
                    num = !this.isLoopShift ? 0 : (this.Count - 1);
                }
                this.CurrentIndex = num;
            }
        }

        public void ShiftRight()
        {
            if (this.Count > 0)
            {
                int num = this.CurrentIndex + 1;
                if (num >= this.Count)
                {
                    num = !this.isLoopShift ? (this.Count - 1) : 0;
                }
                this.CurrentIndex = num;
            }
        }

        public int Count
        {
            get
            {
                return this.toggles.Count;
            }
        }

        public int CurrentIndex
        {
            get
            {
                return this.currentIndex;
            }
            set
            {
                if (value < this.toggles.Count)
                {
                    for (int i = 0; i < this.toggles.Count; i++)
                    {
                        bool flag = i == value;
                        this.toggles[i].set_isOn(flag);
                        if (this.autoToggleInteractiveOff)
                        {
                            this.toggles[i].set_interactable(!flag);
                        }
                    }
                    if (this.currentIndex != value)
                    {
                        this.currentIndex = value;
                        this.OnValueChanged.Invoke(value);
                    }
                }
            }
        }

        public Toggle[] TogglesToArray
        {
            get
            {
                return this.toggles.ToArray();
            }
        }

        [CompilerGenerated]
        private sealed class <Add>c__AnonStorey2
        {
            internal UguiToggleGroupIndexed $this;
            internal Toggle toggle;

            internal void <>m__0(bool isOn)
            {
                this.$this.OnToggleValueChanged(this.toggle);
            }
        }

        [CompilerGenerated]
        private sealed class <Awake>c__AnonStorey0
        {
            internal UguiToggleGroupIndexed $this;
            internal Toggle toggle;

            internal void <>m__0(bool isOn)
            {
                this.$this.OnToggleValueChanged(this.toggle);
            }
        }

        [CompilerGenerated]
        private sealed class <OnToggleValueChanged>c__AnonStorey1
        {
            internal Toggle toggle;

            internal bool <>m__0(Toggle obj)
            {
                return (obj == this.toggle);
            }
        }

        [Serializable]
        public class UguiTabButtonGroupEvent : UnityEvent<int>
        {
        }
    }
}

