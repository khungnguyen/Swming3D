using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LessonButton : BaseButton
{

    private string lessonIndex;

    public void SetButtonInfo(string index)
    {
        lessonIndex = index;
    }
    public void Click()
    {
        Click(lessonIndex.ToString());
    }
    public void SetTextSize(int size) {
        label.fontSize = size;
    }
}

