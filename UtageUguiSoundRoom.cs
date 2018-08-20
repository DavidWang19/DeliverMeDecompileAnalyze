using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Serialization;
using Utage;

[AddComponentMenu("Utage/TemplateUI/SoundRoom")]
public class UtageUguiSoundRoom : UguiView
{
    [FormerlySerializedAs("categoryGirdPage")]
    public UguiCategoryGridPage categoryGridPage;
    [SerializeField]
    private AdvEngine engine;
    [SerializeField]
    private UtageUguiGallery gallery;
    private bool isChangedBgm;
    private bool isInit;
    private List<AdvSoundSettingData> itemDataList = new List<AdvSoundSettingData>();
    public UguiListView listView;
    private readonly int perPageCount = 20;
    private UtageUguiSoundRoomItem playingItem;

    private void CallBackCreateItem(GameObject go, int index)
    {
        UtageUguiSoundRoomItem component = go.GetComponent<UtageUguiSoundRoomItem>();
        AdvSoundSettingData data = this.itemDataList[index];
        component.Init(data, new Action<UtageUguiSoundRoomItem>(this.OnTap), index);
    }

    [DebuggerHidden]
    private IEnumerator CoPlaySound(string path)
    {
        return new <CoPlaySound>c__Iterator1 { path = path, $this = this };
    }

    [DebuggerHidden]
    private IEnumerator CoWaitOpen()
    {
        return new <CoWaitOpen>c__Iterator0 { $this = this };
    }

    private void OnClose()
    {
        this.isInit = false;
        this.listView.ClearItems();
        this.categoryGridPage.Clear();
        if (this.isChangedBgm)
        {
            this.Engine.SoundManager.StopAll(0.2f);
        }
        this.isChangedBgm = false;
    }

    private void OnOpen()
    {
        this.isInit = false;
        this.isChangedBgm = false;
        this.listView.ClearItems();
        this.categoryGridPage.Clear();
        base.StartCoroutine(this.CoWaitOpen());
    }

    private void OnTap(UtageUguiSoundRoomItem item)
    {
        AdvSoundSettingData data = item.Data;
        string path = this.Engine.DataManager.SettingDataManager.SoundSetting.LabelToFilePath(data.Key, SoundType.Bgm);
        if (this.playingItem != null)
        {
            this.playingItem.Stop();
        }
        this.playingItem = item;
        this.playingItem.Play();
        base.StartCoroutine(this.CoPlaySound(path));
    }

    private void OpenCurrentCategory(UguiCategoryGridPage categoryGridPage)
    {
        this.itemDataList = this.Engine.DataManager.SettingDataManager.SoundSetting.GetSoundRoomList();
        int currentIndex = categoryGridPage.categoryToggleGroup.CurrentIndex;
        int index = this.perPageCount * currentIndex;
        int num3 = this.itemDataList.Count - index;
        int count = (num3 < this.perPageCount) ? num3 : this.perPageCount;
        this.itemDataList = this.itemDataList.GetRange(index, count);
        categoryGridPage.OpenCurrentCategory(this.itemDataList.Count, new Action<GameObject, int>(this.CallBackCreateItem));
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
    private sealed class <CoPlaySound>c__Iterator1 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal bool $disposing;
        internal int $PC;
        internal UtageUguiSoundRoom $this;
        internal AssetFile <file>__0;
        internal string path;

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
                    this.$this.isChangedBgm = true;
                    this.<file>__0 = AssetFileManager.Load(this.path, this.$this);
                    break;

                case 1:
                    break;

                default:
                    goto Label_00A7;
            }
            if (!this.<file>__0.IsLoadEnd)
            {
                this.$current = null;
                if (!this.$disposing)
                {
                    this.$PC = 1;
                }
                return true;
            }
            this.$this.Engine.SoundManager.PlayBgm(this.<file>__0);
            this.<file>__0.Unuse(this.$this);
            this.$PC = -1;
        Label_00A7:
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

    [CompilerGenerated]
    private sealed class <CoWaitOpen>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal bool $disposing;
        internal int $PC;
        internal UtageUguiSoundRoom $this;
        internal List<string> <soundRoomListTitles>__0;
        internal int <titilesCount>__0;

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
                case 1:
                    if (this.$this.Engine.IsWaitBootLoading)
                    {
                        this.$current = null;
                        if (!this.$disposing)
                        {
                            this.$PC = 1;
                        }
                        return true;
                    }
                    this.$this.itemDataList = this.$this.Engine.DataManager.SettingDataManager.SoundSetting.GetSoundRoomList();
                    this.<soundRoomListTitles>__0 = new List<string>();
                    this.<titilesCount>__0 = (this.$this.itemDataList.Count / this.$this.perPageCount) + 1;
                    for (int i = 0; i < this.<titilesCount>__0; i++)
                    {
                        int num3 = i;
                        int num4 = this.$this.perPageCount * num3;
                        int num5 = this.$this.itemDataList.Count - num4;
                        int num6 = (num5 < this.$this.perPageCount) ? num5 : this.$this.perPageCount;
                        this.<soundRoomListTitles>__0.Add((((i * this.$this.perPageCount) + 1)).ToString() + "~" + (((i * this.$this.perPageCount) + num6)).ToString());
                    }
                    this.$this.categoryGridPage.Init(this.<soundRoomListTitles>__0.ToArray(), new Action<UguiCategoryGridPage>(this.$this.OpenCurrentCategory));
                    this.$this.isInit = true;
                    this.$PC = -1;
                    break;
            }
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

