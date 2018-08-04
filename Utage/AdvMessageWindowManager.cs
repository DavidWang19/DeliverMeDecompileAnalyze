namespace Utage
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UtageExtensions;

    [AddComponentMenu("Utage/ADV/Internal/MessageWindowManager")]
    public class AdvMessageWindowManager : MonoBehaviour, IAdvSaveData, IBinaryIO
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private AdvMessageWindow <CurrentWindow>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private AdvMessageWindow <LastWindow>k__BackingField;
        private Dictionary<string, AdvMessageWindow> activeWindows = new Dictionary<string, AdvMessageWindow>();
        private Dictionary<string, AdvMessageWindow> allWindows = new Dictionary<string, AdvMessageWindow>();
        private List<string> defaultActiveWindowNameList = new List<string>();
        private AdvEngine engine;
        private bool isInit;
        [SerializeField]
        private MessageWindowEvent onChangeActiveWindows = new MessageWindowEvent();
        [SerializeField]
        private MessageWindowEvent onChangeCurrentWindow = new MessageWindowEvent();
        [SerializeField]
        private MessageWindowEvent onReset = new MessageWindowEvent();
        [SerializeField]
        private MessageWindowEvent onTextChange = new MessageWindowEvent();
        private const int Version = 0;

        private void CalllEventActiveWindows()
        {
            foreach (AdvMessageWindow window in this.AllWindows.Values)
            {
                window.ChangeActive(this.IsActiveWindow(window.Name));
            }
            this.OnChangeActiveWindows.Invoke(this);
        }

        internal void ChangeActiveWindows(List<string> names)
        {
            this.ActiveWindows.Clear();
            foreach (string str in names)
            {
                AdvMessageWindow window;
                if (!this.AllWindows.TryGetValue(str, out window))
                {
                    Debug.LogError(str + " is not found in message windows");
                }
                else
                {
                    this.ActiveWindows.Add(str, window);
                }
            }
            this.CalllEventActiveWindows();
        }

        internal void ChangeCurrentWindow(string name)
        {
            if (!string.IsNullOrEmpty(name) && ((this.CurrentWindow == null) || (this.CurrentWindow.Name != name)))
            {
                AdvMessageWindow window;
                if (!this.ActiveWindows.TryGetValue(name, out window))
                {
                    if (!this.AllWindows.TryGetValue(name, out window))
                    {
                        Debug.LogWarning(name + "is not found in window manager");
                        name = this.DefaultActiveWindowNameList[0];
                        window = this.AllWindows[name];
                    }
                    if (this.CurrentWindow != null)
                    {
                        this.ActiveWindows.Remove(this.CurrentWindow.Name);
                    }
                    this.ActiveWindows.Add(name, window);
                    this.CalllEventActiveWindows();
                }
                this.LastWindow = this.CurrentWindow;
                this.CurrentWindow = window;
                if (this.LastWindow != null)
                {
                    this.LastWindow.ChangeCurrent(false);
                }
                this.CurrentWindow.ChangeCurrent(true);
                this.OnChangeCurrentWindow.Invoke(this);
            }
        }

        internal AdvMessageWindow FindWindow(string name)
        {
            AdvMessageWindow currentWindow = this.CurrentWindow;
            if (!string.IsNullOrEmpty(name) && !this.AllWindows.TryGetValue(name, out currentWindow))
            {
                Debug.LogError(name + "is not found in all message windows");
            }
            return currentWindow;
        }

        private void InitWindows()
        {
            IAdvMessageWindow[] componentsInChildren = base.GetComponentsInChildren<IAdvMessageWindow>(true);
            foreach (IAdvMessageWindow window in componentsInChildren)
            {
                this.allWindows.Add(window.gameObject.get_name(), new AdvMessageWindow(window));
            }
            foreach (IAdvMessageWindow window2 in componentsInChildren)
            {
                if (window2.gameObject.get_activeSelf())
                {
                    this.defaultActiveWindowNameList.Add(window2.gameObject.get_name());
                }
            }
            foreach (IAdvMessageWindow window3 in componentsInChildren)
            {
                window3.OnInit(this);
            }
            this.isInit = true;
        }

        public bool IsActiveWindow(string name)
        {
            return this.ActiveWindows.ContainsKey(name);
        }

        public bool IsCurrent(string name)
        {
            return (this.CurrentWindow.Name == name);
        }

        public virtual void OnClear()
        {
            if (this.DefaultActiveWindowNameList.Count <= 0)
            {
                Debug.LogWarning("defaultWindowNameList is zero");
            }
            else
            {
                this.ChangeActiveWindows(this.DefaultActiveWindowNameList);
                this.ChangeCurrentWindow(this.DefaultActiveWindowNameList[0]);
                foreach (AdvMessageWindow window in this.AllWindows.Values)
                {
                    window.Reset();
                }
                this.OnReset.Invoke(this);
            }
        }

        internal void OnPageTextChange(AdvPage page)
        {
            this.CurrentWindow.PageTextChange(page);
            this.OnTextChange.Invoke(this);
        }

        public virtual void OnRead(BinaryReader reader)
        {
            int num = reader.ReadInt32();
            if (num == 0)
            {
                List<string> names = new List<string>();
                int num2 = reader.ReadInt32();
                for (int i = 0; i < num2; i++)
                {
                    string item = reader.ReadString();
                    byte[] bytes = reader.ReadBytes(reader.ReadInt32());
                    names.Add(item);
                    BinaryUtil.BinaryRead(bytes, new Action<BinaryReader>(this.FindWindow(item).ReadPageData));
                }
                string name = reader.ReadString();
                this.ChangeActiveWindows(names);
                this.ChangeCurrentWindow(name);
            }
            else
            {
                object[] args = new object[] { num };
                Debug.LogError(LanguageErrorMsg.LocalizeTextFormat(ErrorMsg.UnknownVersion, args));
            }
        }

        public virtual void OnWrite(BinaryWriter writer)
        {
            writer.Write(0);
            writer.Write(this.ActiveWindows.Count);
            foreach (KeyValuePair<string, AdvMessageWindow> pair in this.ActiveWindows)
            {
                writer.Write(pair.Key);
                writer.WriteBuffer(new Action<BinaryWriter>(pair.Value.WritePageData));
            }
            string str = (this.CurrentWindow != null) ? this.CurrentWindow.Name : string.Empty;
            writer.Write(str);
        }

        public Dictionary<string, AdvMessageWindow> ActiveWindows
        {
            get
            {
                return this.activeWindows;
            }
        }

        public Dictionary<string, AdvMessageWindow> AllWindows
        {
            get
            {
                if (!this.isInit)
                {
                    this.InitWindows();
                }
                return this.allWindows;
            }
        }

        public AdvMessageWindow CurrentWindow { get; private set; }

        private List<string> DefaultActiveWindowNameList
        {
            get
            {
                if (!this.isInit)
                {
                    this.InitWindows();
                }
                return this.defaultActiveWindowNameList;
            }
        }

        public AdvEngine Engine
        {
            get
            {
                if (this.engine == null)
                {
                }
                return (this.engine = base.GetComponent<AdvEngine>());
            }
        }

        public AdvMessageWindow LastWindow { get; private set; }

        public MessageWindowEvent OnChangeActiveWindows
        {
            get
            {
                return this.onChangeActiveWindows;
            }
        }

        public MessageWindowEvent OnChangeCurrentWindow
        {
            get
            {
                return this.onChangeCurrentWindow;
            }
        }

        public MessageWindowEvent OnReset
        {
            get
            {
                return this.onReset;
            }
        }

        public MessageWindowEvent OnTextChange
        {
            get
            {
                return this.onTextChange;
            }
        }

        public string SaveKey
        {
            get
            {
                return "MessageWindowManager";
            }
        }
    }
}

