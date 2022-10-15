using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LessonButton : BaseButton
{

    private int lessonIndex;

    public void SetButtonInfo(int index)
    {
        lessonIndex = index;
    }
    public void Click()
    {
        Click(lessonIndex.ToString());
    }
}

