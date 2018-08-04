namespace Utage
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    [CreateAssetMenu(menuName="Utage/DicingTextures")]
    public class DicingTextures : ScriptableObject
    {
        [SerializeField, NotEditable]
        private List<Texture2D> atlasTextures = new List<Texture2D>();
        [HelpBox("ImportSetting", HelpBoxAttribute.Type.Info, 0), SerializeField, IntPopup(new int[] { 0x20, 0x40, 0x80, 0x100 })]
        private int cellSize = 0x40;
        [SerializeField, Min(1f)]
        private int padding = 3;
        [SerializeField, NotEditable]
        private List<DicingTextureData> textureDataList = new List<DicingTextureData>();

        internal bool Exists(string pattern)
        {
            <Exists>c__AnonStorey0 storey = new <Exists>c__AnonStorey0 {
                pattern = pattern
            };
            return this.textureDataList.Exists(new Predicate<DicingTextureData>(storey.<>m__0));
        }

        public List<string> GetPattenNameList()
        {
            List<string> list = new List<string>();
            foreach (DicingTextureData data in this.textureDataList)
            {
                list.Add(data.Name);
            }
            return list;
        }

        public Texture2D GetTexture(string name)
        {
            <GetTexture>c__AnonStorey1 storey = new <GetTexture>c__AnonStorey1 {
                name = name
            };
            return this.atlasTextures.Find(new Predicate<Texture2D>(storey.<>m__0));
        }

        internal DicingTextureData GetTextureData(string pattern)
        {
            foreach (DicingTextureData data in this.textureDataList)
            {
                if (data.Name == pattern)
                {
                    return data;
                }
            }
            return null;
        }

        internal List<DicingTextureData> GetTextureDataList(string topDirectory)
        {
            if (string.IsNullOrEmpty(topDirectory))
            {
                return this.TextureDataList;
            }
            if (!topDirectory.EndsWith("/"))
            {
                topDirectory = topDirectory + "/";
            }
            List<DicingTextureData> list = new List<DicingTextureData>();
            foreach (DicingTextureData data in this.TextureDataList)
            {
                if (data.Name.StartsWith(topDirectory))
                {
                    list.Add(data);
                }
            }
            return list;
        }

        public List<DicingTextureData.QuadVerts> GetVerts(DicingTextureData data)
        {
            return data.GetVerts(this);
        }

        public List<Texture2D> AtlasTextures
        {
            get
            {
                return this.atlasTextures;
            }
        }

        public int CellSize
        {
            get
            {
                return this.cellSize;
            }
            set
            {
                this.cellSize = value;
            }
        }

        public int Padding
        {
            get
            {
                return this.padding;
            }
            set
            {
                this.padding = value;
            }
        }

        public List<DicingTextureData> TextureDataList
        {
            get
            {
                return this.textureDataList;
            }
        }

        [CompilerGenerated]
        private sealed class <Exists>c__AnonStorey0
        {
            internal string pattern;

            internal bool <>m__0(DicingTextureData x)
            {
                return (x.Name == this.pattern);
            }
        }

        [CompilerGenerated]
        private sealed class <GetTexture>c__AnonStorey1
        {
            internal string name;

            internal bool <>m__0(Texture2D x)
            {
                return ((x != null) && (x.get_name() == this.name));
            }
        }
    }
}

