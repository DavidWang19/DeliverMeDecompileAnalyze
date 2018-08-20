using System;
using UnityEngine.UI;

public class ButtonEx : Button
{
    public bool IsHover
    {
        get
        {
            return (base.get_currentSelectionState() == 1);
        }
    }
}

