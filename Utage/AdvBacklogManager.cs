namespace Utage
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using UnityEngine;

    //剧情回顾管理器，就是鼠标滚轮往上滚出现的那个
    [AddComponentMenu("Utage/ADV/Internal/BacklogManager")]
    public class AdvBacklogManager : MonoBehaviour, IAdvSaveData, IBinaryIO
    {
        private List<AdvBacklog> backlogs = new List<AdvBacklog>();
        [SerializeField]
        private bool ignoreLog;
        [SerializeField]
        private int maxLog = 10;
        [SerializeField]
        private BacklogEvent onAddData = new BacklogEvent();
        [SerializeField]
        private BacklogEvent onAddPage = new BacklogEvent();
        private const int Version = 0;

        internal void AddCurrentPageLog(AdvCommandText dataInPage, AdvCharacterInfo characterInfo)
        {
            this.onAddData.Invoke(this);
            if (!this.IgnoreLog)
            {
                AdvBacklog lastLog = this.LastLog;
                if (lastLog != null)
                {
                    lastLog.AddData(dataInPage, characterInfo);
                }
            }
        }

        internal void AddIMLog(string logText, string characterName)
        {
            if (!this.IgnoreLog)
            {
                AdvBacklog lastLog = this.LastLog;
                if (lastLog != null)
                {
                    lastLog.AddData(logText, characterName);
                }
            }
        }

        private void AddLog(AdvBacklog log)
        {
            if (!this.IgnoreLog)
            {
                this.backlogs.Add(log);
                if (this.backlogs.Count > this.MaxLog)
                {
                    this.backlogs.RemoveAt(0);
                }
            }
        }

        internal void AddPage()
        {
            this.onAddPage.Invoke(this);
            if (!this.IgnoreLog)
            {
                this.AddLog(new AdvBacklog());
            }
        }

        public void Clear()
        {
            this.backlogs.Clear();
        }

        public void OnClear()
        {
            this.Clear();
        }

        public void OnRead(BinaryReader reader)
        {
            int num = reader.ReadInt32();
            if (num == 0)
            {
                int num2 = reader.ReadInt32();
                for (int i = 0; i < num2; i++)
                {
                    AdvBacklog log = new AdvBacklog();
                    log.Read(reader);
                    if (!log.IsEmpty)
                    {
                        this.AddLog(log);
                    }
                }
            }
            else
            {
                object[] args = new object[] { num };
                Debug.LogError(LanguageErrorMsg.LocalizeTextFormat(ErrorMsg.UnknownVersion, args));
            }
        }

        public void OnWrite(BinaryWriter writer)
        {
            writer.Write(0);
            writer.Write(this.Backlogs.Count);
            foreach (AdvBacklog backlog in this.Backlogs)
            {
                backlog.Write(writer);
            }
        }

        public List<AdvBacklog> Backlogs
        {
            get
            {
                return this.backlogs;
            }
        }

        public bool IgnoreLog
        {
            get
            {
                return this.ignoreLog;
            }
            set
            {
                this.ignoreLog = value;
            }
        }

        public AdvBacklog LastLog
        {
            get
            {
                if (this.Backlogs.Count <= 0)
                {
                    return null;
                }
                return this.Backlogs[this.Backlogs.Count - 1];
            }
        }

        public int MaxLog
        {
            get
            {
                return this.maxLog;
            }
        }

        public BacklogEvent OnAddData
        {
            get
            {
                return this.onAddData;
            }
        }

        public BacklogEvent OnAddPage
        {
            get
            {
                return this.onAddPage;
            }
        }

        public string SaveKey
        {
            get
            {
                return "BacklogManager";
            }
        }
    }
}

