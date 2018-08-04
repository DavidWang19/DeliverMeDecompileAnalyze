using System;
using UnityEngine;
using UnityEngine.UI;
using Utage;

[AddComponentMenu("Utage/TemplateUI/SkipButtonState")]
public class UtageUguiSkipButtonState : MonoBehaviour
{
    [SerializeField]
    private AdvEngine engine;
    public Toggle target;

    private void Update()
    {
        if (this.target != null)
        {
            this.target.set_interactable(this.Engine.Page.EnableSkip());
        }
    }

    public AdvEngine Engine
    {
        get
        {
            if (this.engine == null)
            {
            }
            return (this.engine = Object.FindObjectOfType<AdvEngine>());
        }
    }
}

