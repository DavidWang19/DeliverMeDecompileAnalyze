using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Serialization;
using Utage;

[AddComponentMenu("Utage/TemplateUI/SceneGallery")]
public class UtageUguiSceneGallery : UguiView
{
    [FormerlySerializedAs("categoryGirdPage")]
    public UguiCategoryGridPage categoryGridPage;
    [SerializeField]
    private AdvEngine engine;
    public UtageUguiGallery gallery;
    private bool isInit;
    private List<AdvSceneGallerySettingData> itemDataList = new List<AdvSceneGallerySettingData>();
    public UtageUguiMainGame mainGame;

    [DebuggerHidden]
    private IEnumerator CoWaitOpen()
    {
        return new <CoWaitOpen>c__Iterator0 { $this = this };
    }

    private void CreateItem(GameObject go, int index)
    {
        AdvSceneGallerySettingData data = this.itemDataList[index];
        go.GetComponent<UtageUguiSceneGalleryItem>().Init(data, new Action<UtageUguiSceneGalleryItem>(this.OnTap), this.Engine.SystemSaveData);
    }

    private void OnClose()
    {
        this.categoryGridPage.Clear();
    }

    private void OnEnable()
    {
        this.OnClose();
        this.OnOpen();
    }

    private void OnOpen()
    {
        this.ChangeBgm();
        base.StartCoroutine(this.CoWaitOpen());
    }

    private void OnTap(UtageUguiSceneGalleryItem item)
    {
        this.gallery.Close();
        this.mainGame.OpenSceneGallery(item.Data.ScenarioLabel);
    }

    private void OpenCurrentCategory(UguiCategoryGridPage categoryGridPage)
    {
        this.itemDataList = this.Engine.DataManager.SettingDataManager.SceneGallerySetting.CreateGalleryDataList(categoryGridPage.CurrentCategory);
        categoryGridPage.OpenCurrentCategory(this.itemDataList.Count, new Action<GameObject, int>(this.CreateItem));
    }

    private void Update()
    {
        if (this.isInit && InputUtil.IsMouseRightButtonDown())
        {
            this.Gallery.Back();
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

    public UtageUguiGallery Gallery
    {
        get
        {
            if (this.gallery == null)
            {
            }
            return (this.gallery = Object.FindObjectOfType<UtageUguiGallery>());
        }
    }

    [CompilerGenerated]
    private sealed class <CoWaitOpen>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal bool $disposing;
        internal int $PC;
        internal UtageUguiSceneGallery $this;

        [DebuggerHidden]
        public void Dispose()
        {
            this.$disposing = true;
            this.$PC = -1;
        }

        public bool MoveNext()
        {
            uint num = (uint) this.$PC;
            this.$PC = -1;
            switch (num)
            {
                case 0:
                    this.$this.isInit = false;
                    break;

                case 1:
                    break;

                default:
                    goto Label_00BA;
            }
            if (this.$this.Engine.IsWaitBootLoading)
            {
                this.$current = null;
                if (!this.$disposing)
                {
                    this.$PC = 1;
                }
                return true;
            }
            this.$this.categoryGridPage.Init(this.$this.Engine.DataManager.SettingDataManager.SceneGallerySetting.CreateCategoryList().ToArray(), new Action<UguiCategoryGridPage>(this.$this.OpenCurrentCategory));
            this.$this.isInit = true;
            this.$PC = -1;
        Label_00BA:
            return false;
        }

        [DebuggerHidden]
        public void Reset()
        {
            throw new NotSupportedException();
        }

        object IEnumerator<object>.Current
        {
            [DebuggerHidden]
            get
            {
                return this.$current;
            }
        }

        object IEnumerator.Current
        {
            [DebuggerHidden]
            get
            {
                return this.$current;
            }
        }
    }
}

