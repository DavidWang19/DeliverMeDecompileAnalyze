namespace Utage
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using UnityEngine;

    public class AdvGallerySaveData : IBinaryIO
    {
        private List<string> eventCGLabels = new List<string>();
        private List<string> eventSceneLabels = new List<string>();
        private const int VERSION = 0;

        public void AddCgLabel(string label)
        {
            if (!this.CheckCgLabel(label))
            {
                this.eventCGLabels.Add(label);
            }
        }

        public void AddSceneLabel(string label)
        {
            if (!this.CheckSceneLabels(label))
            {
                this.eventSceneLabels.Add(label);
            }
        }

        public bool CheckCgLabel(string label)
        {
            return this.eventCGLabels.Contains(label);
        }

        public bool CheckSceneLabels(string label)
        {
            return this.eventSceneLabels.Contains(label);
        }

        public void OnRead(BinaryReader reader)
        {
            int num = reader.ReadInt32();
            if (num == 0)
            {
                this.eventSceneLabels.Clear();
                int num2 = reader.ReadInt32();
                for (int i = 0; i < num2; i++)
                {
                    this.eventSceneLabels.Add(reader.ReadString());
                }
                this.eventCGLabels.Clear();
                num2 = reader.ReadInt32();
                for (int j = 0; j < num2; j++)
                {
                    this.eventCGLabels.Add(reader.ReadString());
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
            writer.Write(this.eventSceneLabels.Count);
            foreach (string str in this.eventSceneLabels)
            {
                writer.Write(str);
            }
            writer.Write(this.eventCGLabels.Count);
            foreach (string str2 in this.eventCGLabels)
            {
                writer.Write(str2);
            }
        }

        public string SaveKey
        {
            get
            {
                return "AdvGallerySaveData";
            }
        }
    }
}

