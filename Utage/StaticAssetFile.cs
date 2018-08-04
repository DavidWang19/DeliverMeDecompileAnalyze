namespace Utage
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class StaticAssetFile : AssetFileBase
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private StaticAsset <Asset>k__BackingField;

        public StaticAssetFile(StaticAsset asset, AssetFileManager mangager, AssetFileInfo fileInfo, IAssetFileSettingData settingData) : base(mangager, fileInfo, settingData)
        {
            this.Asset = asset;
            base.Text = this.Asset.Asset as TextAsset;
            base.Texture = this.Asset.Asset as Texture2D;
            base.Sound = this.Asset.Asset as AudioClip;
            base.UnityObject = this.Asset.Asset;
            base.IsLoadEnd = true;
            base.IgnoreUnload = true;
            if (base.Texture != null)
            {
                this.FileType = AssetFileType.Texture;
            }
            else if (base.Sound != null)
            {
                this.FileType = AssetFileType.Sound;
            }
            else if (base.UnityObject != null)
            {
                this.FileType = AssetFileType.UnityObject;
            }
        }

        public override bool CheckCacheOrLocal()
        {
            return true;
        }

        [DebuggerHidden]
        public override IEnumerator LoadAsync(Action onComplete, Action onFailed)
        {
            return new <LoadAsync>c__Iterator0 { onComplete = onComplete };
        }

        public override void Unload()
        {
        }

        public StaticAsset Asset { get; protected set; }

        [CompilerGenerated]
        private sealed class <LoadAsync>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal bool $disposing;
            internal int $PC;
            internal Action onComplete;

            [DebuggerHidden]
            public void Dispose()
            {
            }

            public bool MoveNext()
            {
                this.$PC = -1;
                if (this.$PC == 0)
                {
                    this.onComplete();
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
    }
}

