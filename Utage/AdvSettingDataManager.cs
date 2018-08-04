namespace Utage
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class AdvSettingDataManager
    {
        [CompilerGenerated]
        private static Action<IAdvSetting> <>f__am$cache0;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private AdvImportScenarios <ImportedScenarios>k__BackingField;
        private AdvParticleSetting advParticleSetting = new AdvParticleSetting();
        private AdvAnimationSetting animationSetting = new AdvAnimationSetting();
        private AdvBootSetting bootSetting = new AdvBootSetting();
        private AdvCharacterSetting characterSetting = new AdvCharacterSetting();
        private AdvParamManager defaultParam = new AdvParamManager();
        private AdvEyeBlinkSetting eyeBlinkSetting = new AdvEyeBlinkSetting();
        private AdvLayerSetting layerSetting = new AdvLayerSetting();
        private AdvLipSynchSetting lipSynchSetting = new AdvLipSynchSetting();
        private AdvLocalizeSetting localizeSetting = new AdvLocalizeSetting();
        private AdvSceneGallerySetting sceneGallerySetting = new AdvSceneGallerySetting();
        private List<IAdvSetting> settingDataList;
        private AdvSoundSetting soundSetting = new AdvSoundSetting();
        private AdvTextureSetting textureSetting = new AdvTextureSetting();
        private AdvVideoSetting videoSetting = new AdvVideoSetting();

        public void BootInit(string rootDirResource)
        {
            this.BootSetting.BootInit(rootDirResource);
            if (this.ImportedScenarios != null)
            {
                foreach (AdvChapterData data in this.ImportedScenarios.Chapters)
                {
                    data.BootInit(this);
                }
            }
        }

        internal void DownloadAll()
        {
            if (<>f__am$cache0 == null)
            {
                <>f__am$cache0 = x => x.DownloadAll();
            }
            this.SettingDataList.ForEach(<>f__am$cache0);
        }

        public AdvAnimationSetting AnimationSetting
        {
            get
            {
                return this.animationSetting;
            }
        }

        public AdvBootSetting BootSetting
        {
            get
            {
                return this.bootSetting;
            }
        }

        public AdvCharacterSetting CharacterSetting
        {
            get
            {
                return this.characterSetting;
            }
        }

        public AdvParamManager DefaultParam
        {
            get
            {
                return this.defaultParam;
            }
        }

        public AdvEyeBlinkSetting EyeBlinkSetting
        {
            get
            {
                return this.eyeBlinkSetting;
            }
        }

        public AdvImportScenarios ImportedScenarios { get; set; }

        public AdvLayerSetting LayerSetting
        {
            get
            {
                return this.layerSetting;
            }
        }

        public AdvLipSynchSetting LipSynchSetting
        {
            get
            {
                return this.lipSynchSetting;
            }
        }

        public AdvLocalizeSetting LocalizeSetting
        {
            get
            {
                return this.localizeSetting;
            }
        }

        public AdvParticleSetting ParticleSetting
        {
            get
            {
                return this.advParticleSetting;
            }
        }

        public AdvSceneGallerySetting SceneGallerySetting
        {
            get
            {
                return this.sceneGallerySetting;
            }
        }

        private List<IAdvSetting> SettingDataList
        {
            get
            {
                if (this.settingDataList == null)
                {
                    this.settingDataList = new List<IAdvSetting>();
                    this.settingDataList.Add(this.LayerSetting);
                    this.settingDataList.Add(this.CharacterSetting);
                    this.settingDataList.Add(this.TextureSetting);
                    this.settingDataList.Add(this.SoundSetting);
                    this.settingDataList.Add(this.DefaultParam);
                    this.settingDataList.Add(this.SceneGallerySetting);
                    this.settingDataList.Add(this.LocalizeSetting);
                    this.settingDataList.Add(this.AnimationSetting);
                    this.settingDataList.Add(this.EyeBlinkSetting);
                    this.settingDataList.Add(this.LipSynchSetting);
                    this.settingDataList.Add(this.ParticleSetting);
                }
                return this.settingDataList;
            }
        }

        public AdvSoundSetting SoundSetting
        {
            get
            {
                return this.soundSetting;
            }
        }

        public AdvTextureSetting TextureSetting
        {
            get
            {
                return this.textureSetting;
            }
        }

        public AdvVideoSetting VideoSetting
        {
            get
            {
                return this.videoSetting;
            }
        }
    }
}

