using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BaseButton : MonoBehaviour
{
    public delegate void ClickAction(string action);

    public event ClickAction OnClicked;

    public TMP_Text label;

    public void SetText(string t)
    {
        label.SetText(t);
    }
    public void Click(string action)
    {
        if (OnClicked != null)
        {
            OnClicked(action);
        }
    }
    public void SetTextSize(int size)
    {
        label.fontSize = size;
    }
    public void EnableBold()
    {
        label.fontStyle = FontStyles.Bold;
    }
}