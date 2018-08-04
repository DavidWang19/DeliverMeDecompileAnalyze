namespace Utage
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class AdvRenderTextureSetting
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Vector3 <RenderTextureOffset>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Vector2 <RenderTextureSize>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private AdvRenderTextureMode <RenderTextureType>k__BackingField;

        public Material GetRenderMaterialIfEnable(Material material)
        {
            if (!this.EnableRenderTexture || ((material != null) && (material.get_shader() == ShaderManager.RenderTexture)))
            {
                return material;
            }
            return new Material(ShaderManager.RenderTexture);
        }

        public void Parse(StringGridRow row)
        {
            this.RenderTextureType = AdvParser.ParseCellOptional<AdvRenderTextureMode>(row, AdvColumnName.RenderTexture, AdvRenderTextureMode.None);
            if (this.RenderTextureType != AdvRenderTextureMode.None)
            {
                try
                {
                    float[] numArray = row.ParseCellArray<float>(AdvColumnName.RenderRect.QuickToString());
                    if (numArray.Length != 4)
                    {
                        Debug.LogError(row.ToErrorString("IconRect. Array size is not 4"));
                    }
                    else
                    {
                        this.RenderTextureOffset = new Vector3(-numArray[0], -numArray[1], 1000f);
                        this.RenderTextureSize = new Vector2(numArray[2], numArray[3]);
                    }
                }
                catch (Exception)
                {
                }
            }
        }

        public bool EnableRenderTexture
        {
            get
            {
                return (this.RenderTextureType != AdvRenderTextureMode.None);
            }
        }

        public Vector3 RenderTextureOffset { get; protected set; }

        public Vector2 RenderTextureSize { get; protected set; }

        public AdvRenderTextureMode RenderTextureType { get; protected set; }
    }
}

