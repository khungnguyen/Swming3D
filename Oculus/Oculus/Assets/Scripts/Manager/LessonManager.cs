using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LessonManager : MonoBehaviour
{
    public TextAsset lessonData;


    public static LessonManager instance;

    public LessonList lessons;

    public int curLesson = 0;

    public void Awake()
    {
        instance = this;
        lessons = new LessonList();
        lessons = JsonUtility.FromJson<LessonList>(lessonData.text);
        Debug.Log("lessons" + lessons);
    }
    // Start is called before the first frame update
    void Start()
    {
      
    }
    // Update is called once per frame
  
    public LessionUnit GetLesson(int cur)
    {
       return lessons.lessonList[cur];
    }
    public LessionUnit GetCurLesson()
    {
        return GetLesson(curLesson);
    }
    public void ChangeLesson()
    {
        if(curLesson < lessons.lessonList.Length-1)
        {
            curLesson++;
        }

    }
    public int GetLessonIndex()
    {
        return curLesson;
    }
}
[System.Serializable]
public class LessonList
{
    public LessionUnit[] lessonList;
}
[System.Serializable]
public class LessionUnit
{
   public string lessonName;
   public string starAnimation;
   public ConditionTrigger conditionTrigger;

}
[System.Serializable]
public class ConditionTrigger
{
    public string name;
    public bool value;
}