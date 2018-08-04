namespace Utage
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public abstract class AssetFileBase : AssetFile
    {
        [CompilerGenerated]
        private static Predicate<object> <>f__am$cache0;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private AssetFileInfo <FileInfo>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private AssetFileManager <FileManager>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private AssetFileType <FileType>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <IgnoreUnload>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <IsLoadEnd>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <IsLoadError>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <LoadErrorMsg>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private AssetFileLoadPriority <Priority>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private IAssetFileSettingData <SettingData>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private AudioClip <Sound>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private TextAsset <Text>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Texture2D <Texture>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Object <UnityObject>k__BackingField;
        protected HashSet<object> referenceSet = new HashSet<object>();

        public AssetFileBase(AssetFileManager mangager, AssetFileInfo fileInfo, IAssetFileSettingData settingData)
        {
            this.FileManager = mangager;
            this.FileInfo = fileInfo;
            this.FileType = fileInfo.FileType;
            this.SettingData = settingData;
            this.Priority = AssetFileLoadPriority.DownloadOnly;
        }

        public virtual void AddReferenceComponent(GameObject go)
        {
            go.AddComponent<AssetFileReference>().Init(this);
        }

        public abstract bool CheckCacheOrLocal();
        public abstract IEnumerator LoadAsync(Action onComplete, Action onFailed);
        internal void LoadDummy(AssetFileDummyOnLoadError dummyFiles)
        {
            this.IgnoreUnload = true;
            this.IsLoadEnd = true;
            this.IsLoadError = false;
            switch (this.FileType)
            {
                case AssetFileType.Text:
                    this.Text = dummyFiles.text;
                    break;

                case AssetFileType.Texture:
                    this.Texture = dummyFiles.texture;
                    break;

                case AssetFileType.Sound:
                    this.Sound = dummyFiles.sound;
                    break;

                case AssetFileType.UnityObject:
                    this.UnityObject = dummyFiles.asset;
                    break;
            }
        }

        protected virtual string ParseLoadPath()
        {
            switch (this.FileInfo.StrageType)
            {
                case AssetFileStrageType.Server:
                case AssetFileStrageType.StreamingAssets:
                    if (this.FileInfo.AssetBundleInfo != null)
                    {
                        return FilePathUtil.EncodeUrl(FilePathUtil.ToCacheClearUrl(this.FileInfo.AssetBundleInfo.Url));
                    }
                    Debug.LogError("Not found in assetbundle list " + this.FileName);
                    return FilePathUtil.EncodeUrl(this.FileName);
            }
            return this.FileName;
        }

        internal virtual void ReadyToLoad(AssetFileLoadPriority loadPriority, object referenceObj)
        {
            if (loadPriority < this.Priority)
            {
                this.Priority = loadPriority;
            }
            this.Use(referenceObj);
        }

        public abstract void Unload();
        public virtual void Unuse(object referenceObj)
        {
            if (referenceObj != null)
            {
                this.referenceSet.Remove(referenceObj);
            }
        }

        public virtual void Use(object referenceObj)
        {
            if (referenceObj != null)
            {
                this.referenceSet.Add(referenceObj);
            }
        }

        public AssetFileInfo FileInfo { get; private set; }

        protected AssetFileManager FileManager { get; private set; }

        public virtual string FileName
        {
            get
            {
                return this.FileInfo.FileName;
            }
        }

        public virtual AssetFileType FileType { get; protected set; }

        protected internal bool IgnoreUnload { get; protected set; }

        public bool IsLoadEnd { get; protected set; }

        public bool IsLoadError { get; protected set; }

        public string LoadErrorMsg { get; protected set; }

        protected internal AssetFileLoadPriority Priority { get; protected set; }

        internal int ReferenceCount
        {
            get
            {
                if (this.referenceSet.Contains(null))
                {
                    if (<>f__am$cache0 == null)
                    {
                        <>f__am$cache0 = s => s == null;
                    }
                    this.referenceSet.RemoveWhere(<>f__am$cache0);
                    Debug.LogError(LanguageErrorMsg.LocalizeTextFormat(ErrorMsg.FileReferecedIsNull, new object[0]));
                }
                return this.referenceSet.Count;
            }
        }

        public IAssetFileSettingData SettingData { get; private set; }

        public AudioClip Sound { get; protected set; }

        public TextAsset Text { get; protected set; }

        public Texture2D Texture { get; protected set; }

        public Object UnityObject { get; protected set; }
    }
}

