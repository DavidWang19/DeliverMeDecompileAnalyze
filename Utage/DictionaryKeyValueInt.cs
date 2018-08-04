namespace Utage
{
    using System;
    using System.IO;

    [Serializable]
    public class DictionaryKeyValueInt : SerializableDictionaryBinaryIOKeyValue
    {
        public int value;

        public DictionaryKeyValueInt()
        {
        }

        public DictionaryKeyValueInt(string key, int value)
        {
            this.Init(key, value);
        }

        private void Init(string key, int value)
        {
            base.InitKey(key);
            this.value = value;
        }

        public override void Read(BinaryReader reader)
        {
            base.InitKey(reader.ReadString());
            this.value = reader.ReadInt32();
        }

        public override void Write(BinaryWriter writer)
        {
            writer.Write(base.Key);
            writer.Write(this.value);
        }
    }
}

