namespace Utage
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    [AddComponentMenu("Utage/ADV/Internal/SaveManager")]
    public class AdvSaveManager : MonoBehaviour
    {
        private AdvSaveData autoSaveData;
        private Texture2D captureTexture;
        private AdvSaveData currentAutoSaveData;
        public List<GameObject> CustomSaveDataObjects;
        [SerializeField]
        private SaveSetting defaultSetting;
        [SerializeField]
        private string directoryName = "Save";
        [SerializeField]
        private Utage.FileIOManager fileIOManager;
        [SerializeField]
        private string fileName = "save";
        [SerializeField]
        private bool isAutoSave = true;
        private AdvSaveData quickSaveData;
        private List<AdvSaveData> saveDataList;
        private List<IBinaryIO> saveIoList;
        [SerializeField]
        private SaveType type;
        [SerializeField]
        private SaveSetting webPlayerSetting;

        private void AutoSave()
        {
            if ((this.IsAutoSave && (this.AutoSaveData != null)) && this.CurrentAutoSaveData.IsSaved)
            {
                this.FileIOManager.WriteBinaryEncode(this.CurrentAutoSaveData.Path, new Action<BinaryWriter>(this.CurrentAutoSaveData.Write));
            }
        }

        public void ClearCaptureTexture()
        {
            if (this.captureTexture != null)
            {
                Object.Destroy(this.captureTexture);
                this.captureTexture = null;
            }
        }

        public void DeleteAllSaveData()
        {
            this.DeleteSaveData(this.AutoSaveData);
            this.DeleteSaveData(this.QuickSaveData);
            foreach (AdvSaveData data in this.SaveDataList)
            {
                this.DeleteSaveData(data);
            }
        }

        public void DeleteSaveData(AdvSaveData saveData)
        {
            if (this.FileIOManager.Exists(saveData.Path))
            {
                this.FileIOManager.Delete(saveData.Path);
            }
            saveData.Clear();
        }

        public List<IBinaryIO> GetSaveIoListCreateIfMissing(AdvEngine engine)
        {
            if (this.saveIoList == null)
            {
                this.saveIoList = new List<IBinaryIO>();
                this.saveIoList.AddRange(base.GetComponentsInChildren<IAdvSaveData>(true));
            }
            return this.saveIoList;
        }

        public void Init()
        {
            this.FileIOManager.CreateDirectory(this.ToDirPath());
            this.autoSaveData = new AdvSaveData(AdvSaveData.SaveDataType.Auto, this.ToFilePath("Auto"));
            this.currentAutoSaveData = new AdvSaveData(AdvSaveData.SaveDataType.Auto, this.ToFilePath("Auto"));
            this.quickSaveData = new AdvSaveData(AdvSaveData.SaveDataType.Quick, this.ToFilePath("Quick"));
            this.saveDataList = new List<AdvSaveData>();
            for (int i = 0; i < this.SaveMax; i++)
            {
                AdvSaveData item = new AdvSaveData(AdvSaveData.SaveDataType.Default, this.ToFilePath(string.Empty + (i + 1)));
                this.saveDataList.Add(item);
            }
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                this.AutoSave();
            }
        }

        private void OnApplicationQuit()
        {
            this.AutoSave();
        }

        private void OnDeleteAllSaveDataAndQuit()
        {
            this.DeleteAllSaveData();
            this.isAutoSave = false;
        }

        public void ReadAllSaveData()
        {
            this.ReadAutoSaveData();
            this.ReadQuickSaveData();
            foreach (AdvSaveData data in this.SaveDataList)
            {
                this.ReadSaveData(data);
            }
        }

        public bool ReadAutoSaveData()
        {
            if (!this.isAutoSave)
            {
                return false;
            }
            return this.ReadSaveData(this.AutoSaveData);
        }

        public bool ReadQuickSaveData()
        {
            return this.ReadSaveData(this.QuickSaveData);
        }

        private bool ReadSaveData(AdvSaveData saveData)
        {
            return (this.FileIOManager.Exists(saveData.Path) && this.FileIOManager.ReadBinaryDecode(saveData.Path, new Action<BinaryReader>(saveData.Read)));
        }

        private string ToDirPath()
        {
            string[] args = new string[] { FileIOManagerBase.SdkPersistentDataPath, this.DirectoryName + "/" };
            return FilePathUtil.Combine(args);
        }

        private string ToFilePath(string id)
        {
            string[] args = new string[] { this.ToDirPath(), this.FileName + id };
            return FilePathUtil.Combine(args);
        }

        internal void UpdateAutoSaveData(AdvEngine engine)
        {
            this.CurrentAutoSaveData.UpdateAutoSaveData(engine, null, this.CustomSaveDataIOList, this.GetSaveIoListCreateIfMissing(engine));
        }

        public void WriteSaveData(AdvEngine engine, AdvSaveData saveData)
        {
            if (!this.CurrentAutoSaveData.IsSaved)
            {
                Debug.LogError("SaveData is Disabled");
            }
            else
            {
                saveData.SaveGameData(this.CurrentAutoSaveData, engine, UtageToolKit.CreateResizeTexture(this.CaptureTexture, this.CaptureWidth, this.CaptureHeight));
                this.FileIOManager.WriteBinaryEncode(saveData.Path, new Action<BinaryWriter>(saveData.Write));
            }
        }

        public AdvSaveData AutoSaveData
        {
            get
            {
                return this.autoSaveData;
            }
        }

        public int CaptureHeight
        {
            get
            {
                return this.defaultSetting.CaptureHeight;
            }
        }

        public Texture2D CaptureTexture
        {
            get
            {
                return this.captureTexture;
            }
            set
            {
                this.ClearCaptureTexture();
                this.captureTexture = value;
            }
        }

        public int CaptureWidth
        {
            get
            {
                return this.defaultSetting.CaptureWidth;
            }
        }

        public AdvSaveData CurrentAutoSaveData
        {
            get
            {
                return this.currentAutoSaveData;
            }
        }

        public List<IBinaryIO> CustomSaveDataIOList
        {
            get
            {
                List<IBinaryIO> list = new List<IBinaryIO>();
                foreach (GameObject obj2 in this.CustomSaveDataObjects)
                {
                    IAdvSaveData component = obj2.GetComponent<IAdvSaveData>();
                    if (component == null)
                    {
                        Debug.LogError(obj2.get_name() + "is not contains IAdvCustomSaveDataIO ", obj2);
                    }
                    else
                    {
                        list.Add(component);
                    }
                }
                return list;
            }
        }

        public string DirectoryName
        {
            get
            {
                return this.directoryName;
            }
            set
            {
                this.directoryName = value;
            }
        }

        private Utage.FileIOManager FileIOManager
        {
            get
            {
                if (this.fileIOManager == null)
                {
                }
                return (this.fileIOManager = Object.FindObjectOfType<Utage.FileIOManager>());
            }
        }

        public string FileName
        {
            get
            {
                return this.fileName;
            }
            set
            {
                this.fileName = value;
            }
        }

        public bool IsAutoSave
        {
            get
            {
                return this.isAutoSave;
            }
        }

        public AdvSaveData QuickSaveData
        {
            get
            {
                return this.quickSaveData;
            }
        }

        public List<AdvSaveData> SaveDataList
        {
            get
            {
                return this.saveDataList;
            }
        }

        private int SaveMax
        {
            get
            {
                return this.defaultSetting.SaveMax;
            }
        }

        public SaveType Type
        {
            get
            {
                return this.type;
            }
        }

        [Serializable]
        private class SaveSetting
        {
            [SerializeField]
            private int captureHeight = 0x100;
            [SerializeField]
            private int captureWidth = 0x100;
            [SerializeField]
            private int saveMax = 9;

            public int CaptureHeight
            {
                get
                {
                    return this.captureHeight;
                }
            }

            public int CaptureWidth
            {
                get
                {
                    return this.captureWidth;
                }
            }

            public int SaveMax
            {
                get
                {
                    return this.saveMax;
                }
            }
        }

        public enum SaveType
        {
            Default,
            SavePoint,
            Disable
        }
    }
}

