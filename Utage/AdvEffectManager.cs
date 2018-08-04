namespace Utage
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    [AddComponentMenu("Utage/ADV/EffectManager")]
    public class AdvEffectManager : MonoBehaviour
    {
        private AdvEngine engine;
        [SerializeField]
        private AdvUguiMessageWindowManager messageWindow;
        [SerializeField]
        private List<Texture2D> ruleTextureList = new List<Texture2D>();

        internal Texture2D FindRuleTexture(string name)
        {
            <FindRuleTexture>c__AnonStorey0 storey = new <FindRuleTexture>c__AnonStorey0 {
                name = name
            };
            return this.ruleTextureList.Find(new Predicate<Texture2D>(storey.<>m__0));
        }

        internal GameObject FindTarget(AdvCommandEffectBase command)
        {
            return this.FindTarget(command.Target, command.TargetName);
        }

        internal GameObject FindTarget(TargetType targetType, string targetName)
        {
            switch (targetType)
            {
                case TargetType.Camera:
                    if (!string.IsNullOrEmpty(targetName))
                    {
                        TargetType camera = TargetType.Camera;
                        if (!(targetName == camera.ToString()))
                        {
                            CameraRoot root = this.Engine.CameraManager.FindCameraRoot(targetName);
                            if (root == null)
                            {
                                return null;
                            }
                            return root.get_gameObject();
                        }
                    }
                    return this.Engine.CameraManager.get_gameObject();

                case TargetType.Graphics:
                    return this.Engine.GraphicManager.get_gameObject();

                case TargetType.MessageWindow:
                    return this.MessageWindow.get_gameObject();
            }
            return this.Engine.GraphicManager.FindObjectOrLayer(targetName);
        }

        public AdvEngine Engine
        {
            get
            {
                if (this.engine == null)
                {
                }
                return (this.engine = base.GetComponentInParent<AdvEngine>());
            }
        }

        private AdvUguiMessageWindowManager MessageWindow
        {
            get
            {
                if (this.messageWindow == null)
                {
                }
                return (this.messageWindow = this.Engine.GetComponentInChildren<AdvUguiMessageWindowManager>(true));
            }
        }

        public List<Texture2D> RuleTextureList
        {
            get
            {
                return this.ruleTextureList;
            }
            set
            {
                this.ruleTextureList = value;
            }
        }

        [CompilerGenerated]
        private sealed class <FindRuleTexture>c__AnonStorey0
        {
            internal string name;

            internal bool <>m__0(Texture2D x)
            {
                return (x.get_name() == this.name);
            }
        }

        public enum TargetType
        {
            Default,
            Camera,
            Graphics,
            MessageWindow
        }
    }
}

