namespace Utage
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class WWWEx
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Hash128 <AssetBundleHash>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <AssetBundleVersion>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private byte[] <Bytes>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <IgnoreDebugLog>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Type <LoadType>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Action<WWWEx> <OnUpdate>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private float <Progress>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <RetryCount>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <StoreBytes>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private float <TimeOut>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <Url>k__BackingField;

        public WWWEx(string url)
        {
            this.LoadType = Type.Default;
            this.InitSub(url);
        }

        public WWWEx(string url, int assetBundleVersion)
        {
            this.AssetBundleVersion = assetBundleVersion;
            this.LoadType = Type.Cache;
            this.InitSub(url);
        }

        public WWWEx(string url, Hash128 assetBundleHash)
        {
            this.AssetBundleHash = assetBundleHash;
            this.LoadType = Type.Cache;
            this.InitSub(url);
        }

        private WWW CreateWWW()
        {
            if (this.LoadType != Type.Cache)
            {
                return new WWW(this.Url);
            }
            if (this.AssetBundleHash.get_isValid())
            {
                return WWW.LoadFromCacheOrDownload(this.Url, this.AssetBundleHash);
            }
            return WWW.LoadFromCacheOrDownload(this.Url, this.AssetBundleVersion);
        }

        private void InitSub(string url)
        {
            this.Url = url;
            this.RetryCount = 5;
            this.TimeOut = 5f;
            this.Progress = 0f;
        }

        [DebuggerHidden]
        public IEnumerator LoadAssetBundleAllAsync<T>(bool unloadAllLoadedObjects, Action<T[]> onComplete, Action onFailed) where T: Object
        {
            return new <LoadAssetBundleAllAsync>c__Iterator2<T> { onFailed = onFailed, onComplete = onComplete, unloadAllLoadedObjects = unloadAllLoadedObjects, $this = this };
        }

        public IEnumerator LoadAssetBundleAsync(Action<WWW, AssetBundle> onComplete, Action<WWW> onFailed)
        {
            <LoadAssetBundleAsync>c__AnonStorey4 storey = new <LoadAssetBundleAsync>c__AnonStorey4 {
                onComplete = onComplete,
                onFailed = onFailed,
                $this = this
            };
            return this.LoadAsync(new Action<WWW>(storey.<>m__0), new Action<WWW>(storey.<>m__1));
        }

        [DebuggerHidden]
        public IEnumerator LoadAssetBundleByNameAsync<T>(string assetName, bool unloadAllLoadedObjects, Action<T> onComplete, Action onFailed) where T: Object
        {
            return new <LoadAssetBundleByNameAsync>c__Iterator1<T> { onFailed = onFailed, assetName = assetName, onComplete = onComplete, unloadAllLoadedObjects = unloadAllLoadedObjects, $this = this };
        }

        public IEnumerator LoadAssetBundleMainAssetAsync<T>(bool unloadAllLoadedObjects, Action<WWW, T> onComplete, Action<WWW> onFailed) where T: Object
        {
            <LoadAssetBundleMainAssetAsync>c__AnonStorey5<T> storey = new <LoadAssetBundleMainAssetAsync>c__AnonStorey5<T> {
                onComplete = onComplete,
                onFailed = onFailed,
                unloadAllLoadedObjects = unloadAllLoadedObjects,
                $this = this
            };
            return this.LoadAssetBundleAsync(new Action<WWW, AssetBundle>(storey.<>m__0), new Action<WWW>(storey.<>m__1));
        }

        public IEnumerator LoadAsync(Action<WWW> onComplete, Action<WWW> onFailed = null)
        {
            <LoadAsync>c__AnonStorey3 storey = new <LoadAsync>c__AnonStorey3 {
                onComplete = onComplete,
                onFailed = onFailed,
                $this = this
            };
            return this.LoadAsync(new Action<WWW>(storey.<>m__0), new Action<WWW>(storey.<>m__1), new Action<WWW>(storey.<>m__2));
        }

        private IEnumerator LoadAsync(Action<WWW> onCopmlete, Action<WWW> onFailed, Action<WWW> onTimeOut)
        {
            return this.LoadAsyncSub(onCopmlete, onFailed, onTimeOut, this.RetryCount);
        }

        [DebuggerHidden]
        private IEnumerator LoadAsyncSub(Action<WWW> onCopmlete, Action<WWW> onFailed, Action<WWW> onTimeOut, int retryCount)
        {
            return new <LoadAsyncSub>c__Iterator0 { retryCount = retryCount, onTimeOut = onTimeOut, onFailed = onFailed, onCopmlete = onCopmlete, $this = this };
        }

        public Hash128 AssetBundleHash { get; private set; }

        public int AssetBundleVersion { get; private set; }

        public byte[] Bytes { get; set; }

        public bool IgnoreDebugLog { get; set; }

        public Type LoadType { get; private set; }

        public Action<WWWEx> OnUpdate { get; set; }

        public float Progress { get; private set; }

        public int RetryCount { get; set; }

        public bool StoreBytes { get; set; }

        public float TimeOut { get; set; }

        public string Url { get; private set; }

        [CompilerGenerated]
        private sealed class <LoadAssetBundleAllAsync>c__Iterator2<T> : IEnumerator, IDisposable, IEnumerator<object> where T: Object
        {
            internal object $current;
            internal bool $disposing;
            private <LoadAssetBundleAllAsync>c__AnonStorey7<T> $locvar0;
            internal int $PC;
            internal WWWEx $this;
            internal T[] <assets>__0;
            internal AssetBundleRequest <request>__0;
            internal Action<T[]> onComplete;
            internal Action onFailed;
            internal bool unloadAllLoadedObjects;

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
                        this.$locvar0 = new <LoadAssetBundleAllAsync>c__AnonStorey7<T>();
                        this.$locvar0.<>f__ref$2 = (WWWEx.<LoadAssetBundleAllAsync>c__Iterator2<T>) this;
                        this.$locvar0.onFailed = this.onFailed;
                        this.$locvar0.assetBundle = null;
                        this.$current = this.$this.LoadAssetBundleAsync(new Action<WWW, AssetBundle>(this.$locvar0.<>m__0), new Action<WWW>(this.$locvar0.<>m__1));
                        if (!this.$disposing)
                        {
                            this.$PC = 1;
                        }
                        goto Label_01D7;

                    case 1:
                        if (this.$locvar0.assetBundle != null)
                        {
                            this.<request>__0 = this.$locvar0.assetBundle.LoadAllAssetsAsync<T>();
                            break;
                        }
                        goto Label_01D5;

                    case 2:
                        break;

                    default:
                        goto Label_01D5;
                }
                while (!this.<request>__0.get_isDone())
                {
                    this.$current = null;
                    if (!this.$disposing)
                    {
                        this.$PC = 2;
                    }
                    goto Label_01D7;
                }
                this.<assets>__0 = this.<request>__0.get_allAssets() as T[];
                if ((this.<assets>__0 == null) || (this.<assets>__0.Length <= 0))
                {
                    if (!this.$this.IgnoreDebugLog)
                    {
                        Debug.LogError(this.$this.Url + "   is not AssetBundle of " + typeof(T).Name);
                    }
                    if (this.$locvar0.onFailed != null)
                    {
                        this.$locvar0.onFailed();
                    }
                }
                else if (this.onComplete != null)
                {
                    this.onComplete(this.<assets>__0);
                }
                this.<assets>__0 = null;
                this.<request>__0 = null;
                this.$locvar0.assetBundle.Unload(this.unloadAllLoadedObjects);
                this.$PC = -1;
            Label_01D5:
                return false;
            Label_01D7:
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

            private sealed class <LoadAssetBundleAllAsync>c__AnonStorey7
            {
                internal WWWEx.<LoadAssetBundleAllAsync>c__Iterator2<T> <>f__ref$2;
                internal AssetBundle assetBundle;
                internal Action onFailed;

                internal void <>m__0(WWW _www, AssetBundle _assetBundle)
                {
                    this.assetBundle = _assetBundle;
                }

                internal void <>m__1(WWW _www)
                {
                    if (this.onFailed != null)
                    {
                        this.onFailed();
                    }
                }
            }
        }

        [CompilerGenerated]
        private sealed class <LoadAssetBundleAsync>c__AnonStorey4
        {
            internal WWWEx $this;
            internal Action<WWW, AssetBundle> onComplete;
            internal Action<WWW> onFailed;

            internal void <>m__0(WWW www)
            {
                AssetBundle bundle = www.get_assetBundle();
                if (bundle != null)
                {
                    if (this.onComplete != null)
                    {
                        this.onComplete(www, bundle);
                    }
                }
                else
                {
                    if (!this.$this.IgnoreDebugLog)
                    {
                        Debug.LogError(www.get_url() + " is not assetBundle");
                    }
                    if (this.onFailed != null)
                    {
                        this.onFailed(www);
                    }
                }
            }

            internal void <>m__1(WWW www)
            {
                if (this.onFailed != null)
                {
                    this.onFailed(www);
                }
            }
        }

        [CompilerGenerated]
        private sealed class <LoadAssetBundleByNameAsync>c__Iterator1<T> : IEnumerator, IDisposable, IEnumerator<object> where T: Object
        {
            internal object $current;
            internal bool $disposing;
            private <LoadAssetBundleByNameAsync>c__AnonStorey6<T> $locvar0;
            internal int $PC;
            internal WWWEx $this;
            internal T <asset>__0;
            internal AssetBundleRequest <request>__0;
            internal string assetName;
            internal Action<T> onComplete;
            internal Action onFailed;
            internal bool unloadAllLoadedObjects;

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
                        this.$locvar0 = new <LoadAssetBundleByNameAsync>c__AnonStorey6<T>();
                        this.$locvar0.<>f__ref$1 = (WWWEx.<LoadAssetBundleByNameAsync>c__Iterator1<T>) this;
                        this.$locvar0.onFailed = this.onFailed;
                        this.$locvar0.assetBundle = null;
                        this.$current = this.$this.LoadAssetBundleAsync(new Action<WWW, AssetBundle>(this.$locvar0.<>m__0), new Action<WWW>(this.$locvar0.<>m__1));
                        if (!this.$disposing)
                        {
                            this.$PC = 1;
                        }
                        goto Label_0204;

                    case 1:
                        if (this.$locvar0.assetBundle != null)
                        {
                            this.<request>__0 = this.$locvar0.assetBundle.LoadAssetAsync<T>(this.assetName);
                            break;
                        }
                        goto Label_0202;

                    case 2:
                        break;

                    default:
                        goto Label_0202;
                }
                while (!this.<request>__0.get_isDone())
                {
                    this.$current = null;
                    if (!this.$disposing)
                    {
                        this.$PC = 2;
                    }
                    goto Label_0204;
                }
                this.<asset>__0 = this.<request>__0.get_asset() as T;
                if (this.<asset>__0 == null)
                {
                    if (!this.$this.IgnoreDebugLog)
                    {
                        Debug.LogError(this.$this.Url + "  " + this.assetName + " is not AssetBundle of " + typeof(T).Name);
                    }
                    if (this.$locvar0.onFailed != null)
                    {
                        this.$locvar0.onFailed();
                    }
                }
                else if (this.onComplete != null)
                {
                    this.onComplete(this.<asset>__0);
                }
                this.<asset>__0 = null;
                this.<request>__0 = null;
                this.$locvar0.assetBundle.Unload(this.unloadAllLoadedObjects);
                this.$PC = -1;
            Label_0202:
                return false;
            Label_0204:
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

            private sealed class <LoadAssetBundleByNameAsync>c__AnonStorey6
            {
                internal WWWEx.<LoadAssetBundleByNameAsync>c__Iterator1<T> <>f__ref$1;
                internal AssetBundle assetBundle;
                internal Action onFailed;

                internal void <>m__0(WWW _www, AssetBundle _assetBundle)
                {
                    this.assetBundle = _assetBundle;
                }

                internal void <>m__1(WWW _www)
                {
                    if (this.onFailed != null)
                    {
                        this.onFailed();
                    }
                }
            }
        }

        [CompilerGenerated]
        private sealed class <LoadAssetBundleMainAssetAsync>c__AnonStorey5<T> where T: Object
        {
            internal WWWEx $this;
            internal Action<WWW, T> onComplete;
            internal Action<WWW> onFailed;
            internal bool unloadAllLoadedObjects;

            internal void <>m__0(WWW www, AssetBundle assetBundle)
            {
                T local = assetBundle.get_mainAsset() as T;
                if (local != null)
                {
                    if (this.onComplete != null)
                    {
                        this.onComplete(www, local);
                    }
                }
                else
                {
                    if (!this.$this.IgnoreDebugLog)
                    {
                        Debug.LogError(www.get_url() + " is not AssetBundle of " + typeof(T).Name);
                    }
                    if (this.onFailed != null)
                    {
                        this.onFailed(www);
                    }
                }
                local = null;
                assetBundle.Unload(this.unloadAllLoadedObjects);
            }

            internal void <>m__1(WWW www)
            {
                if (this.onFailed != null)
                {
                    this.onFailed(www);
                }
            }
        }

        [CompilerGenerated]
        private sealed class <LoadAsync>c__AnonStorey3
        {
            internal WWWEx $this;
            internal Action<WWW> onComplete;
            internal Action<WWW> onFailed;

            internal void <>m__0(WWW www)
            {
                this.onComplete(www);
            }

            internal void <>m__1(WWW www)
            {
                if (!this.$this.IgnoreDebugLog)
                {
                    Debug.LogError("WWW load error " + www.get_url() + "\n" + www.get_error());
                }
                if (this.onFailed != null)
                {
                    this.onFailed(www);
                }
            }

            internal void <>m__2(WWW www)
            {
                if (!this.$this.IgnoreDebugLog)
                {
                    Debug.LogError("WWW timeout " + www.get_url());
                }
                if (this.onFailed != null)
                {
                    this.onFailed(www);
                }
            }
        }

        [CompilerGenerated]
        private sealed class <LoadAsyncSub>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal bool $disposing;
            internal int $PC;
            internal WWWEx $this;
            internal bool <isTimeOut>__2;
            internal bool <retry>__0;
            internal float <time>__2;
            internal WWW <www>__1;
            internal Action<WWW> onCopmlete;
            internal Action<WWW> onFailed;
            internal Action<WWW> onTimeOut;
            internal int retryCount;

            private void <>__Finally0()
            {
                if (this.<www>__1 != null)
                {
                    this.<www>__1.Dispose();
                }
            }

            [DebuggerHidden]
            public void Dispose()
            {
                uint num = (uint) this.$PC;
                this.$disposing = true;
                this.$PC = -1;
                switch (num)
                {
                    case 2:
                        try
                        {
                        }
                        finally
                        {
                            this.<>__Finally0();
                        }
                        break;
                }
            }

            public bool MoveNext()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                bool flag = false;
                switch (num)
                {
                    case 0:
                        if (this.$this.LoadType != WWWEx.Type.Cache)
                        {
                            goto Label_0066;
                        }
                        break;

                    case 1:
                        break;

                    case 2:
                        goto Label_0081;

                    case 3:
                        goto Label_0301;

                    default:
                        goto Label_0308;
                }
                if (!Caching.get_ready())
                {
                    this.$current = null;
                    if (!this.$disposing)
                    {
                        this.$PC = 1;
                    }
                    goto Label_030A;
                }
            Label_0066:
                this.<retry>__0 = false;
                this.<www>__1 = this.$this.CreateWWW();
                num = 0xfffffffd;
            Label_0081:
                try
                {
                    switch (num)
                    {
                        case 2:
                            break;

                        default:
                            this.<time>__2 = 0f;
                            this.<isTimeOut>__2 = false;
                            this.$this.Progress = 0f;
                            break;
                    }
                    while (!this.<www>__1.get_isDone() && !this.<isTimeOut>__2)
                    {
                        if (0f < this.$this.TimeOut)
                        {
                            if (this.$this.Progress == this.<www>__1.get_progress())
                            {
                                this.<time>__2 += Time.get_deltaTime();
                                if (this.<time>__2 >= this.$this.TimeOut)
                                {
                                    this.<isTimeOut>__2 = true;
                                }
                            }
                            else
                            {
                                this.<time>__2 = 0f;
                            }
                        }
                        this.$this.Progress = this.<www>__1.get_progress();
                        if (this.$this.OnUpdate != null)
                        {
                            this.$this.OnUpdate(this.$this);
                        }
                        this.$current = null;
                        if (!this.$disposing)
                        {
                            this.$PC = 2;
                        }
                        flag = true;
                        goto Label_030A;
                    }
                    if (this.<isTimeOut>__2)
                    {
                        if (this.retryCount <= 0)
                        {
                            if (this.onTimeOut != null)
                            {
                                this.onTimeOut(this.<www>__1);
                            }
                        }
                        else
                        {
                            this.<retry>__0 = true;
                        }
                    }
                    else if (!string.IsNullOrEmpty(this.<www>__1.get_error()))
                    {
                        if (this.retryCount <= 0)
                        {
                            if (this.onFailed != null)
                            {
                                this.onFailed(this.<www>__1);
                            }
                        }
                        else
                        {
                            this.<retry>__0 = true;
                        }
                    }
                    else
                    {
                        this.$this.Progress = this.<www>__1.get_progress();
                        if (this.$this.StoreBytes)
                        {
                            this.$this.Bytes = this.<www>__1.get_bytes();
                        }
                        if (this.$this.OnUpdate != null)
                        {
                            this.$this.OnUpdate(this.$this);
                        }
                        if (this.onCopmlete != null)
                        {
                            this.onCopmlete(this.<www>__1);
                        }
                    }
                }
                finally
                {
                    if (!flag)
                    {
                    }
                    this.<>__Finally0();
                }
                if (this.<retry>__0)
                {
                    this.$current = this.$this.LoadAsyncSub(this.onCopmlete, this.onFailed, this.onTimeOut, this.retryCount - 1);
                    if (!this.$disposing)
                    {
                        this.$PC = 3;
                    }
                    goto Label_030A;
                }
            Label_0301:
                this.$PC = -1;
            Label_0308:
                return false;
            Label_030A:
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

        public enum Type
        {
            Default,
            Cache
        }
    }
}

