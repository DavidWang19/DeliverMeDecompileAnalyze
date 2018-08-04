namespace Utage
{
    using System;
    using UnityEngine;
    using UnityEngine.Events;

    [AddComponentMenu("Utage/Lib/Events/ApplicationEvent")]
    public class ApplicationEvent : MonoBehaviour
    {
        private static ApplicationEvent instance;
        public UnityEvent OnScreenSizeChanged = new UnityEvent();
        private int screenHeight;
        private int screenWidth;

        private void Awake()
        {
            instance = this;
            this.screenWidth = Screen.get_width();
            this.screenHeight = Screen.get_height();
        }

        public static ApplicationEvent Get()
        {
            if (instance == null)
            {
                GameObject obj2 = new GameObject();
                obj2.set_hideFlags(0x3d);
                instance = obj2.AddComponent<ApplicationEvent>();
            }
            return instance;
        }

        private void Update()
        {
            if ((this.screenWidth != Screen.get_width()) || (this.screenHeight != Screen.get_height()))
            {
                this.screenWidth = Screen.get_width();
                this.screenHeight = Screen.get_height();
                this.OnScreenSizeChanged.Invoke();
            }
        }
    }
}

