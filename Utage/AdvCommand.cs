namespace Utage
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public abstract class AdvCommand
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private AdvScenarioThread <CurrentTread>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private AdvEntityData <EntityData>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <Id>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static bool <IsEditorErrorCheckWaitType>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private StringGridRow <RowData>k__BackingField;
        private static bool isEditorErrorCheck;
        private List<AssetFile> loadFileList;

        protected AdvCommand(StringGridRow row)
        {
            this.RowData = row;
        }

        public void AddLoadFile(AssetFile file)
        {
            if (!this.IsEntityType)
            {
                this.AddLoadFileSub(file);
            }
        }

        public AssetFile AddLoadFile(string path, IAssetFileSettingData settingData)
        {
            if (this.IsEntityType)
            {
                return null;
            }
            return this.AddLoadFileSub(AssetFileManager.GetFileCreateIfMissing(path, settingData));
        }

        private AssetFile AddLoadFileSub(AssetFile file)
        {
            if (this.loadFileList == null)
            {
                this.loadFileList = new List<AssetFile>();
            }
            if (file == null)
            {
                if (!IsEditorErrorCheck)
                {
                    Debug.LogError("file is not found");
                }
                return file;
            }
            this.loadFileList.Add(file);
            return file;
        }

        public void AddLoadGraphic(AdvGraphicInfo graphic)
        {
            if ((graphic != null) && !this.IsEntityType)
            {
                this.AddLoadFileSub(graphic.File);
                if (graphic.SettingData is AdvCharacterSettingData)
                {
                    AdvCharacterSettingData settingData = graphic.SettingData as AdvCharacterSettingData;
                    if ((settingData.Icon != null) && (settingData.Icon.File != null))
                    {
                        this.AddLoadFileSub(settingData.Icon.File);
                    }
                }
            }
        }

        public void AddLoadGraphic(graphic graphic)
        {
            foreach (AdvGraphicInfo info in graphic.InfoList)
            {
                this.AddLoadGraphic(info);
            }
        }

        public abstract void DoCommand(AdvEngine engine);
        public void Download(AdvDataManager dataManager)
        {
            if (this.loadFileList != null)
            {
                foreach (AssetFile file in this.loadFileList)
                {
                    AssetFileManager.Download(file);
                }
            }
        }

        public virtual string[] GetExtraCommandIdArray(AdvCommand next)
        {
            return null;
        }

        public virtual string[] GetJumpLabels()
        {
            return null;
        }

        public virtual void InitFromPageData(AdvScenarioPageData pageData)
        {
        }

        public bool IsEmptyCell(string name)
        {
            return this.RowData.IsEmptyCell(name);
        }

        public bool IsEmptyCell(AdvColumnName name)
        {
            return this.IsEmptyCell(name.QuickToString());
        }

        public bool IsExistLoadFile()
        {
            return ((this.loadFileList != null) && (this.loadFileList.Count > 0));
        }

        public bool IsLoadEnd()
        {
            if (this.loadFileList != null)
            {
                foreach (AssetFile file in this.loadFileList)
                {
                    if (!file.IsLoadEnd)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public virtual bool IsTypePage()
        {
            return false;
        }

        public virtual bool IsTypePageEnd()
        {
            return false;
        }

        public void Load()
        {
            if (this.loadFileList != null)
            {
                foreach (AssetFile file in this.loadFileList)
                {
                    AssetFileManager.Load(file, this);
                }
            }
        }

        public T ParseCell<T>(string name)
        {
            return this.RowData.ParseCell<T>(name);
        }

        public T ParseCell<T>(AdvColumnName name)
        {
            return this.ParseCell<T>(name.QuickToString());
        }

        public T[] ParseCellArray<T>(string name)
        {
            return this.RowData.ParseCellArray<T>(name);
        }

        public T[] ParseCellArray<T>(AdvColumnName name)
        {
            return this.ParseCellArray<T>(name.QuickToString());
        }

        public string ParseCellLocalizedText()
        {
            string columnName = AdvColumnName.Text.QuickToString();
            if ((LanguageManagerBase.Instance != null) && this.RowData.Grid.ContainsColumn(LanguageManagerBase.Instance.CurrentLanguage))
            {
                columnName = LanguageManagerBase.Instance.CurrentLanguage;
            }
            if (this.RowData.IsEmptyCell(columnName))
            {
                return this.RowData.ParseCellOptional<string>(AdvColumnName.Text.QuickToString(), string.Empty);
            }
            return this.RowData.ParseCellOptional<string>(columnName, string.Empty);
        }

        public T ParseCellOptional<T>(string name, T defaultVal)
        {
            return this.RowData.ParseCellOptional<T>(name, defaultVal);
        }

        public T ParseCellOptional<T>(AdvColumnName name, T defaultVal)
        {
            return this.ParseCellOptional<T>(name.QuickToString(), defaultVal);
        }

        public T[] ParseCellOptionalArray<T>(string name, T[] defaultArray)
        {
            return this.RowData.ParseCellOptionalArray<T>(name, defaultArray);
        }

        public T[] ParseCellOptionalArray<T>(AdvColumnName name, T[] defaultArray)
        {
            return this.ParseCellOptionalArray<T>(name.QuickToString(), defaultArray);
        }

        public T? ParseCellOptionalNull<T>(string name) where T: struct
        {
            if (this.IsEmptyCell(name))
            {
                return null;
            }
            return new T?(this.RowData.ParseCell<T>(name));
        }

        public T? ParseCellOptionalNull<T>(AdvColumnName name) where T: struct
        {
            return this.ParseCellOptionalNull<T>(name.QuickToString());
        }

        public string ParseScenarioLabel(AdvColumnName name)
        {
            string str;
            if (!AdvCommandParser.TryParseScenarioLabel(this.RowData, name, out str))
            {
                object[] args = new object[] { this.ParseCell<string>(name) };
                Debug.LogError(this.ToErrorString(LanguageAdvErrorMsg.LocalizeTextFormat(AdvErrorMsg.NotScenarioLabel, args)));
            }
            return str;
        }

        public string ToErrorString(string msg)
        {
            return this.RowData.ToErrorString(msg);
        }

        public bool TryParseCell<T>(string name, out T val)
        {
            return this.RowData.TryParseCell<T>(name, out val);
        }

        public bool TryParseCell<T>(AdvColumnName name, out T val)
        {
            return this.TryParseCell<T>(name.QuickToString(), out val);
        }

        public bool TryParseCellArray<T>(string name, out T[] array)
        {
            return this.RowData.TryParseCellArray<T>(name, out array);
        }

        public bool TryParseCellArray<T>(AdvColumnName name, out T[] array)
        {
            return this.TryParseCellArray<T>(name.QuickToString(), out array);
        }

        public void Unload()
        {
            if (this.loadFileList != null)
            {
                foreach (AssetFile file in this.loadFileList)
                {
                    file.Unuse(this);
                }
            }
        }

        public virtual bool Wait(AdvEngine engine)
        {
            return false;
        }

        public AdvScenarioThread CurrentTread { get; set; }

        internal AdvEntityData EntityData { get; set; }

        internal string Id { get; set; }

        public static bool IsEditorErrorCheck
        {
            get
            {
                return isEditorErrorCheck;
            }
            set
            {
                isEditorErrorCheck = value;
            }
        }

        public static bool IsEditorErrorCheckWaitType
        {
            [CompilerGenerated]
            get
            {
                return <IsEditorErrorCheckWaitType>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <IsEditorErrorCheckWaitType>k__BackingField = value;
            }
        }

        public bool IsEntityType
        {
            get
            {
                return (this.EntityData != null);
            }
        }

        public virtual bool IsIfCommand
        {
            get
            {
                return false;
            }
        }

        public List<AssetFile> LoadFileList
        {
            get
            {
                return this.loadFileList;
            }
        }

        public StringGridRow RowData { get; set; }
    }
}

