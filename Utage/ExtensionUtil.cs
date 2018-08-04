namespace Utage
{
    using System;
    using UnityEngine;

    public static class ExtensionUtil
    {
        public const string AssetBundle = ".unity3d";
        public const string ConvertFileList = ".list.bytes";
        public const string ConvertFileListLog = ".list.log";
        public const string CSV = ".csv";
        public const string Log = ".log";
        public const string Mp3 = ".mp3";
        public const string Ogg = ".ogg";
        public const string TSV = ".tsv";
        public const string Txt = ".txt";
        public const string UtageFile = ".utage";
        public const string Wav = ".wav";

        public static string ChangeSoundExt(string path)
        {
            string str = FilePathUtil.GetExtension(path).ToLower();
            if (str == null)
            {
                return path;
            }
            if (!(str == ".ogg"))
            {
                if (str != ".mp3")
                {
                    return path;
                }
            }
            else
            {
                if (IsSupportOggPlatform())
                {
                    return path;
                }
                return FilePathUtil.ChangeExtension(path, ".mp3");
            }
            if (!IsSupportOggPlatform())
            {
                return path;
            }
            return FilePathUtil.ChangeExtension(path, ".ogg");
        }

        public static AudioType GetAudioType(string path)
        {
            switch (FilePathUtil.GetExtension(path).ToLower())
            {
                case ".mp3":
                    return 13;

                case ".ogg":
                    return 14;
            }
            return 20;
        }

        public static bool IsSupportOggPlatform()
        {
            return true;
        }
    }
}

