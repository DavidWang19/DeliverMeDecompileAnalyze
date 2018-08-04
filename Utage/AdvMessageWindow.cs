namespace Utage
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.CompilerServices;

    public class AdvMessageWindow
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <CharacterLabel>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private IAdvMessageWindow <MessageWindow>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <Name>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <NameText>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private TextData <Text>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <TextLength>k__BackingField;

        public AdvMessageWindow(IAdvMessageWindow messageWindow)
        {
            this.MessageWindow = messageWindow;
            this.Name = this.MessageWindow.gameObject.get_name();
            this.Clear();
        }

        internal void ChangeActive(bool isActive)
        {
            if (!isActive)
            {
                this.Clear();
            }
            this.MessageWindow.OnChangeActive(isActive);
        }

        internal void ChangeCurrent(bool isCurrent)
        {
            this.MessageWindow.OnChangeCurrent(isCurrent);
        }

        private void Clear()
        {
            this.Text = new TextData(string.Empty);
            this.NameText = string.Empty;
            this.CharacterLabel = string.Empty;
            this.TextLength = -1;
        }

        internal void PageTextChange(AdvPage page)
        {
            this.Text = page.TextData;
            this.NameText = page.NameText;
            this.CharacterLabel = page.CharacterLabel;
            this.TextLength = page.CurrentTextLength;
            this.MessageWindow.OnTextChanged(this);
        }

        internal void ReadPageData(BinaryReader reader)
        {
            this.Text = new TextData(reader.ReadString());
            this.NameText = reader.ReadString();
            this.CharacterLabel = reader.ReadString();
            this.TextLength = -1;
            this.MessageWindow.OnTextChanged(this);
        }

        internal void Reset()
        {
            this.Clear();
            this.MessageWindow.OnReset();
        }

        internal void WritePageData(BinaryWriter writer)
        {
            writer.Write(this.Text.OriginalText);
            writer.Write(this.NameText);
            writer.Write(this.CharacterLabel);
        }

        public string CharacterLabel { get; protected set; }

        protected IAdvMessageWindow MessageWindow { get; set; }

        public string Name { get; protected set; }

        public string NameText { get; protected set; }

        public TextData Text { get; protected set; }

        public int TextLength { get; protected set; }
    }
}

