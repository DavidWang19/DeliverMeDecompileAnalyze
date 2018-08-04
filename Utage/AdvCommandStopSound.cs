namespace Utage
{
    using System;

    internal class AdvCommandStopSound : AdvCommand
    {
        private float fadeTime;
        private string[] groups;

        public AdvCommandStopSound(StringGridRow row) : base(row)
        {
            this.fadeTime = 0.15f;
            string[] defaultArray = new string[] { "Bgm", "Ambience" };
            this.groups = base.ParseCellOptionalArray<string>(AdvColumnName.Arg1, defaultArray);
            this.fadeTime = base.ParseCellOptional<float>(AdvColumnName.Arg6, this.fadeTime);
        }

        public override void DoCommand(AdvEngine engine)
        {
            if ((this.groups.Length == 1) && (this.groups[0] == "All"))
            {
                engine.SoundManager.StopAll(this.fadeTime);
            }
            else
            {
                engine.SoundManager.StopGroups(this.groups, this.fadeTime);
            }
        }
    }
}

