using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using Utage;

[AddComponentMenu("Utage/TemplateUI/SaveLoad")]
public class UtageUguiSaveLoad : UguiView
{
    private UtageUguiSaveLoadItem dialogItem;
    [SerializeField]
    private SystemUiDialog2Button dialogSaveLoad;
    [SerializeField]
    private AdvEngine engine;
    [SerializeField]
    private UguiGridPage gridPage;
    private bool isInit;
    private bool isSave;
    private List<AdvSaveData> itemDataList;
    private int lastPage;
    public GameObject loadRoot;
    public UtageUguiMainGame mainGame;
    public GameObject saveRoot;

    private void CallBackCreateItem(GameObject go, int index)
    {
        UtageUguiSaveLoadItem component = go.GetComponent<UtageUguiSaveLoadItem>();
        AdvSaveData data = this.itemDataList[index];
        component.Init(data, new Action<UtageUguiSaveLoadItem>(this.OnDialogSaveLoad), index, this.isSave);
    }

    [DebuggerHidden]
    private IEnumerator CoWaitOpen()
    {
        return new <CoWaitOpen>c__Iterator0 { $this = this };
    }

    private void OnClose()
    {
        this.lastPage = this.gridPage.CurrentPage;
        this.gridPage.ClearItems();
    }

    public void OnDialogSaveLoad(UtageUguiSaveLoadItem item)
    {
        SystemUi.GetInstance().IsEnableInputEscape = false;
        this.dialogItem = item;
        SystemText saveDesc = SystemText.SaveDesc;
        if (!this.isSave)
        {
            saveDesc = SystemText.LoadDesc;
        }
        else if (!item.Data.IsSaved)
        {
            this.OnTap();
            return;
        }
        this.dialogSaveLoad.Open(LanguageSystemText.LocalizeText(saveDesc), LanguageSystemText.LocalizeText(SystemText.Yes), LanguageSystemText.LocalizeText(SystemText.No), new UnityAction(this, (IntPtr) this.OnTap), new UnityAction(this, (IntPtr) this.OnTapCancel));
    }

    private void OnOpen()
    {
        this.isInit = false;
        this.gridPage.ClearItems();
        base.StartCoroutine(this.CoWaitOpen());
    }

    public void OnTap()
    {
        UtageUguiSaveLoadItem dialogItem = this.dialogItem;
        if (this.isSave)
        {
            this.Engine.WriteSaveData(dialogItem.Data);
            dialogItem.Refresh(true);
        }
        else if (dialogItem.Data.IsSaved)
        {
            this.Close();
            this.mainGame.OpenLoadGame(dialogItem.Data);
        }
        this.OnTapCancel();
    }

    private void OnTapCancel()
    {
        SystemUi.GetInstance().IsEnableInputEscape = true;
        this.dialogItem = null;
    }

    public void OpenLoad(UguiView prev)
    {
        this.isSave = false;
        this.saveRoot.SetActive(false);
        this.loadRoot.SetActive(true);
        this.Open(prev);
    }

    public void OpenSave(UguiView prev)
    {
        this.isSave = true;
        this.saveRoot.SetActive(true);
        this.loadRoot.SetActive(false);
        this.Open(prev);
    }

    private void Update()
    {
        if (this.isInit && InputUtil.IsMouseRightButtonDown())
        {
            this.Back();
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

    [CompilerGenerated]
    private sealed class <CoWaitOpen>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal bool $disposing;
        internal int $PC;
        internal UtageUguiSaveLoad $this;
        internal List<AdvSaveData> <list>__0;
        internal AdvSaveManager <saveManager>__0;

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
                    this.<saveManager>__0 = this.$this.Engine.SaveManager;
                    this.<saveManager>__0.ReadAllSaveData();
                    this.<list>__0 = new List<AdvSaveData>();
                    if (this.<saveManager>__0.IsAutoSave)
                    {
                        this.<list>__0.Add(this.<saveManager>__0.AutoSaveData);
                    }
                    this.<list>__0.AddRange(this.<saveManager>__0.SaveDataList);
                    this.$this.itemDataList = this.<list>__0;
                    this.$this.gridPage.Init(this.$this.itemDataList.Count, new Action<GameObject, int>(this.$this.CallBackCreateItem));
                    this.$this.gridPage.CreateItems(this.$this.lastPage);
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

