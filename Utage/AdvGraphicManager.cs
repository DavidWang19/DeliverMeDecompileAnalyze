namespace Utage
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using UnityEngine;
    using UnityEngine.Events;
    using UtageExtensions;

    [AddComponentMenu("Utage/ADV/GraphicManager")]
    public class AdvGraphicManager : MonoBehaviour, IBinaryIO
    {
        [SerializeField]
        private string bgSpriteName = "BG";
        [SerializeField]
        private bool debugAutoResetCanvasPosition;
        private AdvEngine engine;
        private Dictionary<AdvLayerSettingData.LayerType, AdvGraphicGroup> Groups = new Dictionary<AdvLayerSettingData.LayerType, AdvGraphicGroup>();
        private bool isEventMode;
        [SerializeField]
        private float pixelsToUnits = 100f;
        [SerializeField]
        private AdvGraphicRenderTextureManager renderTextureManager;
        [SerializeField]
        private float sortOderToZUnits = 100f;
        private const int Version = 0;
        [SerializeField]
        private AdvVideoManager videoManager;

        internal void AddClickEvent(string name, bool isPolygon, StringGridRow row, UnityAction<BaseEventData> action)
        {
            AdvGraphicObject obj2 = this.FindObject(name);
            if (obj2 == null)
            {
                Debug.LogError("can't find Graphic object" + name);
            }
            else
            {
                IAdvClickEvent componentInChildren = obj2.get_gameObject().GetComponentInChildren<IAdvClickEvent>();
                if (componentInChildren == null)
                {
                    Debug.LogError("can't find IAdvClickEvent Interface in " + name);
                }
                else
                {
                    componentInChildren.AddClickEvent(isPolygon, row, action);
                }
            }
        }

        internal List<AdvGraphicObject> AllGraphics()
        {
            List<AdvGraphicObject> graphics = new List<AdvGraphicObject>();
            foreach (KeyValuePair<AdvLayerSettingData.LayerType, AdvGraphicGroup> pair in this.Groups)
            {
                pair.Value.AddAllGraphics(graphics);
            }
            return graphics;
        }

        public void BootInit(AdvEngine engine, AdvLayerSetting setting)
        {
            this.engine = engine;
            this.Groups.Clear();
            IEnumerator enumerator = Enum.GetValues(typeof(AdvLayerSettingData.LayerType)).GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    AdvLayerSettingData.LayerType current = (AdvLayerSettingData.LayerType) enumerator.Current;
                    AdvGraphicGroup group = new AdvGraphicGroup(current, setting, this);
                    this.Groups.Add(current, group);
                }
            }
            finally
            {
                IDisposable disposable = enumerator as IDisposable;
                if (disposable != null)
                {
                    disposable.Dispose();
                }
            }
        }

        internal void Clear()
        {
            foreach (AdvGraphicGroup group in this.Groups.Values)
            {
                group.Clear();
            }
        }

        internal void CreateCaptureImageObject(string name, string cameraName, string layerName)
        {
            AdvGraphicLayer layer = this.FindLayer(layerName);
            if (layer == null)
            {
                Debug.LogError(layerName + " is not layer name");
            }
            else
            {
                CameraRoot root = this.Engine.CameraManager.FindCameraRoot(cameraName);
                if (root == null)
                {
                    Debug.LogError(cameraName + " is not camera name");
                }
                else
                {
                    AdvGraphicInfo grapic = new AdvGraphicInfo("Capture", name, "2D");
                    layer.GetObjectCreateIfMissing(name, grapic).InitCaptureImage(grapic, root.LetterBoxCamera.CachedCamera);
                }
            }
        }

        internal void DrawObject(string layerName, string label, AdvGraphicOperaitonArg graphicOperaitonArg)
        {
            this.FindLayer(layerName).Draw(label, graphicOperaitonArg);
        }

        internal void FadeOutAllParticle()
        {
            foreach (KeyValuePair<AdvLayerSettingData.LayerType, AdvGraphicGroup> pair in this.Groups)
            {
                pair.Value.FadeOutAllParticle();
            }
        }

        internal void FadeOutParticle(string name)
        {
            foreach (KeyValuePair<AdvLayerSettingData.LayerType, AdvGraphicGroup> pair in this.Groups)
            {
                pair.Value.FadeOutParticle(name);
            }
        }

        internal AdvGraphicLayer FindLayer(string layerName)
        {
            foreach (KeyValuePair<AdvLayerSettingData.LayerType, AdvGraphicGroup> pair in this.Groups)
            {
                AdvGraphicLayer layer = pair.Value.FindLayer(layerName);
                if (layer != null)
                {
                    return layer;
                }
            }
            return null;
        }

        internal AdvGraphicObject FindObject(string name)
        {
            foreach (KeyValuePair<AdvLayerSettingData.LayerType, AdvGraphicGroup> pair in this.Groups)
            {
                AdvGraphicObject obj2 = pair.Value.FindObject(name);
                if (obj2 != null)
                {
                    return obj2;
                }
            }
            return null;
        }

        internal GameObject FindObjectOrLayer(string name)
        {
            AdvGraphicObject obj2 = this.FindObject(name);
            if (obj2 != null)
            {
                return obj2.get_gameObject();
            }
            AdvGraphicLayer layer = this.FindLayer(name);
            if (layer != null)
            {
                return layer.get_gameObject();
            }
            return null;
        }

        public void OnRead(BinaryReader reader)
        {
            int num = reader.ReadInt32();
            if ((num < 0) || (num > 0))
            {
                object[] args = new object[] { num };
                Debug.LogError(LanguageErrorMsg.LocalizeTextFormat(ErrorMsg.UnknownVersion, args));
            }
            else
            {
                this.isEventMode = reader.ReadBoolean();
                int num2 = reader.ReadInt32();
                for (int i = 0; i < num2; i++)
                {
                    AdvLayerSettingData.LayerType type = (AdvLayerSettingData.LayerType) reader.ReadInt32();
                    reader.ReadBuffer(new Action<BinaryReader>(this.Groups[type].Read));
                }
            }
        }

        public void OnWrite(BinaryWriter writer)
        {
            writer.Write(0);
            writer.Write(this.isEventMode);
            writer.Write(this.Groups.Count);
            foreach (KeyValuePair<AdvLayerSettingData.LayerType, AdvGraphicGroup> pair in this.Groups)
            {
                writer.Write(pair.Key);
                writer.WriteBuffer(new Action<BinaryWriter>(pair.Value.Write));
            }
        }

        public void Remake(AdvLayerSetting setting)
        {
            foreach (AdvGraphicGroup group in this.Groups.Values)
            {
                group.DestroyAll();
            }
            this.Groups.Clear();
            IEnumerator enumerator = Enum.GetValues(typeof(AdvLayerSettingData.LayerType)).GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    AdvLayerSettingData.LayerType current = (AdvLayerSettingData.LayerType) enumerator.Current;
                    AdvGraphicGroup group2 = new AdvGraphicGroup(current, setting, this);
                    this.Groups.Add(current, group2);
                }
            }
            finally
            {
                IDisposable disposable = enumerator as IDisposable;
                if (disposable != null)
                {
                    disposable.Dispose();
                }
            }
        }

        internal void RemoveClickEvent(string name)
        {
            AdvGraphicObject obj2 = this.FindObject(name);
            if (obj2 != null)
            {
                IAdvClickEvent componentInChildren = obj2.get_gameObject().GetComponentInChildren<IAdvClickEvent>();
                if (componentInChildren != null)
                {
                    componentInChildren.RemoveClickEvent();
                }
            }
        }

        public AdvGraphicGroup BgManager
        {
            get
            {
                return this.Groups[AdvLayerSettingData.LayerType.Bg];
            }
        }

        public string BgSpriteName
        {
            get
            {
                return this.bgSpriteName;
            }
        }

        public AdvGraphicGroup CharacterManager
        {
            get
            {
                return this.Groups[AdvLayerSettingData.LayerType.Character];
            }
        }

        public bool DebugAutoResetCanvasPosition
        {
            get
            {
                return false;
            }
        }

        internal AdvEngine Engine
        {
            get
            {
                return this.engine;
            }
        }

        public bool IsEventMode
        {
            get
            {
                return this.isEventMode;
            }
            set
            {
                this.isEventMode = value;
            }
        }

        internal bool IsLoading
        {
            get
            {
                foreach (KeyValuePair<AdvLayerSettingData.LayerType, AdvGraphicGroup> pair in this.Groups)
                {
                    if (pair.Value.IsLoading)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public float PixelsToUnits
        {
            get
            {
                return this.pixelsToUnits;
            }
        }

        public AdvGraphicRenderTextureManager RenderTextureManager
        {
            get
            {
                if (this.renderTextureManager == null)
                {
                    this.renderTextureManager = base.get_transform().get_parent().AddChildGameObjectComponent<AdvGraphicRenderTextureManager>("GraphicRenderTextureManager");
                }
                return this.renderTextureManager;
            }
        }

        public string SaveKey
        {
            get
            {
                return "AdvGraphicManager";
            }
        }

        public float SortOderToZUnits
        {
            get
            {
                return this.sortOderToZUnits;
            }
        }

        public AdvGraphicGroup SpriteManager
        {
            get
            {
                return this.Groups[AdvLayerSettingData.LayerType.Sprite];
            }
        }

        public AdvVideoManager VideoManager
        {
            get
            {
                if (this.videoManager == null)
                {
                    this.videoManager = base.get_transform().get_parent().AddChildGameObjectComponent<AdvVideoManager>("VideoManager");
                }
                return this.videoManager;
            }
        }
    }
}

