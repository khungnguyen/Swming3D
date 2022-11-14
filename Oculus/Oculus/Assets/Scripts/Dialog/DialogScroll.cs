using System.Collections.Generic;
using UnityEngine;

public class DialogScroll : Dialog, IButtonEvent
{
    [SerializeField]
    private Transform scrollContent;

    [SerializeField]
    private GameObject baseButton;

    private List<ButtonBase> buttons = new();
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
        if (okFunc != null)
        {
            okFunc(data);
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
}
