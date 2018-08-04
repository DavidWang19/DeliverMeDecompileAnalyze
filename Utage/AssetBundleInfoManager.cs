namespace Utage
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UtageExtensions;

    [AddComponentMenu("Utage/Lib/File/AssetBundleInfoManager")]
    public class AssetBundleInfoManager : MonoBehaviour
    {
        private const string AssetBundleManifestName = "assetbundlemanifest";
        [SerializeField]
        private Utage.AssetFileManager assetFileManager;
        [SerializeField]
        private string cacheDirectoryName = "Cache";
        private Dictionary<string, AssetBundleInfo> dictionary = new Dictionary<string, AssetBundleInfo>(StringComparer.OrdinalIgnoreCase);
        [SerializeField]
        private int retryCount = 5;
        [SerializeField]
        private float timeOut = 5f;
        [SerializeField]
        private bool useCacheManifest = true;

        public void AddAssetBundleInfo(string resourcePath, string assetBunleUrl, int assetBunleVersion)
        {
            try
            {
                this.dictionary.Add(resourcePath, new AssetBundleInfo(assetBunleUrl, assetBunleVersion));
            }
            catch
            {
                Debug.LogError(resourcePath + "is already contains in assetbundleManger");
            }
        }

        public void AddAssetBundleManifest(string rootUrl, AssetBundleManifest manifest)
        {
            foreach (string str in manifest.GetAllAssetBundles())
            {
                Hash128 assetBundleHash = manifest.GetAssetBundleHash(str);
                string[] args = new string[] { rootUrl, str };
                string key = FilePathUtil.Combine(args);
                try
                {
                    this.dictionary.Add(key, new AssetBundleInfo(key, assetBundleHash));
                }
                catch
                {
                    Debug.LogError(key + "is already contains in assetbundleManger");
                }
            }
        }

        public void DeleteAllCache()
        {
            string[] args = new string[] { FileIOManagerBase.SdkTemporaryCachePath, this.cacheDirectoryName };
            this.FileIOManager.DeleteDirectory(FilePathUtil.Combine(args) + "/");
            WrapperUnityVersion.CleanCache();
        }

        public IEnumerator DownloadManifestAsync(string rootUrl, string relativeUrl, Action onComplete, Action onFailed)
        {
            <DownloadManifestAsync>c__AnonStorey0 storey = new <DownloadManifestAsync>c__AnonStorey0 {
                rootUrl = rootUrl,
                relativeUrl = relativeUrl,
                onComplete = onComplete,
                onFailed = onFailed,
                $this = this
            };
            string[] args = new string[] { storey.rootUrl, storey.relativeUrl };
            string url = FilePathUtil.ToCacheClearUrl(FilePathUtil.Combine(args));
            storey.wwwEx = new WWWEx(url);
            storey.wwwEx.StoreBytes = true;
            storey.wwwEx.OnUpdate = new Action<WWWEx>(this.OnDownloadingManifest);
            storey.wwwEx.RetryCount = this.retryCount;
            storey.wwwEx.TimeOut = this.timeOut;
            return storey.wwwEx.LoadAssetBundleByNameAsync<AssetBundleManifest>("assetbundlemanifest", false, new Action<AssetBundleManifest>(storey.<>m__0), new Action(storey.<>m__1));
        }

        public AssetBundleInfo FindAssetBundleInfo(string path)
        {
            AssetBundleInfo info;
            if (!this.dictionary.TryGetValue(path, out info))
            {
                string key = FilePathUtil.ChangeExtension(path, ".asset");
                if (!this.dictionary.TryGetValue(key, out info))
                {
                    return null;
                }
            }
            return info;
        }

        private string GetCachePath(string relativeUrl)
        {
            string[] args = new string[] { FileIOManagerBase.SdkTemporaryCachePath, this.cacheDirectoryName, relativeUrl };
            return FilePathUtil.Combine(args);
        }

        public IEnumerator LoadCacheManifestAsync(string rootUrl, string relativeUrl, Action onComplete, Action onFailed)
        {
            <LoadCacheManifestAsync>c__AnonStorey1 storey = new <LoadCacheManifestAsync>c__AnonStorey1 {
                rootUrl = rootUrl,
                onComplete = onComplete,
                onFailed = onFailed,
                $this = this
            };
            WWWEx ex = new WWWEx(FilePathUtil.AddFileProtocol(this.GetCachePath(relativeUrl))) {
                OnUpdate = new Action<WWWEx>(this.OnDownloadingManifest),
                RetryCount = 0,
                TimeOut = 0.1f
            };
            return ex.LoadAssetBundleByNameAsync<AssetBundleManifest>("assetbundlemanifest", false, new Action<AssetBundleManifest>(storey.<>m__0), new Action(storey.<>m__1));
        }

        private void OnDownloadingManifest(WWWEx wwwEx)
        {
        }

        private Utage.AssetFileManager AssetFileManager
        {
            get
            {
                return ((Component) this).GetComponentCache<Utage.AssetFileManager>(ref this.assetFileManager);
            }
        }

        public string CacheDirectoryName
        {
            get
            {
                return this.cacheDirectoryName;
            }
            set
            {
                this.cacheDirectoryName = value;
            }
        }

        private Utage.FileIOManager FileIOManager
        {
            get
            {
                return this.AssetFileManager.FileIOManager;
            }
        }

        public int RetryCount
        {
            get
            {
                return this.retryCount;
            }
            set
            {
                this.retryCount = value;
            }
        }

        public int TimeOut
        {
            get
            {
                return this.retryCount;
            }
            set
            {
                this.retryCount = value;
            }
        }

        public bool UseCacheManifest
        {
            get
            {
                return this.useCacheManifest;
            }
            set
            {
                this.useCacheManifest = value;
            }
        }

        [CompilerGenerated]
        private sealed class <DownloadManifestAsync>c__AnonStorey0
        {
            internal AssetBundleInfoManager $this;
            internal Action onComplete;
            internal Action onFailed;
            internal string relativeUrl;
            internal string rootUrl;
            internal WWWEx wwwEx;

            internal void <>m__0(AssetBundleManifest manifest)
            {
                this.$this.AddAssetBundleManifest(this.rootUrl, manifest);
                if (this.$this.UseCacheManifest)
                {
                    string cachePath = this.$this.GetCachePath(this.relativeUrl);
                    this.$this.FileIOManager.CreateDirectory(FilePathUtil.GetDirectoryPath(cachePath) + "/");
                    this.$this.FileIOManager.Write(cachePath, this.wwwEx.Bytes);
                }
                if (this.onComplete != null)
                {
                    this.onComplete();
                }
            }

            internal void <>m__1()
            {
                if (this.onFailed != null)
                {
                    this.onFailed();
                }
            }
        }

        [CompilerGenerated]
        private sealed class <LoadCacheManifestAsync>c__AnonStorey1
        {
            internal AssetBundleInfoManager $this;
            internal Action onComplete;
            internal Action onFailed;
            internal string rootUrl;

            internal void <>m__0(AssetBundleManifest manifest)
            {
                this.$this.AddAssetBundleManifest(this.rootUrl, manifest);
                if (this.onComplete != null)
                {
                    this.onComplete();
                }
            }

            internal void <>m__1()
            {
                if (this.onFailed != null)
                {
                    this.onFailed();
                }
            }
        }
    }
}

