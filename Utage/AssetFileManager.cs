namespace Utage
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.Serialization;
    using UtageExtensions;

    [AddComponentMenu("Utage/Lib/File/AssetFileManager"), RequireComponent(typeof(Utage.StaticAssetManager))]
    public class AssetFileManager : MonoBehaviour
    {
        [SerializeField]
        private Utage.AssetBundleInfoManager assetBundleInfoManager;
        [SerializeField]
        private int autoRetryCountOnDonwloadError = 5;
        private Action<AssetFile> callbackError;
        private Utage.CustomLoadManager customLoadManager;
        [SerializeField]
        private AssetFileDummyOnLoadError dummyFiles;
        [SerializeField]
        private bool enableResourcesLoadAsync = true;
        [SerializeField, FormerlySerializedAs("fileIOManger")]
        private Utage.FileIOManager fileIOManager;
        private Dictionary<string, AssetFileBase> fileTbl;
        private static AssetFileManager instance;
        [SerializeField]
        internal bool isDebugBootDeleteChacheAll;
        [SerializeField]
        internal bool isDebugBootDeleteChacheTextAndBinary;
        [SerializeField]
        internal bool isDebugCacheFileName;
        private static bool isEditorErrorCheck;
        [SerializeField]
        internal bool isOutPutDebugLog;
        private bool isWaitingRetry;
        [SerializeField]
        private int loadFileMax = 5;
        private List<AssetFileBase> loadingFileList;
        private List<AssetFileBase> loadWaitFileList;
        [SerializeField, MinMax(0f, 100f, "min", "max")]
        private MinMaxInt rangeOfFilesOnMemory;
        [SerializeField]
        private AssetFileManagerSettings settings;
        private Utage.StaticAssetManager staticAssetManager;
        [SerializeField]
        private float timeOutDownload = 10f;
        private bool unloadingUnusedAssets;
        [SerializeField]
        private UnloadType unloadType;
        private List<AssetFileBase> usingFileList;

        public AssetFileManager()
        {
            MinMaxInt num = new MinMaxInt {
                Min = 10,
                Max = 20
            };
            this.rangeOfFilesOnMemory = num;
            this.unloadType = UnloadType.UnloadUnusedAsset;
            this.loadingFileList = new List<AssetFileBase>();
            this.loadWaitFileList = new List<AssetFileBase>();
            this.usingFileList = new List<AssetFileBase>();
            this.fileTbl = new Dictionary<string, AssetFileBase>();
        }

        internal static void AddAssetFileTypeExtensions(AssetFileType type, string[] extensions)
        {
            GetInstance().Settings.AddExtensions(type, extensions);
        }

        private void AddLoadFile(AssetFileBase file)
        {
            this.TryAddLoadingFileList(file);
        }

        private AssetFileBase AddSub(string path, IAssetFileSettingData settingData)
        {
            AssetFileBase base2;
            if (!this.fileTbl.TryGetValue(path, out base2))
            {
                if (path.Contains(" "))
                {
                    Debug.LogWarning("[" + path + "] contains white space");
                }
                AssetBundleInfo assetBundleInfo = this.AssetBundleInfoManager.FindAssetBundleInfo(path);
                AssetFileInfo fileInfo = new AssetFileInfo(path, this.settings, assetBundleInfo);
                base2 = this.StaticAssetManager.FindAssetFile(this, fileInfo, settingData);
                if (base2 == null)
                {
                    base2 = this.CustomLoadManager.Find(this, fileInfo, settingData);
                    if (base2 == null)
                    {
                        base2 = new AssetFileUtage(this, fileInfo, settingData);
                    }
                }
                this.fileTbl.Add(path, base2);
            }
            return base2;
        }

        private void AddUseList(AssetFileBase file)
        {
            if (!this.usingFileList.Contains(file))
            {
                this.usingFileList.Add(file);
            }
        }

        private void Awake()
        {
            if (null == instance)
            {
                instance = this;
            }
        }

        public static AssetFile BackGroundLoad(string path, object referenceObj)
        {
            return BackGroundLoad(GetFileCreateIfMissing(path, null), referenceObj);
        }

        public static AssetFile BackGroundLoad(AssetFile file, object referenceObj)
        {
            return GetInstance().BackGroundLoadSub(file as AssetFileBase, referenceObj);
        }

        private AssetFile BackGroundLoadSub(AssetFileBase file, object referenceObj)
        {
            this.AddUseList(file);
            file.ReadyToLoad(AssetFileLoadPriority.BackGround, referenceObj);
            this.AddLoadFile(file);
            return file;
        }

        private void CallbackFileLoadError(AssetFile file)
        {
            <CallbackFileLoadError>c__AnonStorey4 storey = new <CallbackFileLoadError>c__AnonStorey4 {
                $this = this,
                errorFile = file as AssetFileBase
            };
            string text = file.LoadErrorMsg + "\n" + file.FileName;
            Debug.LogError(text);
            if (SystemUi.GetInstance() != null)
            {
                if (this.isWaitingRetry)
                {
                    base.StartCoroutine(this.CoWaitRetry(storey.errorFile));
                }
                else
                {
                    this.isWaitingRetry = true;
                    SystemUi.GetInstance().OpenDialog1Button(text, LanguageSystemText.LocalizeText(SystemText.Retry), new UnityAction(storey, (IntPtr) this.<>m__0));
                }
            }
            else
            {
                this.ReloadFileSub(storey.errorFile);
            }
        }

        [DebuggerHidden]
        private IEnumerator CoLoadWait(AssetFileBase file, Action<AssetFile> onComplete)
        {
            return new <CoLoadWait>c__Iterator1 { file = file, onComplete = onComplete, $this = this };
        }

        public static bool ContainsStaticAsset(Object asset)
        {
            return GetInstance().StaticAssetManager.Contains(asset);
        }

        internal static int CountDownloading()
        {
            return GetInstance().CountLoading(AssetFileLoadPriority.DownloadOnly);
        }

        internal static int CountLoading()
        {
            return GetInstance().CountLoading(AssetFileLoadPriority.Preload);
        }

        private int CountLoading(AssetFileLoadPriority priority)
        {
            int num = 0;
            foreach (AssetFileBase base2 in this.loadingFileList)
            {
                if ((base2.Priority <= priority) && !base2.IsLoadEnd)
                {
                    num++;
                }
            }
            foreach (AssetFileBase base3 in this.loadWaitFileList)
            {
                if ((base3.Priority <= priority) && !base3.IsLoadEnd)
                {
                    num++;
                }
            }
            return num;
        }

        [DebuggerHidden]
        private IEnumerator CoWaitRetry(AssetFileBase file)
        {
            return new <CoWaitRetry>c__Iterator0 { file = file, $this = this };
        }

        public static void Download(string path)
        {
            Download(GetFileCreateIfMissing(path, null));
        }

        public static void Download(AssetFile file)
        {
            GetInstance().DownloadSub(file as AssetFileBase);
        }

        private void DownloadSub(AssetFileBase file)
        {
            if (!file.CheckCacheOrLocal())
            {
                file.ReadyToLoad(AssetFileLoadPriority.DownloadOnly, null);
                this.AddLoadFile(file);
            }
        }

        public static Utage.CustomLoadManager GetCustomLoadManager()
        {
            return GetInstance().CustomLoadManager;
        }

        public static AssetFile GetFileCreateIfMissing(string path, IAssetFileSettingData settingData = null)
        {
            if (!IsEditorErrorCheck)
            {
                return GetInstance().AddSub(path, settingData);
            }
            if (path.Contains(" "))
            {
                Debug.LogWarning("[" + path + "] contains white space");
            }
            return null;
        }

        public static AssetFileManager GetInstance()
        {
            if (instance == null)
            {
                instance = Object.FindObjectOfType<AssetFileManager>();
                if (instance == null)
                {
                    Debug.LogError("Not Found AssetFileManager in current scene");
                }
            }
            return instance;
        }

        private int GetTotalOnMemoryFileCount()
        {
            int count = this.loadingFileList.Count;
            foreach (AssetFileBase base2 in this.usingFileList)
            {
                if (!base2.IgnoreUnload && base2.IsLoadEnd)
                {
                    count++;
                }
            }
            return count;
        }

        public static void InitError(Action<AssetFile> callbackError)
        {
            GetInstance().CallbackError = callbackError;
        }

        public static void InitLoadTypeSetting(AssetFileManagerSettings.LoadType loadTypeSetting)
        {
            GetInstance().Settings.BootInit(loadTypeSetting);
        }

        internal static bool IsDownloadEnd()
        {
            return GetInstance().IsLoadEnd(AssetFileLoadPriority.DownloadOnly);
        }

        internal static bool IsInitialized()
        {
            return true;
        }

        internal static bool IsLoadEnd()
        {
            return GetInstance().IsLoadEnd(AssetFileLoadPriority.Preload);
        }

        private bool IsLoadEnd(AssetFileLoadPriority priority)
        {
            foreach (AssetFileBase base2 in this.loadingFileList)
            {
                if ((base2.Priority <= priority) && !base2.IsLoadEnd)
                {
                    return false;
                }
            }
            foreach (AssetFileBase base3 in this.loadWaitFileList)
            {
                if ((base3.Priority <= priority) && !base3.IsLoadEnd)
                {
                    return false;
                }
            }
            return true;
        }

        private void LateUpdate()
        {
            int totalOnMemoryFileCount = this.GetTotalOnMemoryFileCount();
            if (totalOnMemoryFileCount > this.MaxFilesOnMemory)
            {
                this.UnloadUnusedFileList(totalOnMemoryFileCount - this.MinFilesOnMemory);
            }
        }

        public static AssetFile Load(string path, object referenceObj)
        {
            return Load(GetFileCreateIfMissing(path, null), referenceObj);
        }

        public static AssetFile Load(AssetFile file, object referenceObj)
        {
            return GetInstance().LoadSub(file as AssetFileBase, referenceObj);
        }

        public static void Load(AssetFile file, Action<AssetFile> onComplete)
        {
            GetInstance().LoadSub(file as AssetFileBase, onComplete);
        }

        [DebuggerHidden]
        private IEnumerator LoadAsync(AssetFileBase file)
        {
            return new <LoadAsync>c__Iterator2 { file = file, $this = this };
        }

        private void LoadNextFile()
        {
            AssetFileBase item = null;
            foreach (AssetFileBase base3 in this.loadWaitFileList)
            {
                if (item == null)
                {
                    item = base3;
                }
                else if (base3.Priority < item.Priority)
                {
                    item = base3;
                }
            }
            if (item != null)
            {
                if (item.IsLoadEnd)
                {
                    this.loadWaitFileList.Remove(item);
                }
                else if (this.TryAddLoadingFileList(item))
                {
                    this.loadWaitFileList.Remove(item);
                }
                else
                {
                    Debug.LogError("Failed To Load file " + item.FileName);
                }
            }
        }

        private void LoadSub(AssetFileBase file, Action<AssetFile> onComplete)
        {
            base.StartCoroutine(this.CoLoadWait(file, onComplete));
        }

        private AssetFile LoadSub(AssetFileBase file, object referenceObj)
        {
            this.AddUseList(file);
            file.ReadyToLoad(AssetFileLoadPriority.Default, referenceObj);
            this.AddLoadFile(file);
            return file;
        }

        private void OnDestroy()
        {
            this.UnloadUnusedFileList(0x7fffffff);
        }

        public static void Preload(string path, object referenceObj)
        {
            Preload(GetFileCreateIfMissing(path, null), referenceObj);
        }

        public static void Preload(AssetFile file, object referenceObj)
        {
            GetInstance().PreloadSub(file as AssetFileBase, referenceObj);
        }

        private void PreloadSub(AssetFileBase file, object referenceObj)
        {
            this.AddUseList(file);
            file.ReadyToLoad(AssetFileLoadPriority.Preload, referenceObj);
            this.AddLoadFile(file);
        }

        public static void ReloadFile(AssetFile file)
        {
            GetInstance().ReloadFileSub(file as AssetFileBase);
        }

        private void ReloadFileSub(AssetFileBase file)
        {
            base.StartCoroutine(this.LoadAsync(file));
        }

        public static void SetLoadErrorCallBack(Action<AssetFile> callbackError)
        {
            GetInstance().callbackError = callbackError;
        }

        private bool TryAddLoadingFileList(AssetFileBase file)
        {
            if (!file.IsLoadEnd)
            {
                if (this.loadingFileList.Contains(file))
                {
                    return false;
                }
                if (this.loadingFileList.Count < this.loadFileMax)
                {
                    this.loadingFileList.Add(file);
                    if (this.isOutPutDebugLog)
                    {
                        Debug.Log("Load Start :" + file.FileName);
                    }
                    base.StartCoroutine(this.LoadAsync(file));
                    return true;
                }
                if (!this.loadWaitFileList.Contains(file))
                {
                    this.loadWaitFileList.Add(file);
                    return false;
                }
            }
            return false;
        }

        private void UnloadUnusedAssets(int count)
        {
            switch (this.unloadType)
            {
                case UnloadType.UnloadUnusedAsset:
                    if (count > 0)
                    {
                        break;
                    }
                    return;

                case UnloadType.UnloadUnusedAssetAlways:
                    break;

                default:
                    return;
            }
            if (!this.unloadingUnusedAssets && base.get_gameObject().get_activeInHierarchy())
            {
                base.StartCoroutine(this.UnloadUnusedAssetsAsync());
            }
        }

        [DebuggerHidden]
        private IEnumerator UnloadUnusedAssetsAsync()
        {
            return new <UnloadUnusedAssetsAsync>c__Iterator3 { $this = this };
        }

        private void UnloadUnusedFileList(int count)
        {
            if ((this.usingFileList.Count > 0) && (count > 0))
            {
                int num = 0;
                List<AssetFileBase> list = new List<AssetFileBase>();
                foreach (AssetFileBase base2 in this.usingFileList)
                {
                    if (((count <= 0) || base2.IgnoreUnload) || (!base2.IsLoadEnd || (base2.ReferenceCount > 0)))
                    {
                        list.Add(base2);
                    }
                    else
                    {
                        if (this.isOutPutDebugLog)
                        {
                            Debug.Log("Unload " + base2.FileName);
                        }
                        base2.Unload();
                        count--;
                        if (base2.FileType == AssetFileType.UnityObject)
                        {
                            num++;
                        }
                    }
                }
                this.UnloadUnusedAssets(num);
                this.usingFileList = list;
            }
        }

        public Utage.AssetBundleInfoManager AssetBundleInfoManager
        {
            get
            {
                return ((Component) this).GetComponentCacheCreateIfMissing<Utage.AssetBundleInfoManager>(ref this.assetBundleInfoManager);
            }
            set
            {
                this.assetBundleInfoManager = value;
            }
        }

        public int AutoRetryCountOnDonwloadError
        {
            get
            {
                return this.autoRetryCountOnDonwloadError;
            }
            set
            {
                this.autoRetryCountOnDonwloadError = value;
            }
        }

        public Action<AssetFile> CallbackError
        {
            get
            {
                if (this.callbackError == null)
                {
                }
                return (this.callbackError = new Action<AssetFile>(this.CallbackFileLoadError));
            }
            set
            {
                this.callbackError = value;
            }
        }

        private Utage.CustomLoadManager CustomLoadManager
        {
            get
            {
                return ((Component) this).GetComponentCacheCreateIfMissing<Utage.CustomLoadManager>(ref this.customLoadManager);
            }
        }

        public bool EnableResourcesLoadAsync
        {
            get
            {
                return this.enableResourcesLoadAsync;
            }
            set
            {
                this.enableResourcesLoadAsync = value;
            }
        }

        public Utage.FileIOManager FileIOManager
        {
            get
            {
                return ((Component) this).GetComponentCache<Utage.FileIOManager>(ref this.fileIOManager);
            }
            set
            {
                this.fileIOManager = value;
            }
        }

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

        public int MaxFilesOnMemory
        {
            get
            {
                return this.rangeOfFilesOnMemory.Max;
            }
        }

        public int MinFilesOnMemory
        {
            get
            {
                return this.rangeOfFilesOnMemory.Min;
            }
        }

        public AssetFileManagerSettings Settings
        {
            get
            {
                return this.settings;
            }
            set
            {
                this.settings = value;
            }
        }

        private Utage.StaticAssetManager StaticAssetManager
        {
            get
            {
                return ((Component) this).GetComponentCacheCreateIfMissing<Utage.StaticAssetManager>(ref this.staticAssetManager);
            }
        }

        public float TimeOutDownload
        {
            get
            {
                return this.timeOutDownload;
            }
            set
            {
                this.timeOutDownload = value;
            }
        }

        internal UnloadType UnloadUnusedType
        {
            get
            {
                return this.unloadType;
            }
        }

        [CompilerGenerated]
        private sealed class <CallbackFileLoadError>c__AnonStorey4
        {
            internal AssetFileManager $this;
            internal AssetFileBase errorFile;

            internal void <>m__0()
            {
                this.$this.isWaitingRetry = false;
                this.$this.ReloadFileSub(this.errorFile);
            }
        }

        [CompilerGenerated]
        private sealed class <CoLoadWait>c__Iterator1 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal bool $disposing;
            internal int $PC;
            internal AssetFileManager $this;
            internal AssetFileBase file;
            internal Action<AssetFile> onComplete;

            [DebuggerHidden]
            public void Dispose()
            {
                this.$disposing = true;
                this.$PC = -1;
            }

            public bool MoveNext()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                switch (num)
                {
                    case 0:
                        if (!this.file.IsLoadEnd)
                        {
                            this.$this.LoadSub(this.file, this.$this);
                            break;
                        }
                        this.onComplete(this.file);
                        goto Label_00A0;

                    case 1:
                        break;

                    default:
                        goto Label_00A7;
                }
                while (!this.file.IsLoadEnd)
                {
                    this.$current = null;
                    if (!this.$disposing)
                    {
                        this.$PC = 1;
                    }
                    return true;
                }
                this.onComplete(this.file);
            Label_00A0:
                this.$PC = -1;
            Label_00A7:
                return false;
            }

            [DebuggerHidden]
            public void Reset()
            {
                throw new NotSupportedException();
            }

            object IEnumerator<object>.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.$current;
                }
            }

            object IEnumerator.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.$current;
                }
            }
        }

        [CompilerGenerated]
        private sealed class <CoWaitRetry>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal bool $disposing;
            internal int $PC;
            internal AssetFileManager $this;
            internal AssetFileBase file;

            [DebuggerHidden]
            public void Dispose()
            {
                this.$disposing = true;
                this.$PC = -1;
            }

            public bool MoveNext()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                switch (num)
                {
                    case 0:
                    case 1:
                        if (this.$this.isWaitingRetry)
                        {
                            this.$current = null;
                            if (!this.$disposing)
                            {
                                this.$PC = 1;
                            }
                            return true;
                        }
                        this.$this.ReloadFileSub(this.file);
                        this.$PC = -1;
                        break;
                }
                return false;
            }

            [DebuggerHidden]
            public void Reset()
            {
                throw new NotSupportedException();
            }

            object IEnumerator<object>.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.$current;
                }
            }

            object IEnumerator.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.$current;
                }
            }
        }

        [CompilerGenerated]
        private sealed class <LoadAsync>c__Iterator2 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal bool $disposing;
            private <LoadAsync>c__AnonStorey5 $locvar0;
            internal int $PC;
            internal AssetFileManager $this;
            internal AssetFileBase file;

            [DebuggerHidden]
            public void Dispose()
            {
                this.$disposing = true;
                this.$PC = -1;
            }

            public bool MoveNext()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                switch (num)
                {
                    case 0:
                        this.$locvar0 = new <LoadAsync>c__AnonStorey5();
                        this.$locvar0.<>f__ref$2 = this;
                        this.$locvar0.file = this.file;
                        this.$current = this.$locvar0.file.LoadAsync(new Action(this.$locvar0.<>m__0), new Action(this.$locvar0.<>m__1));
                        if (!this.$disposing)
                        {
                            this.$PC = 1;
                        }
                        return true;

                    case 1:
                        this.$PC = -1;
                        break;
                }
                return false;
            }

            [DebuggerHidden]
            public void Reset()
            {
                throw new NotSupportedException();
            }

            object IEnumerator<object>.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.$current;
                }
            }

            object IEnumerator.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.$current;
                }
            }

            private sealed class <LoadAsync>c__AnonStorey5
            {
                internal AssetFileManager.<LoadAsync>c__Iterator2 <>f__ref$2;
                internal AssetFileBase file;

                internal void <>m__0()
                {
                    if (this.<>f__ref$2.$this.isOutPutDebugLog)
                    {
                        Debug.Log("Load End :" + this.file.FileName);
                    }
                    this.<>f__ref$2.$this.loadingFileList.Remove(this.file);
                    this.<>f__ref$2.$this.LoadNextFile();
                }

                internal void <>m__1()
                {
                    if (this.<>f__ref$2.$this.dummyFiles.isEnable)
                    {
                        if (this.<>f__ref$2.$this.dummyFiles.outputErrorLog)
                        {
                            Debug.LogError("Load Failed. Dummy file loaded:" + this.file.FileName + "\n" + this.file.LoadErrorMsg);
                        }
                        this.file.LoadDummy(this.<>f__ref$2.$this.dummyFiles);
                        this.<>f__ref$2.$this.loadingFileList.Remove(this.file);
                        this.<>f__ref$2.$this.LoadNextFile();
                    }
                    else
                    {
                        Debug.LogError("Load Failed :" + this.file.FileName + "\n" + this.file.LoadErrorMsg);
                        if (this.<>f__ref$2.$this.CallbackError != null)
                        {
                            this.<>f__ref$2.$this.CallbackError(this.file);
                        }
                    }
                }
            }
        }

        [CompilerGenerated]
        private sealed class <UnloadUnusedAssetsAsync>c__Iterator3 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal bool $disposing;
            internal int $PC;
            internal AssetFileManager $this;

            [DebuggerHidden]
            public void Dispose()
            {
                this.$disposing = true;
                this.$PC = -1;
            }

            public bool MoveNext()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                switch (num)
                {
                    case 0:
                        if (this.$this.isOutPutDebugLog)
                        {
                            Debug.Log("UnloadUnusedAssets");
                        }
                        this.$this.unloadingUnusedAssets = true;
                        this.$current = Resources.UnloadUnusedAssets();
                        if (!this.$disposing)
                        {
                            this.$PC = 1;
                        }
                        return true;

                    case 1:
                        this.$this.unloadingUnusedAssets = false;
                        this.$PC = -1;
                        break;
                }
                return false;
            }

            [DebuggerHidden]
            public void Reset()
            {
                throw new NotSupportedException();
            }

            object IEnumerator<object>.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.$current;
                }
            }

            object IEnumerator.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.$current;
                }
            }
        }

        internal enum UnloadType
        {
            None,
            UnloadUnusedAsset,
            UnloadUnusedAssetAlways,
            NoneAndUnloadAssetBundleTrue
        }
    }
}

