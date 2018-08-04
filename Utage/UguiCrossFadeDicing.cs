namespace Utage
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    [AddComponentMenu("Utage/Lib/UI/CrossFadeDicing")]
    public class UguiCrossFadeDicing : UguiCrossFadeRawImage
    {
        private DicingTextureData fadePatternData;

        internal void CrossFade(DicingTextureData fadePatternData, Texture fadeTexture, float time, Action onComplete)
        {
            this.fadePatternData = fadePatternData;
            this.Target.SetAllDirty();
            base.CrossFade(fadeTexture, time, onComplete);
        }

        public override void RebuildVertex(VertexHelper vh)
        {
            if (this.fadePatternData != null)
            {
                vh.Clear();
                Rect pixelAdjustedRect = this.Target.GetPixelAdjustedRect();
                Color color = this.Target.get_color();
                DicingImage target = this.Target as DicingImage;
                float num = pixelAdjustedRect.get_width() / ((float) this.fadePatternData.Width);
                float num2 = pixelAdjustedRect.get_height() / ((float) this.fadePatternData.Height);
                int num3 = 0;
                List<DicingTextureData.QuadVerts> list = target.GetVerts(target.PatternData);
                List<DicingTextureData.QuadVerts> list2 = target.GetVerts(this.fadePatternData);
                int count = list.Count;
                if (count != list2.Count)
                {
                    count = Mathf.Min(count, list2.Count);
                    Debug.LogError(string.Format("Not equal texture size {0} and {1}", target.PatternData.Name, this.fadePatternData.Name));
                }
                for (int i = 0; i < count; i++)
                {
                    DicingTextureData.QuadVerts verts = list[i];
                    DicingTextureData.QuadVerts verts2 = list2[i];
                    if ((!target.SkipTransParentCell || !verts.isAllTransparent) || !verts2.isAllTransparent)
                    {
                        Vector4 vector = new Vector4(pixelAdjustedRect.get_x() + (num * verts.v.x), pixelAdjustedRect.get_y() + (num2 * verts.v.y), pixelAdjustedRect.get_x() + (num * verts.v.z), pixelAdjustedRect.get_y() + (num2 * verts.v.w));
                        Rect uvRect = verts.uvRect;
                        Rect rect3 = verts2.uvRect;
                        float introduced15 = uvRect.get_xMin();
                        float introduced16 = rect3.get_xMin();
                        vh.AddVert(new Vector3(vector.x, vector.y), color, new Vector2(introduced15, uvRect.get_yMin()), new Vector2(introduced16, rect3.get_yMin()), Vector3.get_zero(), Vector4.get_zero());
                        float introduced17 = uvRect.get_xMin();
                        float introduced18 = rect3.get_xMin();
                        vh.AddVert(new Vector3(vector.x, vector.w), color, new Vector2(introduced17, uvRect.get_yMax()), new Vector2(introduced18, rect3.get_yMax()), Vector3.get_zero(), Vector4.get_zero());
                        float introduced19 = uvRect.get_xMax();
                        float introduced20 = rect3.get_xMax();
                        vh.AddVert(new Vector3(vector.z, vector.w), color, new Vector2(introduced19, uvRect.get_yMax()), new Vector2(introduced20, rect3.get_yMax()), Vector3.get_zero(), Vector4.get_zero());
                        float introduced21 = uvRect.get_xMax();
                        float introduced22 = rect3.get_xMax();
                        vh.AddVert(new Vector3(vector.z, vector.y), color, new Vector2(introduced21, uvRect.get_yMin()), new Vector2(introduced22, rect3.get_yMin()), Vector3.get_zero(), Vector4.get_zero());
                        vh.AddTriangle(num3, num3 + 1, num3 + 2);
                        vh.AddTriangle(num3 + 2, num3 + 3, num3);
                        num3 += 4;
                    }
                }
            }
        }

        public override Graphic Target
        {
            get
            {
                if (base.target == null)
                {
                }
                return (base.target = base.GetComponent<DicingImage>());
            }
        }
    }
}

