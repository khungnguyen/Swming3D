using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IOnExerciseLoaded
{
    void OnLoaded();
}
public class ExerciseManager : MonoBehaviour
{
    static List<IOnExerciseLoaded> OnLoadCallbacks = new List<IOnExerciseLoaded>();
    public static ExerciseManager instance;

    public Exercises exercises;

    public int curLesson = 0;

    public void Awake()
    {
        instance = this;
        exercises = new Exercises();
        Debug.Log("Create exersie instance");
        
    }
    public void SetExercises(string text)
    {
        exercises = JsonUtility.FromJson<Exercises>(text);
        curLesson = 0;
        TriggerCallback();
        Debug.Log("Create exersie Data");
    }
    // Start is called before the first frame update
    void Start()
    {
      
    }
    // Update is called once per frame
  
    public ExerciseUnit GetExercise(int cur)
    {
       return exercises.ExerciseList[cur];
    }
    public ExerciseUnit GetCurxercise()
    {
        return GetExercise(curLesson);
    }
    public void ChangeNextExercise()
    {
        if(curLesson < exercises.ExerciseList.Length-1)
        {
            curLesson++;
        }

    }
    public int GetExerciseIndex()
    {
        return curLesson;
    }
    public static void AddCallbackTarget(IOnExerciseLoaded re)
    {
        IOnExerciseLoaded reuslt = OnLoadCallbacks.Find(e => e == re);
        if (reuslt == null)
        {
            OnLoadCallbacks.Add(re);
        }
    }
    public static void RemoveCallBackTarget(IOnExerciseLoaded re)
    {
        int index = OnLoadCallbacks.FindIndex(e => e == re);
        if (index != -1)
        {
            OnLoadCallbacks.RemoveAt(index);
        }
    }
    public static void TriggerCallback()
    {
        foreach (var re in OnLoadCallbacks)
        {
            re.OnLoaded();
        }
    }
}
[System.Serializable]
public class Exercises
{
    public ExerciseUnit[] ExerciseList;
    public string Exercise;
    public UIButton[] Buttons;
    public ButtonActions[] Actions;
}
[System.Serializable]
public class ExerciseUnit
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
[System.Serializable]
public class UIButton
{
    public string name;
    public string action;
    public int displayOrder;
}
[System.Serializable]
public class ButtonActions
{
    public string name;
    public string action;
    public int showDisplayerOrder;
}