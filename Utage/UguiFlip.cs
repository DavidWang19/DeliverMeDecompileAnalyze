namespace Utage
{
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    [AddComponentMenu("Utage/Lib/UI/Flip")]
    public class UguiFlip : BaseMeshEffect
    {
        [SerializeField]
        private bool flipX;
        [SerializeField]
        private bool flipY;

        public override void ModifyMesh(VertexHelper vh)
        {
            if (base.get_enabled() && (this.FlipX || this.FlipY))
            {
                Rect rect = base.get_graphic().get_rectTransform().get_rect();
                Vector2 vector = base.get_graphic().get_rectTransform().get_pivot();
                float num = (-(vector.x - 0.5f) * rect.get_width()) * 2f;
                float num2 = (-(vector.y - 0.5f) * rect.get_height()) * 2f;
                UIVertex vertex = new UIVertex();
                for (int i = 0; i < vh.get_currentVertCount(); i++)
                {
                    vh.PopulateUIVertex(ref vertex, i);
                    if (this.FlipX)
                    {
                        vertex.position.x = -vertex.position.x + num;
                    }
                    if (this.FlipY)
                    {
                        vertex.position.y = -vertex.position.y + num2;
                    }
                    vh.SetUIVertex(vertex, i);
                }
            }
        }

        public bool FlipX
        {
            get
            {
                return this.flipX;
            }
            set
            {
                this.flipX = value;
                base.get_graphic().SetVerticesDirty();
            }
        }

        public bool FlipY
        {
            get
            {
                return this.flipY;
            }
            set
            {
                this.flipY = value;
                base.get_graphic().SetVerticesDirty();
            }
        }
    }
}

