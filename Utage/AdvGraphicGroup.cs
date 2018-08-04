namespace Utage
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UtageExtensions;

    public class AdvGraphicGroup
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private AdvGraphicLayer <DefaultLayer>k__BackingField;
        private List<AdvGraphicLayer> layers = new List<AdvGraphicLayer>();
        protected AdvGraphicManager manager;
        protected AdvLayerSettingData.LayerType type;
        private const int Version = 0;

        internal AdvGraphicGroup(AdvLayerSettingData.LayerType type, AdvLayerSetting setting, AdvGraphicManager manager)
        {
            this.type = type;
            this.manager = manager;
            foreach (AdvLayerSettingData data in setting.List)
            {
                if (data.Type == type)
                {
                    Type[] typeArray1 = new Type[] { typeof(RectTransform), typeof(Canvas) };
                    GameObject child = new GameObject(data.Name, typeArray1);
                    manager.get_transform().AddChild(child);
                    AdvGraphicLayer item = child.AddComponent<AdvGraphicLayer>();
                    item.Init(manager, data);
                    this.layers.Add(item);
                    if (data.IsDefault)
                    {
                        this.DefaultLayer = item;
                    }
                }
            }
        }

        internal void AddAllGraphics(List<AdvGraphicObject> graphics)
        {
            foreach (AdvGraphicLayer layer in this.layers)
            {
                layer.AddAllGraphics(graphics);
            }
        }

        internal List<AdvGraphicObject> AllGraphics()
        {
            List<AdvGraphicObject> graphics = new List<AdvGraphicObject>();
            foreach (AdvGraphicLayer layer in this.layers)
            {
                layer.AddAllGraphics(graphics);
            }
            return graphics;
        }

        internal List<AdvGraphicLayer> AllGraphicsLayers()
        {
            List<AdvGraphicLayer> list = new List<AdvGraphicLayer>();
            foreach (AdvGraphicLayer layer in this.layers)
            {
                if (layer.CurrentGraphics.Count > 0)
                {
                    list.Add(layer);
                }
            }
            return list;
        }

        internal virtual void Clear()
        {
            foreach (AdvGraphicLayer layer in this.layers)
            {
                layer.Clear();
            }
        }

        internal void DestroyAll()
        {
            foreach (AdvGraphicLayer layer in this.layers)
            {
                layer.Clear();
                Object.Destroy(layer.get_gameObject());
            }
            this.layers.Clear();
            this.DefaultLayer = null;
        }

        internal AdvGraphicObject Draw(string layerName, string name, AdvGraphicOperaitonArg arg)
        {
            return this.FindLayerOrDefault(layerName).Draw(name, arg);
        }

        internal AdvGraphicObject DrawCharacter(string layerName, string name, AdvGraphicOperaitonArg arg)
        {
            <DrawCharacter>c__AnonStorey0 storey = new <DrawCharacter>c__AnonStorey0 {
                name = name,
                layerName = layerName
            };
            AdvGraphicLayer layer = this.layers.Find(new Predicate<AdvGraphicLayer>(storey.<>m__0));
            AdvGraphicLayer layer2 = this.layers.Find(new Predicate<AdvGraphicLayer>(storey.<>m__1));
            if (layer2 == null)
            {
                layer2 = (layer != null) ? layer : this.DefaultLayer;
            }
            if ((layer != layer2) && (layer != null))
            {
                layer.FadeOut(storey.name, arg.GetSkippedFadeTime(this.manager.Engine));
            }
            return layer2.DrawToDefault(storey.name, arg);
        }

        internal AdvGraphicObject DrawToDefault(string name, AdvGraphicOperaitonArg arg)
        {
            return this.DefaultLayer.DrawToDefault(name, arg);
        }

        internal virtual void FadeOut(string name, float fadeTime)
        {
            AdvGraphicLayer layer = this.FindLayerFromObjectName(name);
            if (layer != null)
            {
                layer.FadeOut(name, fadeTime);
            }
        }

        internal virtual void FadeOutAll(float fadeTime)
        {
            foreach (AdvGraphicLayer layer in this.layers)
            {
                layer.FadeOutAll(fadeTime);
            }
        }

        internal void FadeOutAllParticle()
        {
            foreach (AdvGraphicLayer layer in this.layers)
            {
                layer.FadeOutAllParticle();
            }
        }

        internal void FadeOutParticle(string name)
        {
            foreach (AdvGraphicLayer layer in this.layers)
            {
                layer.FadeOutParticle(name);
            }
        }

        internal AdvGraphicLayer FindLayer(string name)
        {
            <FindLayer>c__AnonStorey1 storey = new <FindLayer>c__AnonStorey1 {
                name = name
            };
            return this.layers.Find(new Predicate<AdvGraphicLayer>(storey.<>m__0));
        }

        internal AdvGraphicLayer FindLayerFromObjectName(string name)
        {
            foreach (AdvGraphicLayer layer in this.layers)
            {
                if (layer.Contains(name))
                {
                    return layer;
                }
            }
            return null;
        }

        internal AdvGraphicLayer FindLayerOrDefault(string name)
        {
            <FindLayerOrDefault>c__AnonStorey2 storey = new <FindLayerOrDefault>c__AnonStorey2 {
                name = name
            };
            AdvGraphicLayer local1 = this.layers.Find(new Predicate<AdvGraphicLayer>(storey.<>m__0));
            if (local1 != null)
            {
                return local1;
            }
            return this.DefaultLayer;
        }

        internal AdvGraphicObject FindObject(string name)
        {
            foreach (AdvGraphicLayer layer in this.layers)
            {
                AdvGraphicObject obj2 = layer.Find(name);
                if (obj2 != null)
                {
                    return obj2;
                }
            }
            return null;
        }

        internal bool IsContians(string layerName, string name)
        {
            if (string.IsNullOrEmpty(layerName))
            {
                return (this.FindObject(name) != null);
            }
            AdvGraphicLayer layer = this.FindLayer(layerName);
            return ((layer != null) && (layer.Find(name) != null));
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
                int num2 = reader.ReadInt32();
                for (int i = 0; i < num2; i++)
                {
                    string name = reader.ReadString();
                    AdvGraphicLayer layer = this.FindLayer(name);
                    if (layer != null)
                    {
                        reader.ReadBuffer(new Action<BinaryReader>(layer.Read));
                    }
                    else
                    {
                        reader.SkipBuffer();
                    }
                }
            }
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write(0);
            writer.Write(this.layers.Count);
            foreach (AdvGraphicLayer layer in this.layers)
            {
                writer.Write(layer.get_name());
                writer.WriteBuffer(new Action<BinaryWriter>(layer.Write));
            }
        }

        internal AdvGraphicLayer DefaultLayer { get; set; }

        internal bool IsLoading
        {
            get
            {
                foreach (AdvGraphicLayer layer in this.layers)
                {
                    if (layer.IsLoading)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        [CompilerGenerated]
        private sealed class <DrawCharacter>c__AnonStorey0
        {
            internal string layerName;
            internal string name;

            internal bool <>m__0(AdvGraphicLayer item)
            {
                return item.IsEqualDefaultGraphicName(this.name);
            }

            internal bool <>m__1(AdvGraphicLayer item)
            {
                return (item.SettingData.Name == this.layerName);
            }
        }

        [CompilerGenerated]
        private sealed class <FindLayer>c__AnonStorey1
        {
            internal string name;

            internal bool <>m__0(AdvGraphicLayer item)
            {
                return (item.get_name() == this.name);
            }
        }

        [CompilerGenerated]
        private sealed class <FindLayerOrDefault>c__AnonStorey2
        {
            internal string name;

            internal bool <>m__0(AdvGraphicLayer item)
            {
                return (item.SettingData.Name == this.name);
            }
        }
    }
}

