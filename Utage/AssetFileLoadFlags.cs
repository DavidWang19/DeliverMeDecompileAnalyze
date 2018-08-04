namespace Utage
{
    using System;

    [Flags]
    public enum AssetFileLoadFlags
    {
        Audio3D = 2,
        None = 0,
        Streaming = 1,
        TextureMipmap = 4,
        Tsv = 8
    }
}

