namespace Utage
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    [AddComponentMenu("Utage/Lib/System UI/DebugPrint")]
    public class DebugPrint : MonoBehaviour
    {
        private int frame;
        private float frameRate;
        private static DebugPrint instance;
        private const float INTERVAL = 1f;
        private List<string> logList = new List<string>();
        private float memSizeGC;
        private float memSizeGraphic;
        private float memSizeMonoHeap;
        private float memSizeMonoUsedHeap;
        private float memSizeSystem;
        private float memSizeUsedHeap;
        private float oldTime;

        private void AddLog(string message)
        {
            this.AddLogSub(message);
        }

        private void AddLogError(string message)
        {
            this.AddLogSub(message);
        }

        private void AddLogSub(string message)
        {
            this.logList.Add(message);
        }

        private void AddLogWarning(string message)
        {
            this.AddLogSub(message);
        }

        private void Awake()
        {
            if (null == instance)
            {
                instance = this;
            }
        }

        private string FpsToString()
        {
            return string.Format("FPS:{0,3:#0.} Simple:{1,3:#0.00}\n", this.frameRate, 1f / Time.get_deltaTime());
        }

        public static string GetDebugString()
        {
            return (GetInstance().VersionString() + GetInstance().FpsToString() + GetInstance().MemToString());
        }

        private static DebugPrint GetInstance()
        {
            if (null == instance)
            {
                instance = Object.FindObjectOfType(typeof(DebugPrint));
            }
            return instance;
        }

        private List<string> GetLogList()
        {
            return GetInstance().logList;
        }

        public static string GetLogString()
        {
            string str = string.Empty;
            foreach (string str2 in GetInstance().logList)
            {
                str = str + str2 + "\n";
            }
            return str;
        }

        public static void Log(object message)
        {
            GetInstance().AddLog(message as string);
        }

        public static void LogError(object message)
        {
            GetInstance().AddLogError(message as string);
        }

        public static void LogException(Exception ex)
        {
            GetInstance().AddLogError(ex.Message);
        }

        public static void LogWarning(object message)
        {
            GetInstance().AddLogWarning(message as string);
        }

        private string MemToString()
        {
            string[] textArray1 = new string[] { "Mem:\nSystem ", this.memSizeSystem.ToString(), "\nGraphic ", this.memSizeGraphic.ToString(), "\nGC ", this.memSizeGC.ToString(), "\nUsedHeap ", this.memSizeUsedHeap.ToString(), "\nMonoHeap ", this.memSizeMonoHeap.ToString(), "\nMonoUsedHeap ", this.memSizeMonoUsedHeap.ToString(), "\n" };
            return string.Concat(textArray1);
        }

        private void Start()
        {
            this.oldTime = Time.get_realtimeSinceStartup();
            Debug.Log("Utage Ver 3.2.0 Start!");
        }

        private void Update()
        {
            this.UpdateFPS();
            this.UpdateMemSize();
        }

        private void UpdateFPS()
        {
            this.frame++;
            float num = Time.get_realtimeSinceStartup() - this.oldTime;
            if (num >= 1f)
            {
                this.frameRate = ((float) this.frame) / num;
                this.oldTime = Time.get_realtimeSinceStartup();
                this.frame = 0;
            }
        }

        private void UpdateMemSize()
        {
            this.memSizeSystem = SystemInfo.get_systemMemorySize();
            this.memSizeGraphic = SystemInfo.get_graphicsMemorySize();
            this.memSizeGC = ((1f * GC.GetTotalMemory(false)) / 1024f) / 1024f;
            this.memSizeUsedHeap = WrapperUnityVersion.UsedHeapMegaSize();
            this.memSizeMonoHeap = WrapperUnityVersion.MonoHeapMegaSize();
            this.memSizeMonoUsedHeap = WrapperUnityVersion.MonoUsedMegaSize();
        }

        private string VersionString()
        {
            return string.Format("Version:{0}  Unity:{1}  UTAGE:{2} \n", Application.get_version(), Application.get_unityVersion(), "3.2.0");
        }
    }
}

