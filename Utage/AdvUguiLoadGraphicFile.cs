namespace Utage
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.UI;
    using UtageExtensions;

    [ExecuteInEditMode, AddComponentMenu("Utage/Lib/UI/LoadGraphicFile")]
    public class AdvUguiLoadGraphicFile : MonoBehaviour
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private AssetFile <File>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Graphic <GraphicComponent>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private AdvGraphicInfo <GraphicInfo>k__BackingField;
        private AdvGraphicLoader loader;
        public UnityEvent OnLoadEnd;
        [SerializeField]
        private SizeSetting sizeSetting;

        private T ChangeGraphicComponent<T>() where T: Graphic
        {
            if (this.GraphicComponent == null)
            {
                this.GraphicComponent = base.GetComponent<Graphic>();
            }
            if (this.GraphicComponent != null)
            {
                if (this.GraphicComponent is T)
                {
                    return (this.GraphicComponent as T);
                }
                Object.DestroyImmediate(this.GraphicComponent);
            }
            this.GraphicComponent = base.get_gameObject().AddComponent<T>();
            return (this.GraphicComponent as T);
        }

        public void ClearFile()
        {
            Object.Destroy(this.GraphicComponent);
            this.GraphicComponent = null;
            AssetFileReference component = base.GetComponent<AssetFileReference>();
            if (component != null)
            {
                Object.Destroy(component);
            }
            this.File = null;
            this.Loader.Unload();
            this.GraphicInfo = null;
        }

        [DebuggerHidden]
        private IEnumerator CoWaitTextureFileLoading()
        {
            return new <CoWaitTextureFileLoading>c__Iterator0 { $this = this };
        }

        private void InitSize(Vector2 resouceSize)
        {
            switch (this.RectSizeSetting)
            {
                case SizeSetting.TextureSize:
                    (this.GraphicComponent.get_transform() as RectTransform).SetSize(resouceSize.x, resouceSize.y);
                    break;

                case SizeSetting.GraphicSize:
                    if (this.GraphicInfo != null)
                    {
                        float width = resouceSize.x * this.GraphicInfo.Scale.x;
                        float height = resouceSize.y * this.GraphicInfo.Scale.y;
                        (this.GraphicComponent.get_transform() as RectTransform).SetSize(width, height);
                        break;
                    }
                    Debug.LogError("graphic is null");
                    break;
            }
        }

        public void LoadFile(AdvGraphicInfo graphic)
        {
            <LoadFile>c__AnonStorey1 storey = new <LoadFile>c__AnonStorey1 {
                graphic = graphic,
                $this = this
            };
            this.GraphicInfo = storey.graphic;
            this.Loader.LoadGraphic(storey.graphic, new Action(storey.<>m__0));
        }

        public void LoadTextureFile(string path)
        {
            this.ClearFile();
            this.File = AssetFileManager.Load(path, this);
            this.File.AddReferenceComponent(base.get_gameObject());
            this.File.Unuse(this);
            base.StartCoroutine(this.CoWaitTextureFileLoading());
        }

        private AssetFile File { get; set; }

        private Graphic GraphicComponent { get; set; }

        private AdvGraphicInfo GraphicInfo { get; set; }

        public AdvGraphicLoader Loader
        {
            get
            {
                return ((Component) this).GetComponentCacheCreateIfMissing<AdvGraphicLoader>(ref this.loader);
            }
        }

        public SizeSetting RectSizeSetting
        {
            get
            {
                return this.sizeSetting;
            }
            set
            {
                this.sizeSetting = value;
            }
        }

        [CompilerGenerated]
        private sealed class <CoWaitTextureFileLoading>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal bool $disposing;
            internal int $PC;
            internal AdvUguiLoadGraphicFile $this;

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
                        if (!this.$this.File.IsLoadEnd)
                        {
                            this.$current = null;
                            if (!this.$disposing)
                            {
                                this.$PC = 1;
                            }
                            return true;
                        }
                        if (this.$this.File.Texture != null)
                        {
                            RawImage image = this.$this.ChangeGraphicComponent<RawImage>();
                            image.set_texture(this.$this.File.Texture);
                            this.$this.InitSize(new Vector2((float) image.get_texture().get_width(), (float) image.get_texture().get_height()));
                        }
                        this.$this.OnLoadEnd.Invoke();
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
        private sealed class <LoadFile>c__AnonStorey1
        {
            internal AdvUguiLoadGraphicFile $this;
            internal AdvGraphicInfo graphic;

            internal void <>m__0()
            {
                string fileType = this.graphic.FileType;
                if (fileType != null)
                {
                    if (!(fileType == "2D") && !(fileType == string.Empty))
                    {
                        if (fileType == "Dicing")
                        {
                            DicingImage image2 = this.$this.ChangeGraphicComponent<DicingImage>();
                            image2.DicingData = this.graphic.File.UnityObject as DicingTextures;
                            string subFileName = this.graphic.SubFileName;
                            image2.ChangePattern(subFileName);
                            this.$this.InitSize(new Vector2((float) image2.PatternData.Width, (float) image2.PatternData.Height));
                            goto Label_011C;
                        }
                    }
                    else
                    {
                        RawImage image = this.$this.ChangeGraphicComponent<RawImage>();
                        image.set_texture(this.graphic.File.Texture);
                        this.$this.InitSize(new Vector2((float) image.get_texture().get_width(), (float) image.get_texture().get_height()));
                        goto Label_011C;
                    }
                }
                Debug.LogError(this.graphic.FileType + " is not support ");
            Label_011C:
                this.$this.OnLoadEnd.Invoke();
            }
        }

        public enum SizeSetting
        {
            RectSize,
            TextureSize,
            GraphicSize
        }
    }
}

