using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IOnExerciseLoaded
{
    void OnLoaded(int lessonIndex);
}
public class ExerciseManager : MonoBehaviour
{
    const string TAG="[ExerciseManager] ";
    static List<IOnExerciseLoaded> OnLoadCallbacks = new List<IOnExerciseLoaded>();
    public static ExerciseManager instance;

    public Exercises exercises;

    public int curLesson = 0;
    public int curExercise= 0;

    public void Awake()
    {
        instance = this;
        exercises = new Exercises();
    }
    public void SetExercises(string text,int lessonIndex)
    {
        exercises = JsonUtility.FromJson<Exercises>(text);
        curLesson = lessonIndex;
        curExercise = 0;
        TriggerCallback();
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

    public string GetAnimator()
    {
        return exercises.AnimatorController;
    }
    public ExerciseUnit GetCurxercise()
    {
        return GetExercise(curExercise);
    }
    public ExerciseUnit ChangeNextExercise()
    {

        if (curExercise < exercises.ExerciseList.Length - 1)
        {
            curExercise++;

        }
        return GetCurxercise();
    }
    public ExerciseUnit ChangeExercise(int index)
    {
        if (curExercise != index && index <= exercises.ExerciseList.Length - 1)
        {
            curExercise = index;
        }
        return GetCurxercise();
    }
    public int GetExerciseIndex()
    {
        return curExercise;
    }
    public int GetTotalExerciseLength()
    {
        return exercises.ExerciseList.Length;
    }
    public string GetStartPoint()
    {
        return exercises.startPointName;
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
    public void TriggerCallback()
    {
        foreach (var re in OnLoadCallbacks)
        {
            re.OnLoaded(curLesson);
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
    public string AnimatorController;

    public string startPointName;
}
[System.Serializable]
public class ExerciseUnit
{
    public string lessonName;
    public string startAnimation;
    public string startExerciseAnimation;

    public string startController;
    public ConditionTrigger conditionTrigger;

    public string startPointName;

    public string modelReplaceName;

    public string[] property;

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
    public ActionProperty[] action;
    public int showDisplayerOrder;
}
[System.Serializable]
public class ActionProperty
{
    public string name;
    public string[] property;
}