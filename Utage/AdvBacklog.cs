namespace Utage
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Text;
    using UnityEngine;

    public class AdvBacklog
    {
        private List<AdvBacklogDataInPage> dataList = new List<AdvBacklogDataInPage>();
        private const int Version = 0;

        internal void AddData(string logText, string characterName)
        {
            this.dataList.Add(new AdvBacklogDataInPage(logText, characterName));
        }

        internal void AddData(AdvCommandText log, AdvCharacterInfo characterInfo)
        {
            this.dataList.Add(new AdvBacklogDataInPage(log, characterInfo));
        }

        public string FindCharacerLabel(string voiceFileName)
        {
            foreach (AdvBacklogDataInPage page in this.dataList)
            {
                if (page.VoiceFileName == voiceFileName)
                {
                    return page.CharacterLabel;
                }
            }
            return string.Empty;
        }

        internal void Read(BinaryReader reader)
        {
            this.dataList.Clear();
            int version = reader.ReadInt32();
            if (version == 0)
            {
                int num2 = reader.ReadInt32();
                for (int i = 0; i < num2; i++)
                {
                    AdvBacklogDataInPage item = new AdvBacklogDataInPage();
                    item.Read(reader, version);
                    this.dataList.Add(item);
                }
            }
            else
            {
                object[] args = new object[] { version };
                Debug.LogError(LanguageErrorMsg.LocalizeTextFormat(ErrorMsg.UnknownVersion, args));
            }
        }

        internal void Write(BinaryWriter writer)
        {
            writer.Write(0);
            writer.Write(this.dataList.Count);
            foreach (AdvBacklogDataInPage page in this.dataList)
            {
                writer.Write(page.LogText);
                writer.Write(page.CharacterLabel);
                writer.Write(page.CharacterNameText);
                writer.Write(page.VoiceFileName);
            }
        }

        internal int CountVoice
        {
            get
            {
                int num = 0;
                foreach (AdvBacklogDataInPage page in this.dataList)
                {
                    if (!string.IsNullOrEmpty(page.VoiceFileName))
                    {
                        num++;
                    }
                }
                return num;
            }
        }

        public bool IsEmpty
        {
            get
            {
                return (this.dataList.Count <= 0);
            }
        }

        public string MainCharacterNameText
        {
            get
            {
                foreach (AdvBacklogDataInPage page in this.dataList)
                {
                    if (!string.IsNullOrEmpty(page.CharacterNameText))
                    {
                        return page.CharacterNameText;
                    }
                }
                return string.Empty;
            }
        }

        public string MainVoiceFileName
        {
            get
            {
                foreach (AdvBacklogDataInPage page in this.dataList)
                {
                    if (!string.IsNullOrEmpty(page.VoiceFileName))
                    {
                        return page.VoiceFileName;
                    }
                }
                return string.Empty;
            }
        }

        public string Text
        {
            get
            {
                StringBuilder builder = new StringBuilder();
                foreach (AdvBacklogDataInPage page in this.dataList)
                {
                    builder.Append(page.LogText);
                }
                char[] trimChars = new char[] { '\n' };
                return builder.ToString().TrimEnd(trimChars);
            }
        }

        private class AdvBacklogDataInPage
        {
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private string <CharacterLabel>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private string <CharacterNameText>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private string <LogText>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private string <VoiceFileName>k__BackingField;

            public AdvBacklogDataInPage()
            {
                this.LogText = string.Empty;
                this.CharacterLabel = string.Empty;
                this.CharacterNameText = string.Empty;
                this.VoiceFileName = string.Empty;
            }

            public AdvBacklogDataInPage(string content, string characterNameText)
            {
                this.LogText = content;
                this.CharacterLabel = string.Empty;
                this.CharacterNameText = characterNameText;
                this.VoiceFileName = string.Empty;
            }

            public AdvBacklogDataInPage(AdvCommandText dataInPage, AdvCharacterInfo characterInfo)
            {
                this.LogText = string.Empty;
                this.VoiceFileName = string.Empty;
                if (characterInfo != null)
                {
                    this.CharacterLabel = characterInfo.Label;
                    this.CharacterNameText = characterInfo.LocalizeNameText;
                }
                else
                {
                    this.CharacterLabel = string.Empty;
                    this.CharacterNameText = string.Empty;
                }
                this.LogText = TextParser.MakeLogText(dataInPage.ParseCellLocalizedText());
                if (dataInPage.VoiceFile != null)
                {
                    this.VoiceFileName = dataInPage.VoiceFile.FileName;
                    this.LogText = TextParser.AddTag(this.LogText, "sound", dataInPage.VoiceFile.FileName);
                }
                else
                {
                    this.VoiceFileName = string.Empty;
                }
                if (dataInPage.IsNextBr)
                {
                    this.LogText = this.LogText + "\n";
                }
            }

            internal void Read(BinaryReader reader, int version)
            {
                this.LogText = reader.ReadString();
                this.CharacterLabel = reader.ReadString();
                this.CharacterNameText = reader.ReadString();
                this.VoiceFileName = reader.ReadString();
            }

            internal void Write(BinaryWriter writer)
            {
                writer.Write(this.LogText);
                writer.Write(this.CharacterLabel);
                writer.Write(this.CharacterNameText);
                writer.Write(this.VoiceFileName);
            }

            public string CharacterLabel { get; private set; }

            public string CharacterNameText { get; private set; }

            public string LogText { get; private set; }

            public string VoiceFileName { get; private set; }
        }
    }
}

