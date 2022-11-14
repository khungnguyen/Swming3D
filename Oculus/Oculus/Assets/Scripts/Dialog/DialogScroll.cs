using UnityEngine;

public class DialogScroll : Dialog, IButtonEvent
{
    [SerializeField]
    private Transform scrollContent;

    [SerializeField]
    private GameObject baseButton;
    public ButtonBase AddButton(GameObject button)
    {
        var buttonBase = Instantiate(button, scrollContent).GetComponent<ButtonBase>();
        buttonBase.OnClicked += OnClicked;
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
}
