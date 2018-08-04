namespace Utage
{
    using System;
    using System.IO;

    [Serializable]
    public class DictionaryKeyValueDouble : SerializableDictionaryBinaryIOKeyValue
    {
        public double value;

        public DictionaryKeyValueDouble()
        {
        }

        public DictionaryKeyValueDouble(string key, double value)
        {
            this.Init(key, value);
        }

        private void Init(string key, double value)
        {
            base.InitKey(key);
            this.value = value;
        }

        public override void Read(BinaryReader reader)
        {
            base.InitKey(reader.ReadString());
            this.value = reader.ReadDouble();
        }

        public override void Write(BinaryWriter writer)
        {
            writer.Write(base.Key);
            writer.Write(this.value);
        }
    }
}

