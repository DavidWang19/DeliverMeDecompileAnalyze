namespace Utage
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.Events;
    using UtageExtensions;

    [AddComponentMenu("Utage/ADV/Internal/GraphicLayer")]
    public class AdvGraphicLayer : MonoBehaviour
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private UnityEngine.Camera <Camera>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private UnityEngine.Canvas <Canvas>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private AdvGraphicObject <DefaultObject>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Utage.LetterBoxCamera <LetterBoxCamera>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private AdvGraphicManager <Manager>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private AdvLayerSettingData <SettingData>k__BackingField;
        private Dictionary<string, AdvGraphicObject> currentGraphics = new Dictionary<string, AdvGraphicObject>();
        private Transform rootObjects;
        private const int Version = 0;

        internal void AddAllGraphics(List<AdvGraphicObject> graphics)
        {
            graphics.AddRange(this.currentGraphics.Values);
        }

        private bool CheckChangeDafaultObject(string name, AdvGraphicOperaitonArg arg)
        {
            if (this.DefaultObject == null)
            {
                return false;
            }
            if (this.DefaultObject.get_name() != name)
            {
                return true;
            }
            if (this.DefaultObject.LastResource == null)
            {
                return false;
            }
            return ((arg.Graphic.FileType != this.DefaultObject.LastResource.FileType) || this.DefaultObject.TargetObject.CheckFailedCrossFade(arg.Graphic));
        }

        internal void Clear()
        {
            List<AdvGraphicObject> list = new List<AdvGraphicObject>(this.currentGraphics.Values);
            foreach (AdvGraphicObject obj2 in list)
            {
                obj2.Clear();
            }
            this.currentGraphics.Clear();
            this.DefaultObject = null;
        }

        [DebuggerHidden]
        private IEnumerator CoDelayOut(AdvGraphicObject obj, float delay)
        {
            return new <CoDelayOut>c__Iterator0 { delay = delay, obj = obj };
        }

        internal bool Contains(string name)
        {
            return this.currentGraphics.ContainsKey(name);
        }

        private AdvGraphicObject CreateObject(string name, AdvGraphicInfo grapic)
        {
            AdvGraphicObject component;
            GameObject obj3;
            if (grapic.TryGetAdvGraphicObjectPrefab(out obj3))
            {
                GameObject obj4 = Object.Instantiate<GameObject>(obj3);
                obj4.set_name(name);
                component = obj4.GetComponent<AdvGraphicObject>();
                this.rootObjects.AddChild(component.get_gameObject());
            }
            else
            {
                component = this.rootObjects.AddChildGameObjectComponent<AdvGraphicObject>(name);
            }
            component.Init(this, grapic);
            if (this.currentGraphics.Count == 0)
            {
                this.ResetCanvasRectTransform();
            }
            this.currentGraphics.Add(component.get_name(), component);
            return component;
        }

        internal void DelayOut(string name, float delay)
        {
            AdvGraphicObject obj2;
            if (this.currentGraphics.TryGetValue(name, out obj2))
            {
                this.Remove(obj2);
                base.StartCoroutine(this.CoDelayOut(obj2, delay));
            }
        }

        internal AdvGraphicObject Draw(string name, AdvGraphicOperaitonArg arg)
        {
            <Draw>c__AnonStorey1 storey;
            storey = new <Draw>c__AnonStorey1 {
                arg = arg,
                $this = this,
                obj = this.GetObjectCreateIfMissing(name, storey.arg.Graphic)
            };
            storey.obj.Loader.LoadGraphic(storey.arg.Graphic, new Action(storey.<>m__0));
            return storey.obj;
        }

        internal AdvGraphicObject DrawToDefault(string name, AdvGraphicOperaitonArg arg)
        {
            if (this.CheckChangeDafaultObject(name, arg))
            {
                if (this.SettingData.Type == AdvLayerSettingData.LayerType.Bg)
                {
                    this.DelayOut(this.DefaultObject.get_name(), arg.GetSkippedFadeTime(this.Engine));
                }
                else
                {
                    this.FadeOut(this.DefaultObject.get_name(), arg.GetSkippedFadeTime(this.Engine));
                }
            }
            this.DefaultObject = this.Draw(name, arg);
            return this.DefaultObject;
        }

        internal void FadeOut(string name, float fadeTime)
        {
            AdvGraphicObject obj2;
            if (this.currentGraphics.TryGetValue(name, out obj2))
            {
                obj2.FadeOut(fadeTime);
                this.Remove(obj2);
            }
        }

        internal void FadeOutAll(float fadeTime)
        {
            List<AdvGraphicObject> list = new List<AdvGraphicObject>(this.currentGraphics.Values);
            foreach (AdvGraphicObject obj2 in list)
            {
                obj2.FadeOut(fadeTime);
            }
            this.currentGraphics.Clear();
            this.DefaultObject = null;
        }

        internal void FadeOutAllParticle()
        {
            List<AdvGraphicObject> list = new List<AdvGraphicObject>(this.currentGraphics.Values);
            foreach (AdvGraphicObject obj2 in list)
            {
                if (obj2.TargetObject is AdvGraphicObjectParticle)
                {
                    obj2.FadeOut(0f);
                    this.Remove(obj2);
                }
            }
        }

        internal void FadeOutParticle(string name)
        {
            AdvGraphicObject obj2;
            if (this.currentGraphics.TryGetValue(name, out obj2) && (obj2.TargetObject is AdvGraphicObjectParticle))
            {
                obj2.FadeOut(0f);
                this.Remove(obj2);
            }
        }

        internal AdvGraphicObject Find(string name)
        {
            AdvGraphicObject obj2;
            if (this.currentGraphics.TryGetValue(name, out obj2))
            {
                return obj2;
            }
            return null;
        }

        internal AdvGraphicObject GetObjectCreateIfMissing(string name, AdvGraphicInfo grapic)
        {
            AdvGraphicObject obj2;
            if (grapic == null)
            {
                Debug.LogError(name + " grapic is null");
                return null;
            }
            if (!this.currentGraphics.TryGetValue(name, out obj2))
            {
                obj2 = this.CreateObject(name, grapic);
            }
            return obj2;
        }

        public void Init(AdvGraphicManager manager, AdvLayerSettingData settingData)
        {
            this.Manager = manager;
            this.SettingData = settingData;
            this.Canvas = base.GetComponent<UnityEngine.Canvas>();
            this.Canvas.set_additionalShaderChannels(0x19);
            if (!string.IsNullOrEmpty(this.SettingData.LayerMask))
            {
                this.Canvas.get_gameObject().set_layer(LayerMask.NameToLayer(this.SettingData.LayerMask));
            }
            this.Canvas.set_sortingOrder(this.SettingData.Order);
            this.Camera = this.Engine.CameraManager.FindCameraByLayer(this.Canvas.get_gameObject().get_layer());
            if (this.Camera == null)
            {
                Debug.LogError("Cant find camera");
                this.Camera = this.Engine.CameraManager.FindCameraByLayer(0);
            }
            this.LetterBoxCamera = this.Camera.get_gameObject().GetComponent<Utage.LetterBoxCamera>();
            this.Canvas.set_worldCamera(this.Camera);
            this.Canvas.get_gameObject().AddComponent<GraphicRaycaster>().set_enabled(false);
            this.rootObjects = this.Canvas.get_transform();
            this.ResetCanvasRectTransform();
            if (this.Manager.DebugAutoResetCanvasPosition)
            {
                this.LetterBoxCamera.OnGameScreenSizeChange.AddListener(new UnityAction<Utage.LetterBoxCamera>(this, (IntPtr) this.<Init>m__0));
            }
        }

        internal bool IsEqualDefaultGraphicName(string name)
        {
            return ((this.DefaultObject != null) && (this.DefaultObject.get_name() == name));
        }

        public void Read(BinaryReader reader)
        {
            int num = reader.ReadInt32();
            if ((num < 0) || (num > 0))
            {
                object[] args = new object[] { num };
                Debug.LogError(LanguageErrorMsg.LocalizeTextFormat(ErrorMsg.UnknownVersion, args));
            }
            else
            {
                reader.ReadLocalTransform(base.get_transform());
                int num2 = reader.ReadInt32();
                for (int i = 0; i < num2; i++)
                {
                    <Read>c__AnonStorey2 storey = new <Read>c__AnonStorey2 {
                        $this = this
                    };
                    string str = reader.ReadString();
                    storey.graphic = null;
                    reader.ReadBuffer(new Action<BinaryReader>(storey.<>m__0));
                    byte[] buffer = reader.ReadBuffer();
                    this.CreateObject(str, storey.graphic).Read(buffer, storey.graphic);
                }
                string name = reader.ReadString();
                this.DefaultObject = this.Find(name);
            }
        }

        internal void Remove(AdvGraphicObject obj)
        {
            if (this.currentGraphics.ContainsValue(obj))
            {
                this.currentGraphics.Remove(obj.get_name());
            }
            if (this.DefaultObject == obj)
            {
                this.DefaultObject = null;
            }
        }

        internal void ResetCanvasRectTransform()
        {
            float num;
            float num2;
            float num3;
            float num4;
            RectTransform t = this.Canvas.get_transform() as RectTransform;
            this.SettingData.Horizontal.GetBorderdPositionAndSize(this.GameScreenSize.x, out num, out num2);
            this.SettingData.Vertical.GetBorderdPositionAndSize(this.GameScreenSize.y, out num3, out num4);
            t.set_localPosition((Vector3) (new Vector3(num, num3, this.SettingData.Z) / this.Manager.PixelsToUnits));
            t.SetSize(num2, num4);
            t.set_localScale((Vector3) (this.SettingData.Scale / this.Manager.PixelsToUnits));
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write(0);
            writer.WriteLocalTransform(base.get_transform());
            int num = 0;
            foreach (KeyValuePair<string, AdvGraphicObject> pair in this.currentGraphics)
            {
                if (pair.Value.LastResource.DataType == "Capture")
                {
                    Debug.LogError("Caputure image not support on save");
                }
                else
                {
                    num++;
                }
            }
            writer.Write(num);
            foreach (KeyValuePair<string, AdvGraphicObject> pair2 in this.currentGraphics)
            {
                if (pair2.Value.LastResource.DataType != "Capture")
                {
                    writer.Write(pair2.Key);
                    writer.WriteBuffer(new Action<BinaryWriter>(pair2.Value.LastResource.OnWrite));
                    writer.WriteBuffer(new Action<BinaryWriter>(pair2.Value.Write));
                }
            }
            writer.Write((this.DefaultObject != null) ? this.DefaultObject.get_name() : string.Empty);
        }

        public UnityEngine.Camera Camera { get; private set; }

        public UnityEngine.Canvas Canvas { get; private set; }

        public Dictionary<string, AdvGraphicObject> CurrentGraphics
        {
            get
            {
                return this.currentGraphics;
            }
        }

        public AdvGraphicObject DefaultObject { get; private set; }

        public AdvEngine Engine
        {
            get
            {
                return this.Manager.Engine;
            }
        }

        public Vector2 GameScreenSize
        {
            get
            {
                return this.LetterBoxCamera.CurrentSize;
            }
        }

        internal bool IsLoading
        {
            get
            {
                foreach (KeyValuePair<string, AdvGraphicObject> pair in this.currentGraphics)
                {
                    if (pair.Value.Loader.IsLoading)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public Utage.LetterBoxCamera LetterBoxCamera { get; private set; }

        public AdvGraphicManager Manager { get; private set; }

        public AdvLayerSettingData SettingData { get; private set; }

        [CompilerGenerated]
        private sealed class <CoDelayOut>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal bool $disposing;
            internal int $PC;
            internal float delay;
            internal AdvGraphicObject obj;

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
                        this.$current = new WaitForSeconds(this.delay);
                        if (!this.$disposing)
                        {
                            this.$PC = 1;
                        }
                        return true;

                    case 1:
                        if (this.obj != null)
                        {
                            this.obj.Clear();
                        }
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
        private sealed class <Draw>c__AnonStorey1
        {
            internal AdvGraphicLayer $this;
            internal AdvGraphicOperaitonArg arg;
            internal AdvGraphicObject obj;

            internal void <>m__0()
            {
                this.obj.Draw(this.arg, this.arg.GetSkippedFadeTime(this.$this.Engine));
            }
        }

        [CompilerGenerated]
        private sealed class <Read>c__AnonStorey2
        {
            internal AdvGraphicLayer $this;
            internal AdvGraphicInfo graphic;

            internal void <>m__0(BinaryReader x)
            {
                this.graphic = AdvGraphicInfo.ReadGraphicInfo(this.$this.Engine, x);
            }
        }
    }
}

