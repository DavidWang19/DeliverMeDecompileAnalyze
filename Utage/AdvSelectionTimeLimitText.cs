namespace Utage
{
    using System;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.UI;

    [AddComponentMenu("Utage/ADV/Extra/SelectionTimeLimitText")]
    public class AdvSelectionTimeLimitText : MonoBehaviour
    {
        [SerializeField]
        protected AdvEngine engine;
        [SerializeField]
        protected GameObject targetRoot;
        [SerializeField]
        protected Text text;
        protected AdvSelectionTimeLimit timeLimit;

        private void Awake()
        {
            this.Engine.SelectionManager.OnBeginWaitInput.AddListener(new UnityAction<AdvSelectionManager>(this, (IntPtr) this.OnBeginWaitInput));
            this.Engine.SelectionManager.OnUpdateWaitInput.AddListener(new UnityAction<AdvSelectionManager>(this, (IntPtr) this.OnUpdateWaitInput));
            this.Engine.SelectionManager.OnSelected.AddListener(new UnityAction<AdvSelectionManager>(this, (IntPtr) this.OnSelected));
            this.Engine.SelectionManager.OnClear.AddListener(new UnityAction<AdvSelectionManager>(this, (IntPtr) this.OnClear));
            this.TargetRoot.SetActive(false);
        }

        private void OnBeginWaitInput(AdvSelectionManager selection)
        {
            this.timeLimit = Object.FindObjectOfType<AdvSelectionTimeLimit>();
            if (this.timeLimit != null)
            {
                this.TargetRoot.SetActive(true);
            }
        }

        private void OnClear(AdvSelectionManager selection)
        {
            this.TargetRoot.SetActive(false);
        }

        private void OnSelected(AdvSelectionManager selection)
        {
            this.TargetRoot.SetActive(false);
        }

        private void OnUpdateWaitInput(AdvSelectionManager selection)
        {
            if (this.TargetRoot.get_activeSelf() && (this.timeLimit != null))
            {
                this.Target.set_text(string.Empty + Mathf.CeilToInt(this.timeLimit.limitTime - this.timeLimit.TimeCount));
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

        public Text Target
        {
            get
            {
                if (this.text == null)
                {
                }
                return (this.text = this.TargetRoot.GetComponentInChildren<Text>());
            }
        }

        public GameObject TargetRoot
        {
            get
            {
                if (this.targetRoot == null)
                {
                }
                return (this.targetRoot = base.get_gameObject());
            }
        }
    }
}

