namespace Utage
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class AdvCharacterInfo
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private graphic <Graphic>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <IsHide>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <Label>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <NameText>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <Pattern>k__BackingField;

        private AdvCharacterInfo(string label, string nameText, string pattern, bool isHide, graphic graphic)
        {
            this.Label = label;
            this.NameText = nameText;
            this.Pattern = pattern;
            this.IsHide = isHide;
            this.Graphic = graphic;
        }

        public static AdvCharacterInfo Create(AdvCommand command, AdvSettingDataManager dataManager)
        {
            <Create>c__AnonStorey0 storey = new <Create>c__AnonStorey0();
            if (command.IsEmptyCell(AdvColumnName.Arg1))
            {
                return null;
            }
            string nameText = command.ParseCell<string>(AdvColumnName.Arg1);
            storey.characterLabel = nameText;
            storey.isHide = false;
            storey.erroMsg = string.Empty;
            string pattern = ParserUtil.ParseTagTextToString(command.ParseCellOptional<string>(AdvColumnName.Arg2, string.Empty), new Func<string, string, bool>(storey.<>m__0));
            if (!string.IsNullOrEmpty(storey.erroMsg))
            {
                Debug.LogError(storey.erroMsg);
                return null;
            }
            if (!dataManager.CharacterSetting.Contains(storey.characterLabel))
            {
                return new AdvCharacterInfo(storey.characterLabel, nameText, pattern, storey.isHide, null);
            }
            AdvCharacterSettingData characterData = dataManager.CharacterSetting.GetCharacterData(storey.characterLabel, pattern);
            if (characterData == null)
            {
                Debug.LogError(command.ToErrorString(storey.characterLabel + ", " + pattern + " is not contained in Chactecter Sheet"));
                return null;
            }
            if (!string.IsNullOrEmpty(characterData.NameText) && (nameText == storey.characterLabel))
            {
                nameText = characterData.NameText;
            }
            return new AdvCharacterInfo(storey.characterLabel, nameText, pattern, storey.isHide, characterData.Graphic);
        }

        public graphic Graphic { get; private set; }

        public bool IsHide { get; private set; }

        public string Label { get; private set; }

        public string LocalizeNameText
        {
            get
            {
                return LanguageManagerBase.Instance.LocalizeText(TextParser.MakeLogText(this.NameText));
            }
        }

        public string NameText { get; private set; }

        public string Pattern { get; private set; }

        [CompilerGenerated]
        private sealed class <Create>c__AnonStorey0
        {
            internal string characterLabel;
            internal string erroMsg;
            internal bool isHide;

            internal bool <>m__0(string tagName, string arg)
            {
                bool flag = false;
                if (tagName != null)
                {
                    if (!(tagName == "Off"))
                    {
                        if (tagName == "Character")
                        {
                            this.characterLabel = arg;
                            goto Label_0062;
                        }
                    }
                    else
                    {
                        this.isHide = true;
                        goto Label_0062;
                    }
                }
                this.erroMsg = "Unkownn Tag <" + tagName + ">";
                flag = true;
            Label_0062:
                return !flag;
            }
        }
    }
}

