using System;
using UnityEngine;
using Utage;

[AddComponentMenu("Utage/TemplateUI/Title")]
public class UtageUguiTitle : UguiView
{
    public UtageUguiConfig config;
    public UtageUguiLoadWait download;
    public GameObject downloadButton;
    public UtageUguiGallery gallery;
    public UtageUguiSaveLoad load;
    public UtageUguiMainGame mainGame;
    [SerializeField]
    private AdvEngineStarter starter;

    private void OnCloseLoadChapter(string startLabel)
    {
        this.download.onClose.RemoveAllListeners();
        this.Close();
        this.mainGame.OpenStartLabel(startLabel);
    }

    private void OnOpen()
    {
        if (this.downloadButton != null)
        {
            this.downloadButton.SetActive(false);
        }
    }

    public void OnTapConfig()
    {
        this.Close();
        this.config.Open(this);
    }

    public void OnTapDownLoad()
    {
        this.Close();
        this.download.Open(this);
    }

    public void OnTapGallery()
    {
        this.Close();
        this.gallery.Open(this);
    }

    public void OnTapLoad()
    {
        this.Close();
        this.load.OpenLoad(this);
    }

    public void OnTapStart()
    {
        this.Close();
        this.mainGame.OpenStartGame();
    }

    public void OnTapStartLabel(string label)
    {
        this.Close();
        this.mainGame.OpenStartLabel(label);
    }

    public AdvEngineStarter Starter
    {
        get
        {
            if (this.starter == null)
            {
            }
            return (this.starter = Object.FindObjectOfType<AdvEngineStarter>());
        }
    }
}

