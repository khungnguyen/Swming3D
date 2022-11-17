using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ButtonBaseRoom : ButtonBase
{
    [SerializeField]
    private TMP_Text description;
    public void SetDescription(string t)
    {
        description.gameObject.SetActive(true);
        description.SetText(t);
    }
    void Awake()
    {
        description.gameObject.SetActive(false);
    }
}