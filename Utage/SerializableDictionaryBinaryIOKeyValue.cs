namespace Utage
{
    using System;
    using System.IO;

    [Serializable]
    public abstract class SerializableDictionaryBinaryIOKeyValue : SerializableDictionaryKeyValue
    {
        protected SerializableDictionaryBinaryIOKeyValue()
        {
        }

        public abstract void Read(BinaryReader reader);
        public abstract void Write(BinaryWriter writer);
    }
}

