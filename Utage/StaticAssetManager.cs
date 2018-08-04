namespace Utage
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    [AddComponentMenu("Utage/Lib/File/StaticAssetManager")]
    public class StaticAssetManager : MonoBehaviour
    {
        [SerializeField]
        private List<StaticAsset> assets = new List<StaticAsset>();

        public bool Contains(Object asset)
        {
            foreach (StaticAsset asset2 in this.Assets)
            {
                if (asset2.Asset == asset)
                {
                    return true;
                }
            }
            return false;
        }

        public AssetFileBase FindAssetFile(AssetFileManager mangager, AssetFileInfo fileInfo, IAssetFileSettingData settingData)
        {
            <FindAssetFile>c__AnonStorey0 storey = new <FindAssetFile>c__AnonStorey0();
            if (this.Assets == null)
            {
                return null;
            }
            storey.assetName = FilePathUtil.GetFileNameWithoutExtension(fileInfo.FileName);
            StaticAsset asset = this.Assets.Find(new Predicate<StaticAsset>(storey.<>m__0));
            if (asset == null)
            {
                return null;
            }
            return new StaticAssetFile(asset, mangager, fileInfo, settingData);
        }

        private List<StaticAsset> Assets
        {
            get
            {
                return this.assets;
            }
        }

        [CompilerGenerated]
        private sealed class <FindAssetFile>c__AnonStorey0
        {
            internal string assetName;

            internal bool <>m__0(StaticAsset x)
            {
                return (x.Asset.get_name() == this.assetName);
            }
        }
    }
}

