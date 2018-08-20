using DG.Tweening;
using System;
using System.Collections;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Utage;

//行动管理器，就是六重养成部分的主界面
public class ActionManager : MonoBehaviour, IAdvSaveData, IBinaryIO
{
    public Transform actionButtonParent;
    public GameObject[] actionButtonPrefabs;
    public CanvasGroup actionCanvas;
    private bool active;
    [Header("Stage3")]
    public string chuangzuo3;
    private AdvEngine engine;
    public string lianxiyanchang3;
    public string lianxiyanzou3;
    public GameObject paramEntryPrefab;
    [Tooltip("Order must be same as paramNames")]
    public string[] paramLables;
    [Tooltip("Order must be same as paramLables")]
    public string[] paramNames;
    public Transform paramParent;
    [Tooltip("Just for display")]
    public string[] paramSubLables;
    [Header("Stage2")]
    public string shuxiyinyue2;
    private int stageNo;
    [HideInInspector]
    public bool wait;
    private bool weekend;
    public string xinshangyinyue3;
    public string xiuxi1;
    public string xiuxi2;
    public string xiuxi3;
    [Header("JumpValues"), Header("Stage1")]
    public string xuexi1;
    public string xuexi2;
    public string xuexi3;
    public string xuexiyueli3;
    public string yueduilianxi3;
    public string yundong1;
    public string yundong2;
    public string yundong3;

    //似乎所有按钮都绑定的一个事件，根据触发按钮名响应不同的功能
    private void ActionButtonClick(string actionButtonName)
    {
        if (actionButtonName == "外出")
        {
            this.engine.Param.SetParameter<bool>("waichu", true);
            this.ExitActionPanel();
        }
        else
        {
            this.engine.Param.SetParameter<bool>("waichu", false);
            switch (this.stageNo)
            {
                case 1:
                    if (actionButtonName == "学习")
                    {
                        this.engine.Param.SetParameter<string>("actionControl", this.xuexi1);
                    }
                    else if (actionButtonName == "运动")
                    {
                        this.engine.Param.SetParameter<string>("actionControl", this.yundong1);
                    }
                    else if (actionButtonName == "休息")
                    {
                        this.engine.Param.SetParameter<string>("actionControl", this.xiuxi1);
                    }
                    break;

                case 2:
                    if (actionButtonName == "熟悉音乐")
                    {
                        this.engine.Param.SetParameter<string>("actionControl", this.shuxiyinyue2);
                    }
                    else if (actionButtonName == "学习")
                    {
                        this.engine.Param.SetParameter<string>("actionControl", this.xuexi2);
                    }
                    else if (actionButtonName == "运动")
                    {
                        this.engine.Param.SetParameter<string>("actionControl", this.yundong2);
                    }
                    else if (actionButtonName == "休息")
                    {
                        this.engine.Param.SetParameter<string>("actionControl", this.xiuxi2);
                    }
                    break;

                case 3:
                    if (actionButtonName == "创作")
                    {
                        this.engine.Param.SetParameter<string>("actionControl", this.chuangzuo3);
                    }
                    else if (actionButtonName == "学习乐理")
                    {
                        this.engine.Param.SetParameter<string>("actionControl", this.xuexiyueli3);
                    }
                    else if (actionButtonName == "练习演奏")
                    {
                        this.engine.Param.SetParameter<string>("actionControl", this.lianxiyanzou3);
                    }
                    else if (actionButtonName == "练习演唱")
                    {
                        this.engine.Param.SetParameter<string>("actionControl", this.lianxiyanchang3);
                    }
                    else if (actionButtonName == "欣赏音乐")
                    {
                        this.engine.Param.SetParameter<string>("actionControl", this.xinshangyinyue3);
                    }
                    else if (actionButtonName == "学习")
                    {
                        this.engine.Param.SetParameter<string>("actionControl", this.xuexi3);
                    }
                    else if (actionButtonName == "运动")
                    {
                        this.engine.Param.SetParameter<string>("actionControl", this.yundong3);
                    }
                    else if (actionButtonName == "休息")
                    {
                        this.engine.Param.SetParameter<string>("actionControl", this.xiuxi3);
                    }
                    else if (actionButtonName == "乐队练习")
                    {
                        this.engine.Param.SetParameter<string>("actionControl", this.yueduilianxi3);
                    }
                    break;
            }
            this.ExitActionPanel();
        }
    }

    //判断actionButtonName名字的按钮能否显示
    private bool CanShowAction(string actionButtonName)
    {
        switch (this.stageNo)
        {
            case 1:
                if ((!(actionButtonName == "学习") && !(actionButtonName == "运动")) && (!(actionButtonName == "休息") && !(actionButtonName == "外出")))
                {
                    return false;
                }
                if (!this.weekend && (actionButtonName == "外出"))
                {
                    return false;
                }
                return true;

            case 2:
                if ((!(actionButtonName == "熟悉音乐") && !(actionButtonName == "学习")) && ((!(actionButtonName == "运动") && !(actionButtonName == "休息")) && !(actionButtonName == "外出")))
                {
                    return false;
                }
                if (!this.weekend && (actionButtonName == "外出"))
                {
                    return false;
                }
                return true;

            case 3:
                if (((((actionButtonName == "创作") || (actionButtonName == "学习乐理")) || ((actionButtonName == "练习演奏") || (actionButtonName == "练习演唱"))) || ((((actionButtonName == "欣赏音乐") || (actionButtonName == "学习")) || ((actionButtonName == "运动") || (actionButtonName == "休息"))) || ((actionButtonName == "外出") || (actionButtonName == "乐队练习")))) && (this.weekend || (!(actionButtonName == "外出") && !(actionButtonName == "乐队练习"))))
                {
                    if (((actionButtonName == "乐队练习") && (this.engine.Param.GetParameterInt("day") >= 0x29)) && ((this.engine.Param.GetParameterInt("day") <= 0xc9) && (this.engine.Param.GetParameterInt("dayOfWeek") == 6)))
                    {
                        return false;
                    }
                    return true;
                }
                return false;
        }
        return false;
    }

    //判断paramName名字的数值能否显示
    private bool CanShowParam(string paramName)
    {
        switch (this.stageNo)
        {
            case 1:
                if (this.engine.Param.GetParameterInt("六重阶段") != 4)
                {
                    if (((!(paramName == "语文") && !(paramName == "数学")) && (!(paramName == "外语") && !(paramName == "综合"))) && (!(paramName == "体质") && !(paramName == "疲劳")))
                    {
                        return false;
                    }
                    return true;
                }
                if ((!(paramName == "成绩") && !(paramName == "体质")) && !(paramName == "疲劳"))
                {
                    return false;
                }
                return true;

            case 2:
                if (((!(paramName == "语文") && !(paramName == "数学")) && (!(paramName == "外语") && !(paramName == "综合"))) && ((!(paramName == "体质") && !(paramName == "疲劳")) && !(paramName == "音乐")))
                {
                    return false;
                }
                return true;

            case 3:
                if (((!(paramName == "成绩") && !(paramName == "乐理")) && (!(paramName == "唱功") && !(paramName == "演奏"))) && ((!(paramName == "审美") && !(paramName == "舞台")) && ((!(paramName == "熟练") && !(paramName == "体质")) && !(paramName == "疲劳"))))
                {
                    return false;
                }
                return true;
        }
        return false;
    }

    //退出行动面板，一般在点击按钮开始执行日常时调用
    private void ExitActionPanel()
    {
        ShortcutExtensions46.DOFade(this.actionCanvas, 0f, 0.5f);
        this.actionCanvas.set_blocksRaycasts(false);
        this.wait = false;
        this.active = false;
    }

    public void OnClear()
    {
        this.wait = false;
        this.actionCanvas.set_alpha(0f);
        this.actionCanvas.set_blocksRaycasts(false);
        this.active = false;
    }

    public void OnRead(BinaryReader reader)
    {
        this.active = reader.ReadBoolean();
        this.wait = reader.ReadBoolean();
        this.stageNo = reader.ReadInt32();
        this.weekend = reader.ReadBoolean();
        AdvEngine component = GameObject.Find("AdvEngine").GetComponent<AdvEngine>();
        //如果存档时行动主界面是激活的
        if (this.active)
        {
        	//显示行动主界面
            this.ShowActionPanel(component, this.stageNo, this.weekend, true);
        }
    }

    public void OnWrite(BinaryWriter writer)
    {
        writer.Write(this.active);
        writer.Write(this.wait);
        writer.Write(this.stageNo);
        writer.Write(this.weekend);
    }

    //显示按钮
    private void ShowActionButtons()
    {
        IEnumerator enumerator = this.actionButtonParent.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                Transform current = (Transform) enumerator.Current;
                current.get_gameObject().SetActive(false);
            }
        }
        finally
        {
            IDisposable disposable = enumerator as IDisposable;
            if (disposable != null)
            {
                disposable.Dispose();
            }
        }
        for (int i = 0; i < this.actionButtonPrefabs.Length; i++)
        {
            if (this.CanShowAction(this.actionButtonPrefabs[i].get_name()))
            {
                <ShowActionButtons>c__AnonStorey0 storey = new <ShowActionButtons>c__AnonStorey0 {
                    $this = this,
                    temp = this.actionButtonPrefabs[i].get_name()
                };
                Button component = this.actionButtonParent.Find(storey.temp).GetComponent<Button>();
                component.get_gameObject().SetActive(true);
                component.get_onClick().RemoveAllListeners();
                component.get_onClick().AddListener(new UnityAction(storey, (IntPtr) this.<>m__0));
            }
        }
    }

    //显示行动面板
    public void ShowActionPanel(AdvEngine advEngine, int stage, bool isWeekend = false, bool onRead = false)
    {
        this.active = true;
        ShortcutExtensions46.DOFade(this.actionCanvas, 1f, !onRead ? 0.5f : 0f);
        this.actionCanvas.set_blocksRaycasts(true);
        this.engine = advEngine;
        this.stageNo = stage;
        this.weekend = isWeekend;
        this.engine.Param.TrySetParameter("actionGoMap", false);
        this.ShowParams();
        this.ShowActionButtons();
        base.get_transform().Find("arr1").get_gameObject().SetActive(this.stageNo != 2);
        base.get_transform().Find("arr2").get_gameObject().SetActive(this.stageNo == 2);
    }

    //显示数值
    private void ShowParams()
    {
        IEnumerator enumerator = this.paramParent.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                Object.Destroy(((Transform) enumerator.Current).get_gameObject());
            }
        }
        finally
        {
            IDisposable disposable = enumerator as IDisposable;
            if (disposable != null)
            {
                disposable.Dispose();
            }
        }
        int num = 0;
        for (int i = 0; i < this.paramLables.Length; i++)
        {
            if (this.CanShowParam(this.paramNames[i]))
            {
                GameObject obj2 = Object.Instantiate<GameObject>(this.paramEntryPrefab, this.paramParent, false);
                obj2.GetComponent<Text>().set_text(this.paramLables[i]);
                obj2.get_transform().Find("SubTitle").GetComponent<Text>().set_text(this.paramSubLables[i]);
                obj2.get_transform().Find("Score").GetComponent<Text>().set_text(this.engine.Param.GetParameter(this.paramNames[i]).ToString());
                num++;
            }
        }
        RectTransform component = base.get_transform().Find("ParamPanel").GetComponent<RectTransform>();
        component.set_sizeDelta(new Vector2(component.get_rect().get_width(), (float) (140 + (num * 70))));
    }

    public string SaveKey
    {
        get
        {
            return "ActionManager";
        }
    }

    [CompilerGenerated]
    private sealed class <ShowActionButtons>c__AnonStorey0
    {
        internal ActionManager $this;
        internal string temp;

        internal void <>m__0()
        {
            this.$this.ActionButtonClick(this.temp);
        }
    }
}

