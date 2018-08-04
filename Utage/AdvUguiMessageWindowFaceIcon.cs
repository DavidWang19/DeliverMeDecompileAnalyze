namespace Utage
{
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.UI;
    using UtageExtensions;

    [AddComponentMenu("Utage/ADV/UguiMessageWindowFaceIcon")]
    public class AdvUguiMessageWindowFaceIcon : MonoBehaviour
    {
        [SerializeField]
        protected AdvEngine engine;
        private GameObject iconRoot;
        private AdvGraphicObject targetObject;

        private void Awake()
        {
            this.Engine.Page.OnChangeText.AddListener(new UnityAction<AdvPage>(this, (IntPtr) this.OnChangeText));
            this.Engine.MessageWindowManager.OnReset.AddListener(new UnityAction<AdvMessageWindowManager>(this, (IntPtr) this.OnReset));
        }

        private T ChangeIconComponent<T>() where T: Component
        {
            T component = null;
            if (this.iconRoot != null)
            {
                component = this.iconRoot.GetComponent<T>();
                if (component != null)
                {
                    this.iconRoot.SetActive(true);
                    return component;
                }
            }
            if (this.iconRoot != null)
            {
                Object.Destroy(this.iconRoot);
            }
            component = base.get_transform().AddChildGameObjectComponent<T>("Icon");
            this.iconRoot = component.get_gameObject();
            RectTransform t = this.iconRoot.get_transform() as RectTransform;
            if (t != null)
            {
                t.SetStretch();
            }
            return component;
        }

        private void ChangeReference(AssetFile file, GameObject go)
        {
            foreach (AssetFileReference reference in go.GetComponents<AssetFileReference>())
            {
                Object.Destroy(reference);
            }
            file.AddReferenceComponent(go);
        }

        private void HideIcon()
        {
            if ((this.iconRoot != null) && this.iconRoot.get_activeSelf())
            {
                this.iconRoot.SetActive(false);
            }
        }

        private void OnChangeText(AdvPage page)
        {
            if (!this.TyrSetIcon(page))
            {
                this.targetObject = null;
                this.HideIcon();
            }
        }

        private void OnReset(AdvMessageWindowManager window)
        {
            if (this.iconRoot != null)
            {
                Object.Destroy(this.iconRoot);
                this.iconRoot = null;
            }
        }

        private IconGraphicType ParseIconGraphicType(AdvGraphicInfo info, string characterLabel)
        {
            switch (info.FileType)
            {
                case "Dicing":
                    return IconGraphicType.Dicing;

                case "2D":
                case string.Empty:
                    return IconGraphicType.Default;
            }
            AdvGraphicObject obj2 = this.Engine.GraphicManager.FindObject(characterLabel);
            if ((obj2 != null) && obj2.EnableRenderTexture)
            {
                return IconGraphicType.RenderTexture;
            }
            return IconGraphicType.NotSupport;
        }

        private void SetIconDicing(AdvGraphicInfo graphic, Rect rect)
        {
            DicingImage image = this.ChangeIconComponent<DicingImage>();
            DicingTextures unityObject = graphic.File.UnityObject as DicingTextures;
            string subFileName = graphic.SubFileName;
            image.DicingData = unityObject;
            image.ChangePattern(subFileName);
            float width = image.PatternData.Width;
            float height = image.PatternData.Height;
            image.UvRect = rect.ToUvRect(width, height);
            this.ChangeReference(graphic.File, image.get_gameObject());
        }

        private void SetIconDicingPattern(AssetFile file, string pattern)
        {
            DicingImage image = this.ChangeIconComponent<DicingImage>();
            DicingTextures unityObject = file.UnityObject as DicingTextures;
            image.DicingData = unityObject;
            image.ChangePattern(pattern);
            image.UvRect = new Rect(0f, 0f, 1f, 1f);
            this.ChangeReference(file, image.get_gameObject());
        }

        private void SetIconImage(AssetFile file)
        {
            <SetIconImage>c__AnonStorey0 storey = new <SetIconImage>c__AnonStorey0 {
                file = file,
                $this = this
            };
            AssetFileManager.Load(storey.file, new Action<AssetFile>(storey.<>m__0));
        }

        private void SetIconRectImage(AdvGraphicInfo graphic, Rect rect)
        {
            RawImage image = this.ChangeIconComponent<RawImage>();
            image.set_material(null);
            Texture2D texture = graphic.File.Texture;
            image.set_texture(texture);
            float w = texture.get_width();
            float h = texture.get_height();
            image.set_uvRect(rect.ToUvRect(w, h));
            this.ChangeReference(graphic.File, image.get_gameObject());
        }

        private void SetIconRenderTexture(Rect rect)
        {
            AdvGraphicObject targetObject = this.targetObject;
            if (targetObject.RenderTextureSpace != null)
            {
                RawImage image = this.ChangeIconComponent<RawImage>();
                if (targetObject.RenderTextureSpace.RenderTextureType == AdvRenderTextureMode.Image)
                {
                    image.set_material(new Material(ShaderManager.DrawByRenderTexture));
                }
                Texture renderTexture = targetObject.RenderTextureSpace.RenderTexture;
                image.set_texture(renderTexture);
                float w = renderTexture.get_width();
                float h = renderTexture.get_height();
                Transform transform = targetObject.TargetObject.get_transform();
                float x = transform.get_localScale().x;
                float y = transform.get_localScale().y;
                rect.set_position(new Vector2(rect.get_position().x * x, rect.get_position().y * y));
                rect.set_size(new Vector2(rect.get_size().x * x, rect.get_size().y * y));
                image.set_uvRect(rect.ToUvRect(w, h));
            }
        }

        private bool TyrSetIcon(AdvPage page)
        {
            this.targetObject = null;
            AdvCharacterInfo characterInfo = page.CharacterInfo;
            if (((characterInfo != null) && (characterInfo.Graphic != null)) && (characterInfo.Graphic.Main != null))
            {
                AdvGraphicInfo main = characterInfo.Graphic.Main;
                AdvCharacterSettingData settingData = main.SettingData as AdvCharacterSettingData;
                if (settingData == null)
                {
                    return false;
                }
                AdvCharacterSettingData.IconInfo icon = settingData.Icon;
                if (icon.IconType == AdvCharacterSettingData.IconInfo.Type.None)
                {
                    return false;
                }
                this.targetObject = this.Engine.GraphicManager.FindObject(characterInfo.Label);
                switch (icon.IconType)
                {
                    case AdvCharacterSettingData.IconInfo.Type.None:
                        goto Label_0120;

                    case AdvCharacterSettingData.IconInfo.Type.IconImage:
                        this.SetIconImage(icon.File);
                        return true;

                    case AdvCharacterSettingData.IconInfo.Type.DicingPattern:
                        this.SetIconDicingPattern(icon.File, icon.IconSubFileName);
                        return true;

                    case AdvCharacterSettingData.IconInfo.Type.RectImage:
                        switch (this.ParseIconGraphicType(main, characterInfo.Label))
                        {
                            case IconGraphicType.Default:
                                this.SetIconRectImage(main, icon.IconRect);
                                return true;

                            case IconGraphicType.Dicing:
                                this.SetIconDicing(main, icon.IconRect);
                                return true;

                            case IconGraphicType.RenderTexture:
                                this.SetIconRenderTexture(icon.IconRect);
                                return true;
                        }
                        break;

                    default:
                        goto Label_0120;
                }
            }
            return false;
        Label_0120:
            return false;
        }

        private void Update()
        {
            if (this.targetObject == null)
            {
                this.HideIcon();
            }
        }

        public AdvEngine Engine
        {
            get
            {
                if (this.engine == null)
                {
                }
                return (this.engine = Object.FindObjectOfType<AdvEngine>());
            }
        }

        [CompilerGenerated]
        private sealed class <SetIconImage>c__AnonStorey0
        {
            internal AdvUguiMessageWindowFaceIcon $this;
            internal AssetFile file;

            internal void <>m__0(AssetFile x)
            {
                RawImage image = this.$this.ChangeIconComponent<RawImage>();
                image.set_material(null);
                Texture2D texture = x.Texture;
                image.set_texture(texture);
                image.set_uvRect(new Rect(0f, 0f, 1f, 1f));
                this.$this.ChangeReference(this.file, image.get_gameObject());
            }
        }

        private enum IconGraphicType
        {
            Default,
            Dicing,
            RenderTexture,
            NotSupport
        }
    }
}

