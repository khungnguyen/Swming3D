using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ButtonBaseRoom : ButtonBase
{
    [SerializeField]
    private TMP_Text description;
    public void SetDescription(string t) {
        description.SetText(t);
    }

}