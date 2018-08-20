using System;
using UnityEngine;
using Utage;

[AddComponentMenu("Utage/TemplateUI/Gallery")]
public class UtageUguiGallery : UguiView
{
    [SerializeField]
    private AdvEngine engine;
    private int tabIndex = -1;
    public UguiView[] views;

    private void OnOpen()
    {
        this.Engine.SoundManager.StopAll(0.2f);
        if (this.tabIndex >= 0)
        {
            this.views[this.tabIndex].ToggleOpen(true);
        }
    }

    public void OnTabIndexChanged(int index)
    {
        if (index >= this.views.Length)
        {
            Debug.LogError("index < views.Length");
        }
        else
        {
            for (int i = 0; i < this.views.Length; i++)
            {
                if (i != index)
                {
                    this.views[i].ToggleOpen(false);
                }
            }
            this.views[index].ToggleOpen(true);
            this.tabIndex = index;
        }
    }

    public void Sleep()
    {
        base.get_gameObject().SetActive(false);
    }

    public void WakeUp()
    {
        base.get_gameObject().SetActive(true);
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

