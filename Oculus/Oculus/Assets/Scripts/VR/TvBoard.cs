using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class TvBoard : MonoBehaviourPunCallbacks, IReceiver,IOnExerciseLoaded
{
    public TMP_Text lessonTitle;
    public TMP_Text ExcerciseTitle;
    public void OnActionReceiver(EventCodes theEvent, object[] packages)
    {
        switch (theEvent)
        {
            case EventCodes.ActionSettingUpLesson:
                var curLesson = (int)packages[0];
                Utils.LogError(this, "ActionSettingUpLesson", curLesson);
                break;
            case EventCodes.ActionInitLesson:
                var group = (int)packages[0];
                var index = (int)packages[1];
                Utils.LogError(this,"Init Lesson Index" + index);
                lessonTitle.SetText(LessonManager.LessonGroupName[(int)group]);
                Utils.LogError(this,"ExerciseManager.instance.exercises.Exercise" + ExerciseManager.instance.exercises.Exercise);
                ExcerciseTitle.SetText(ExerciseManager.instance.exercises.Exercise);
                break; 
            case EventCodes.ActionResetLesson:
                lessonTitle.SetText("----");
                ExcerciseTitle.SetText("----");
                break;

        }
    }

    // Start is called before the first frame update
    void Start()
    {
       
    }
    // Update is called once per frame
    void Update()
    {

    }

    public override void OnEnable()
    {
        ConnectionManager.AddCallbackTarget(this);
        ExerciseManager.AddCallbackTarget(this);
    
    }
    public override void OnDisable()
    {
        ConnectionManager.RemoveCallBackTarget(this);
        ExerciseManager.RemoveCallBackTarget(this);
    }

    public void OnLoaded(int lessonIndex,LessonGroupType groupType)
    {
        ExcerciseTitle.SetText(ExerciseManager.instance.exercises.Exercise);
    }
}
