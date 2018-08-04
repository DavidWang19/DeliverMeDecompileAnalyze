namespace Utage
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    [AddComponentMenu("Utage/Lib/UI/RichOutline")]
    public class UguiRichOutline : Outline
    {
        public int copyCount = 0x10;

        public override void ModifyMesh(VertexHelper vh)
        {
            if (this.IsActive())
            {
                List<UIVertex> verts = ListPool<UIVertex>.Get();
                vh.GetUIVertexStream(verts);
                this.ModifyVerticesSub(verts);
                vh.Clear();
                vh.AddUIVertexTriangleStream(verts);
                ListPool<UIVertex>.Release(verts);
            }
        }

        private void ModifyVerticesSub(List<UIVertex> verts)
        {
            int num = 0;
            int count = verts.Count;
            for (int i = 0; i < this.copyCount; i++)
            {
                float num4 = Mathf.Sin((6.283185f * i) / ((float) this.copyCount)) * base.get_effectDistance().x;
                float num5 = Mathf.Cos((6.283185f * i) / ((float) this.copyCount)) * base.get_effectDistance().y;
                base.ApplyShadow(verts, base.get_effectColor(), num, verts.Count, num4, num5);
                num = count;
                count = verts.Count;
            }
        }
    }
}

