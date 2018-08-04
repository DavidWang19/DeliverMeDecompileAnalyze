namespace Utage
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public static class AdvCommandParser
    {
        [CompilerGenerated]
        private static Dictionary<string, int> <>f__switch$map1;
        public const string IdAmbience = "Ambience";
        public const string IdBeginMacro = "BeginMacro";
        public const string IdBg = "Bg";
        public const string IdBgEvent = "BgEvent";
        public const string IdBgEventOff = "BgEventOff";
        public const string IdBgm = "Bgm";
        public const string IdBgOff = "BgOff";
        public const string IdCaptureImage = "CaptureImage";
        public const string IdChangeMessageWindow = "ChangeMessageWindow";
        public const string IdChangeSoundVolume = "ChangeSoundVolume";
        public const string IdCharacter = "Character";
        public const string IdCharacterOff = "CharacterOff";
        public const string IdComment = "Comment";
        public const string IdEffect = "Effect";
        public const string IdElse = "Else";
        public const string IdElseIf = "ElseIf";
        public const string IdEndIf = "EndIf";
        public const string IdEndMacro = "EndMacro";
        public const string IdEndPage = "EndPage";
        public const string IdEndScenario = "EndScenario";
        public const string IdEndSceneGallery = "EndSceneGallery";
        public const string IdEndSubroutine = "EndSubroutine";
        public const string IdEndThread = "EndThread";
        public const string IdError = "Error";
        public const string IdFadeIn = "FadeIn";
        public const string IdFadeOut = "FadeOut";
        public const string IdGuiActive = "GuiActive";
        public const string IdGuiPosition = "GuiPosition";
        public const string IdGuiReset = "GuiReset";
        public const string IdGuiSize = "GuiSize";
        public const string IdHideMenuButton = "HideMenuButton";
        public const string IdHideMessageWindow = "HideMessageWindow";
        public const string IdIf = "If";
        public const string IdImageEffect = "ImageEffect";
        public const string IdImageEffectOff = "ImageEffectOff";
        public const string IdInitMessageWindow = "InitMessageWindow";
        public const string IdJump = "Jump";
        public const string IdJumpRandom = "JumpRandom";
        public const string IdJumpRandomEnd = "JumpRandomEnd";
        public const string IdJumpSubroutine = "JumpSubroutine";
        public const string IdJumpSubroutineRandom = "JumpSubroutineRandom";
        public const string IdJumpSubroutineRandomEnd = "JumpSubroutineRandomEnd";
        public const string IdLayerOff = "LayerOff";
        public const string IdLayerReset = "LayerReset";
        public const string IdMovie = "Movie";
        public const string IdPageControler = "PageControl";
        public const string IdParam = "Param";
        public const string IdParticle = "Particle";
        public const string IdParticleOff = "ParticleOff";
        public const string IdPauseScenario = "PauseScenario";
        public const string IdPlayAnimation = "PlayAnimation";
        public const string IdRuleFadeIn = "RuleFadeIn";
        public const string IdRuleFadeOut = "RuleFadeOut";
        public const string IdScenarioLabel = "ScenarioLabel";
        public const string IdSe = "Se";
        public const string IdSelection = "Selection";
        public const string IdSelectionClick = "SelectionClick";
        public const string IdSelectionEnd = "SelectionEnd";
        public const string IdSendMessage = "SendMessage";
        public const string IdSendMessageByName = "SendMessageByName";
        public const string IdShake = "Shake";
        public const string IdShowMenuButton = "ShowMenuButton";
        public const string IdShowMessageWindow = "ShowMessageWindow";
        public const string IdSprite = "Sprite";
        public const string IdSpriteOff = "SpriteOff";
        public const string IdStopAmbience = "StopAmbience";
        public const string IdStopBgm = "StopBgm";
        public const string IdStopSe = "StopSe";
        public const string IdStopSound = "StopSound";
        public const string IdStopVoice = "StopVoice";
        public const string IdText = "Text";
        public const string IdThread = "Thread";
        public const string IdTween = "Tween";
        public const string IdVibrate = "Vibrate";
        public const string IdVideo = "Video";
        public const string IdVoice = "Voice";
        public const string IdWait = "Wait";
        public const string IdWaitCustom = "WaitCustom";
        public const string IdWaitInput = "WaitInput";
        public const string IdWaitThread = "WaitThread";
        public const string IdZoomCamera = "ZoomCamera";
        public static CreateCustomCommandFromID OnCreateCustomCommandFromID;
        [Obsolete("Use OnCreateCustomCommandFromID  instead")]
        public static CreateCustomCommandFromID OnCreateCustomCommnadFromID;

        public static AdvCommand CreateCommand(StringGridRow row, AdvSettingDataManager dataManager)
        {
            if (row.IsCommentOut || IsComment(row))
            {
                return null;
            }
            AdvCommand command = CreateCommand(ParseCommandID(row), row, dataManager);
            if (command != null)
            {
                return command;
            }
            if (row.IsAllEmptyCellNamedColumn())
            {
                return command;
            }
            Debug.LogError(row.ToErrorString(LanguageAdvErrorMsg.LocalizeTextFormat(AdvErrorMsg.CommandParseNull, new object[0])));
            return new AdvCommandError(row);
        }

        public static AdvCommand CreateCommand(string id, StringGridRow row, AdvSettingDataManager dataManager)
        {
            AdvCommand command = null;
            if (OnCreateCustomCommandFromID != null)
            {
                OnCreateCustomCommandFromID(id, row, dataManager, ref command);
            }
            if (command == null)
            {
                command = CreateCommandDefault(id, row, dataManager);
            }
            if (command != null)
            {
                command.Id = id;
            }
            return command;
        }

        private static AdvCommand CreateCommandDefault(string id, StringGridRow row, AdvSettingDataManager dataManager)
        {
            if (id != null)
            {
                int num;
                if (<>f__switch$map1 == null)
                {
                    Dictionary<string, int> dictionary = new Dictionary<string, int>(0x4c);
                    dictionary.Add("Character", 0);
                    dictionary.Add("Text", 1);
                    dictionary.Add("CharacterOff", 2);
                    dictionary.Add("Bg", 3);
                    dictionary.Add("BgOff", 4);
                    dictionary.Add("BgEvent", 5);
                    dictionary.Add("BgEventOff", 6);
                    dictionary.Add("Sprite", 7);
                    dictionary.Add("SpriteOff", 8);
                    dictionary.Add("Particle", 9);
                    dictionary.Add("ParticleOff", 10);
                    dictionary.Add("LayerOff", 11);
                    dictionary.Add("LayerReset", 12);
                    dictionary.Add("Movie", 13);
                    dictionary.Add("Video", 14);
                    dictionary.Add("Tween", 15);
                    dictionary.Add("FadeIn", 0x10);
                    dictionary.Add("FadeOut", 0x11);
                    dictionary.Add("Shake", 0x12);
                    dictionary.Add("Vibrate", 0x13);
                    dictionary.Add("ZoomCamera", 20);
                    dictionary.Add("PlayAnimation", 0x15);
                    dictionary.Add("RuleFadeIn", 0x16);
                    dictionary.Add("RuleFadeOut", 0x17);
                    dictionary.Add("CaptureImage", 0x18);
                    dictionary.Add("ImageEffect", 0x19);
                    dictionary.Add("ImageEffectOff", 0x1a);
                    dictionary.Add("Se", 0x1b);
                    dictionary.Add("StopSe", 0x1c);
                    dictionary.Add("Bgm", 0x1d);
                    dictionary.Add("StopBgm", 30);
                    dictionary.Add("Ambience", 0x1f);
                    dictionary.Add("StopAmbience", 0x20);
                    dictionary.Add("Voice", 0x21);
                    dictionary.Add("StopVoice", 0x22);
                    dictionary.Add("StopSound", 0x23);
                    dictionary.Add("ChangeSoundVolume", 0x24);
                    dictionary.Add("Wait", 0x25);
                    dictionary.Add("WaitInput", 0x26);
                    dictionary.Add("WaitCustom", 0x27);
                    dictionary.Add("Param", 40);
                    dictionary.Add("Selection", 0x29);
                    dictionary.Add("SelectionEnd", 0x2a);
                    dictionary.Add("SelectionClick", 0x2b);
                    dictionary.Add("Jump", 0x2c);
                    dictionary.Add("JumpRandom", 0x2d);
                    dictionary.Add("JumpRandomEnd", 0x2e);
                    dictionary.Add("JumpSubroutine", 0x2f);
                    dictionary.Add("JumpSubroutineRandom", 0x30);
                    dictionary.Add("JumpSubroutineRandomEnd", 0x31);
                    dictionary.Add("EndSubroutine", 50);
                    dictionary.Add("If", 0x33);
                    dictionary.Add("ElseIf", 0x34);
                    dictionary.Add("Else", 0x35);
                    dictionary.Add("EndIf", 0x36);
                    dictionary.Add("ShowMessageWindow", 0x37);
                    dictionary.Add("HideMessageWindow", 0x38);
                    dictionary.Add("ShowMenuButton", 0x39);
                    dictionary.Add("HideMenuButton", 0x3a);
                    dictionary.Add("ChangeMessageWindow", 0x3b);
                    dictionary.Add("InitMessageWindow", 60);
                    dictionary.Add("GuiReset", 0x3d);
                    dictionary.Add("GuiActive", 0x3e);
                    dictionary.Add("GuiPosition", 0x3f);
                    dictionary.Add("GuiSize", 0x40);
                    dictionary.Add("SendMessage", 0x41);
                    dictionary.Add("SendMessageByName", 0x42);
                    dictionary.Add("EndScenario", 0x43);
                    dictionary.Add("PauseScenario", 0x44);
                    dictionary.Add("EndSceneGallery", 0x45);
                    dictionary.Add("PageControl", 70);
                    dictionary.Add("ScenarioLabel", 0x47);
                    dictionary.Add("Thread", 0x48);
                    dictionary.Add("WaitThread", 0x49);
                    dictionary.Add("EndThread", 0x4a);
                    dictionary.Add("EndPage", 0x4b);
                    <>f__switch$map1 = dictionary;
                }
                if (<>f__switch$map1.TryGetValue(id, out num))
                {
                    switch (num)
                    {
                        case 0:
                            return new AdvCommandCharacter(row, dataManager);

                        case 1:
                            return new AdvCommandText(row, dataManager);

                        case 2:
                            return new AdvCommandCharacterOff(row);

                        case 3:
                            return new AdvCommandBg(row, dataManager);

                        case 4:
                            return new AdvCommandBgOff(row);

                        case 5:
                            return new AdvCommandBgEvent(row, dataManager);

                        case 6:
                            return new AdvCommandBgEventOff(row);

                        case 7:
                            return new AdvCommandSprite(row, dataManager);

                        case 8:
                            return new AdvCommandSpriteOff(row);

                        case 9:
                            return new AdvCommandParticle(row, dataManager);

                        case 10:
                            return new AdvCommandParticleOff(row);

                        case 11:
                            return new AdvCommandLayerOff(row, dataManager);

                        case 12:
                            return new AdvCommandLayerReset(row, dataManager);

                        case 13:
                            return new AdvCommandMovie(row, dataManager);

                        case 14:
                            return new AdvCommandVideo(row, dataManager);

                        case 15:
                            return new AdvCommandTween(row, dataManager);

                        case 0x10:
                            return new AdvCommandFadeIn(row);

                        case 0x11:
                            return new AdvCommandFadeOut(row);

                        case 0x12:
                            return new AdvCommandShake(row, dataManager);

                        case 0x13:
                            return new AdvCommandVibrate(row, dataManager);

                        case 20:
                            return new AdvCommandZoomCamera(row, dataManager);

                        case 0x15:
                            return new AdvCommandPlayAnimatin(row, dataManager);

                        case 0x16:
                            return new AdvCommandRuleFadeIn(row);

                        case 0x17:
                            return new AdvCommandRuleFadeOut(row);

                        case 0x18:
                            return new AdvCommandCaptureImage(row);

                        case 0x19:
                            return new AdvCommandImageEffect(row, dataManager);

                        case 0x1a:
                            return new AdvCommandImageEffectOff(row, dataManager);

                        case 0x1b:
                            return new AdvCommandSe(row, dataManager);

                        case 0x1c:
                            return new AdvCommandStopSe(row, dataManager);

                        case 0x1d:
                            return new AdvCommandBgm(row, dataManager);

                        case 30:
                            return new AdvCommandStopBgm(row);

                        case 0x1f:
                            return new AdvCommandAmbience(row, dataManager);

                        case 0x20:
                            return new AdvCommandStopAmbience(row);

                        case 0x21:
                            return new AdvCommandVoice(row, dataManager);

                        case 0x22:
                            return new AdvCommandStopVoice(row);

                        case 0x23:
                            return new AdvCommandStopSound(row);

                        case 0x24:
                            return new AdvCommandChangeSoundVolume(row);

                        case 0x25:
                            return new AdvCommandWait(row);

                        case 0x26:
                            return new AdvCommandWaitInput(row);

                        case 0x27:
                            return new AdvCommandWaitCustom(row);

                        case 40:
                            return new AdvCommandParam(row, dataManager);

                        case 0x29:
                            return new AdvCommandSelection(row, dataManager);

                        case 0x2a:
                            return new AdvCommandSelectionEnd(row, dataManager);

                        case 0x2b:
                            return new AdvCommandSelectionClick(row, dataManager);

                        case 0x2c:
                            return new AdvCommandJump(row, dataManager);

                        case 0x2d:
                            return new AdvCommandJumpRandom(row, dataManager);

                        case 0x2e:
                            return new AdvCommandJumpRandomEnd(row, dataManager);

                        case 0x2f:
                            return new AdvCommandJumpSubroutine(row, dataManager);

                        case 0x30:
                            return new AdvCommandJumpSubroutineRandom(row, dataManager);

                        case 0x31:
                            return new AdvCommandJumpSubroutineRandomEnd(row, dataManager);

                        case 50:
                            return new AdvCommandEndSubroutine(row, dataManager);

                        case 0x33:
                            return new AdvCommandIf(row, dataManager);

                        case 0x34:
                            return new AdvCommandElseIf(row, dataManager);

                        case 0x35:
                            return new AdvCommandElse(row);

                        case 0x36:
                            return new AdvCommandEndIf(row);

                        case 0x37:
                            return new AdvCommandShowMessageWindow(row);

                        case 0x38:
                            return new AdvCommandHideMessageWindow(row);

                        case 0x39:
                            return new AdvCommandShowMenuButton(row);

                        case 0x3a:
                            return new AdvCommandHideMenuButton(row);

                        case 0x3b:
                            return new AdvCommandMessageWindowChangeCurrent(row);

                        case 60:
                            return new AdvCommandMessageWindowInit(row);

                        case 0x3d:
                            return new AdvCommandGuiReset(row);

                        case 0x3e:
                            return new AdvCommandGuiActive(row);

                        case 0x3f:
                            return new AdvCommandGuiPosition(row);

                        case 0x40:
                            return new AdvCommandGuiSize(row);

                        case 0x41:
                            return new AdvCommandSendMessage(row);

                        case 0x42:
                            return new AdvCommandSendMessageByName(row);

                        case 0x43:
                            return new AdvCommandEndScenario(row);

                        case 0x44:
                            return new AdvCommandPauseScenario(row);

                        case 0x45:
                            return new AdvCommandEndSceneGallery(row);

                        case 70:
                            return new AdvCommandPageControler(row, dataManager);

                        case 0x47:
                            return new AdvCommandScenarioLabel(row);

                        case 0x48:
                            return new AdvCommandThread(row);

                        case 0x49:
                            return new AdvCommandWaitThread(row);

                        case 0x4a:
                            return new AdvCommandEndThread(row);

                        case 0x4b:
                            return new AdvCommandEndPage(row);
                    }
                }
            }
            return null;
        }

        private static bool IsComment(StringGridRow row)
        {
            string str = AdvParser.ParseCellOptional<string>(row, AdvColumnName.Command, string.Empty);
            if (string.IsNullOrEmpty(str))
            {
                return false;
            }
            return ((str == "Comment") || (((str.Length >= 2) && (str[0] == '/')) && (str[1] == '/')));
        }

        public static bool IsScenarioLabel(string str)
        {
            return ((!string.IsNullOrEmpty(str) && (str.Length >= 2)) && (str[0] == '*'));
        }

        private static string ParseCommandID(StringGridRow row)
        {
            string str = AdvParser.ParseCellOptional<string>(row, AdvColumnName.Command, string.Empty);
            if (string.IsNullOrEmpty(str))
            {
                if (!AdvParser.IsEmptyCell(row, AdvColumnName.Arg1))
                {
                    return "Character";
                }
                if (AdvParser.IsEmptyCell(row, AdvColumnName.Text) && AdvParser.IsEmptyCell(row, AdvColumnName.PageCtrl))
                {
                    return null;
                }
                return "Text";
            }
            if (IsScenarioLabel(str))
            {
                str = "ScenarioLabel";
            }
            return str;
        }

        public static string ParseScenarioLabel(StringGridRow row, AdvColumnName name)
        {
            string str;
            if (!TryParseScenarioLabel(row, name, out str))
            {
                object[] args = new object[] { str };
                Debug.LogError(row.ToErrorString(LanguageAdvErrorMsg.LocalizeTextFormat(AdvErrorMsg.NotScenarioLabel, args)));
            }
            return str;
        }

        public static bool TryParseScenarioLabel(StringGridRow row, AdvColumnName columnName, out string scenarioLabel)
        {
            string str = AdvParser.ParseCell<string>(row, columnName);
            if (!IsScenarioLabel(str))
            {
                scenarioLabel = str;
                return false;
            }
            if ((str.Length >= 3) && (str[1] == '*'))
            {
                scenarioLabel = row.Grid.SheetName + '*' + str.Substring(2);
            }
            else
            {
                scenarioLabel = str.Substring(1);
            }
            return true;
        }

        public delegate void CreateCustomCommandFromID(string id, StringGridRow row, AdvSettingDataManager dataManager, ref AdvCommand command);
    }
}

