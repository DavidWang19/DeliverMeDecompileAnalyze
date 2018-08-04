namespace Utage
{
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class AdvCommandPlayAnimatin : AdvCommandEffectBase
    {
        private string animationName;

        public AdvCommandPlayAnimatin(StringGridRow row, AdvSettingDataManager dataManager) : base(row)
        {
            this.animationName = base.ParseCell<string>(AdvColumnName.Arg2);
        }

        protected override void OnStartEffect(GameObject target, AdvEngine engine, AdvScenarioThread thread)
        {
            <OnStartEffect>c__AnonStorey0 storey = new <OnStartEffect>c__AnonStorey0 {
                thread = thread,
                $this = this
            };
            AdvAnimationData data = engine.DataManager.SettingDataManager.AnimationSetting.Find(this.animationName);
            if (data == null)
            {
                Debug.LogError(base.RowData.ToErrorString("Animation " + this.animationName + " is not found"));
                this.OnComplete(storey.thread);
            }
            else
            {
                AdvAnimationPlayer player = target.AddComponent<AdvAnimationPlayer>();
                player.AutoDestory = true;
                player.EnableSave = true;
                player.Play(data.Clip, engine.Page.SkippedSpeed, new Action(storey.<>m__0));
            }
        }

        [CompilerGenerated]
        private sealed class <OnStartEffect>c__AnonStorey0
        {
            internal AdvCommandPlayAnimatin $this;
            internal AdvScenarioThread thread;

            internal void <>m__0()
            {
                this.$this.OnComplete(this.thread);
            }
        }
    }
}

