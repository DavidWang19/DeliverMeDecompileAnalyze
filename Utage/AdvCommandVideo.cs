namespace Utage
{
    using System;
    using UnityEngine;

    internal class AdvCommandVideo : AdvCommand
    {
        private Color cameraClearColor;
        private CameraClearFlags cameraClearFlags;
        private string cameraName;
        private bool cancel;
        private AssetFile file;
        private bool isEndPlay;
        private string label;
        private bool loop;

        public AdvCommandVideo(StringGridRow row, AdvSettingDataManager dataManager) : base(row)
        {
            this.isEndPlay = true;
            this.label = base.ParseCell<string>(AdvColumnName.Arg1);
            this.cameraName = base.ParseCell<string>(AdvColumnName.Arg2);
            this.loop = base.ParseCellOptional<bool>(AdvColumnName.Arg3, false);
            this.cancel = base.ParseCellOptional<bool>(AdvColumnName.Arg4, true);
            string[] args = new string[] { dataManager.BootSetting.ResourceDir, "Video" };
            string path = FilePathUtil.Combine(args);
            string[] textArray2 = new string[] { path, this.label };
            path = FilePathUtil.Combine(textArray2);
            this.file = base.AddLoadFile(path, new AdvCommandSetting(this));
        }

        public override void DoCommand(AdvEngine engine)
        {
            engine.GraphicManager.VideoManager.Play(this.label, this.cameraName, this.file, this.loop, this.cancel);
            this.isEndPlay = false;
        }

        public override bool Wait(AdvEngine engine)
        {
            if (!this.isEndPlay)
            {
                if (engine.UiManager.IsInputTrig)
                {
                    engine.GraphicManager.VideoManager.Cancel(this.label);
                }
                this.isEndPlay = engine.GraphicManager.VideoManager.IsEndPlay(this.label);
                if (this.isEndPlay)
                {
                    engine.GraphicManager.VideoManager.Complete(this.label);
                    Camera camera = engine.EffectManager.FindTarget(AdvEffectManager.TargetType.Camera, this.cameraName).GetComponentInChildren<Camera>();
                    this.cameraClearFlags = camera.get_clearFlags();
                    this.cameraClearColor = camera.get_backgroundColor();
                    camera.set_clearFlags(2);
                    camera.set_backgroundColor(Color.get_black());
                }
                return true;
            }
            Camera componentInChildren = engine.EffectManager.FindTarget(AdvEffectManager.TargetType.Camera, this.cameraName).GetComponentInChildren<Camera>();
            componentInChildren.set_clearFlags(this.cameraClearFlags);
            componentInChildren.set_backgroundColor(this.cameraClearColor);
            return false;
        }
    }
}

