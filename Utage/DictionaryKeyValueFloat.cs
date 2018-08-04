namespace Utage
{
    using System;
    using System.IO;

    [Serializable]
    public class DictionaryKeyValueFloat : SerializableDictionaryBinaryIOKeyValue
    {
        public float value;

        public DictionaryKeyValueFloat()
        {
        }

        public DictionaryKeyValueFloat(string key, float value)
        {
            this.Init(key, value);
        }

        private void Init(string key, float value)
        {
            base.InitKey(key);
            this.value = value;
        }

        public override void Read(BinaryReader reader)
        {
            base.InitKey(reader.ReadString());
            this.value = reader.ReadSingle();
        }

        public override void Write(BinaryWriter writer)
        {
            writer.Write(base.Key);
            writer.Write(this.value);
        }
    }
}

