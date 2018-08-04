namespace Utage
{
    using System;
    using UnityEngine;

    public interface AssetFile
    {
        void AddReferenceComponent(GameObject go);
        void Unuse(object obj);
        void Use(object obj);

        string FileName { get; }

        AssetFileType FileType { get; }

        bool IsLoadEnd { get; }

        bool IsLoadError { get; }

        string LoadErrorMsg { get; }

        IAssetFileSettingData SettingData { get; }

        AudioClip Sound { get; }

        TextAsset Text { get; }

        Texture2D Texture { get; }

        Object UnityObject { get; }
    }
}

