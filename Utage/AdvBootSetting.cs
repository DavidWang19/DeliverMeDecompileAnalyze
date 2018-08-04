namespace Utage
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    [Serializable]
    public class AdvBootSetting
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <ResourceDir>k__BackingField;
        private DefaultDirInfo ambienceDirInfo;
        private DefaultDirInfo bgDirInfo;
        private DefaultDirInfo bgmDirInfo;
        private DefaultDirInfo characterDirInfo;
        private DefaultDirInfo eventDirInfo;
        private DefaultDirInfo particleDirInfo;
        private DefaultDirInfo seDirInfo;
        private DefaultDirInfo spriteDirInfo;
        private DefaultDirInfo thumbnailDirInfo;
        private DefaultDirInfo videoDirInfo;
        private DefaultDirInfo voiceDirInfo;

        public void BootInit(string resourceDir)
        {
            this.ResourceDir = resourceDir;
            DefaultDirInfo info = new DefaultDirInfo {
                defaultDir = "Texture/Character",
                defaultExt = ".png"
            };
            this.characterDirInfo = info;
            info = new DefaultDirInfo {
                defaultDir = "Texture/BG",
                defaultExt = ".jpg"
            };
            this.bgDirInfo = info;
            info = new DefaultDirInfo {
                defaultDir = "Texture/Event",
                defaultExt = ".jpg"
            };
            this.eventDirInfo = info;
            info = new DefaultDirInfo {
                defaultDir = "Texture/Sprite",
                defaultExt = ".png"
            };
            this.spriteDirInfo = info;
            info = new DefaultDirInfo {
                defaultDir = "Texture/Thumbnail",
                defaultExt = ".jpg"
            };
            this.thumbnailDirInfo = info;
            info = new DefaultDirInfo {
                defaultDir = "Sound/BGM",
                defaultExt = ".wav"
            };
            this.bgmDirInfo = info;
            info = new DefaultDirInfo {
                defaultDir = "Sound/SE",
                defaultExt = ".wav"
            };
            this.seDirInfo = info;
            info = new DefaultDirInfo {
                defaultDir = "Sound/Ambience",
                defaultExt = ".wav"
            };
            this.ambienceDirInfo = info;
            info = new DefaultDirInfo {
                defaultDir = "Sound/Voice",
                defaultExt = ".wav"
            };
            this.voiceDirInfo = info;
            info = new DefaultDirInfo {
                defaultDir = "Particle",
                defaultExt = ".prefab"
            };
            this.particleDirInfo = info;
            info = new DefaultDirInfo {
                defaultDir = "Video",
                defaultExt = ".mp4"
            };
            this.videoDirInfo = info;
            this.InitDefaultDirInfo(this.ResourceDir, this.characterDirInfo);
            this.InitDefaultDirInfo(this.ResourceDir, this.bgDirInfo);
            this.InitDefaultDirInfo(this.ResourceDir, this.eventDirInfo);
            this.InitDefaultDirInfo(this.ResourceDir, this.spriteDirInfo);
            this.InitDefaultDirInfo(this.ResourceDir, this.thumbnailDirInfo);
            this.InitDefaultDirInfo(this.ResourceDir, this.bgmDirInfo);
            this.InitDefaultDirInfo(this.ResourceDir, this.seDirInfo);
            this.InitDefaultDirInfo(this.ResourceDir, this.ambienceDirInfo);
            this.InitDefaultDirInfo(this.ResourceDir, this.voiceDirInfo);
            this.InitDefaultDirInfo(this.ResourceDir, this.particleDirInfo);
            this.InitDefaultDirInfo(this.ResourceDir, this.videoDirInfo);
        }

        public string GetLocalizeVoiceFilePath(string file)
        {
            if (!LanguageManagerBase.Instance.IgnoreLocalizeVoice)
            {
                string currentLanguage = LanguageManagerBase.Instance.CurrentLanguage;
                if (LanguageManagerBase.Instance.VoiceLanguages.Contains(currentLanguage))
                {
                    return this.VoiceDirInfo.FileNameToPath(file, currentLanguage);
                }
            }
            return this.VoiceDirInfo.FileNameToPath(file);
        }

        private void InitDefaultDirInfo(string root, DefaultDirInfo info)
        {
            string[] args = new string[] { root, info.defaultDir };
            info.defaultDir = FilePathUtil.Combine(args);
        }

        public DefaultDirInfo AmbienceDirInfo
        {
            get
            {
                return this.ambienceDirInfo;
            }
        }

        public DefaultDirInfo BgDirInfo
        {
            get
            {
                return this.bgDirInfo;
            }
        }

        public DefaultDirInfo BgmDirInfo
        {
            get
            {
                return this.bgmDirInfo;
            }
        }

        public DefaultDirInfo CharacterDirInfo
        {
            get
            {
                return this.characterDirInfo;
            }
        }

        public DefaultDirInfo EventDirInfo
        {
            get
            {
                return this.eventDirInfo;
            }
        }

        public DefaultDirInfo ParticleDirInfo
        {
            get
            {
                return this.particleDirInfo;
            }
        }

        public string ResourceDir { get; set; }

        public DefaultDirInfo SeDirInfo
        {
            get
            {
                return this.seDirInfo;
            }
        }

        public DefaultDirInfo SpriteDirInfo
        {
            get
            {
                return this.spriteDirInfo;
            }
        }

        public DefaultDirInfo ThumbnailDirInfo
        {
            get
            {
                return this.thumbnailDirInfo;
            }
        }

        public DefaultDirInfo VideoDirInfo
        {
            get
            {
                return this.videoDirInfo;
            }
        }

        public DefaultDirInfo VoiceDirInfo
        {
            get
            {
                return this.voiceDirInfo;
            }
        }

        [Serializable]
        public class DefaultDirInfo
        {
            public string defaultDir;
            public string defaultExt;

            public string FileNameToPath(string fileName)
            {
                return this.FileNameToPath(fileName, string.Empty);
            }

            public string FileNameToPath(string fileName, string LocalizeDir)
            {
                string str;
                if (string.IsNullOrEmpty(fileName))
                {
                    return fileName;
                }
                if (FilePathUtil.IsAbsoluteUri(fileName))
                {
                    str = fileName;
                }
                else
                {
                    try
                    {
                        if (string.IsNullOrEmpty(FilePathUtil.GetExtension(fileName)))
                        {
                            fileName = fileName + this.defaultExt;
                        }
                        str = this.defaultDir + LocalizeDir + "/" + fileName;
                    }
                    catch (Exception exception)
                    {
                        Debug.LogError(fileName + "  " + exception.ToString());
                        str = this.defaultDir + LocalizeDir + "/" + fileName;
                    }
                }
                return ExtensionUtil.ChangeSoundExt(str);
            }
        }
    }
}

