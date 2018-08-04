namespace Utage
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UtageExtensions;

    public class AdvGuiBase
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <HasChanged>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private GameObject <Target>k__BackingField;
        private UnityEngine.Canvas canvas;
        private UnityEngine.RectTransform canvasRectTransform;
        private byte[] defaultData;
        private UnityEngine.RectTransform rectTransform;
        private const int Version = 0;

        public AdvGuiBase(GameObject target)
        {
            this.Target = target;
            this.HasChanged = true;
            this.defaultData = this.ToBuffer();
            this.HasChanged = false;
        }

        protected virtual void Read(BinaryReader reader)
        {
            int num = reader.ReadInt32();
            if (num >= 0)
            {
                bool flag = reader.ReadBoolean();
                if (flag)
                {
                    this.HasChanged = flag;
                    this.ReadChanged(reader);
                    return;
                }
                this.Reset();
            }
            object[] args = new object[] { num };
            throw new Exception(LanguageErrorMsg.LocalizeTextFormat(ErrorMsg.UnknownVersion, args));
        }

        public void ReadBuffer(byte[] buffer)
        {
            BinaryUtil.BinaryRead(buffer, new Action<BinaryReader>(this.Read));
        }

        protected virtual void ReadChanged(BinaryReader reader)
        {
            this.Target.SetActive(reader.ReadBoolean());
            reader.ReadRectTransfom(this.RectTransform);
        }

        internal void Reset()
        {
            if (this.HasChanged)
            {
                this.ReadBuffer(this.defaultData);
                this.HasChanged = false;
            }
        }

        public void SetActive(bool isActive)
        {
            this.HasChanged = true;
            this.Target.SetActive(isActive);
        }

        public void SetPosition(float? x, float? y)
        {
            this.HasChanged = true;
            Vector3 vector = this.CanvasRectTransform.InverseTransformPoint(this.RectTransform.get_position());
            if (x.HasValue)
            {
                vector.x = x.Value;
            }
            if (y.HasValue)
            {
                vector.y = y.Value;
            }
            vector = this.CanvasRectTransform.TransformPoint(vector);
            this.RectTransform.set_position(vector);
        }

        internal void SetSize(float? x, float? y)
        {
            this.HasChanged = true;
            if (x.HasValue)
            {
                this.RectTransform.SetSizeWithCurrentAnchors(0, x.Value);
            }
            if (y.HasValue)
            {
                this.RectTransform.SetSizeWithCurrentAnchors(1, y.Value);
            }
        }

        public byte[] ToBuffer()
        {
            return BinaryUtil.BinaryWrite(new Action<BinaryWriter>(this.Write));
        }

        protected void Write(BinaryWriter writer)
        {
            writer.Write(0);
            writer.Write(this.HasChanged);
            if (this.HasChanged)
            {
                this.WriteChanged(writer);
            }
        }

        protected virtual void WriteChanged(BinaryWriter writer)
        {
            writer.Write(this.Target.get_activeSelf());
            writer.WriteRectTransfom(this.RectTransform);
        }

        public UnityEngine.Canvas Canvas
        {
            get
            {
                if (this.canvas == null)
                {
                }
                return (this.canvas = this.Target.GetComponentInParent<UnityEngine.Canvas>());
            }
        }

        public UnityEngine.RectTransform CanvasRectTransform
        {
            get
            {
                if (this.canvasRectTransform == null)
                {
                    this.canvasRectTransform = this.Canvas.get_transform() as UnityEngine.RectTransform;
                }
                return this.canvasRectTransform;
            }
        }

        public bool HasChanged { get; private set; }

        public string Name
        {
            get
            {
                return this.Target.get_name();
            }
        }

        public UnityEngine.RectTransform RectTransform
        {
            get
            {
                if (this.rectTransform == null)
                {
                    this.rectTransform = this.Target.get_transform() as UnityEngine.RectTransform;
                }
                return this.rectTransform;
            }
        }

        public GameObject Target { get; private set; }
    }
}

