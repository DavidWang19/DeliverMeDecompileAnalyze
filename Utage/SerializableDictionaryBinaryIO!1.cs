namespace Utage
{
    using System;
    using System.IO;

    [Serializable]
    public abstract class SerializableDictionaryBinaryIO<T> : SerializableDictionary<T> where T: SerializableDictionaryBinaryIOKeyValue, new()
    {
        protected SerializableDictionaryBinaryIO()
        {
        }

        public void Read(BinaryReader reader)
        {
            base.Clear();
            int num = reader.ReadInt32();
            for (int i = 0; i < num; i++)
            {
                T val = Activator.CreateInstance<T>();
                val.Read(reader);
                base.Add(val);
            }
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write(base.Count);
            foreach (SerializableDictionaryBinaryIOKeyValue value2 in base.List)
            {
                value2.Write(writer);
            }
        }
    }
}

