namespace Utage
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Text.RegularExpressions;
    using UnityEngine;

    public class AdvSheetParser
    {
        [CompilerGenerated]
        private static Dictionary<string, int> <>f__switch$map2;
        [CompilerGenerated]
        private static Dictionary<string, int> <>f__switch$map3;
        private static readonly Regex AnimationSheetNameRegix = new Regex(@"(.+)\[\]", RegexOptions.IgnorePatternWhitespace);
        private const string SheetNameAnimation = "Animation";
        public const string SheetNameBoot = "Boot";
        private const string SheetNameCharacter = "Character";
        private const string SheetNameEyeBlink = "EyeBlink";
        private const string SheetNameLayer = "Layer";
        private const string SheetNameLipSynch = "LipSynch";
        private const string SheetNameLocalize = "Localize";
        private const string SheetNameParticle = "Particle";
        private static readonly Regex SheetNameRegex = new Regex(@"(.+)\{\}", RegexOptions.IgnorePatternWhitespace);
        public const string SheetNameScenario = "Scenario";
        private const string SheetNameSceneGallery = "SceneGallery";
        private const string SheetNameSound = "Sound";
        private const string SheetNameTexture = "Texture";

        public static IAdvSetting FindSettingData(AdvSettingDataManager settingManager, string sheetName)
        {
            if (sheetName != null)
            {
                int num;
                if (<>f__switch$map3 == null)
                {
                    Dictionary<string, int> dictionary = new Dictionary<string, int>(9);
                    dictionary.Add("Character", 0);
                    dictionary.Add("Texture", 1);
                    dictionary.Add("Sound", 2);
                    dictionary.Add("Layer", 3);
                    dictionary.Add("SceneGallery", 4);
                    dictionary.Add("Localize", 5);
                    dictionary.Add("EyeBlink", 6);
                    dictionary.Add("LipSynch", 7);
                    dictionary.Add("Particle", 8);
                    <>f__switch$map3 = dictionary;
                }
                if (<>f__switch$map3.TryGetValue(sheetName, out num))
                {
                    switch (num)
                    {
                        case 0:
                            return settingManager.CharacterSetting;

                        case 1:
                            return settingManager.TextureSetting;

                        case 2:
                            return settingManager.SoundSetting;

                        case 3:
                            return settingManager.LayerSetting;

                        case 4:
                            return settingManager.SceneGallerySetting;

                        case 5:
                            return settingManager.LocalizeSetting;

                        case 6:
                            return settingManager.EyeBlinkSetting;

                        case 7:
                            return settingManager.LipSynchSetting;

                        case 8:
                            return settingManager.ParticleSetting;
                    }
                }
            }
            if (IsParamSheetName(sheetName))
            {
                return settingManager.DefaultParam;
            }
            if (IsAnimationSheetName(sheetName))
            {
                return settingManager.AnimationSetting;
            }
            object[] args = new object[] { sheetName };
            Debug.LogError(LanguageAdvErrorMsg.LocalizeTextFormat(AdvErrorMsg.NotSettingSheet, args));
            return null;
        }

        private static bool IsAnimationSheetName(string sheetName)
        {
            return ((sheetName == "Animation") || AnimationSheetNameRegix.Match(sheetName).Success);
        }

        public static bool IsDisableSheetName(string sheetName)
        {
            return ((sheetName != null) && ((sheetName == "Boot") || (sheetName == "Scenario")));
        }

        public static bool IsParamSheetName(string sheetName)
        {
            return ((sheetName == "Param") || SheetNameRegex.Match(sheetName).Success);
        }

        public static bool IsScenarioSheet(string sheetName)
        {
            if (IsDisableSheetName(sheetName))
            {
                return false;
            }
            if (IsSettingsSheet(sheetName))
            {
                return false;
            }
            return true;
        }

        public static bool IsSettingsSheet(string sheetName)
        {
            if (sheetName != null)
            {
                int num;
                if (<>f__switch$map2 == null)
                {
                    Dictionary<string, int> dictionary = new Dictionary<string, int>(10);
                    dictionary.Add("Scenario", 0);
                    dictionary.Add("Character", 0);
                    dictionary.Add("Texture", 0);
                    dictionary.Add("Sound", 0);
                    dictionary.Add("Layer", 0);
                    dictionary.Add("SceneGallery", 0);
                    dictionary.Add("Localize", 0);
                    dictionary.Add("EyeBlink", 0);
                    dictionary.Add("LipSynch", 0);
                    dictionary.Add("Particle", 0);
                    <>f__switch$map2 = dictionary;
                }
                if (<>f__switch$map2.TryGetValue(sheetName, out num) && (num == 0))
                {
                    return true;
                }
            }
            return (IsParamSheetName(sheetName) || IsAnimationSheetName(sheetName));
        }

        public static string ToBootTsvTagName(string sheetName)
        {
            string str = sheetName;
            if (IsParamSheetName(sheetName))
            {
                str = "Param";
            }
            else if (IsAnimationSheetName(sheetName))
            {
                str = "Animation";
            }
            return (str + "Setting");
        }
    }
}

