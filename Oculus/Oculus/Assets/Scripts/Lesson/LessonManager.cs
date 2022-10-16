using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LessonManager : MonoBehaviour,IReciever
{
    // Start is called before the first frame update
    public TextAsset[] lessonData;

    public ExerciseManager exerciesComp;

    public static LessonManager instance;
    private void Awake()
    {
        instance = this;
    }
    public void OnEnable()
    {
        ConnectionManager.AddCallbackTarget(this);
    }
    public void OnDisable()
    {
        ConnectionManager.RemoveCallBackTarget(this);
    }

    public void OnActionReciver(EventCodes theEvent, object[] packages)
    {
        switch (theEvent)
        {
            case EventCodes.ActionInitLesson:
                var index = (int)packages[0];
                ExerciseManager.instance.SetExercises(lessonData[index].text);
                break;
           
        }
    }
   
}
