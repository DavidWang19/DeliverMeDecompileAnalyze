using DG.Tweening;
using System;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Utage;

public class InputCanvas : MonoBehaviour
{
    private bool active;
    private Text[] contents;
    private AdvEngine engine;
    private string inputText = string.Empty;
    private Regex regex;
    [HideInInspector]
    public bool wait;

    private void Exit()
    {
        this.wait = false;
        this.active = false;
        ShortcutExtensions46.DOFade(base.get_transform().GetComponent<CanvasGroup>(), 0f, 0f);
        base.get_transform().GetComponent<CanvasGroup>().set_blocksRaycasts(false);
    }

    public void InputConfirmButtonClick()
    {
        this.engine.Param.SetParameter<string>("八重玩法输入", this.inputText.ToUpper());
        this.Exit();
    }

    public void InputResetButtonClick()
    {
        this.Reset();
    }

    private void OnCountDownFinished()
    {
        base.get_gameObject().SetActive(false);
    }

    private void Reset()
    {
        for (int i = 0; i < 6; i++)
        {
            this.contents[i].set_text(string.Empty);
        }
        this.inputText = string.Empty;
    }

    public void ShowInputPanel(AdvEngine engine)
    {
        this.wait = true;
        this.active = true;
        ShortcutExtensions46.DOFade(base.get_transform().GetComponent<CanvasGroup>(), 1f, 0f);
        base.get_transform().GetComponent<CanvasGroup>().set_blocksRaycasts(true);
        this.engine = engine;
        this.Reset();
    }

    private void Start()
    {
        base.get_transform().Find("confirm").GetComponent<Button>().get_onClick().AddListener(new UnityAction(this, (IntPtr) this.InputConfirmButtonClick));
        base.get_transform().Find("reset").GetComponent<Button>().get_onClick().AddListener(new UnityAction(this, (IntPtr) this.InputResetButtonClick));
        this.contents = base.get_transform().GetComponentsInChildren<Text>();
        this.regex = new Regex("^[A-Za-z]+$");
    }

    private void Update()
    {
        if (this.active && (this.inputText.Length < 6))
        {
            string input = Input.get_inputString();
            if (this.regex.IsMatch(input))
            {
                this.inputText = this.inputText + input;
            }
            for (int i = 0; i < this.inputText.Length; i++)
            {
                this.contents[i].set_text((string.Empty + this.inputText[i]).ToUpper());
            }
        }
    }
}

