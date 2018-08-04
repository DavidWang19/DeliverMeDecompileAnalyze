using System;
using UnityEngine;
using Utage;

[ExecuteInEditMode, AddComponentMenu("Utage/Lib/Other/BootCustomProjectSetting")]
public class BootCustomProjectSetting : MonoBehaviour
{
    [SerializeField]
    private Utage.CustomProjectSetting customProjectSetting;

    public Utage.CustomProjectSetting CustomProjectSetting
    {
        get
        {
            return this.customProjectSetting;
        }
        set
        {
            this.customProjectSetting = value;
        }
    }
}

