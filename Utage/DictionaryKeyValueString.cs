namespace Utage
{
    using System;
    using System.IO;

    [Serializable]
    public class DictionaryKeyValueString : SerializableDictionaryBinaryIOKeyValue
    {
        public string value;

        public DictionaryKeyValueString()
        {
        }

        public DictionaryKeyValueString(string key, string value)
        {
            this.Init(key, value);
        }

        private void Init(string key, string value)
        {
            base.InitKey(key);
            this.value = value;
        }

        public override void Read(BinaryReader reader)
        {
            base.InitKey(reader.ReadString());
            this.value = reader.ReadString();
        }

        public override void Write(BinaryWriter writer)
        {
            writer.Write(base.Key);
            writer.Write(this.value);
        }
    }
}

