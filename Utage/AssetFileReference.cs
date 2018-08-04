namespace Utage
{
    using System;
    using UnityEngine;

    [AddComponentMenu("Utage/Lib/File/AssetFileReference")]
    public class AssetFileReference : MonoBehaviour
    {
        private AssetFile file;

        public void Init(AssetFile file)
        {
            this.file = file;
            this.file.Use(this);
        }

        private void OnDestroy()
        {
            this.file.Unuse(this);
        }

        public AssetFile File
        {
            get
            {
                return this.file;
            }
        }
    }
}

