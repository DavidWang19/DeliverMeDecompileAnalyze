namespace Utage
{
    using System;
    using System.IO;
    using System.Text;
    using UnityEngine;

    [AddComponentMenu("Utage/Lib/File/FileIOManager")]
    public class FileIOManager : FileIOManagerBase
    {
        [SerializeField]
        private string cryptKey = "InputOriginalKey";
        private byte[] cryptKeyBytes;

        public override void CreateDirectory(string path)
        {
            string directoryName = Path.GetDirectoryName(path);
            if (!Directory.Exists(directoryName))
            {
                Directory.CreateDirectory(directoryName);
            }
        }

        public override byte[] Decode(byte[] bytes)
        {
            return FileIOManagerBase.CustomDecode(this.CryptKeyBytes, bytes);
        }

        public override void DecodeNoCompress(byte[] bytes)
        {
            FileIOManagerBase.CustomDecodeNoCompress(this.CryptKeyBytes, bytes, 0, bytes.Length);
        }

        public override void Delete(string path)
        {
            File.Delete(path);
        }

        public override void DeleteDirectory(string path)
        {
            string directoryName = Path.GetDirectoryName(path);
            if (Directory.Exists(directoryName))
            {
                Directory.Delete(directoryName, true);
            }
        }

        public override byte[] Encode(byte[] bytes)
        {
            return FileIOManagerBase.CustomEncode(this.CryptKeyBytes, bytes);
        }

        public override byte[] EncodeNoCompress(byte[] bytes)
        {
            FileIOManagerBase.CustomEncodeNoCompress(this.CryptKeyBytes, bytes, 0, bytes.Length);
            return bytes;
        }

        public override bool Exists(string path)
        {
            return File.Exists(path);
        }

        protected byte[] FileReadAllBytes(string path)
        {
            return File.ReadAllBytes(path);
        }

        protected void FileWriteAllBytes(string path, byte[] bytes)
        {
            File.WriteAllBytes(path, bytes);
        }

        private void OnValidate()
        {
            this.cryptKeyBytes = Encoding.UTF8.GetBytes(this.cryptKey);
        }

        public override bool ReadBinaryDecode(string path, Action<BinaryReader> callbackRead)
        {
            try
            {
                if (!this.Exists(path))
                {
                    return false;
                }
                using (MemoryStream stream = new MemoryStream(FileIOManagerBase.CustomDecode(this.CryptKeyBytes, this.FileReadAllBytes(path))))
                {
                    using (BinaryReader reader = new BinaryReader(stream))
                    {
                        callbackRead(reader);
                    }
                }
                return true;
            }
            catch (Exception exception)
            {
                object[] args = new object[] { path, exception.ToString() };
                Debug.LogError(LanguageErrorMsg.LocalizeTextFormat(ErrorMsg.FileRead, args));
                return false;
            }
        }

        public override bool Write(string path, byte[] bytes)
        {
            try
            {
                using (FileStream stream = new FileStream(path, FileMode.Create, FileAccess.Write))
                {
                    int offset = 0;
                    do
                    {
                        int count = Math.Min(0x40000, bytes.Length - offset);
                        stream.Write(bytes, offset, count);
                        offset += count;
                    }
                    while (offset < bytes.Length);
                }
                return true;
            }
            catch (Exception exception)
            {
                Debug.LogError(exception.ToString());
                return false;
            }
        }

        public override bool WriteBinaryEncode(string path, Action<BinaryWriter> callbackWrite)
        {
            try
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    using (BinaryWriter writer = new BinaryWriter(stream))
                    {
                        callbackWrite(writer);
                    }
                    this.FileWriteAllBytes(path, FileIOManagerBase.CustomEncode(this.CryptKeyBytes, stream.ToArray()));
                }
                return true;
            }
            catch (Exception exception)
            {
                object[] args = new object[] { path, exception.ToString() };
                Debug.LogError(LanguageErrorMsg.LocalizeTextFormat(ErrorMsg.FileWrite, args));
                return false;
            }
        }

        public override bool WriteEncode(string path, byte[] bytes)
        {
            try
            {
                this.FileWriteAllBytes(path, FileIOManagerBase.CustomEncode(this.CryptKeyBytes, bytes));
                return true;
            }
            catch (Exception exception)
            {
                object[] args = new object[] { path, exception.ToString() };
                Debug.LogError(LanguageErrorMsg.LocalizeTextFormat(ErrorMsg.FileWrite, args));
                return false;
            }
        }

        public override bool WriteEncodeNoCompress(string path, byte[] bytes)
        {
            try
            {
                using (FileStream stream = new FileStream(path, FileMode.Create, FileAccess.Write))
                {
                    int srcOffset = 0;
                    do
                    {
                        int count = Math.Min(0x40000, bytes.Length - srcOffset);
                        Buffer.BlockCopy(bytes, srcOffset, FileIOManagerBase.workBufferArray, 0, count);
                        FileIOManagerBase.CustomEncodeNoCompress(this.CryptKeyBytes, FileIOManagerBase.workBufferArray, 0, count);
                        stream.Write(FileIOManagerBase.workBufferArray, 0, count);
                        srcOffset += count;
                    }
                    while (srcOffset < bytes.Length);
                }
                return true;
            }
            catch (Exception exception)
            {
                Debug.LogError(exception.ToString());
                return false;
            }
        }

        public override bool WriteSound(string path, AudioClip audioClip)
        {
            try
            {
                FileIOManagerBase.audioHeader[0] = audioClip.get_samples();
                FileIOManagerBase.audioHeader[2] = audioClip.get_frequency();
                FileIOManagerBase.audioHeader[1] = audioClip.get_channels();
                int num = audioClip.get_samples() * audioClip.get_channels();
                using (FileStream stream = new FileStream(path, FileMode.Create, FileAccess.Write))
                {
                    Buffer.BlockCopy(FileIOManagerBase.audioHeader, 0, FileIOManagerBase.workBufferArray, 0, 12);
                    FileIOManagerBase.CustomEncodeNoCompress(this.CryptKeyBytes, FileIOManagerBase.workBufferArray, 0, 12);
                    stream.Write(FileIOManagerBase.workBufferArray, 0, 12);
                    int num2 = 0;
                    do
                    {
                        int num3 = Math.Min(FileIOManagerBase.audioSamplesWorkArray.Length, num - num2);
                        audioClip.GetData(FileIOManagerBase.audioSamplesWorkArray, num2 / audioClip.get_channels());
                        for (int i = 0; i < num3; i++)
                        {
                            FileIOManagerBase.audioShortWorkArray[i] = (short) (32767f * FileIOManagerBase.audioSamplesWorkArray[i]);
                        }
                        int count = num3 * 2;
                        Buffer.BlockCopy(FileIOManagerBase.audioShortWorkArray, 0, FileIOManagerBase.workBufferArray, 0, count);
                        FileIOManagerBase.CustomEncodeNoCompress(this.CryptKeyBytes, FileIOManagerBase.workBufferArray, 0, count);
                        stream.Write(FileIOManagerBase.workBufferArray, 0, count);
                        num2 += num3;
                    }
                    while (num2 < num);
                }
                return true;
            }
            catch (Exception exception)
            {
                Debug.LogError(exception.ToString());
                return false;
            }
        }

        public byte[] CryptKeyBytes
        {
            get
            {
                if ((this.cryptKeyBytes == null) || (this.cryptKeyBytes.Length == 0))
                {
                    this.cryptKeyBytes = Encoding.UTF8.GetBytes(this.cryptKey);
                }
                return this.cryptKeyBytes;
            }
        }
    }
}

