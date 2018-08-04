namespace Utage
{
    using System;

    internal class AdvCommandChangeSoundVolume : AdvCommand
    {
        private string[] groups;
        private float volume;

        public AdvCommandChangeSoundVolume(StringGridRow row) : base(row)
        {
            this.groups = base.ParseCellArray<string>(AdvColumnName.Arg1);
            this.volume = base.ParseCell<float>(AdvColumnName.Arg2);
        }

        public override void DoCommand(AdvEngine engine)
        {
            if ((this.groups.Length == 1) && (this.groups[0] == "All"))
            {
                engine.SoundManager.SetGroupVolume("Bgm", this.volume);
                engine.SoundManager.SetGroupVolume("Ambience", this.volume);
                engine.SoundManager.SetGroupVolume("Se", this.volume);
                engine.SoundManager.SetGroupVolume("Voice", this.volume);
            }
            else
            {
                foreach (string str in this.groups)
                {
                    engine.SoundManager.SetGroupVolume(str, this.volume);
                }
            }
        }
    }
}

