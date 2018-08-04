namespace Utage
{
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public abstract class FileIOManagerBase : MonoBehaviour
    {
        [CompilerGenerated]
        private static Func<byte[], byte[], byte[]> <>f__mg$cache0;
        [CompilerGenerated]
        private static Func<byte[], byte[], byte[]> <>f__mg$cache1;
        [CompilerGenerated]
        private static Action<byte[], byte[], int, int> <>f__mg$cache2;
        [CompilerGenerated]
        private static Action<byte[], byte[], int, int> <>f__mg$cache3;
        protected static int[] audioHeader;
        protected const int audioHeaderSize = 12;
        protected static float[] audioSamplesWorkArray;
        protected static short[] audioShortWorkArray;
        private static Func<byte[], byte[], byte[]> customDecode;
        private static Action<byte[], byte[], int, int> customDecodeNoCompress;
        private static Func<byte[], byte[], byte[]> customEncode;
        private static Action<byte[], byte[], int, int> customEncodeNoCompress;
        protected const int maxAudioWorkSize = 0x20000;
        protected const int maxWorkBufferSize = 0x40000;
        private const string sdkDirectoryName = "Utage/";
        protected static byte[] workBufferArray;

        static FileIOManagerBase()
        {
            if (<>f__mg$cache0 == null)
            {
                <>f__mg$cache0 = new Func<byte[], byte[], byte[]>(FileIOManagerBase.DefaultEncode);
            }
            customEncode = <>f__mg$cache0;
            if (<>f__mg$cache1 == null)
            {
                <>f__mg$cache1 = new Func<byte[], byte[], byte[]>(FileIOManagerBase.DefaultDecode);
            }
            customDecode = <>f__mg$cache1;
            if (<>f__mg$cache2 == null)
            {
                <>f__mg$cache2 = new Action<byte[], byte[], int, int>(FileIOManagerBase.DefaultEncodeNoCompress);
            }
            customEncodeNoCompress = <>f__mg$cache2;
            if (<>f__mg$cache3 == null)
            {
                <>f__mg$cache3 = new Action<byte[], byte[], int, int>(FileIOManagerBase.DefaultDecodeNoCompress);
            }
            customDecodeNoCompress = <>f__mg$cache3;
            audioHeader = new int[3];
            workBufferArray = new byte[0x40000];
            audioShortWorkArray = new short[0x20000];
            audioSamplesWorkArray = new float[0x20000];
        }

        protected FileIOManagerBase()
        {
        }

        public abstract void CreateDirectory(string path);
        public abstract byte[] Decode(byte[] bytes);
        public abstract void DecodeNoCompress(byte[] bytes);
        private static byte[] DefaultDecode(byte[] keyBytes, byte[] bytes)
        {
            Crypt.DecryptXor(keyBytes, bytes);
            return Compression.Decompress(bytes);
        }

        private static void DefaultDecodeNoCompress(byte[] keyBytes, byte[] bytes, int offset, int count)
        {
            Crypt.DecryptXor(keyBytes, bytes, offset, count);
        }

        private static byte[] DefaultEncode(byte[] keyBytes, byte[] bytes)
        {
            byte[] buffer = Compression.Compress(bytes);
            Crypt.EncryptXor(keyBytes, buffer);
            return buffer;
        }

        private static void DefaultEncodeNoCompress(byte[] keyBytes, byte[] bytes, int offset, int count)
        {
            Crypt.EncryptXor(keyBytes, bytes, offset, count);
        }

        public abstract void Delete(string path);
        public abstract void DeleteDirectory(string path);
        public abstract byte[] Encode(byte[] bytes);
        public abstract byte[] EncodeNoCompress(byte[] bytes);
        public abstract bool Exists(string path);
        public static AudioClip ReadAudioFromMem(string name, byte[] bytes)
        {
            return ReadAudioFromMem(name, bytes, false);
        }

        public static AudioClip ReadAudioFromMem(string name, byte[] bytes, bool is3D)
        {
            Buffer.BlockCopy(bytes, 0, audioHeader, 0, 12);
            AudioClip clip = WrapperUnityVersion.CreateAudioClip(name, audioHeader[0], audioHeader[1], audioHeader[2], is3D, false);
            int num = audioHeader[0] * audioHeader[1];
            int num2 = 0;
            int srcOffset = 12;
            do
            {
                int num4 = Math.Min(audioSamplesWorkArray.Length, num - num2);
                Buffer.BlockCopy(bytes, srcOffset, audioShortWorkArray, 0, num4 * 2);
                srcOffset += num4 * 2;
                float[] numArray = (num4 != audioSamplesWorkArray.Length) ? new float[num4] : audioSamplesWorkArray;
                for (int i = 0; i < num4; i++)
                {
                    numArray[i] = (1f * audioShortWorkArray[i]) / 32767f;
                }
                clip.SetData(numArray, num2 / clip.get_channels());
                num2 += num4;
            }
            while (num2 < num);
            return clip;
        }

        public abstract bool ReadBinaryDecode(string path, Action<BinaryReader> callbackRead);
        public static int ToMagicID(char id0, char id1, char id2, char id3)
        {
            return ((((id3 << 0x18) + (id2 << 0x10)) + (id1 << 8)) + id0);
        }

        public abstract bool Write(string path, byte[] bytes);
        public abstract bool WriteBinaryEncode(string path, Action<BinaryWriter> callbackWrite);
        public abstract bool WriteEncode(string path, byte[] bytes);
        public abstract bool WriteEncodeNoCompress(string path, byte[] bytes);
        public abstract bool WriteSound(string path, AudioClip audioClip);

        public static Func<byte[], byte[], byte[]> CustomDecode
        {
            get
            {
                return customDecode;
            }
            set
            {
                customDecode = value;
            }
        }

        public static Action<byte[], byte[], int, int> CustomDecodeNoCompress
        {
            get
            {
                return customDecodeNoCompress;
            }
            set
            {
                customDecodeNoCompress = value;
            }
        }

        public static Func<byte[], byte[], byte[]> CustomEncode
        {
            get
            {
                return customEncode;
            }
            set
            {
                customEncode = value;
            }
        }

        public static Action<byte[], byte[], int, int> CustomEncodeNoCompress
        {
            get
            {
                return customEncodeNoCompress;
            }
            set
            {
                customEncodeNoCompress = value;
            }
        }

        public static string SdkPersistentDataPath
        {
            get
            {
                string[] args = new string[] { Application.get_persistentDataPath(), "Utage/" };
                return FilePathUtil.Combine(args);
            }
        }

        public static string SdkTemporaryCachePath
        {
            get
            {
                string[] args = new string[] { Application.get_temporaryCachePath(), "Utage/" };
                return FilePathUtil.Combine(args);
            }
        }

        protected enum SoundHeader
        {
            Samples,
            Channels,
            Frequency,
            Max
        }
    }
}

