namespace Utage
{
    using System;
    using UnityEngine;
    using UnityEngine.Events;

    [AddComponentMenu("Utage/ADV/Extra/SelectionTimeLimit")]
    public class AdvSelectionTimeLimit : MonoBehaviour
    {
        [SerializeField]
        private bool disable;
        [SerializeField]
        protected AdvEngine engine;
        public float limitTime = 10f;
        [SerializeField]
        protected AdvUguiSelection selection;
        private float time;
        public int timeLimitIndex = -1;

        private void Awake()
        {
            this.Engine.SelectionManager.OnBeginWaitInput.AddListener(new UnityAction<AdvSelectionManager>(this, (IntPtr) this.OnBeginWaitInput));
            this.Engine.SelectionManager.OnUpdateWaitInput.AddListener(new UnityAction<AdvSelectionManager>(this, (IntPtr) this.OnUpdateWaitInput));
        }

        private void OnBeginWaitInput(AdvSelectionManager selection)
        {
            this.time = -Time.get_deltaTime();
        }

        private void OnUpdateWaitInput(AdvSelectionManager selection)
        {
            this.time += Time.get_deltaTime();
            if ((this.time >= this.limitTime) && this.Engine.SelectionManager.IsWaitInput)
            {
                if (this.timeLimitIndex < 0)
                {
                    if (this.Selection != null)
                    {
                        selection.Select(this.Selection.Data);
                    }
                }
                else
                {
                    selection.Select(this.timeLimitIndex);
                }
            }
        }

        public bool Disable
        {
            get
            {
                return this.disable;
            }
            set
            {
                this.disable = value;
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

        public AdvUguiSelection Selection
        {
            get
            {
                if (this.selection == null)
                {
                }
                return (this.selection = base.GetComponent<AdvUguiSelection>());
            }
        }

        public float TimeCount
        {
            get
            {
                return this.time;
            }
        }
    }
}

