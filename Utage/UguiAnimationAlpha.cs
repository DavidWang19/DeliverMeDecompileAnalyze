namespace Utage
{
    using System;
    using UnityEngine;

    [AddComponentMenu("Utage/Lib/UI/Animation/Alpha")]
    public class UguiAnimationAlpha : UguiAnimation
    {
        [SerializeField]
        private float by;
        [SerializeField]
        private float from;
        private float lerpFrom;
        private float lerpTo;
        [SerializeField]
        private float to;

        protected override void StartAnimation()
        {
            switch (base.Type)
            {
                case UguiAnimation.AnimationType.To:
                    this.lerpFrom = base.TargetGraphic.get_color().a;
                    this.lerpTo = this.To;
                    break;

                case UguiAnimation.AnimationType.From:
                    this.lerpFrom = this.From;
                    this.lerpTo = base.TargetGraphic.get_color().a;
                    break;

                case UguiAnimation.AnimationType.FromTo:
                    this.lerpFrom = this.From;
                    this.lerpTo = this.To;
                    break;

                case UguiAnimation.AnimationType.By:
                    this.lerpFrom = 0f;
                    this.lerpTo = this.By;
                    break;
            }
            Color color3 = base.TargetGraphic.get_color();
            color3.a = this.lerpFrom;
            base.TargetGraphic.set_color(color3);
        }

        protected override void UpdateAnimation(float value)
        {
            Color color = base.TargetGraphic.get_color();
            float num = base.LerpValue(this.lerpFrom, this.lerpTo);
            if (base.Type == UguiAnimation.AnimationType.By)
            {
                color.a += num;
            }
            else
            {
                color.a = num;
            }
            base.TargetGraphic.set_color(color);
        }

        public float By
        {
            get
            {
                return this.by;
            }
            set
            {
                this.by = value;
            }
        }

        public float From
        {
            get
            {
                return this.from;
            }
            set
            {
                this.from = value;
            }
        }

        public float To
        {
            get
            {
                return this.to;
            }
            set
            {
                this.to = value;
            }
        }
    }
}

