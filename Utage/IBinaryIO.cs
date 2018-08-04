namespace Utage
{
    using System;
    using System.IO;

    public interface IBinaryIO
    {
        void OnRead(BinaryReader reader);
        void OnWrite(BinaryWriter writer);

        string SaveKey { get; }
    }
}

