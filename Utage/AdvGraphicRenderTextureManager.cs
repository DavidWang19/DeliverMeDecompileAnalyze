namespace Utage
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using UtageExtensions;

    [AddComponentMenu("Utage/ADV/GraphicRenderTextureManager")]
    public class AdvGraphicRenderTextureManager : MonoBehaviour
    {
        public float offset = 10000f;
        private List<AdvRenderTextureSpace> spaceList = new List<AdvRenderTextureSpace>();

        internal AdvRenderTextureSpace CreateSpace()
        {
            AdvRenderTextureSpace item = base.get_transform().AddChildGameObjectComponent<AdvRenderTextureSpace>("RenderTextureSpace");
            int num = 0;
            while (num < this.spaceList.Count)
            {
                if (this.spaceList[num] == null)
                {
                    this.spaceList[num] = item;
                    break;
                }
                num++;
            }
            if (num >= this.spaceList.Count)
            {
                this.spaceList.Add(item);
            }
            item.get_transform().set_localPosition(new Vector3(0f, (num + 1) * this.offset, 0f));
            return item;
        }
    }
}

