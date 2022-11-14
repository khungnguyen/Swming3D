using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogNotice : Dialog, IButtonEvent
{
    [SerializeField]
    private TMP_Text tmpcontent;

    public override Dialog Init(string title, Action<object> ok, Action<object> cancel)
    {
        return base.Init(title, ok, cancel);
    }
    public void AddContent(string content) {
        tmpcontent.SetText(content);
    }
    public void OnClicked(object data)
    {
        if (okFunc != null)
        {
            okFunc(data);
        }
    }
}
