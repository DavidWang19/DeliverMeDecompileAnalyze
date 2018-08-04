namespace Utage
{
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class AdvCommandZoomCamera : AdvCommandEffectBase
    {
        private bool isEmptyZoom;
        private bool isEmptyZoomCenter;
        private float time;
        private float zoom;
        private Vector2 zoomCenter;

        public AdvCommandZoomCamera(StringGridRow row, AdvSettingDataManager dataManager) : base(row)
        {
            base.targetType = AdvEffectManager.TargetType.Camera;
            this.isEmptyZoom = base.IsEmptyCell(AdvColumnName.Arg2);
            this.zoom = base.ParseCellOptional<float>(AdvColumnName.Arg2, 1f);
            this.isEmptyZoomCenter = base.IsEmptyCell(AdvColumnName.Arg3) && base.IsEmptyCell(AdvColumnName.Arg4);
            this.zoomCenter.x = base.ParseCellOptional<float>(AdvColumnName.Arg3, 0f);
            this.zoomCenter.y = base.ParseCellOptional<float>(AdvColumnName.Arg4, 0f);
            this.time = base.ParseCellOptional<float>(AdvColumnName.Arg6, 0.2f);
        }

        protected override void OnStartEffect(GameObject target, AdvEngine engine, AdvScenarioThread thread)
        {
            <OnStartEffect>c__AnonStorey1 storey = new <OnStartEffect>c__AnonStorey1 {
                thread = thread,
                $this = this
            };
            if (target != null)
            {
                <OnStartEffect>c__AnonStorey0 storey2;
                storey2 = new <OnStartEffect>c__AnonStorey0 {
                    <>f__ref$1 = storey,
                    camera = target.GetComponentInChildren<LetterBoxCamera>(),
                    zoom0 = storey2.camera.Zoom2D,
                    zoomTo = !this.isEmptyZoom ? this.zoom : storey2.zoom0,
                    center0 = (storey2.zoom0 != 1f) ? storey2.camera.Zoom2DCenter : this.zoomCenter,
                    centerTo = !this.isEmptyZoomCenter ? this.zoomCenter : storey2.center0,
                    timer = target.AddComponent<Timer>()
                };
                storey2.timer.AutoDestroy = true;
                storey2.timer.StartTimer(engine.Page.ToSkippedTime(this.time), new Action<Timer>(storey2.<>m__0), new Action<Timer>(storey2.<>m__1), 0f);
            }
            else
            {
                object[] args = new object[] { "SpriteCamera" };
                Debug.LogError(LanguageAdvErrorMsg.LocalizeTextFormat(AdvErrorMsg.NotFoundTweenGameObject, args));
                this.OnComplete(storey.thread);
            }
        }

        [CompilerGenerated]
        private sealed class <OnStartEffect>c__AnonStorey0
        {
            internal AdvCommandZoomCamera.<OnStartEffect>c__AnonStorey1 <>f__ref$1;
            internal LetterBoxCamera camera;
            internal Vector2 center0;
            internal Vector2 centerTo;
            internal Timer timer;
            internal float zoom0;
            internal float zoomTo;

            internal void <>m__0(Timer x)
            {
                float curve = this.timer.GetCurve(this.zoom0, this.zoomTo);
                Vector2 center = this.timer.GetCurve(this.center0, this.centerTo);
                this.camera.SetZoom2D(curve, center);
            }

            internal void <>m__1(Timer x)
            {
                if (this.zoomTo == 1f)
                {
                    this.camera.Zoom2DCenter = Vector2.get_zero();
                }
                this.<>f__ref$1.$this.OnComplete(this.<>f__ref$1.thread);
            }
        }

        [CompilerGenerated]
        private sealed class <OnStartEffect>c__AnonStorey1
        {
            internal AdvCommandZoomCamera $this;
            internal AdvScenarioThread thread;
        }
    }
}

