using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum ExaminorAction
{

    StartExercise,
    TriggerFinalAnimation,
    TriggerAnimation,
    NextExercise,
    GoToExercise,
    GoToLessonMenu,
    EnableInteractable,
    DisableInteractable,
}

public enum ActionPropertyType
{
    DelayAfterAnim,
}
public class ExerciseActionControl : MonoBehaviour, IButtonAction, IOnExerciseLoaded
{
    enum DIALOG
    {
        CONFIRM,
        PLAY
    }
    public GameObject panel;

    public GameObject lessonPanel;

    public GameObject lessonUISpace;

    public GameObject buttonPrefab;

    public TMP_Text lessonName;

    public TMP_Text exerciseName;

    private ExerciseUnit curExercise;

    void Start()
    {


    }
    /*
     * Function called when Exercise was loaded fully
     * Create 1st UI
     */
    public void OnLoaded(int lesson)
    {
        curExercise = ExerciseManager.instance.GetCurxercise();

        CreateButtonDialog();
        UpdateLessonName();
        SettingUpLesson(lesson);
    }
    public void CreateButtonDialog(int level = 1)
    {

        var buttons = ExerciseManager.instance.exercises.Buttons;
        for (int g = 0; g < lessonUISpace.transform.childCount; g++)
        {
            Destroy(lessonUISpace.transform.GetChild(g).gameObject);
        }
        foreach (var bt in buttons)
        {
            if (bt.displayOrder == level)
            {
                if (bt.action == ExaminorAction.NextExercise.ToString())
                {
                    int currentExerciseIndex = ExerciseManager.instance.GetExerciseIndex();
                    int totalExerciseLength = ExerciseManager.instance.GetTotalExerciseLength();
                    Debug.Log("currentExerciseIndex" + currentExerciseIndex);
                    Debug.Log("totalExerciseLength" + (totalExerciseLength - 1));
                    if (currentExerciseIndex == totalExerciseLength - 1)
                    {
                        continue;
                    }
                }
                GameObject newbt = Instantiate(buttonPrefab, lessonUISpace.transform);
                newbt.name = bt.name;
                newbt.GetComponent<ExerciseButton>().SetButtonInfo(bt);
                newbt.GetComponent<ExerciseButton>().OnClicked += HandlerAction;
            }
        }
        StartCoroutine(AddContentFitter());
    }
    IEnumerator AddContentFitter()
    {
        yield return new WaitForEndOfFrame();
        lessonUISpace.GetComponent<VerticalLayoutGroup>().enabled = false;
        lessonUISpace.GetComponent<VerticalLayoutGroup>().enabled = true;
        GetComponent<VerticalLayoutGroup>().enabled = false;
        GetComponent<VerticalLayoutGroup>().enabled = true;
    }
    public void HandlerAction(string action)
    {
        Debug.LogError("Button Click With Action" + action);
        ButtonActions ac = GetButtonAction(action);
        if (ac != null)
        {
            foreach (ActionProperty actionProperty in ac.action)
            {
                string miniAction = actionProperty.name;
                if (miniAction == ExaminorAction.StartExercise.ToString())
                {
                    StartLesson();
                }
                else if (miniAction == ExaminorAction.TriggerFinalAnimation.ToString())
                {
                    FinalAnimation();
                }
                else if (miniAction == ExaminorAction.NextExercise.ToString())
                {
                    NextExercise();
                }
                else if (miniAction == ExaminorAction.TriggerAnimation.ToString())
                {
                    StartAnimtion(actionProperty.property);
                }
                else if (miniAction == ExaminorAction.GoToLessonMenu.ToString())
                {
                    GoToLessonUI();
                }
                else if (miniAction == ExaminorAction.EnableInteractable.ToString())
                {
                    bool OnlyAfterAnim = false;
                    if (actionProperty.property == ActionPropertyType.DelayAfterAnim.ToString())
                    {
                        OnlyAfterAnim = true;
                    }
                    EnableInteractable(OnlyAfterAnim);
                }
                else if (miniAction == ExaminorAction.DisableInteractable.ToString())
                {
                    bool OnlyAfterAnim = false;
                    if (actionProperty.property == ActionPropertyType.DelayAfterAnim.ToString())
                    {
                        OnlyAfterAnim = true;
                    }
                    DisableInteractable(OnlyAfterAnim);
                }
                else if (miniAction == ExaminorAction.GoToExercise.ToString())
                {

                    int newExerscise = int.Parse(actionProperty.property);
                    NextExercise(newExerscise);
                }
            }
            if (ac.showDisplayerOrder != 0)
            {
                CreateButtonDialog(ac.showDisplayerOrder);
            }
            //quy
        }
    }
    public void CloseUI()
    {
        transform.gameObject.SetActive(false);
    }
    public void GoToLessonUI()
    {
        CloseUI();
        lessonPanel.SetActive(true);
    }
    public ButtonActions GetButtonAction(string name)
    {
        var actions = ExerciseManager.instance.exercises.Actions;
        return (new List<ButtonActions>(actions)).Find(e => e.name == name);
    }
    public void EnableInteractable(bool OnlyAfterAnim)
    {
        object[] packages = new object[2];
        packages[0] = PhotonNetwork.NickName;
        packages[1] = OnlyAfterAnim;
        ConnectionManager.instance.SendAction(EventCodes.ActionEnableInteractable, packages);
    }
    public void DisableInteractable(bool OnlyAfterAnim)
    {
        object[] packages = new object[2];
        packages[0] = PhotonNetwork.NickName;
        packages[1] = OnlyAfterAnim;
        ConnectionManager.instance.SendAction(EventCodes.ActionDisableInteractable, packages);
    }
    public void StartLesson()
    {
        object[] packages = new object[2];
        packages[0] = PhotonNetwork.NickName;
        packages[1] = ExerciseManager.instance.GetExerciseIndex();
        ConnectionManager.instance.SendAction(EventCodes.ActionStartExercise, packages);
    }

    public void SettingUpLesson(int lessonIndex)
    {
        object[] packages = new object[1];
        packages[0] = lessonIndex;
        ConnectionManager.instance.SendAction(EventCodes.ActionSettingUpLesson, packages);
    }
    public void FinalAnimation()
    {
        sendActionPlayAnim(curExercise.conditionTrigger.name);
    }
    public void StartAnimtion(string name)
    {
        sendActionPlayAnim(name);
    }
    public void NextExercise(int index = -1)
    {
        if (index == -1)
        {
            ExerciseManager.instance.ChangeNextExercise();
           
        }
        else
        {
            ExerciseManager.instance.ChangeExercise(index);
        }
         curExercise = ExerciseManager.instance.GetCurxercise();
        object[] packages = new object[3];
        packages[0] = PhotonNetwork.NickName;
        packages[1] = ExerciseManager.instance.GetExerciseIndex();
        ConnectionManager.instance.SendAction(EventCodes.ActionNextExercise, packages);
        UpdateLessonName();
    }
    public void sendActionPlayAnim(string trigger = "Amimation")
    {
        object[] packages = new object[2];
        packages[0] = PhotonNetwork.NickName;
        packages[1] = trigger;
        ConnectionManager.instance.SendAction(EventCodes.ActionPlayAnimation, packages);
    }
    public void UpdateLessonName()
    {
        lessonName.text = ExerciseManager.instance.exercises.Exercise;
        exerciseName.text = curExercise.lessonName;
    }

    public void OnClicked(string action)
    {
        throw new System.NotImplementedException();
    }
    public void OnEnable()
    {
        ExerciseManager.AddCallbackTarget(this);
    }
    public void OnDisable()
    {
        ExerciseManager.RemoveCallBackTarget(this);
    }

}
