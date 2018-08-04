namespace Utage
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class AdvCharacterSettingData : AdvSettingDictinoayItemBase
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private IconInfo <Icon>k__BackingField;
        public static ParseCustomFileTypeRootDir CallbackParseCustomFileTypeRootDir;
        private Utage.graphic graphic;
        private string name;
        private string nameText;
        private string pattern;

        internal void AddGraphicInfo(StringGridRow row)
        {
            this.Graphic.Add("Character", row, this);
        }

        internal void BootInit(AdvSettingDataManager dataManager)
        {
            <BootInit>c__AnonStorey0 storey = new <BootInit>c__AnonStorey0 {
                dataManager = dataManager,
                $this = this
            };
            this.Graphic.BootInit(new Func<string, string, string>(storey.<>m__0), storey.dataManager);
            this.Icon.BootInit(new Func<string, string>(storey.<>m__1));
        }

        private string FileNameToPath(string fileName, string fileType, AdvBootSetting settingData)
        {
            string rootDir = null;
            if (CallbackParseCustomFileTypeRootDir != null)
            {
                CallbackParseCustomFileTypeRootDir(fileType, ref rootDir);
                if (rootDir != null)
                {
                    string[] args = new string[] { settingData.ResourceDir, rootDir, fileName };
                    return FilePathUtil.Combine(args);
                }
            }
            return settingData.CharacterDirInfo.FileNameToPath(fileName);
        }

        internal void Init(string name, string pattern, string nameText, StringGridRow row)
        {
            this.name = name;
            this.pattern = pattern;
            base.RowData = row;
            base.InitKey(AdvCharacterSetting.ToDataKey(name, pattern));
            this.nameText = nameText;
            this.graphic = new Utage.graphic(base.Key);
            if (!AdvParser.IsEmptyCell(row, AdvColumnName.FileName))
            {
                this.AddGraphicInfo(row);
            }
            this.Icon = new IconInfo(row);
        }

        public override bool InitFromStringGridRow(StringGridRow row)
        {
            Debug.LogError("Not Use");
            return false;
        }

        public Utage.graphic Graphic
        {
            get
            {
                return this.graphic;
            }
        }

        public IconInfo Icon { get; private set; }

        public string Name
        {
            get
            {
                return this.name;
            }
        }

        public string NameText
        {
            get
            {
                return this.nameText;
            }
        }

        public string Pattern
        {
            get
            {
                return this.pattern;
            }
        }

        [CompilerGenerated]
        private sealed class <BootInit>c__AnonStorey0
        {
            internal AdvCharacterSettingData $this;
            internal AdvSettingDataManager dataManager;

            internal string <>m__0(string fileName, string fileType)
            {
                return this.$this.FileNameToPath(fileName, fileType, this.dataManager.BootSetting);
            }

            internal string <>m__1(string fileName)
            {
                return this.dataManager.BootSetting.CharacterDirInfo.FileNameToPath(fileName);
            }
        }

        public class IconInfo
        {
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private AssetFile <File>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private string <FileName>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private Rect <IconRect>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private string <IconSubFileName>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private Type <IconType>k__BackingField;

            public IconInfo(StringGridRow row)
            {
                this.FileName = AdvParser.ParseCellOptional<string>(row, AdvColumnName.Icon, string.Empty);
                if (!string.IsNullOrEmpty(this.FileName))
                {
                    if (!AdvParser.IsEmptyCell(row, AdvColumnName.IconSubFileName))
                    {
                        this.IconType = Type.DicingPattern;
                        this.IconSubFileName = AdvParser.ParseCell<string>(row, AdvColumnName.IconSubFileName);
                    }
                    else
                    {
                        this.IconType = Type.IconImage;
                    }
                }
                else if (!AdvParser.IsEmptyCell(row, AdvColumnName.IconRect))
                {
                    float[] numArray = row.ParseCellArray<float>(AdvColumnName.IconRect.QuickToString());
                    if (numArray.Length == 4)
                    {
                        this.IconType = Type.RectImage;
                        this.IconRect = new Rect(numArray[0], numArray[1], numArray[2], numArray[3]);
                    }
                    else
                    {
                        Debug.LogError(row.ToErrorString("IconRect. Array size is not 4"));
                    }
                }
                else
                {
                    this.IconType = Type.None;
                }
            }

            public void BootInit(Func<string, string> FileNameToPath)
            {
                if (!string.IsNullOrEmpty(this.FileName))
                {
                    this.File = AssetFileManager.GetFileCreateIfMissing(FileNameToPath(this.FileName), null);
                }
            }

            public AssetFile File { get; set; }

            public string FileName { get; internal set; }

            public Rect IconRect { get; internal set; }

            public string IconSubFileName { get; internal set; }

            public Type IconType { get; internal set; }

            public enum Type
            {
                None,
                IconImage,
                DicingPattern,
                RectImage
            }
        }

        public delegate void ParseCustomFileTypeRootDir(string fileType, ref string rootDir);
    }
}

