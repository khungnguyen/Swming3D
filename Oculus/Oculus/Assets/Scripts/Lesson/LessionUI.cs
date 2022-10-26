using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;

public class LessionUI : MonoBehaviour,IButtonAction
{
    public GameObject lessonButtonPrefab;

    public GameObject PanleUserDecison;

    public Transform scrollContent;


    public void OnClicked(string action)
    {
        var lessonIndex = int.Parse(action);
        SendActionInitLesson(lessonIndex);
        PanleUserDecison.SetActive(true);
        ExerciseManager.instance.SetExercises(LessonManager.instance.lessonData[lessonIndex].text,lessonIndex);
        transform.gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
       for(var i = 0; i < LessonManager.instance.GetLessons().Count; i++ )
        {
           var go = Instantiate(lessonButtonPrefab, scrollContent);
            var comp = go.GetComponent<LessonButton>();
            comp.SetText(LessonManager.instance.GetLessons()[i].Exercise);
            comp.SetButtonInfo(i);
            comp.OnClicked += OnClicked;
            
        }
    }
    public void SendActionInitLesson(int index)
    {
        object[] packages = new object[1];
        packages[0] = index;
        ConnectionManager.instance.SendAction(EventCodes.ActionInitLesson, packages,ReceiverGroup.Others);
    }
}
