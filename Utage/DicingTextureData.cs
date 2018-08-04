namespace Utage
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    [Serializable]
    public class DicingTextureData
    {
        [SerializeField]
        private string atlasName;
        [SerializeField]
        private List<int> cellIndexList = new List<int>();
        [SerializeField]
        private int height;
        [SerializeField]
        private string name;
        [SerializeField]
        private int transparentIndex;
        [NonSerialized]
        private List<QuadVerts> verts;
        [SerializeField]
        private int width;

        public void ForeachVertexList(Rect uvRect, bool skipTransParentCell, DicingTextures textures, Action<Rect, Rect> function)
        {
            <ForeachVertexList>c__AnonStorey1 storey = new <ForeachVertexList>c__AnonStorey1 {
                function = function,
                $this = this
            };
            if ((uvRect.get_width() != 0f) && (uvRect.get_height() != 0f))
            {
                if (uvRect.get_xMin() < 0f)
                {
                    uvRect.set_x(uvRect.get_x() + Mathf.CeilToInt(-uvRect.get_xMin()));
                }
                if (uvRect.get_yMin() < 0f)
                {
                    uvRect.set_y(uvRect.get_y() + Mathf.CeilToInt(-uvRect.get_yMin()));
                }
                bool flag = false;
                if (uvRect.get_width() < 0f)
                {
                    uvRect.set_width(uvRect.get_width() * -1f);
                    flag = true;
                }
                bool flag2 = false;
                if (uvRect.get_height() < 0f)
                {
                    uvRect.set_height(uvRect.get_height() * -1f);
                    flag2 = true;
                }
                storey.scaleX = 1f / uvRect.get_width();
                storey.fipOffsetX = 0f;
                if (flag)
                {
                    storey.scaleX *= -1f;
                    storey.fipOffsetX = this.Width;
                }
                storey.scaleY = 1f / uvRect.get_height();
                storey.fipOffsetY = 0f;
                if (flag2)
                {
                    storey.scaleY *= -1f;
                    storey.fipOffsetY = this.Height;
                }
                float num = uvRect.get_yMin() % 1f;
                float num2 = uvRect.get_yMax() % 1f;
                if (num2 == 0f)
                {
                    num2 = 1f;
                }
                storey.offsetY = 0f;
                bool flag3 = true;
                bool flag4 = false;
                storey.rect = new Rect();
                do
                {
                    <ForeachVertexList>c__AnonStorey2 storey2 = new <ForeachVertexList>c__AnonStorey2 {
                        <>f__ref$1 = storey
                    };
                    storey.rect.set_yMin(!flag3 ? 0f : num);
                    flag4 = ((storey.offsetY + 1f) - storey.rect.get_yMin()) >= uvRect.get_height();
                    storey.rect.set_yMax(!flag4 ? 1f : num2);
                    float num3 = uvRect.get_xMin() % 1f;
                    float num4 = uvRect.get_xMax() % 1f;
                    if (num4 == 0f)
                    {
                        num4 = 1f;
                    }
                    storey2.offsetX = 0f;
                    bool flag5 = true;
                    bool flag6 = false;
                    do
                    {
                        storey.rect.set_xMin(!flag5 ? 0f : num3);
                        flag6 = ((storey2.offsetX + 1f) - storey.rect.get_xMin()) >= uvRect.get_width();
                        storey.rect.set_xMax(!flag6 ? 1f : num4);
                        this.ForeachVertexListSub(storey.rect, skipTransParentCell, textures, new Action<Rect, Rect>(storey2.<>m__0));
                        storey2.offsetX += storey.rect.get_width();
                        flag5 = false;
                    }
                    while (!flag6);
                    storey.offsetY += storey.rect.get_height();
                    flag3 = false;
                }
                while (!flag4);
            }
        }

        public void ForeachVertexList(Rect position, Rect uvRect, bool skipTransParentCell, DicingTextures textures, Action<Rect, Rect> function)
        {
            <ForeachVertexList>c__AnonStorey0 storey;
            storey = new <ForeachVertexList>c__AnonStorey0 {
                position = position,
                function = function,
                scale = new Vector2(storey.position.get_width() / ((float) this.Width), storey.position.get_height() / ((float) this.Height))
            };
            this.ForeachVertexList(uvRect, skipTransParentCell, textures, new Action<Rect, Rect>(storey.<>m__0));
        }

        private void ForeachVertexListSub(Rect uvRect, bool skipTransParentCell, DicingTextures textures, Action<Rect, Rect> function)
        {
            Texture2D texture = textures.GetTexture(this.AtlasName);
            float num = texture.get_width();
            float num2 = texture.get_height();
            List<QuadVerts> list = this.GetVerts(textures);
            Rect rect = new Rect(uvRect.get_x() * this.Width, uvRect.get_y() * this.Height, uvRect.get_width() * this.Width, uvRect.get_height() * this.Height);
            for (int i = 0; i < list.Count; i++)
            {
                QuadVerts verts = list[i];
                if (!skipTransParentCell || !verts.isAllTransparent)
                {
                    float x = verts.v.x;
                    float z = verts.v.z;
                    float y = verts.v.y;
                    float w = verts.v.w;
                    Rect rect2 = verts.uvRect;
                    if ((((x <= rect.get_xMax()) && (y <= rect.get_yMax())) && (z >= rect.get_x())) && (w >= rect.get_y()))
                    {
                        if (x < rect.get_x())
                        {
                            rect2.set_xMin(rect2.get_xMin() + ((rect.get_x() - x) / num));
                            x = rect.get_x();
                        }
                        if (z > rect.get_xMax())
                        {
                            rect2.set_xMax(rect2.get_xMax() + ((rect.get_xMax() - z) / num));
                            z = rect.get_xMax();
                        }
                        if (y < rect.get_y())
                        {
                            rect2.set_yMin(rect2.get_yMin() + ((rect.get_y() - y) / num2));
                            y = rect.get_y();
                        }
                        if (w > rect.get_yMax())
                        {
                            rect2.set_yMax(rect2.get_yMax() + ((rect.get_yMax() - w) / num2));
                            w = rect.get_yMax();
                        }
                        function(new Rect(x, y, z - x, w - y), rect2);
                    }
                }
            }
        }

        internal List<QuadVerts> GetVerts(DicingTextures textures)
        {
            if (this.verts == null)
            {
                this.InitVerts(textures);
            }
            return this.verts;
        }

        private void InitVerts(DicingTextures atlas)
        {
            if (atlas != null)
            {
                this.verts = new List<QuadVerts>();
                int cellSize = atlas.CellSize;
                int num2 = cellSize - (atlas.Padding * 2);
                int num3 = Mathf.CeilToInt((1f * this.Width) / ((float) num2));
                int num4 = Mathf.CeilToInt((1f * this.Height) / ((float) num2));
                int num5 = atlas.GetTexture(this.AtlasName).get_width();
                int num6 = atlas.GetTexture(this.AtlasName).get_height();
                int num7 = Mathf.CeilToInt((1f * num5) / ((float) cellSize));
                int num8 = 0;
                for (int i = 0; i < num4; i++)
                {
                    float num10 = i * num2;
                    float num11 = Mathf.Min(num10 + num2, (float) this.Height);
                    for (int j = 0; j < num3; j++)
                    {
                        QuadVerts item = new QuadVerts();
                        float num13 = j * num2;
                        float num14 = Mathf.Min(num13 + num2, (float) this.Width);
                        item.v = new Vector4(num13, num10, num14, num11);
                        int num15 = this.cellIndexList[num8];
                        item.isAllTransparent = num15 == this.transparentIndex;
                        float num16 = (num15 % num7) * cellSize;
                        float num17 = (num15 / num7) * cellSize;
                        float num18 = (1f * (num16 + atlas.Padding)) / ((float) num5);
                        float num19 = (1f * (num17 + atlas.Padding)) / ((float) num6);
                        float num20 = (1f * (num14 - num13)) / ((float) num5);
                        float num21 = (1f * (num11 - num10)) / ((float) num6);
                        item.uvRect = new Rect(num18, num19, num20, num21);
                        this.verts.Add(item);
                        num8++;
                    }
                }
            }
        }

        public string AtlasName
        {
            get
            {
                return this.atlasName;
            }
        }

        public int Height
        {
            get
            {
                return this.height;
            }
        }

        public string Name
        {
            get
            {
                return this.name;
            }
        }

        public int Width
        {
            get
            {
                return this.width;
            }
        }

        [CompilerGenerated]
        private sealed class <ForeachVertexList>c__AnonStorey0
        {
            internal Action<Rect, Rect> function;
            internal Rect position;
            internal Vector2 scale;

            internal void <>m__0(Rect r1, Rect r2)
            {
                r1.set_xMin(r1.get_xMin() * this.scale.x);
                r1.set_xMax(r1.get_xMax() * this.scale.x);
                r1.set_x(r1.get_x() + this.position.get_x());
                r1.set_yMin(r1.get_yMin() * this.scale.y);
                r1.set_yMax(r1.get_yMax() * this.scale.y);
                r1.set_y(r1.get_y() + this.position.get_y());
                this.function(r1, r2);
            }
        }

        [CompilerGenerated]
        private sealed class <ForeachVertexList>c__AnonStorey1
        {
            internal DicingTextureData $this;
            internal float fipOffsetX;
            internal float fipOffsetY;
            internal Action<Rect, Rect> function;
            internal float offsetY;
            internal Rect rect;
            internal float scaleX;
            internal float scaleY;
        }

        [CompilerGenerated]
        private sealed class <ForeachVertexList>c__AnonStorey2
        {
            internal DicingTextureData.<ForeachVertexList>c__AnonStorey1 <>f__ref$1;
            internal float offsetX;

            internal void <>m__0(Rect r1, Rect r2)
            {
                r1.set_xMin(r1.get_xMin() * this.<>f__ref$1.scaleX);
                r1.set_xMax(r1.get_xMax() * this.<>f__ref$1.scaleX);
                r1.set_x(r1.get_x() + ((((this.offsetX - this.<>f__ref$1.rect.get_xMin()) * this.<>f__ref$1.scaleX) * this.<>f__ref$1.$this.Width) + this.<>f__ref$1.fipOffsetX));
                r1.set_yMin(r1.get_yMin() * this.<>f__ref$1.scaleY);
                r1.set_yMax(r1.get_yMax() * this.<>f__ref$1.scaleY);
                r1.set_y(r1.get_y() + ((((this.<>f__ref$1.offsetY - this.<>f__ref$1.rect.get_yMin()) * this.<>f__ref$1.scaleY) * this.<>f__ref$1.$this.Height) + this.<>f__ref$1.fipOffsetY));
                this.<>f__ref$1.function(r1, r2);
            }
        }

        public class QuadVerts
        {
            public bool isAllTransparent;
            public Rect uvRect;
            public Vector4 v;
        }
    }
}

