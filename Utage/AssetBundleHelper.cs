namespace Utage
{
    using System;
    using UnityEngine;

    public class AssetBundleHelper
    {
        public static AssetBundleTargetFlags RuntimeAssetBundleTarget()
        {
            return RuntimePlatformToBuildTargetFlag(Application.get_platform());
        }

        public static AssetBundleTargetFlags RuntimePlatformToBuildTargetFlag(RuntimePlatform platform)
        {
            switch (platform)
            {
                case 8:
                    return AssetBundleTargetFlags.iOS;

                case 11:
                    return AssetBundleTargetFlags.Android;

                case 2:
                    return AssetBundleTargetFlags.Windows;

                case 1:
                    return AssetBundleTargetFlags.OSX;

                case 0x11:
                    return AssetBundleTargetFlags.WebGL;
            }
            Debug.LogError("Not support " + platform.ToString());
            return 0;
        }
    }
}

