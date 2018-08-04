namespace Utage
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    [AddComponentMenu("Utage/Lib/File/CustomLoadManager")]
    public class CustomLoadManager : MonoBehaviour
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private FindAsset <OnFindAsset>k__BackingField;

        public AssetFileBase Find(AssetFileManager mangager, AssetFileInfo fileInfo, IAssetFileSettingData settingData)
        {
            if (this.OnFindAsset != null)
            {
                AssetFileBase asset = null;
                this.OnFindAsset(mangager, fileInfo, settingData, ref asset);
                if (asset != null)
                {
                    return asset;
                }
            }
            return null;
        }

        public FindAsset OnFindAsset { get; set; }

        public delegate void FindAsset(AssetFileManager mangager, AssetFileInfo fileInfo, IAssetFileSettingData settingData, ref AssetFileBase asset);
    }
}

