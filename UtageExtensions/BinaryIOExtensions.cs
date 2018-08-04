namespace UtageExtensions
{
    using System;
    using System.IO;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public static class BinaryIOExtensions
    {
        public static Bounds ReadBounds(this BinaryReader reader)
        {
            return new Bounds(reader.ReadVector3(), reader.ReadVector3());
        }

        internal static byte[] ReadBuffer(this BinaryReader reader)
        {
            return reader.ReadBytes(reader.ReadInt32());
        }

        internal static void ReadBuffer(this BinaryReader reader, Action<BinaryReader> onRead)
        {
            long position = reader.BaseStream.Position;
            int num2 = reader.ReadInt32();
            long num3 = (position + 4L) + num2;
            bool flag = false;
            try
            {
                onRead(reader);
                flag = reader.BaseStream.Position != num3;
            }
            catch (Exception exception)
            {
                Debug.LogError(exception.Message);
                flag = true;
            }
            if (flag)
            {
                Debug.LogError("Read Buffer Size Error");
                reader.BaseStream.Position = num3;
            }
        }

        public static Color ReadColor(this BinaryReader reader)
        {
            return new Color(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
        }

        public static void ReadJson(this BinaryReader reader, object obj)
        {
            JsonUtility.FromJsonOverwrite(reader.ReadString(), obj);
        }

        public static void ReadLocalTransform(this BinaryReader reader, Transform transform)
        {
            transform.set_localPosition(reader.ReadVector3());
            transform.set_localEulerAngles(reader.ReadVector3());
            transform.set_localScale(reader.ReadVector3());
        }

        public static Quaternion ReadQuaternion(this BinaryReader reader)
        {
            return new Quaternion(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
        }

        public static Rect ReadRect(this BinaryReader reader)
        {
            return new Rect(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
        }

        internal static void ReadRectTransfom(this BinaryReader reader, RectTransform rectTransform)
        {
            reader.ReadLocalTransform(rectTransform);
            rectTransform.set_anchoredPosition3D(reader.ReadVector3());
            rectTransform.set_anchorMin(reader.ReadVector2());
            rectTransform.set_anchorMax(reader.ReadVector2());
            rectTransform.set_sizeDelta(reader.ReadVector2());
            rectTransform.set_pivot(reader.ReadVector2());
        }

        public static Vector2 ReadVector2(this BinaryReader reader)
        {
            return new Vector2(reader.ReadSingle(), reader.ReadSingle());
        }

        public static Vector3 ReadVector3(this BinaryReader reader)
        {
            return new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
        }

        public static Vector4 ReadVector4(this BinaryReader reader)
        {
            return new Vector4(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
        }

        internal static void SkipBuffer(this BinaryReader reader)
        {
            int num = reader.ReadInt32();
            Stream baseStream = reader.BaseStream;
            baseStream.Position += num;
        }

        public static void Write(this BinaryWriter writer, AnimationCurve animationCurve)
        {
            throw new NotImplementedException();
        }

        public static void Write(this BinaryWriter writer, Bounds bounds)
        {
            writer.Write(bounds.get_center());
            writer.Write(bounds.get_size());
        }

        public static void Write(this BinaryWriter writer, Color color)
        {
            writer.Write(color.r);
            writer.Write(color.g);
            writer.Write(color.b);
            writer.Write(color.a);
        }

        public static void Write(this BinaryWriter writer, Quaternion quaternion)
        {
            writer.Write(quaternion.x);
            writer.Write(quaternion.y);
            writer.Write(quaternion.z);
            writer.Write(quaternion.w);
        }

        public static void Write(this BinaryWriter writer, Rect rect)
        {
            writer.Write(rect.get_xMin());
            writer.Write(rect.get_yMin());
            writer.Write(rect.get_width());
            writer.Write(rect.get_height());
        }

        public static void Write(this BinaryWriter writer, Vector2 vector)
        {
            writer.Write(vector.x);
            writer.Write(vector.y);
        }

        public static void Write(this BinaryWriter writer, Vector3 vector)
        {
            writer.Write(vector.x);
            writer.Write(vector.y);
            writer.Write(vector.z);
        }

        public static void Write(this BinaryWriter writer, Vector4 vector)
        {
            writer.Write(vector.x);
            writer.Write(vector.y);
            writer.Write(vector.z);
            writer.Write(vector.w);
        }

        internal static void WriteBuffer(this BinaryWriter writer, byte[] bytes)
        {
            writer.Write(bytes.Length);
            writer.Write(bytes);
        }

        internal static void WriteBuffer(this BinaryWriter writer, Action<BinaryWriter> onWrite)
        {
            long position = writer.BaseStream.Position;
            Stream baseStream = writer.BaseStream;
            baseStream.Position += 4L;
            onWrite(writer);
            long num2 = writer.BaseStream.Position;
            int num3 = (int) ((num2 - position) - 4L);
            writer.BaseStream.Position = position;
            writer.Write(num3);
            writer.BaseStream.Position = num2;
        }

        public static void WriteJson(this BinaryWriter writer, object obj)
        {
            writer.Write(JsonUtility.ToJson(obj));
        }

        public static void WriteLocalTransform(this BinaryWriter writer, Transform transform)
        {
            writer.Write(transform.get_localPosition());
            writer.Write(transform.get_localEulerAngles());
            writer.Write(transform.get_localScale());
        }

        public static void WriteRectTransfom(this BinaryWriter writer, RectTransform rectTransform)
        {
            writer.WriteLocalTransform(rectTransform);
            writer.Write(rectTransform.get_anchoredPosition3D());
            writer.Write(rectTransform.get_anchorMin());
            writer.Write(rectTransform.get_anchorMax());
            writer.Write(rectTransform.get_sizeDelta());
            writer.Write(rectTransform.get_pivot());
        }
    }
}

