namespace Utage
{
    using System;
    using System.IO;

    [Serializable]
    public class DictionaryKeyValueBool : SerializableDictionaryBinaryIOKeyValue
    {
        public bool value;

        public DictionaryKeyValueBool()
        {
        }

        public DictionaryKeyValueBool(string key, bool value)
        {
            this.Init(key, value);
        }

        private void Init(string key, bool value)
        {
            base.InitKey(key);
            this.value = value;
        }

        public override void Read(BinaryReader reader)
        {
            base.InitKey(reader.ReadString());
            this.value = reader.ReadBoolean();
        }

        public override void Write(BinaryWriter writer)
        {
            writer.Write(base.Key);
            writer.Write(this.value);
        }
    }
}

