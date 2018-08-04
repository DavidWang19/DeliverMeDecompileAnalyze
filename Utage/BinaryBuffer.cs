namespace Utage
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using UnityEngine;
    using UtageExtensions;

    public class BinaryBuffer
    {
        private Dictionary<string, byte[]> buffers = new Dictionary<string, byte[]>();

        public BinaryBuffer Clone()
        {
            BinaryBuffer buffer = new BinaryBuffer();
            foreach (string str in this.Buffers.Keys)
            {
                buffer.Buffers.Add(str, this.Buffers[str]);
            }
            return buffer;
        }

        public void MakeBuffer(List<IBinaryIO> ioList)
        {
            this.Buffers.Clear();
            ioList.ForEach(delegate (IBinaryIO x) {
                if (this.Buffers.ContainsKey(x.SaveKey))
                {
                    Debug.LogError(string.Format("Save data Key [{0}] is already exsits. Please use another key.", x.SaveKey));
                }
                else
                {
                    byte[] buffer = BinaryUtil.BinaryWrite(new Action<BinaryWriter>(x.OnWrite));
                    this.Buffers.Add(x.SaveKey, buffer);
                }
            });
        }

        public void Overrirde(IBinaryIO io)
        {
            if (this.Buffers.ContainsKey(io.SaveKey))
            {
                BinaryUtil.BinaryRead(this.Buffers[io.SaveKey], new Action<BinaryReader>(io.OnRead));
            }
            else
            {
                Debug.LogError(string.Format("Not found Save data Key [{0}] ", io.SaveKey));
            }
        }

        public void Overrirde(List<IBinaryIO> ioList)
        {
            ioList.ForEach(x => this.Overrirde(x));
        }

        public void Read(BinaryReader reader)
        {
            this.Buffers.Clear();
            int num = reader.ReadInt32();
            for (int i = 0; i < num; i++)
            {
                this.Buffers.Add(reader.ReadString(), reader.ReadBuffer());
            }
        }

        public static void Read(BinaryReader reader, List<IBinaryIO> ioList)
        {
            BinaryBuffer buffer = new BinaryBuffer();
            buffer.Read(reader);
            buffer.Overrirde(ioList);
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write(this.Buffers.Count);
            foreach (string str in this.Buffers.Keys)
            {
                writer.Write(str);
                writer.WriteBuffer(this.Buffers[str]);
            }
        }

        public static void Write(BinaryWriter writer, List<IBinaryIO> ioList)
        {
            BinaryBuffer buffer = new BinaryBuffer();
            buffer.MakeBuffer(ioList);
            buffer.Write(writer);
        }

        private Dictionary<string, byte[]> Buffers
        {
            get
            {
                return this.buffers;
            }
        }

        public bool IsEmpty
        {
            get
            {
                return (this.Buffers.Count <= 0);
            }
        }
    }
}

