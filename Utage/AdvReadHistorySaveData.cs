namespace Utage
{
    using System;
    using System.IO;
    using UnityEngine;

    public class AdvReadHistorySaveData : IBinaryIO
    {
        private DictionaryInt data = new DictionaryInt();
        private const int VERSION = 0;

        public void AddReadPage(string scenarioLabel, int page)
        {
            DictionaryKeyValueInt num;
            if (this.data.TryGetValue(scenarioLabel, out num))
            {
                if (num.value < page)
                {
                    num.value = page;
                }
            }
            else
            {
                this.data.Add(new DictionaryKeyValueInt(scenarioLabel, page));
            }
        }

        public bool CheckReadPage(string scenarioLabel, int pageNo)
        {
            DictionaryKeyValueInt num;
            return (this.data.TryGetValue(scenarioLabel, out num) && (pageNo <= num.value));
        }

        public void OnRead(BinaryReader reader)
        {
            int num = reader.ReadInt32();
            if (num == 0)
            {
                this.data.Read(reader);
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
            this.data.Write(writer);
        }

        public string SaveKey
        {
            get
            {
                return "AdvReadHistorySaveData";
            }
        }
    }
}

