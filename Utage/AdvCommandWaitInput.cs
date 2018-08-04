namespace Utage
{
    using System;
    using UnityEngine;

    internal class AdvCommandWaitInput : AdvCommand
    {
        private float time;
        private float waitEndTime;

        public AdvCommandWaitInput(StringGridRow row) : base(row)
        {
            this.time = base.ParseCellOptional<float>(AdvColumnName.Arg6, -1f);
        }

        public override void DoCommand(AdvEngine engine)
        {
            if (base.CurrentTread.IsMainThread)
            {
                engine.Page.IsWaitingInputCommand = true;
            }
            this.waitEndTime = Time.get_time() + (!engine.Page.CheckSkip() ? this.time : (this.time / engine.Config.SkipSpped));
        }

        private bool IsWaitng(AdvEngine engine)
        {
            if (engine.Page.CheckSkip())
            {
                return false;
            }
            if (engine.UiManager.IsInputTrig)
            {
                return false;
            }
            if (this.time > 0f)
            {
                return (Time.get_time() < this.waitEndTime);
            }
            return true;
        }

        public override bool Wait(AdvEngine engine)
        {
            if (this.IsWaitng(engine))
            {
                return true;
            }
            if (engine.Config.VoiceStopType == VoiceStopType.OnClick)
            {
                engine.SoundManager.StopVoiceIgnoreLoop();
            }
            engine.UiManager.ClearPointerDown();
            if (base.CurrentTread.IsMainThread)
            {
                engine.Page.IsWaitingInputCommand = false;
            }
            return false;
        }
    }
}

