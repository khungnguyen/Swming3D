using Photon.Pun;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public enum BUTTON_ACTIONS {

    StartExercise,
    TriggerFinalAnimation,
    TriggerAnimation,
    NextExercise,
    GoToLessonMenu
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

    private ExerciseUnit curExercise;

    void Start()
    {

      
    }
    /*
     * Function called when Exercise was loaded fully
     * Create 1st UI
     */
    public void OnLoaded()
    {
        curExercise = ExerciseManager.instance.GetCurxercise();
        CreateButtonDialog();
        UpdateLessonName();
    }
    public void CreateButtonDialog(int level = 1)
    {
        var buttons = ExerciseManager.instance.exercises.Buttons;
        for(int g =0; g < lessonUISpace.transform.childCount;g++)
        {
            Destroy(lessonUISpace.transform.GetChild(g).gameObject);
        }
        foreach (var bt in buttons)
        {
            if(bt.displayOrder == level)
            {
                GameObject newbt = Instantiate(buttonPrefab, lessonUISpace.transform);
                newbt.name = bt.name;
                newbt.GetComponent<ExerciseButton>().SetButtonInfo(bt);
                newbt.GetComponent<ExerciseButton>().OnClicked += HandlerAction;
            }
        }

    }
    public void HandlerAction(string action)
    {
        Debug.LogError("Button Click With Action" + action);
        ButtonActions ac = GetButtonAction(action);
        if(ac != null)
        {
            if(ac.action == BUTTON_ACTIONS.StartExercise.ToString())
            {
                StartLesson();
            }
            else if(ac.action == BUTTON_ACTIONS.TriggerFinalAnimation.ToString())
            {
                StartAnimtion();
            }
            else if(ac.action == BUTTON_ACTIONS.NextExercise.ToString())
            {
                NextLesson();
            }
            else if(ac.action == BUTTON_ACTIONS.TriggerAnimation.ToString())
            {
                StartAnimtion();
            }
            else if(ac.action == BUTTON_ACTIONS.GoToLessonMenu.ToString())
            {
                GoToLessonUI();
            }
            if(ac.showDisplayerOrder != 0)
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

    public void StartLesson()
    {
        object[] packages = new object[3];
        packages[0] = PhotonNetwork.NickName;
        packages[1] = curExercise.lessonName;
        packages[2] = curExercise.starAnimation;
        ConnectionManager.instance.SendAction(EventCodes.ActionYes, packages);
    }
    public void StartAnimtion()
    {
        sendActionPlayAnim(curExercise.conditionTrigger.name);
    }
    public void NextLesson()
    {
        ExerciseManager.instance.ChangeNextExercise();
        curExercise = ExerciseManager.instance.GetCurxercise();
        object[] packages = new object[3];
        packages[0] = PhotonNetwork.NickName;
        packages[1] = ExerciseManager.instance.GetExerciseIndex();
        ConnectionManager.instance.SendAction(EventCodes.ActionNo, packages);
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
        lessonName.text = curExercise.lessonName;
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
