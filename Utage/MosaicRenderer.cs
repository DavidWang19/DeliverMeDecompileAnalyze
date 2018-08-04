namespace Utage
{
    using System;
    using UnityEngine;
    using UnityEngine.Rendering;

    [AddComponentMenu("Utage/Lib/Effect/MosaicRenderer")]
    public class MosaicRenderer : MonoBehaviour
    {
        public bool autoScale;
        [Hide("IgnoreAutoScale")]
        public int gameScreenHeight = 600;
        [Hide("IgnoreAutoScale")]
        public int gameScreenWidth = 800;
        [Range(1f, 20f)]
        public float mosaicSize = 10f;
        public string sortingLayerName = string.Empty;
        public int sortingOrder = 0x3e8;
        public CompareFunction zTest = 8;

        private void LateUpdate()
        {
            Renderer component = base.GetComponent<Renderer>();
            if (component != null)
            {
                component.set_sortingLayerName(this.sortingLayerName);
                component.set_sortingOrder(this.sortingOrder);
                component.set_enabled(true);
                float num = 1f;
                if (this.autoScale)
                {
                    num = Mathf.Min((1f * Screen.get_width()) / ((float) this.gameScreenWidth), (1f * Screen.get_height()) / ((float) this.gameScreenHeight));
                }
                component.get_material().SetFloat("_Size", (float) Mathf.CeilToInt(this.mosaicSize * num));
                component.get_material().SetInt("_ZTest", this.zTest);
            }
        }

        private void OnValidate()
        {
            Renderer component = base.GetComponent<Renderer>();
            if (component != null)
            {
                component.set_sortingLayerName(this.sortingLayerName);
                component.set_sortingOrder(this.sortingOrder);
            }
        }

        public bool IgnoreAutoScale
        {
            get
            {
                return !this.autoScale;
            }
        }
    }
}

