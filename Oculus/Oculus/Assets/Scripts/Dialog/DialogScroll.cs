using System;
using System.Collections.Generic;
using UnityEngine;

public class DialogScroll : Dialog, IButtonEvent
{
    [SerializeField]
    private Transform scrollContent;

    [SerializeField]
    private GameObject baseButton;

    private List<ButtonBase> buttons = new();

    private Action<object> selectedFunc;
    public ButtonBase AddButton(GameObject button)
    {
        var buttonBase = Instantiate(button, scrollContent).GetComponent<ButtonBase>();
        buttonBase.OnClicked += OnClicked;
        buttons.Add(buttonBase);
        return buttonBase;
    }
    public ButtonBase AddButton()
    {
        return AddButton(baseButton);
    }
    public void OnClicked(object data)
    {
        if (selectedFunc != null)
        {
            selectedFunc(data);
        }
    }
    public void ClearAllButtons()
    {
        buttons.ForEach(e =>
        {
            if (e != null)
            {
                Destroy(e.gameObject);
            }

        });
        buttons.Clear();
    }
    public void RemoveButtonByData(object data)
    {
        var button = buttons.Find(e => e.GetData() == data);
        Destroy(button.gameObject);
    }
    public virtual Dialog Init(DialogOption option, Action<object> selectedItemFunc, Action<object> okFunc, Action<object> cancelFunc)
    {
        selectedFunc = selectedItemFunc;
        return base.Init(option, okFunc, cancelFunc);
    }
}

