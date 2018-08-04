namespace Utage
{
    using System;
    using UnityEngine;

    [AddComponentMenu("Utage/ADV/Extra/VideoLoadPathChanger")]
    public class AdvVideoLoadPathChanger : MonoBehaviour
    {
        [SerializeField]
        private string rootPath = string.Empty;

        private void Awake()
        {
            CustomLoadManager customLoadManager = AssetFileManager.GetCustomLoadManager();
            customLoadManager.OnFindAsset = (Utage.CustomLoadManager.FindAsset) Delegate.Combine(customLoadManager.OnFindAsset, new Utage.CustomLoadManager.FindAsset(this.FindAsset));
        }

        private void FindAsset(AssetFileManager mangager, AssetFileInfo fileInfo, IAssetFileSettingData settingData, ref AssetFileBase asset)
        {
            if (this.IsVideoType(fileInfo, settingData))
            {
                asset = new AdvLocalVideoFile(this, mangager, fileInfo, settingData);
            }
        }

        private bool IsVideoType(AssetFileInfo fileInfo, IAssetFileSettingData settingData)
        {
            if (fileInfo.FileType != AssetFileType.UnityObject)
            {
                return false;
            }
            if (settingData is AdvCommandSetting)
            {
                AdvCommandSetting setting = settingData as AdvCommandSetting;
                return (setting.Command is AdvCommandVideo);
            }
            AdvGraphicInfo info = settingData as AdvGraphicInfo;
            return ((info != null) && (info.FileType == "Video"));
        }

        public string RootPath
        {
            get
            {
                return this.rootPath;
            }
        }
    }
}

