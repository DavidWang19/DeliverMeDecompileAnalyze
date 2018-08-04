namespace Utage
{
    using System;

    public class Crypt
    {
        public static void DecryptXor(byte[] key, byte[] buffer)
        {
            DecryptXor(key, buffer, 0, buffer.Length);
        }

        public static void DecryptXor(byte[] key, byte[] buffer, int offset, int count)
        {
            if ((key != null) && (key.Length > 0))
            {
                int length = key.Length;
                for (int i = offset; i < (offset + count); i++)
                {
                    byte num3 = key[i % length];
                    if ((buffer[i] != 0) && (buffer[i] != num3))
                    {
                        buffer[i] = (byte) (buffer[i] ^ num3);
                    }
                }
            }
        }

        public static void EncryptXor(byte[] key, byte[] buffer)
        {
            EncryptXor(key, buffer, 0, buffer.Length);
        }

        public static void EncryptXor(byte[] key, byte[] buffer, int offset, int count)
        {
            if ((key != null) && (key.Length > 0))
            {
                int length = key.Length;
                for (int i = offset; i < (offset + count); i++)
                {
                    if (buffer[i] != 0)
                    {
                        byte num3 = key[i % length];
                        buffer[i] = (byte) (buffer[i] ^ num3);
                        if (buffer[i] == 0)
                        {
                            buffer[i] = num3;
                        }
                    }
                }
            }
        }
    }
}

