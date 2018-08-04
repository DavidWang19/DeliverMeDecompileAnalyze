namespace Utage
{
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    internal class AdvCommandRuleFadeIn : AdvCommandEffectBase
    {
        private AdvTransitionArgs data;

        public AdvCommandRuleFadeIn(StringGridRow row) : base(row)
        {
            string textureName = base.ParseCell<string>(AdvColumnName.Arg2);
            float vague = base.ParseCellOptional<float>(AdvColumnName.Arg3, 0.2f);
            float time = base.ParseCellOptional<float>(AdvColumnName.Arg6, 0.2f);
            this.data = new AdvTransitionArgs(textureName, vague, time);
        }

        protected override void OnStartEffect(GameObject target, AdvEngine engine, AdvScenarioThread thread)
        {
            <OnStartEffect>c__AnonStorey0 storey = new <OnStartEffect>c__AnonStorey0 {
                thread = thread,
                $this = this
            };
            IAdvFade componentInChildren = target.GetComponentInChildren<IAdvFade>(true);
            if (componentInChildren == null)
            {
                Debug.LogError("Can't find [ " + base.TargetName + " ]");
                this.OnComplete(storey.thread);
            }
            else
            {
                componentInChildren.RuleFadeIn(engine, this.data, new Action(storey.<>m__0));
            }
        }

        [CompilerGenerated]
        private sealed class <OnStartEffect>c__AnonStorey0
        {
            internal AdvCommandRuleFadeIn $this;
            internal AdvScenarioThread thread;

            internal void <>m__0()
            {
                this.$this.OnComplete(this.thread);
            }
        }
    }
}

