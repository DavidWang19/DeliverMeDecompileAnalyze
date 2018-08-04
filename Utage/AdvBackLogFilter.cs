namespace Utage
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Events;

    [AddComponentMenu("Utage/ADV/Extra/BackLogFilter")]
    public class AdvBackLogFilter : MonoBehaviour
    {
        [SerializeField]
        private bool disable;
        [SerializeField]
        protected AdvEngine engine;
        public List<string> filterMessageWindowNames;

        public AdvBackLogFilter()
        {
            string[] collection = new string[] { "MessageWindow" };
            this.filterMessageWindowNames = new List<string>(collection);
        }

        private void Awake()
        {
            this.Engine.BacklogManager.OnAddPage.AddListener(new UnityAction<AdvBacklogManager>(this, (IntPtr) this.OnAddPage));
        }

        private void OnAddPage(AdvBacklogManager backlogManager)
        {
            backlogManager.IgnoreLog = !this.filterMessageWindowNames.Contains(this.Engine.MessageWindowManager.CurrentWindow.Name);
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
    }
}

