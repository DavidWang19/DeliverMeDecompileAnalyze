namespace Utage
{
    using System;
    using UtageExtensions;

    public class AdvCommandSendMessage : AdvCommand
    {
        private string arg2;
        private string arg3;
        private string arg4;
        private string arg5;
        private bool isWait;
        private string name;
        private string text;
        private string voice;
        private int voiceVersion;

        public AdvCommandSendMessage(StringGridRow row) : base(row)
        {
            this.name = base.ParseCell<string>(AdvColumnName.Arg1);
            this.arg2 = base.ParseCellOptional<string>(AdvColumnName.Arg2, string.Empty);
            this.arg3 = base.ParseCellOptional<string>(AdvColumnName.Arg3, string.Empty);
            this.arg4 = base.ParseCellOptional<string>(AdvColumnName.Arg4, string.Empty);
            this.arg5 = base.ParseCellOptional<string>(AdvColumnName.Arg5, string.Empty);
            this.voice = base.ParseCellOptional<string>(AdvColumnName.Voice, string.Empty);
            this.voiceVersion = base.ParseCellOptional<int>(AdvColumnName.VoiceVersion, 0);
        }

        public override void DoCommand(AdvEngine engine)
        {
            this.text = base.ParseCellLocalizedText();
            engine.ScenarioPlayer.SendMessageTarget.SafeSendMessage("OnDoCommand", this, false);
        }

        public override bool Wait(AdvEngine engine)
        {
            engine.ScenarioPlayer.SendMessageTarget.SafeSendMessage("OnWait", this, false);
            return this.IsWait;
        }

        public string Arg2
        {
            get
            {
                return this.arg2;
            }
        }

        public string Arg3
        {
            get
            {
                return this.arg3;
            }
        }

        public string Arg4
        {
            get
            {
                return this.arg4;
            }
        }

        public string Arg5
        {
            get
            {
                return this.arg5;
            }
        }

        public bool IsWait
        {
            get
            {
                return this.isWait;
            }
            set
            {
                this.isWait = value;
            }
        }

        public string Name
        {
            get
            {
                return this.name;
            }
        }

        public string Text
        {
            get
            {
                return this.text;
            }
        }

        public string Voice
        {
            get
            {
                return this.voice;
            }
        }

        public int VoiceVersion
        {
            get
            {
                return this.voiceVersion;
            }
        }
    }
}

