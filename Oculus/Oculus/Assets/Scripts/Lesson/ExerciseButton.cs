using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ExerciseButton : BaseButton
{
  
    private UIButton info;

    public void SetButtonInfo(UIButton info)
    {
        this.info = info;
        SetText(info.name);
    }

    public void Click()
    {
        Click(info.action);
    }
}

