namespace Utage
{
    using System;
    using System.IO;

    public class BinaryUtil
    {
        public static void BinaryRead(byte[] bytes, Action<BinaryReader> onRead)
        {
            using (MemoryStream stream = new MemoryStream(bytes))
            {
                using (BinaryReader reader = new BinaryReader(stream))
                {
                    onRead(reader);
                }
            }
        }

        public static void BinaryReadFromString(string str, Action<BinaryReader> onRead)
        {
            BinaryRead(Convert.FromBase64String(str), onRead);
        }

        public static byte[] BinaryWrite(Action<BinaryWriter> onWrite)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(stream))
                {
                    onWrite(writer);
                }
                return stream.ToArray();
            }
        }

        public static string BinaryWriteToString(Action<BinaryWriter> onWrite)
        {
            return Convert.ToBase64String(BinaryWrite(onWrite));
        }
    }
}

