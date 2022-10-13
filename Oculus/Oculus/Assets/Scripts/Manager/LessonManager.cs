using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LessonManager : MonoBehaviour
{
    public TextAsset lessonData;


    static LessonManager instance;

    public LessonList lessons;

    public void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        lessons = new LessonList();
        lessons = JsonUtility.FromJson<LessonList>(lessonData.text);
        Debug.Log("lessons" + lessons); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void CreateMission()
    {

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
   public ConditionTrigger conditionTrigger;

}
[System.Serializable]
public class ConditionTrigger
{
    public string name;
    public bool value;
}