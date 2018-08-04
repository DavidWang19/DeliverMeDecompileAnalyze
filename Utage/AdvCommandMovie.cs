namespace Utage
{
    using System;
    using UnityEngine;

    internal class AdvCommandMovie : AdvCommand
    {
        private bool cancel;
        private string label;
        private bool loop;
        private float time;
        private float waitTime;

        public AdvCommandMovie(StringGridRow row, AdvSettingDataManager dataManager) : base(row)
        {
            this.label = base.ParseCell<string>(AdvColumnName.Arg1);
            this.loop = base.ParseCellOptional<bool>(AdvColumnName.Arg2, false);
            this.cancel = base.ParseCellOptional<bool>(AdvColumnName.Arg3, true);
            this.waitTime = base.ParseCellOptional<float>(AdvColumnName.Arg6, -1f);
        }

        public override void DoCommand(AdvEngine engine)
        {
            if (WrapperMoviePlayer.GetInstance().OverrideRootDirectory)
            {
                string[] args = new string[] { WrapperMoviePlayer.GetInstance().RootDirectory, this.label };
                WrapperMoviePlayer.Play(FilePathUtil.Combine(args), this.loop, this.cancel);
            }
            else
            {
                string[] textArray2 = new string[] { engine.DataManager.SettingDataManager.BootSetting.ResourceDir, "Movie" };
                string str = FilePathUtil.Combine(textArray2);
                string[] textArray3 = new string[] { str, this.label };
                WrapperMoviePlayer.Play(FilePathUtil.Combine(textArray3), this.loop, this.cancel);
            }
            this.time = 0f;
        }

        public override bool Wait(AdvEngine engine)
        {
            if (engine.UiManager.IsInputTrig)
            {
                WrapperMoviePlayer.Cancel();
            }
            bool flag = WrapperMoviePlayer.IsPlaying();
            if (this.waitTime >= 0f)
            {
                if (this.time >= this.waitTime)
                {
                    flag = false;
                }
                this.time += Time.get_deltaTime();
            }
            return flag;
        }
    }
}

