using DG.Tweening;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Utage;

public class DateManager : MonoBehaviour, IAdvSaveData, IBinaryIO
{
    private bool active;
    public CanvasGroup dateCanvas;
    public Text Day;
    public Text DayOfWeek;
    public Text Month;

    private void FormatDate(DateTime date)
    {
        string str = string.Empty;
        switch (date.DayOfWeek)
        {
            case System.DayOfWeek.Sunday:
                str = "周日";
                break;

            case System.DayOfWeek.Monday:
                str = "周一";
                break;

            case System.DayOfWeek.Tuesday:
                str = "周二";
                break;

            case System.DayOfWeek.Wednesday:
                str = "周三";
                break;

            case System.DayOfWeek.Thursday:
                str = "周四";
                break;

            case System.DayOfWeek.Friday:
                str = "周五";
                break;

            case System.DayOfWeek.Saturday:
                str = "周六";
                break;
        }
        this.DayOfWeek.set_text(str);
        this.Month.set_text(date.Month.ToString());
        this.Day.set_text("/" + date.Day);
    }

    public void HideDate()
    {
        ShortcutExtensions46.DOFade(this.dateCanvas, 0f, 0.5f);
        this.active = false;
    }

    public void OnClear()
    {
        this.dateCanvas.set_alpha(0f);
        this.active = false;
    }

    public void OnRead(BinaryReader reader)
    {
        this.active = reader.ReadBoolean();
        if (this.active)
        {
            AdvEngine component = GameObject.Find("AdvEngine").GetComponent<AdvEngine>();
            this.ShowDate(component);
        }
        else
        {
            this.dateCanvas.set_alpha(0f);
        }
    }

    public void OnWrite(BinaryWriter writer)
    {
        writer.Write(this.active);
    }

    public void ShowDate(AdvEngine engine)
    {
        DateTime time;
        ShortcutExtensions46.DOFade(this.dateCanvas, 1f, 0.5f);
        this.active = true;
        int parameterInt = engine.Param.GetParameterInt("day");
        if (parameterInt <= 11)
        {
            time = new DateTime(0x7dd, 5, 0x13).AddDays((double) parameterInt);
        }
        else
        {
            time = new DateTime(0x7dc, 5, 20).AddDays((double) parameterInt);
        }
        engine.Param.SetParameterBoolean("weekday", (time.DayOfWeek != System.DayOfWeek.Sunday) && (time.DayOfWeek != System.DayOfWeek.Saturday));
        engine.Param.SetParameterInt("dayOfWeek", (int) time.DayOfWeek);
        this.FormatDate(time);
    }

    public string SaveKey
    {
        get
        {
            return "DateManager";
        }
    }
}

