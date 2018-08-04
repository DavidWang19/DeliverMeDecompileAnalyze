namespace Utage
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.InteropServices;
    using UnityEngine;
    using UtageExtensions;

    [RequireComponent(typeof(Camera))]
    public abstract class ImageEffectBase : MonoBehaviour
    {
        private List<Material> createdMaterials = new List<Material>();
        private const int Version = 0;

        protected ImageEffectBase()
        {
        }

        protected virtual bool CheckResources()
        {
            if (this.CheckSupport() && this.CheckShaderAndCreateMaterial())
            {
                return true;
            }
            base.set_enabled(false);
            Debug.LogWarning("The image effect " + this.ToString() + " has been disabled as it's not supported on the current platform.");
            return false;
        }

        protected abstract bool CheckShaderAndCreateMaterial();
        protected bool CheckSupport()
        {
            return this.CheckSupport(this.NeedRenderTexture, this.NeedDepth, this.NeedHdr);
        }

        protected bool CheckSupport(bool needRenderTexture, bool needDepth, bool needHdr)
        {
            if (!this.CheckSupportSub(needRenderTexture, needDepth, needHdr))
            {
                return false;
            }
            if (needDepth)
            {
                Camera component = base.GetComponent<Camera>();
                component.set_depthTextureMode(component.get_depthTextureMode() | 1);
            }
            return true;
        }

        protected bool CheckSupportSub(bool needRenderTexture, bool needDepth, bool needHdr)
        {
            if (!ImageEffectUtil.SupportsImageEffects)
            {
                return false;
            }
            if (needRenderTexture && !ImageEffectUtil.SupportsRenderTextures)
            {
                return false;
            }
            if (needDepth && !ImageEffectUtil.SupportsDepth)
            {
                return false;
            }
            if (needHdr && !ImageEffectUtil.SupportsHDRTextures)
            {
                return false;
            }
            return true;
        }

        private void ClearCreatedMaterials()
        {
            foreach (Material material in this.createdMaterials)
            {
                Object.Destroy(material);
            }
            this.createdMaterials.Clear();
        }

        protected virtual void OnDestroy()
        {
            this.ClearCreatedMaterials();
        }

        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            if (!this.CheckResources())
            {
                Graphics.Blit(source, destination);
            }
            else
            {
                this.RenderImage(source, destination);
            }
        }

        public virtual void Read(BinaryReader reader)
        {
            int num = reader.ReadInt32();
            if ((num < 0) || (num > 0))
            {
                object[] args = new object[] { num };
                Debug.LogError(LanguageErrorMsg.LocalizeTextFormat(ErrorMsg.UnknownVersion, args));
            }
            else
            {
                this.StoreObjectsOnJsonRead();
                base.set_enabled(reader.ReadBoolean());
                reader.ReadJson(this);
                this.RestoreObjectsOnJsonRead();
            }
        }

        protected abstract void RenderImage(RenderTexture source, RenderTexture destination);
        protected abstract void RestoreObjectsOnJsonRead();
        public abstract void SetShaders(params Shader[] shadres);
        private void Start()
        {
            this.CheckResources();
        }

        protected abstract void StoreObjectsOnJsonRead();
        protected bool TryCheckShaderAndCreateMaterialSub(Shader s, Material m2Create, out Material mat)
        {
            mat = null;
            if (s == null)
            {
                Debug.Log("Missing shader in " + this.ToString());
                return false;
            }
            if (!s.get_isSupported())
            {
                Debug.Log("The shader " + s.ToString() + " on effect " + this.ToString() + " is not supported on this platform!");
                return false;
            }
            if ((m2Create != null) && (m2Create.get_shader() == s))
            {
                mat = m2Create;
                return true;
            }
            m2Create = new Material(s);
            this.createdMaterials.Add(m2Create);
            m2Create.set_hideFlags(0x34);
            mat = m2Create;
            return true;
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write(0);
            writer.Write(base.get_enabled());
            writer.WriteJson(this);
        }

        protected virtual bool NeedDepth
        {
            get
            {
                return false;
            }
        }

        protected virtual bool NeedHdr
        {
            get
            {
                return false;
            }
        }

        protected virtual bool NeedRenderTexture
        {
            get
            {
                return false;
            }
        }
    }
}

