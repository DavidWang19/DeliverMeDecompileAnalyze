using System;
using UnityEngine;

[AddComponentMenu("Utage/TemplateUI/ConfigTaggedMasterVolume")]
public class UtageUguiConfigTaggedMasterVolume : MonoBehaviour
{
    public UtageUguiConfig config;
    public string volumeTag = string.Empty;

    public void OnValugeChanged(float value)
    {
        if (!string.IsNullOrEmpty(this.volumeTag))
        {
            this.config.OnValugeChangedTaggedMasterVolume(this.volumeTag, value);
        }
    }
}

