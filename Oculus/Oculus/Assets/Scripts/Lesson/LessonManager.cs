using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public enum LessonGroupType
{
    ZERO,
    ONE,
    TWO,
    THREE,
    FOUR
}

[System.Serializable]
public struct LessonGroup
{
    public LessonGroupType groupType;
    public List<LessonItem> listLesson;

    public List<LessonItem> GetLessonItems()
    {
        return listLesson;
    }
}
[System.Serializable]
public class LessonItem
{
    public TextAsset jsonData;
    public Exercises lesson;

    public void Init()
    {
        lesson = JsonUtility.FromJson<Exercises>(jsonData.text);
    }
}
public class LessonManager : MonoBehaviour, IReceiver
{
    public static string[] LessonGroupName = {
    "Time 00 - 10 minutes",
    "Time 10 - 25 minutes",
    "Time 25 - 40 minutes",
    };  

    [SerializeField]
    private List<LessonGroup> lessonGroupData;
    public static LessonManager instance;
    private void Awake()
    {
        instance = this;
        lessonGroupData.ForEach(e =>
        {
            e.GetLessonItems().ForEach(i =>
            {
                i.lesson = JsonUtility.FromJson<Exercises>(i.jsonData.text);
            });
        });
    }
    public List<LessonGroup> GetLessonGroups()
    {
        return lessonGroupData;
    }
    public List<Exercises> GetLessons(LessonGroupType group = LessonGroupType.ZERO)
    {
        List<LessonItem> list = lessonGroupData.Find(e => e.groupType == group).listLesson;
        // Utils.LogError("GetLessons", list.Count);
        if (list != null)
        {
            List<Exercises> lessons = new();
            list.ForEach(e =>
            {
                lessons.Add(e.lesson);
            });
            return lessons;
        }
        return null;

    }
    public void OnEnable()
    {
        ConnectionManager.AddCallbackTarget(this);

    }
    public void OnDisable()
    {
        ConnectionManager.RemoveCallBackTarget(this);
    }

    public void OnActionReceiver(EventCodes theEvent, object[] packages)
    {
        switch (theEvent)
        {
            case EventCodes.ActionInitLesson:

                var group = (int)packages[0];
                var index = (int)packages[1];
                Debug.LogError("Init Lesson Index" + index);
                ExerciseManager.instance.SetExercises(GetLessons((LessonGroupType)group)[index], index);
                break;

        }
    }

}
