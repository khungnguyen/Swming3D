using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LessonManager : MonoBehaviour, IReciever
{
    // Start is called before the first frame update
    public TextAsset[] lessonData;

    public ExerciseManager exerciesComp;

    public static LessonManager instance;
    private List<Exercises> listLesson = new List<Exercises>();
    private void Awake()
    {
        instance = this;
        for (var i = 0; i < lessonData.Length; i++)
        {
            listLesson.Add(JsonUtility.FromJson<Exercises>(lessonData[i].text));
        }

    }

    public List<Exercises> GetLessons() {
        return listLesson;
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
                Debug.LogError("Init Lesson Index" + index);
                ExerciseManager.instance.SetExercises(lessonData[index].text, index);
                break;

        }
    }

}
