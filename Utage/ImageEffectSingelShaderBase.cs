namespace Utage
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    [RequireComponent(typeof(Camera))]
    public abstract class ImageEffectSingelShaderBase : ImageEffectBase
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private UnityEngine.Material <Material>k__BackingField;
        [SerializeField]
        private UnityEngine.Shader shader;
        private UnityEngine.Shader tmpShader;

        protected ImageEffectSingelShaderBase()
        {
        }

        protected override bool CheckShaderAndCreateMaterial()
        {
            UnityEngine.Material material;
            bool flag = base.TryCheckShaderAndCreateMaterialSub(this.Shader, this.Material, out material);
            this.Material = material;
            return flag;
        }

        protected override void RestoreObjectsOnJsonRead()
        {
            this.shader = this.tmpShader;
        }

        public override void SetShaders(params UnityEngine.Shader[] shadres)
        {
            if (shadres.Length > 0)
            {
                this.Shader = shadres[0];
            }
        }

        protected override void StoreObjectsOnJsonRead()
        {
            this.tmpShader = this.shader;
        }

        protected UnityEngine.Material Material { get; set; }

        public UnityEngine.Shader Shader
        {
            get
            {
                return this.shader;
            }
            set
            {
                this.shader = value;
            }
        }
    }
}

