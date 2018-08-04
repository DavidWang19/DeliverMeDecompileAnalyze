namespace Utage
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    [AddComponentMenu("Utage/ADV/Internal/SystemSaveData")]
    public class AdvSystemSaveData : MonoBehaviour
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <Path>k__BackingField;
        [SerializeField]
        private string directoryName = "Save";
        [SerializeField]
        private bool dontUseSystemSaveData;
        private AdvEngine engine;
        [SerializeField]
        private Utage.FileIOManager fileIOManager;
        [SerializeField]
        private string fileName = "system";
        private AdvGallerySaveData galleryData = new AdvGallerySaveData();
        [SerializeField]
        private bool isAutoSaveOnQuit = true;
        private bool isInit;
        private static readonly int MagicID = FileIOManagerBase.ToMagicID('S', 'y', 's', 't');
        private AdvReadHistorySaveData readData = new AdvReadHistorySaveData();
        private AdvSelectedHistorySaveData selectionData = new AdvSelectedHistorySaveData();
        private const int Version = 4;

        private void AutoSave()
        {
            if (this.isAutoSaveOnQuit)
            {
                this.Write();
            }
        }

        public void Delete()
        {
            this.FileIOManager.Delete(this.Path);
        }

        public void Init(AdvEngine engine)
        {
            this.engine = engine;
            if (!this.TryReadSaveData())
            {
                this.InitDefault();
            }
            this.isInit = true;
        }

        private void InitDefault()
        {
            this.engine.Config.InitDefault();
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
            this.Delete();
            this.isAutoSaveOnQuit = false;
        }

        private void ReadBinary(BinaryReader reader)
        {
            if (reader.ReadInt32() != MagicID)
            {
                throw new Exception("Read File Id Error");
            }
            int num2 = reader.ReadInt32();
            if (num2 == 4)
            {
                BinaryBuffer.Read(reader, this.DataList);
            }
            else
            {
                object[] args = new object[] { num2 };
                throw new Exception(LanguageErrorMsg.LocalizeTextFormat(ErrorMsg.UnknownVersion, args));
            }
        }

        private bool TryReadSaveData()
        {
            if (this.DontUseSystemSaveData)
            {
                return false;
            }
            string[] args = new string[] { FileIOManagerBase.SdkPersistentDataPath, this.DirectoryName };
            string path = FilePathUtil.Combine(args);
            this.FileIOManager.CreateDirectory(path);
            string[] textArray2 = new string[] { path, this.FileName };
            this.Path = FilePathUtil.Combine(textArray2);
            if (!this.FileIOManager.Exists(this.Path))
            {
                return false;
            }
            return this.FileIOManager.ReadBinaryDecode(this.Path, new Action<BinaryReader>(this.ReadBinary));
        }

        public void Write()
        {
            if (!this.DontUseSystemSaveData && this.isInit)
            {
                this.FileIOManager.WriteBinaryEncode(this.Path, new Action<BinaryWriter>(this.WriteBinary));
            }
        }

        private void WriteBinary(BinaryWriter writer)
        {
            writer.Write(MagicID);
            writer.Write(4);
            BinaryBuffer.Write(writer, this.DataList);
        }

        private List<IBinaryIO> DataList
        {
            get
            {
                return new List<IBinaryIO> { this.ReadData, this.SelectionData, this.Engine.Config, this.GalleryData, this.Engine.Param.SystemData };
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

        public bool DontUseSystemSaveData
        {
            get
            {
                return this.dontUseSystemSaveData;
            }
        }

        private AdvEngine Engine
        {
            get
            {
                return this.engine;
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

        public AdvGallerySaveData GalleryData
        {
            get
            {
                return this.galleryData;
            }
        }

        public string Path { get; private set; }

        public AdvReadHistorySaveData ReadData
        {
            get
            {
                return this.readData;
            }
        }

        public AdvSelectedHistorySaveData SelectionData
        {
            get
            {
                return this.selectionData;
            }
        }
    }
}

