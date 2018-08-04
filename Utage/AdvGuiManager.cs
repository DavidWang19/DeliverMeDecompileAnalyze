namespace Utage
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.InteropServices;
    using UnityEngine;
    using UtageExtensions;

    [AddComponentMenu("Utage/ADV/GuiManager")]
    public class AdvGuiManager : MonoBehaviour, IAdvSaveData, IBinaryIO
    {
        [SerializeField]
        private List<GameObject> guiObjects = new List<GameObject>();
        private Dictionary<string, AdvGuiBase> objects = new Dictionary<string, AdvGuiBase>();
        private const int Version = 0;

        private void Awake()
        {
            foreach (GameObject obj2 in this.guiObjects)
            {
                if (!this.objects.ContainsKey(obj2.get_name()))
                {
                    this.objects.Add(obj2.get_name(), new AdvGuiBase(obj2));
                }
            }
        }

        public void OnClear()
        {
            foreach (AdvGuiBase base2 in this.objects.Values)
            {
                base2.Reset();
            }
        }

        public void OnRead(BinaryReader reader)
        {
            int num = reader.ReadInt32();
            if (num == 0)
            {
                int num2 = reader.ReadInt32();
                for (int i = 0; i < num2; i++)
                {
                    AdvGuiBase base2;
                    string key = reader.ReadString();
                    int count = reader.ReadInt32();
                    byte[] buffer = reader.ReadBytes(count);
                    if (this.objects.TryGetValue(key, out base2))
                    {
                        base2.ReadBuffer(buffer);
                    }
                    else
                    {
                        Debug.LogError(key + " is not found in GuiManager");
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
            writer.Write(this.objects.Count);
            foreach (string str in this.objects.Keys)
            {
                writer.Write(str);
                byte[] bytes = this.objects[str].ToBuffer();
                writer.WriteBuffer(bytes);
            }
        }

        internal bool TryGet(string name, out AdvGuiBase gui)
        {
            return this.objects.TryGetValue(name, out gui);
        }

        public Dictionary<string, AdvGuiBase> Objects
        {
            get
            {
                return this.objects;
            }
        }

        public string SaveKey
        {
            get
            {
                return "GuiManager";
            }
        }
    }
}

