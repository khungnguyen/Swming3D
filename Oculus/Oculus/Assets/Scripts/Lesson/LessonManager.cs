using System;
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
public struct ExerciseGroupStruct {
    public ExerciseGroupEnum Group;
    public string GroupButtonName;
}
[System.Serializable]
public enum ExerciseGroupEnum
{
    NA,
    G1,
    G2,
    G3,
    G4,
    G5,
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
    [NonSerialized]
    public Exercises lesson;

    public bool enable = true;
    public void Init()
    {
        lesson = JsonUtility.FromJson<Exercises>(jsonData.text);
    }
    public ExerciseGroupStruct groupInfo;
}
public class LessonManager : MonoBehaviour, IReceiver
{
    public static string[] LessonGroupName = {
    "TIME 00-10 Minutes",
    "TIME 10-25 Minutes",
    "TIME 25-40 Minutes",
    "TIME 40-45 Minutes",
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
    public List<Exercises> GetExerciseList(LessonGroupType group = LessonGroupType.ZERO)
    {
        List<LessonItem> list = (lessonGroupData.Find(e => e.groupType == group).listLesson).FindAll(i => i.enable == true);
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
     public List<LessonItem> GetLessons(LessonGroupType group = LessonGroupType.ZERO)
    {
        List<LessonItem> list = (lessonGroupData.Find(e => e.groupType == group).listLesson).FindAll(i => i.enable == true);
       
        return list;

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
                ExerciseManager.instance.SetExercises(GetExerciseList((LessonGroupType)group)[index], index, (LessonGroupType)group);
                break;

        }
    }

}
