namespace Utage
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.UI;
    using UtageExtensions;

    [AddComponentMenu("Utage/ADV/Internal/RenderTextureSpace")]
    public class AdvRenderTextureSpace : MonoBehaviour
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private UnityEngine.Canvas <Canvas>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private UnityEngine.UI.CanvasScaler <CanvasScaler>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Camera <RenderCamera>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private GameObject <RenderRoot>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private UnityEngine.RenderTexture <RenderTexture>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private AdvRenderTextureSetting <Setting>k__BackingField;

        private void CreateCamera(float pixelsToUnits)
        {
            this.RenderCamera = base.get_gameObject().AddComponent<Camera>();
            this.RenderCamera.get_gameObject().set_layer(base.get_gameObject().get_layer());
            this.RenderCamera.set_cullingMask(((int) 1) << base.get_gameObject().get_layer());
            this.RenderCamera.set_depth(-100f);
            this.RenderCamera.set_clearFlags(2);
            this.RenderCamera.set_backgroundColor((this.RenderTextureType != AdvRenderTextureMode.Image) ? new Color(0f, 0f, 0f, 0f) : new Color(0f, 0f, 0f, 1f));
            this.RenderCamera.set_orthographic(true);
            this.RenderCamera.set_orthographicSize((this.Setting.RenderTextureSize.y / pixelsToUnits) / 2f);
        }

        private void CreateCanvas()
        {
            Type[] typeArray1 = new Type[] { typeof(RectTransform), typeof(UnityEngine.Canvas) };
            GameObject child = new GameObject("Canvas", typeArray1);
            this.RenderCamera.get_transform().AddChild(child);
            this.Canvas = child.GetComponent<UnityEngine.Canvas>();
            this.Canvas.set_additionalShaderChannels(0x19);
            this.RenderCamera.set_nearClipPlane(-1f);
            this.Canvas.set_renderMode(1);
            this.Canvas.set_worldCamera(this.RenderCamera);
            this.CanvasScaler = this.Canvas.get_gameObject().AddComponent<UnityEngine.UI.CanvasScaler>();
            this.CanvasScaler.set_referenceResolution(this.Setting.RenderTextureSize);
            this.CanvasScaler.set_uiScaleMode(0);
            this.CanvasScaler.set_scaleFactor(1f);
            this.CanvasScaler.set_screenMatchMode(1);
            this.RenderRoot = this.Canvas.get_transform().AddChildGameObjectComponent<RectTransform>("Root").get_gameObject();
        }

        private void CreateRoot(AdvGraphicInfo graphic, float pixelsToUnits)
        {
            if (graphic.IsUguiComponentType)
            {
                this.CreateCanvas();
            }
            else
            {
                this.RenderRoot = this.RenderCamera.get_transform().AddChildGameObject("Root");
                this.RenderRoot.get_transform().set_localPosition((Vector3) (this.Setting.RenderTextureOffset / pixelsToUnits));
                this.RenderRoot.get_transform().set_localScale(graphic.Scale);
            }
        }

        private void CreateTexture()
        {
            int x = (int) this.Setting.RenderTextureSize.x;
            int y = (int) this.Setting.RenderTextureSize.y;
            this.RenderTexture = new UnityEngine.RenderTexture(x, y, 0x10, 0);
            this.RenderCamera.set_targetTexture(this.RenderTexture);
        }

        internal void Init(AdvGraphicInfo graphic, float pixelsToUnits)
        {
            this.Setting = graphic.RenderTextureSetting;
            this.CreateCamera(pixelsToUnits);
            this.CreateTexture();
            this.CreateRoot(graphic, pixelsToUnits);
        }

        private void OnDestroy()
        {
            if (this.RenderTexture != null)
            {
                this.RenderTexture.Release();
                Object.Destroy(this.RenderTexture);
            }
        }

        private void Update()
        {
            if (!this.RenderTexture.IsCreated())
            {
                this.RenderTexture.Create();
            }
        }

        private UnityEngine.Canvas Canvas { get; set; }

        private UnityEngine.UI.CanvasScaler CanvasScaler { get; set; }

        private Camera RenderCamera { get; set; }

        public GameObject RenderRoot { get; private set; }

        public UnityEngine.RenderTexture RenderTexture { get; private set; }

        public AdvRenderTextureMode RenderTextureType
        {
            get
            {
                return this.Setting.RenderTextureType;
            }
        }

        private AdvRenderTextureSetting Setting { get; set; }
    }
}

