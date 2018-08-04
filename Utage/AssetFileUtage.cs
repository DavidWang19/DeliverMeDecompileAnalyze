namespace Utage
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    internal class AssetFileUtage : AssetFileBase
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private UnityEngine.AssetBundle <AssetBundle>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <LoadPath>k__BackingField;

        public AssetFileUtage(AssetFileManager assetFileManager, AssetFileInfo fileInfo, IAssetFileSettingData settingData) : base(assetFileManager, fileInfo, settingData)
        {
            this.LoadPath = this.ParseLoadPath();
        }

        public override bool CheckCacheOrLocal()
        {
            if (base.FileInfo.StrageType == AssetFileStrageType.Server)
            {
                return Caching.IsVersionCached(this.LoadPath, base.FileInfo.AssetBundleInfo.Hash);
            }
            return true;
        }

        private Type GetResourceType()
        {
            switch (this.FileType)
            {
                case AssetFileType.Text:
                    return typeof(TextAsset);

                case AssetFileType.Texture:
                    return typeof(Texture2D);

                case AssetFileType.Sound:
                    return typeof(AudioClip);
            }
            return typeof(Object);
        }

        private void LoadAsset(Object asset, Action onComplete, Action onFailed)
        {
            if (asset == null)
            {
                this.SetLoadError("LoadResource Error");
                onFailed();
            }
            else
            {
                switch (this.FileType)
                {
                    case AssetFileType.Text:
                        base.Text = asset as TextAsset;
                        if (null == base.Text)
                        {
                            this.SetLoadError("LoadResource Error");
                        }
                        break;

                    case AssetFileType.Texture:
                        base.Texture = asset as Texture2D;
                        if (null == base.Texture)
                        {
                            this.SetLoadError("LoadResource Error");
                        }
                        break;

                    case AssetFileType.Sound:
                        base.Sound = asset as AudioClip;
                        if (null == base.Sound)
                        {
                            this.SetLoadError("LoadResource Error");
                        }
                        break;

                    default:
                        base.UnityObject = asset;
                        if (null == base.UnityObject)
                        {
                            this.SetLoadError("LoadResource Error");
                        }
                        break;
                }
                if (base.IsLoadError)
                {
                    onFailed();
                }
                else
                {
                    onComplete();
                }
            }
        }

        [DebuggerHidden]
        private IEnumerator LoadAssetBundleAsync(string path, Action onComplete, Action onFailed)
        {
            return new <LoadAssetBundleAsync>c__Iterator3 { path = path, onComplete = onComplete, onFailed = onFailed, $this = this };
        }

        [DebuggerHidden]
        private IEnumerator LoadAssetBundleAsync(UnityEngine.AssetBundle assetBundle, Action onComplete, Action onFailed)
        {
            return new <LoadAssetBundleAsync>c__Iterator4 { assetBundle = assetBundle, onFailed = onFailed, onComplete = onComplete, $this = this };
        }

        [DebuggerHidden]
        public override IEnumerator LoadAsync(Action onComplete, Action onFailed)
        {
            return new <LoadAsync>c__Iterator0 { onComplete = onComplete, onFailed = onFailed, $this = this };
        }

        [DebuggerHidden]
        private IEnumerator LoadAsyncSub(string path, Action onComplete, Action onFailed)
        {
            return new <LoadAsyncSub>c__Iterator1 { path = path, onComplete = onComplete, onFailed = onFailed, $this = this };
        }

        private void LoadResource(string loadPath, Action onComplete, Action onFailed)
        {
            loadPath = FilePathUtil.GetPathWithoutExtension(loadPath);
            Object asset = Resources.Load(loadPath, this.GetResourceType());
            this.LoadAsset(asset, onComplete, onFailed);
        }

        [DebuggerHidden]
        private IEnumerator LoadResourceAsync(string loadPath, Action onComplete, Action onFailed)
        {
            return new <LoadResourceAsync>c__Iterator2 { loadPath = loadPath, onComplete = onComplete, onFailed = onFailed, $this = this };
        }

        private WWWEx MakeWWWEx(string path)
        {
            if (base.FileInfo.AssetBundleInfo == null)
            {
                return new WWWEx(path);
            }
            if (base.FileInfo.AssetBundleInfo.Hash.get_isValid())
            {
                return new WWWEx(path, base.FileInfo.AssetBundleInfo.Hash);
            }
            return new WWWEx(path, base.FileInfo.AssetBundleInfo.Version);
        }

        private void SetLoadError(string errorMsg)
        {
            base.LoadErrorMsg = errorMsg + " : load from " + this.LoadPath;
            base.IsLoadError = true;
        }

        public override void Unload()
        {
            switch (this.FileType)
            {
                case AssetFileType.Text:
                    Resources.UnloadAsset(base.Text);
                    break;

                case AssetFileType.Texture:
                    Resources.UnloadAsset(base.Texture);
                    break;

                case AssetFileType.Sound:
                    Resources.UnloadAsset(base.Sound);
                    break;
            }
            base.Text = null;
            base.Texture = null;
            base.Sound = null;
            base.UnityObject = null;
            if (this.AssetBundle != null)
            {
                this.AssetBundle.Unload(true);
                this.AssetBundle = null;
            }
            base.IsLoadEnd = false;
            base.Priority = AssetFileLoadPriority.DownloadOnly;
        }

        protected UnityEngine.AssetBundle AssetBundle { get; set; }

        protected string LoadPath { get; set; }

        [CompilerGenerated]
        private sealed class <LoadAssetBundleAsync>c__Iterator3 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal bool $disposing;
            private <LoadAssetBundleAsync>c__AnonStorey6 $locvar0;
            internal int $PC;
            internal AssetFileUtage $this;
            internal WWWEx <wwwEx>__0;
            internal Action onComplete;
            internal Action onFailed;
            internal string path;

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
                        this.$locvar0 = new <LoadAssetBundleAsync>c__AnonStorey6();
                        this.$locvar0.<>f__ref$3 = this;
                        this.$locvar0.onComplete = this.onComplete;
                        this.$locvar0.onFailed = this.onFailed;
                        this.<wwwEx>__0 = this.$this.MakeWWWEx(this.path);
                        this.<wwwEx>__0.RetryCount = this.$this.FileManager.AutoRetryCountOnDonwloadError;
                        this.<wwwEx>__0.TimeOut = this.$this.FileManager.TimeOutDownload;
                        this.$this.AssetBundle = null;
                        this.$locvar0.assetBundle = null;
                        this.$current = this.<wwwEx>__0.LoadAsync(new Action<WWW>(this.$locvar0.<>m__0), new Action<WWW>(this.$locvar0.<>m__1));
                        if (!this.$disposing)
                        {
                            this.$PC = 1;
                        }
                        goto Label_016F;

                    case 1:
                        if (this.$locvar0.assetBundle == null)
                        {
                            break;
                        }
                        this.$current = this.$this.LoadAssetBundleAsync(this.$locvar0.assetBundle, this.$locvar0.onComplete, this.$locvar0.onFailed);
                        if (!this.$disposing)
                        {
                            this.$PC = 2;
                        }
                        goto Label_016F;

                    case 2:
                        break;

                    default:
                        goto Label_016D;
                }
                this.$PC = -1;
            Label_016D:
                return false;
            Label_016F:
                return true;
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

            private sealed class <LoadAssetBundleAsync>c__AnonStorey6
            {
                internal AssetFileUtage.<LoadAssetBundleAsync>c__Iterator3 <>f__ref$3;
                internal AssetBundle assetBundle;
                internal Action onComplete;
                internal Action onFailed;

                internal void <>m__0(WWW www)
                {
                    if (this.<>f__ref$3.$this.Priority == AssetFileLoadPriority.DownloadOnly)
                    {
                        www.get_assetBundle().Unload(true);
                        this.onComplete();
                    }
                    else
                    {
                        this.assetBundle = www.get_assetBundle();
                        if (this.assetBundle == null)
                        {
                            this.<>f__ref$3.$this.SetLoadError(www.get_url() + " is not assetBundle");
                            this.onFailed();
                        }
                    }
                }

                internal void <>m__1(WWW www)
                {
                    this.onFailed();
                }
            }
        }

        [CompilerGenerated]
        private sealed class <LoadAssetBundleAsync>c__Iterator4 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal bool $disposing;
            internal int $PC;
            internal AssetFileUtage $this;
            internal Object[] <assets>__0;
            internal AssetBundleRequest <request>__0;
            internal AssetBundle assetBundle;
            internal Action onComplete;
            internal Action onFailed;

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
                        this.<request>__0 = this.assetBundle.LoadAllAssetsAsync(this.$this.GetResourceType());
                        break;

                    case 1:
                        break;

                    default:
                        goto Label_0140;
                }
                if (!this.<request>__0.get_isDone())
                {
                    this.$current = null;
                    if (!this.$disposing)
                    {
                        this.$PC = 1;
                    }
                    return true;
                }
                this.<assets>__0 = this.<request>__0.get_allAssets();
                if ((this.<assets>__0 == null) || (this.<assets>__0.Length <= 0))
                {
                    this.$this.SetLoadError("AssetBundleType Error");
                    this.assetBundle.Unload(true);
                    this.onFailed();
                }
                else
                {
                    this.$this.LoadAsset(this.<assets>__0[0], this.onComplete, this.onFailed);
                    this.<assets>__0 = null;
                    this.<request>__0 = null;
                    if ((this.$this.FileType == AssetFileType.UnityObject) && (this.$this.FileManager.UnloadUnusedType == AssetFileManager.UnloadType.NoneAndUnloadAssetBundleTrue))
                    {
                        this.$this.AssetBundle = this.assetBundle;
                    }
                    else
                    {
                        this.assetBundle.Unload(false);
                    }
                }
                this.$PC = -1;
            Label_0140:
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
        private sealed class <LoadAsync>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal bool $disposing;
            private <LoadAsync>c__AnonStorey5 $locvar0;
            internal int $PC;
            internal AssetFileUtage $this;
            internal Action onComplete;
            internal Action onFailed;

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
                        this.$locvar0.<>f__ref$0 = this;
                        this.$locvar0.onComplete = this.onComplete;
                        this.$locvar0.onFailed = this.onFailed;
                        this.$this.IsLoadEnd = false;
                        this.$this.IsLoadError = false;
                        this.$current = this.$this.LoadAsyncSub(this.$this.LoadPath, new Action(this.$locvar0.<>m__0), new Action(this.$locvar0.<>m__1));
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
                internal AssetFileUtage.<LoadAsync>c__Iterator0 <>f__ref$0;
                internal Action onComplete;
                internal Action onFailed;

                internal void <>m__0()
                {
                    if (this.<>f__ref$0.$this.Priority != AssetFileLoadPriority.DownloadOnly)
                    {
                        this.<>f__ref$0.$this.IsLoadEnd = true;
                    }
                    this.onComplete();
                }

                internal void <>m__1()
                {
                    this.<>f__ref$0.$this.IsLoadError = true;
                    this.onFailed();
                }
            }
        }

        [CompilerGenerated]
        private sealed class <LoadAsyncSub>c__Iterator1 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal bool $disposing;
            internal AssetFileStrageType $locvar0;
            internal int $PC;
            internal AssetFileUtage $this;
            internal Action onComplete;
            internal Action onFailed;
            internal string path;

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
                        this.$locvar0 = this.$this.FileInfo.StrageType;
                        if (this.$locvar0 == AssetFileStrageType.Resources)
                        {
                            if (this.$this.FileManager.EnableResourcesLoadAsync)
                            {
                                this.$current = this.$this.LoadResourceAsync(this.path, this.onComplete, this.onFailed);
                                if (!this.$disposing)
                                {
                                    this.$PC = 1;
                                }
                                goto Label_0104;
                            }
                            this.$this.LoadResource(this.path, this.onComplete, this.onFailed);
                            break;
                        }
                        this.$current = this.$this.LoadAssetBundleAsync(this.path, this.onComplete, this.onFailed);
                        if (!this.$disposing)
                        {
                            this.$PC = 2;
                        }
                        goto Label_0104;

                    case 1:
                    case 2:
                        break;

                    default:
                        goto Label_0102;
                }
                this.$PC = -1;
            Label_0102:
                return false;
            Label_0104:
                return true;
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
        private sealed class <LoadResourceAsync>c__Iterator2 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal bool $disposing;
            internal int $PC;
            internal AssetFileUtage $this;
            internal ResourceRequest <request>__0;
            internal string loadPath;
            internal Action onComplete;
            internal Action onFailed;

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
                        this.loadPath = FilePathUtil.GetPathWithoutExtension(this.loadPath);
                        this.<request>__0 = Resources.LoadAsync(this.loadPath, this.$this.GetResourceType());
                        break;

                    case 1:
                        break;

                    default:
                        goto Label_00A7;
                }
                if (!this.<request>__0.get_isDone())
                {
                    this.$current = null;
                    if (!this.$disposing)
                    {
                        this.$PC = 1;
                    }
                    return true;
                }
                this.$this.LoadAsset(this.<request>__0.get_asset(), this.onComplete, this.onFailed);
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
    }
}

