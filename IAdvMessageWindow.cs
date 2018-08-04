using System;
using UnityEngine;
using Utage;

public interface IAdvMessageWindow
{
    void OnChangeActive(bool isActive);
    void OnChangeCurrent(bool isCurrent);
    void OnInit(AdvMessageWindowManager windowManager);
    void OnReset();
    void OnTextChanged(AdvMessageWindow window);

    GameObject gameObject { get; }
}

