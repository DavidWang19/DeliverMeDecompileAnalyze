namespace Utage
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UtageExtensions;

    [Serializable]
    public class AdvSaveData
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private DateTime <Date>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <FileVersion>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <Path>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <Title>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private SaveDataType <Type>k__BackingField;
        public BinaryBuffer Buffer = new BinaryBuffer();
        private static readonly int MagicID = FileIOManagerBase.ToMagicID('S', 'a', 'v', 'e');
        private Texture2D texture;
        public const int Version = 10;

        public AdvSaveData(SaveDataType type, string path)
        {
            this.Type = type;
            this.Path = path;
            this.Clear();
        }

        public void Clear()
        {
            this.Buffer = new BinaryBuffer();
            if (this.Texture != null)
            {
                Object.Destroy(this.Texture);
            }
            this.Texture = null;
            this.FileVersion = -1;
            this.Title = string.Empty;
        }

        public void LoadGameData(AdvEngine engine, List<IBinaryIO> customSaveIoList, List<IBinaryIO> saveIoList)
        {
            this.Buffer.Overrirde(engine.Param.DefaultData);
            this.Buffer.Overrirde((IBinaryIO) engine.GraphicManager);
            this.Buffer.Overrirde((IBinaryIO) engine.CameraManager);
            this.Buffer.Overrirde((IBinaryIO) engine.SoundManager);
            this.Buffer.Overrirde(customSaveIoList);
            this.Buffer.Overrirde(saveIoList);
        }

        public void Read(BinaryReader reader)
        {
            this.Clear();
            if (reader.ReadInt32() != MagicID)
            {
                throw new Exception("Read File Id Error");
            }
            int num2 = reader.ReadInt32();
            if (num2 >= 10)
            {
                this.FileVersion = num2;
                this.Date = new DateTime(reader.ReadInt64());
                int count = reader.ReadInt32();
                if (count > 0)
                {
                    byte[] buffer = reader.ReadBytes(count);
                    Texture2D textured = new Texture2D(1, 1, 3, false);
                    ImageConversion.LoadImage(textured, buffer);
                    this.Texture = textured;
                }
                else
                {
                    this.Texture = null;
                }
                this.Title = reader.ReadString();
                this.Buffer.Read(reader);
            }
            else
            {
                this.Clear();
                object[] args = new object[] { num2 };
                throw new Exception(LanguageErrorMsg.LocalizeTextFormat(ErrorMsg.UnknownVersion, args));
            }
        }

        public AdvParamManager ReadParam(AdvEngine engine)
        {
            AdvParamManager manager = new AdvParamManager();
            manager.InitDefaultAll(engine.DataManager.SettingDataManager.DefaultParam);
            this.Buffer.Overrirde(manager.DefaultData);
            return manager;
        }

        public void SaveGameData(AdvSaveData autoSave, AdvEngine engine, Texture2D tex)
        {
            this.Clear();
            this.Buffer = autoSave.Buffer.Clone();
            this.Date = DateTime.Now;
            this.Texture = tex;
            this.FileVersion = autoSave.FileVersion;
            this.Title = autoSave.Title;
            this.Title = engine.MessageWindowManager.CurrentWindow.Text.OriginalText.Substring(0, engine.MessageWindowManager.CurrentWindow.Text.OriginalText.Length / 2);
        }

        public void UpdateAutoSaveData(AdvEngine engine, Texture2D tex, List<IBinaryIO> customSaveIoList, List<IBinaryIO> saveIoList)
        {
            this.Clear();
            List<IBinaryIO> ioList = new List<IBinaryIO> {
                engine.ScenarioPlayer,
                engine.Param.DefaultData,
                engine.GraphicManager,
                engine.CameraManager,
                engine.SoundManager
            };
            ioList.AddRange(customSaveIoList);
            ioList.AddRange(saveIoList);
            this.Buffer.MakeBuffer(ioList);
            this.Date = DateTime.Now;
            this.Texture = tex;
            this.Title = engine.Page.SaveDataTitle;
        }

        public void Write(BinaryWriter writer)
        {
            this.Date = DateTime.Now;
            writer.Write(MagicID);
            writer.Write(10);
            writer.Write(this.Date.Ticks);
            if (this.Texture != null)
            {
                byte[] bytes = ImageConversion.EncodeToPNG(this.Texture);
                writer.WriteBuffer(bytes);
            }
            else
            {
                writer.Write(0);
            }
            writer.Write(this.Title);
            this.Buffer.Write(writer);
        }

        public DateTime Date { get; set; }

        public int FileVersion { get; private set; }

        public bool IsSaved
        {
            get
            {
                return !this.Buffer.IsEmpty;
            }
        }

        public string Path { get; private set; }

        public Texture2D Texture
        {
            get
            {
                return this.texture;
            }
            set
            {
                this.texture = value;
                if ((this.texture != null) && (this.texture.get_wrapMode() != 1))
                {
                    this.texture.set_wrapMode(1);
                }
            }
        }

        public string Title { get; private set; }

        public SaveDataType Type { get; private set; }

        public enum SaveDataType
        {
            Default,
            Quick,
            Auto
        }
    }
}

