using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ButtonBase : MonoBehaviour
{
    public delegate void ClickAction(object action);

    public event ClickAction OnClicked;

    public TMP_Text label;

    private object info;

    public void SetText(string t)
    {
        label.SetText(t);
    }
    public void Click()
    {
        if (OnClicked != null)
        {
            OnClicked(info);
        }
    }
    public void SetData(object data) {
        info = data;
    }
}