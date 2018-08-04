namespace Utage
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public static class ImageEffectUtil
    {
        private static List<ImageEffectPattern> patterns;

        static ImageEffectUtil()
        {
            List<ImageEffectPattern> list = new List<ImageEffectPattern>();
            ImageEffectType colorFade = ImageEffectType.ColorFade;
            Shader[] shaders = new Shader[] { Shader.Find(ShaderManager.ColorFade) };
            list.Add(new ImageEffectPattern(colorFade.ToString(), typeof(ColorFade), shaders));
            ImageEffectType bloom = ImageEffectType.Bloom;
            Shader[] shaderArray2 = new Shader[] { Shader.Find(ShaderManager.BloomName) };
            list.Add(new ImageEffectPattern(bloom.ToString(), typeof(Bloom), shaderArray2));
            ImageEffectType blur = ImageEffectType.Blur;
            Shader[] shaderArray3 = new Shader[] { Shader.Find(ShaderManager.BlurName) };
            list.Add(new ImageEffectPattern(blur.ToString(), typeof(Blur), shaderArray3));
            ImageEffectType mosaic = ImageEffectType.Mosaic;
            Shader[] shaderArray4 = new Shader[] { Shader.Find(ShaderManager.MosaicName) };
            list.Add(new ImageEffectPattern(mosaic.ToString(), typeof(Mosaic), shaderArray4));
            ImageEffectType grayScale = ImageEffectType.GrayScale;
            Shader[] shaderArray5 = new Shader[] { Shader.Find(ShaderManager.GrayScaleName) };
            list.Add(new ImageEffectPattern(grayScale.ToString(), typeof(Grayscale), shaderArray5));
            ImageEffectType motionBlur = ImageEffectType.MotionBlur;
            Shader[] shaderArray6 = new Shader[] { Shader.Find(ShaderManager.MotionBlurName) };
            list.Add(new ImageEffectPattern(motionBlur.ToString(), typeof(MotionBlur), shaderArray6));
            ImageEffectType screenOverlay = ImageEffectType.ScreenOverlay;
            Shader[] shaderArray7 = new Shader[] { Shader.Find(ShaderManager.BlendModesOverlayName) };
            list.Add(new ImageEffectPattern(screenOverlay.ToString(), typeof(ScreenOverlay), shaderArray7));
            ImageEffectType sepia = ImageEffectType.Sepia;
            Shader[] shaderArray8 = new Shader[] { Shader.Find(ShaderManager.SepiatoneName) };
            list.Add(new ImageEffectPattern(sepia.ToString(), typeof(SepiaTone), shaderArray8));
            ImageEffectType negaPosi = ImageEffectType.NegaPosi;
            Shader[] shaderArray9 = new Shader[] { Shader.Find(ShaderManager.NegaPosiName) };
            list.Add(new ImageEffectPattern(negaPosi.ToString(), typeof(NegaPosi), shaderArray9));
            ImageEffectType fishEye = ImageEffectType.FishEye;
            Shader[] shaderArray10 = new Shader[] { Shader.Find(ShaderManager.FisheyeName) };
            list.Add(new ImageEffectPattern(fishEye.ToString(), typeof(FishEye), shaderArray10));
            ImageEffectType twirl = ImageEffectType.Twirl;
            Shader[] shaderArray11 = new Shader[] { Shader.Find(ShaderManager.TwirlName) };
            list.Add(new ImageEffectPattern(twirl.ToString(), typeof(Twirl), shaderArray11));
            ImageEffectType vortex = ImageEffectType.Vortex;
            Shader[] shaderArray12 = new Shader[] { Shader.Find(ShaderManager.VortexName) };
            list.Add(new ImageEffectPattern(vortex.ToString(), typeof(Vortex), shaderArray12));
            patterns = list;
        }

        public static void RenderDistortion(Material material, RenderTexture source, RenderTexture destination, float angle, Vector2 center, Vector2 radius)
        {
            if (source.get_texelSize().y < 0f)
            {
                center.y = 1f - center.y;
                angle = -angle;
            }
            Matrix4x4 matrixx = Matrix4x4.TRS(Vector3.get_zero(), Quaternion.Euler(0f, 0f, angle), Vector3.get_one());
            material.SetMatrix("_RotationMatrix", matrixx);
            material.SetVector("_CenterRadius", new Vector4(center.x, center.y, radius.x, radius.y));
            material.SetFloat("_Angle", angle * 0.01745329f);
            Graphics.Blit(source, destination, material);
        }

        internal static string ToImageEffectType(Type ComponentType)
        {
            <ToImageEffectType>c__AnonStorey1 storey = new <ToImageEffectType>c__AnonStorey1 {
                ComponentType = ComponentType
            };
            ImageEffectPattern pattern = patterns.Find(new Predicate<ImageEffectPattern>(storey.<>m__0));
            if (pattern == null)
            {
                return string.Empty;
            }
            return pattern.type;
        }

        internal static bool TryGetComonentCreateIfMissing(string type, out ImageEffectBase component, out bool alreadyEnabled, GameObject target)
        {
            Type type2;
            Shader[] shaderArray;
            alreadyEnabled = false;
            if (!TryParse(type, out type2, out shaderArray))
            {
                Debug.LogError(type + " is not find in Image effect patterns");
                component = null;
                return false;
            }
            component = target.GetComponent(type2) as ImageEffectBase;
            if (component == null)
            {
                component = target.get_gameObject().AddComponent(type2) as ImageEffectBase;
                component.SetShaders(shaderArray);
            }
            else
            {
                alreadyEnabled = component.get_enabled();
            }
            return true;
        }

        internal static bool TryParse(string type, out Type ComponentType, out Shader[] Shaders)
        {
            <TryParse>c__AnonStorey0 storey = new <TryParse>c__AnonStorey0 {
                type = type
            };
            ImageEffectPattern pattern = patterns.Find(new Predicate<ImageEffectPattern>(storey.<>m__0));
            if (pattern == null)
            {
                ComponentType = null;
                Shaders = null;
                return false;
            }
            ComponentType = pattern.componentType;
            Shaders = pattern.shaders;
            return true;
        }

        public static bool SupportDX11
        {
            get
            {
                return ((SystemInfo.get_graphicsShaderLevel() >= 50) && SystemInfo.get_supportsComputeShaders());
            }
        }

        public static bool SupportsDepth
        {
            get
            {
                return SystemInfo.SupportsRenderTextureFormat(1);
            }
        }

        public static bool SupportsHDRTextures
        {
            get
            {
                return SystemInfo.SupportsRenderTextureFormat(2);
            }
        }

        public static bool SupportsImageEffects
        {
            get
            {
                return SystemInfo.get_supportsImageEffects();
            }
        }

        public static bool SupportsRenderTextures
        {
            get
            {
                return true;
            }
        }

        [CompilerGenerated]
        private sealed class <ToImageEffectType>c__AnonStorey1
        {
            internal Type ComponentType;

            internal bool <>m__0(ImageEffectUtil.ImageEffectPattern x)
            {
                return (x.componentType == this.ComponentType);
            }
        }

        [CompilerGenerated]
        private sealed class <TryParse>c__AnonStorey0
        {
            internal string type;

            internal bool <>m__0(ImageEffectUtil.ImageEffectPattern x)
            {
                return (x.type == this.type);
            }
        }

        private class ImageEffectPattern
        {
            public Type componentType;
            public Shader[] shaders;
            public string type;

            internal ImageEffectPattern(string type, Type componentType, Shader[] shaders)
            {
                this.type = type;
                this.componentType = componentType;
                this.shaders = shaders;
            }
        }
    }
}

