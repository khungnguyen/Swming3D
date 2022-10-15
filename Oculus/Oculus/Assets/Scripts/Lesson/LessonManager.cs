using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LessonManager : MonoBehaviour
{
    // Start is called before the first frame update
    public TextAsset[] lessonData;

    public ExerciseManager exerciesComp;

    public static LessonManager instance;
    private void Awake()
    {
        instance = this;
    }
}
