using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum ExaminerAction
{

    StartExercise,
    TriggerFinalAnimation,
    TriggerAnimation,
    NextExercise,
    GoToExercise,
    GoToLessonMenu,
    EnableInteractable,
    DisableInteractable,
    SaveDataValue,
    ChangeAnimController,
    CorrectTransform,
    TriggerAnimationWithSaveKey,
    TriggerAnimationWithOutSaveKey,
    ClearDataSave,
    ReplaceModel,
    TriggerAnimationReplaceModel,
    SettingUpLesson,

    ChangeLessonTitle,
    StopAnimation,
    ActivateExtension,
    EnableBodyMoving,
    ChangeModel,
    EnableButtons,
}
public enum ActionPropertyType
{
    DelayAfterAnim,
    ReplaceByModel,
    ReplaceByInCorrectModel
}
public class ExerciseActionControl : MonoBehaviour, IButtonAction, IOnExerciseLoaded
{
    const string TAG = "[ExerciseActionControl] ";
    struct DataSave
    {
        public string key;
        public string value;
    };

    private List<DataSave> dataSave = new List<DataSave>();
    enum DIALOG
    {
        CONFIRM,
        PLAY
    }
    public GameObject panel;

    public GameObject lessonPanel;

    public GameObject lessonUISpace;

    public GameObject buttonPrefab;

    [SerializeField]
    private Transform dialogParent;

    public TMP_Text lessonName;

    public TMP_Text exerciseName;
    private bool useNewUI = VRAppDebug.USE_NEW_MENU_DESIGN;

    private ExerciseUnit curExercise;

    private int currentLessonIndex;

    private DialogScroll examinerDialog;

    /*
     * Function called when Exercise was loaded fully
     * Create 1st UI
     */
    void Start()
    {
        if (useNewUI)
        {
            Utils.DestroyTransformChildren(transform);
        }

    }
    public void OnLoaded(int lesson)
    {
        curExercise = ExerciseManager.instance.GetCurExercise();
        currentLessonIndex = lesson;
        CreateButtonDialog();
        UpdateLessonName(curExercise.lessonName);
        SettingUpLesson(lesson);
        string startAction = ExerciseManager.instance.exercises.startAction;
        if (!string.IsNullOrEmpty(startAction))
        {
            HandlerAction(startAction);
        }
    }
    public void CreateButtonDialog(int level = 1)
    {
        var buttons = ExerciseManager.instance.exercises.Buttons;
        if (useNewUI)
        {
            examinerDialog = ResourceManager.instance.CreateDialog<DialogScroll>(DialogType.DialogExersiesAction, dialogParent);
            DialogOption option = new() { title = ExerciseManager.instance.GetCurExercise().lessonName, description = "", hideButton = true };
            examinerDialog.Init(option, (object select) =>
            {
                examinerDialog.Hide(() =>
                {
                    HandlerAction((string)select);
                });

            }, null, (object cancel) =>
            {

            }).Show();
            foreach (var bt in buttons)
            {
                if (bt.displayOrder == level)
                {
                    if (bt.action == ExaminerAction.NextExercise.ToString())
                    {
                        int currentExerciseIndex = ExerciseManager.instance.GetExerciseIndex();
                        int totalExerciseLength = ExerciseManager.instance.GetTotalExerciseLength();

                        if (currentExerciseIndex == totalExerciseLength - 1)
                        {
                            continue;
                        }
                    }
                    ButtonBaseRoom butt = (ButtonBaseRoom)examinerDialog.AddButton();
                    butt.SetText(bt.name);
                    //butt.SetDescription("");
                    butt.SetData(bt.action);
                }
            }
        }
        else
        {
            for (int g = 0; g < lessonUISpace.transform.childCount; g++)
            {
                Destroy(lessonUISpace.transform.GetChild(g).gameObject);
            }
            foreach (var bt in buttons)
            {
                if (bt.displayOrder == level)
                {
                    if (bt.action == ExaminerAction.NextExercise.ToString())
                    {
                        int currentExerciseIndex = ExerciseManager.instance.GetExerciseIndex();
                        int totalExerciseLength = ExerciseManager.instance.GetTotalExerciseLength();
                        Utils.Log(this, currentExerciseIndex, "currentExerciseIndex", totalExerciseLength, "totalExerciseLength");
                        if (currentExerciseIndex == totalExerciseLength - 1) // cheating too tired to fix it
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

    }
    IEnumerator AddContentFitter()
    {
        yield return new WaitForEndOfFrame();
        lessonUISpace.GetComponent<VerticalLayoutGroup>().enabled = false;
        lessonUISpace.GetComponent<VerticalLayoutGroup>().enabled = true;
        GetComponent<VerticalLayoutGroup>().enabled = false;
        GetComponent<VerticalLayoutGroup>().enabled = true;
    }
    enum BUTTON_SHOW
    {
        KEEP_CURRENT_STATUS,
        HIDE_BUTTONS,
        SHOW_BUTTON
    };
    public void HandlerAction(string action)
    {
        Debug.LogError(TAG + "Button Click With Action" + action);
        ButtonActions ac = GetButtonAction(action);
        if (ac != null)
        {
            BUTTON_SHOW needToEnableButton = BUTTON_SHOW.KEEP_CURRENT_STATUS;
            foreach (ActionProperty actionProperty in ac.action)
            {
                string miniAction = actionProperty.name;
                if (miniAction == ExaminerAction.StartExercise.ToString())
                {
                    StartLesson();
                }
                else if (miniAction == ExaminerAction.TriggerFinalAnimation.ToString())
                {
                    FinalAnimation();
                }
                else if (miniAction == ExaminerAction.NextExercise.ToString())
                {
                    NextExercise();
                }
                else if (miniAction == ExaminerAction.TriggerAnimation.ToString())
                {
                    StartAnimation(actionProperty.property[0]);
                }
                else if (miniAction == ExaminerAction.GoToLessonMenu.ToString())
                {
                    GoToLessonUI();
                }
                else if (miniAction == ExaminerAction.EnableInteractable.ToString())
                {
                    bool OnlyAfterAnim = false;
                    if (actionProperty.property[0] == ActionPropertyType.DelayAfterAnim.ToString())
                    {
                        OnlyAfterAnim = true;
                    }
                    EnableInteractable(OnlyAfterAnim);
                }
                else if (miniAction == ExaminerAction.DisableInteractable.ToString())
                {
                    bool OnlyAfterAnim = false;
                    if (actionProperty.property[0] == ActionPropertyType.DelayAfterAnim.ToString())
                    {
                        OnlyAfterAnim = true;
                    }
                    DisableInteractable(OnlyAfterAnim);
                }
                else if (miniAction == ExaminerAction.GoToExercise.ToString())
                {
                    int newExerscise = int.Parse(actionProperty.property[0]);
                    Debug.LogError("GoToExercise" + newExerscise);
                    NextExercise(newExerscise);
                }
                else if (miniAction == ExaminerAction.SaveDataValue.ToString())
                {

                    for (int i = 0; i < actionProperty.property.Length; i += 2)
                    {
                        DataSave data;
                        data.key = actionProperty.property[i];
                        data.value = actionProperty.property[i + 1];
                        SaveData(data);
                    }
                }
                else if (miniAction == ExaminerAction.ChangeAnimController.ToString())
                {
                    string key = actionProperty.property[0];
                    List<string> choices = GetExercisePropertiesByName(key);
                    if (choices.Count > 0)
                    {
                        int ran = Random.Range(0, choices.Count - 1);
                        Debug.Log(TAG + "Select right controller by key" + choices[ran]);
                        ChangeController(choices[ran]);
                    }
                    else
                    {
                        Debug.LogError(TAG + "No property in Exercise" + key);
                        ChangeController(key);
                    }

                }
                else if (miniAction == ExaminerAction.TriggerAnimationWithSaveKey.ToString())
                {
                    string key = actionProperty.property[0];
                    string saveValue = GetDataSaveByKey(key).value;
                    if (saveValue != null && saveValue.Length > 0)
                    {
                        StartAnimation(saveValue);
                    }
                    else
                    {
                        Debug.LogError(TAG + "Could not find Save Key " + key);
                    }
                }
                else if (miniAction == ExaminerAction.TriggerAnimationWithOutSaveKey.ToString())
                {
                    string key = actionProperty.property[0];
                    string saveValue = GetDataSaveByKey(key).value;
                    if (saveValue != null && saveValue.Length > 0)
                    {
                        List<string> l = GetExercisePropertiesByName("TriggerName");
                        foreach (string i in l)
                        {
                            if (i != saveValue)
                            {
                                StartAnimation(i);
                            }
                        }
                    }
                    else
                    {
                        Debug.LogError(TAG + "Could not find Save Key " + key);
                    }
                }
                else if (miniAction == ExaminerAction.CorrectTransform.ToString())
                {
                    string key = actionProperty.property[0];
                    CorrectTransform(key);
                }
                // We not use any more action
                else if (miniAction == ExaminerAction.ReplaceModel.ToString())
                {
                    string modelFilter = actionProperty.property[0];
                    List<string> choices = GetExercisePropertiesByName(modelFilter);
                    int randomOne = Random.Range(0, choices.Count);
                    Debug.LogError(TAG + "ReplaceModel" + choices[randomOne]);
                    ReplaceModel(choices[randomOne]);
                }
                else if (miniAction == ExaminerAction.TriggerAnimationReplaceModel.ToString())
                {
                    StartAnimation(actionProperty.property[0], true);
                }
                else if (miniAction == ExaminerAction.ClearDataSave.ToString())
                {
                    ClearDataSave();
                }
                else if (miniAction == ExaminerAction.SettingUpLesson.ToString())
                {
                    SettingUpLesson(currentLessonIndex);
                }
                else if (miniAction == ExaminerAction.ChangeLessonTitle.ToString())
                {
                    UpdateLessonName(actionProperty.property[0]);
                }
                else if (miniAction == ExaminerAction.StopAnimation.ToString())
                {
                    StopAnimation();
                }
                else if (miniAction == ExaminerAction.ActivateExtension.ToString())
                {
                    string transformName = actionProperty.property[0];
                    bool active = actionProperty.property[1] == "True";
                    ActivateExtension(transformName, active);
                }
                else if (miniAction == ExaminerAction.EnableBodyMoving.ToString())
                {
                    bool active = actionProperty.property[0] == "True";
                    ActivateBodyMoving(active);
                }
                else if (miniAction == ExaminerAction.EnableButtons.ToString())
                {
                    bool active = actionProperty.property[0] == "True";
                    needToEnableButton = active ? BUTTON_SHOW.SHOW_BUTTON : BUTTON_SHOW.HIDE_BUTTONS;
                }
                else if (miniAction == ExaminerAction.ChangeModel.ToString())
                {
                    string modelName = actionProperty.property[0];
                    string startPoint = actionProperty.property[1];
                    string animatorKey = actionProperty.property[2];
                    string animation = actionProperty.property[3];
                    List<string> choices = GetExercisePropertiesByName(animatorKey);
                    string animator = "";
                    if (choices.Count > 0)
                    {
                        int ran = Random.Range(0, choices.Count - 1);
                        Debug.Log(TAG + "Select right controller by key" + choices[ran]);
                        animator = choices[ran];
                    }
                    else
                    {
                        animator = animatorKey;
                    }

                    ChangeModel(modelName, startPoint, animator, animation);
                }
                // End No Use actions

            }
            if (ac.showDisplayOrder != 0)
            {
                CreateButtonDialog(ac.showDisplayOrder);
            }
            if (needToEnableButton != BUTTON_SHOW.KEEP_CURRENT_STATUS)
            {
                EnableButtons(needToEnableButton == BUTTON_SHOW.SHOW_BUTTON);
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
    // Unused this Action Any more
    public void ReplaceModel(string newModelName)
    {
        object[] packages = new object[2];
        packages[0] = newModelName;
        ConnectionManager.instance.SendAction(EventCodes.ActionReplaceModel, packages);
    }
    // End Unused
    public void ChangeController(string controllerName)
    {
        object[] packages = new object[1];
        packages[0] = controllerName;
        ConnectionManager.instance.SendAction(EventCodes.ActionChangeController, packages);
    }
    public void CorrectTransform(string snapToTransform)
    {
        object[] packages = new object[1];
        packages[0] = snapToTransform;
        ConnectionManager.instance.SendAction(EventCodes.ActionCorrectTransform, packages);
    }
    public void EnableInteractable(bool OnlyAfterAnim)
    {
        //////
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
        UpdateLessonName(ExerciseManager.instance.GetCurExercise().lessonName);
    }

    public void SettingUpLesson(int lessonIndex)
    {
        object[] packages = new object[1];
        packages[0] = lessonIndex;
        ConnectionManager.instance.SendAction(EventCodes.ActionSettingUpLesson, packages);
    }
    public void FinalAnimation()
    {
        StartAnimation(curExercise.conditionTrigger.name);
    }
    public void StartAnimation(string name, bool useReplaceModel = false)
    {
        EnableButtons(false);
        SendActionPlayAnim(name, useReplaceModel);

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
        curExercise = ExerciseManager.instance.GetCurExercise();
        Debug.LogError("NextExercise " + ExerciseManager.instance.GetExerciseIndex() + " name " + curExercise.lessonName);
        object[] packages = new object[1];
        packages[0] = ExerciseManager.instance.GetExerciseIndex();
        ConnectionManager.instance.SendAction(EventCodes.ActionNextExercise, packages);
        UpdateLessonName(curExercise.lessonName);
    }
    public void SendActionPlayAnim(string trigger, bool forReplaceModel)
    {
        object[] packages = new object[2];
        packages[0] = forReplaceModel;
        packages[1] = trigger;
        ConnectionManager.instance.SendAction(EventCodes.ActionPlayAnimation, packages);
    }
    public void UpdateLessonName(string title)
    {
        exerciseName.text = title;
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
    private List<string> GetExercisePropertiesByName(string filter)
    {
        ExerciseUnit unit = ExerciseManager.instance.GetCurExercise();
        List<string> choices = new List<string>();
        for (int i = 0; i < unit.property.Length; i += 2)
        {
            if (unit.property[i] == filter)
            {
                choices.Add(unit.property[i + 1]);
            }
        }
        return choices;
    }
    private DataSave GetDataSaveByKey(string key)
    {
        return dataSave.Find(e => e.key == key);
    }
    private void SaveData(DataSave data)
    {
        Debug.Log(TAG + "Saving key" + data.key + "value" + data.value);
        dataSave.Add(data);
    }
    private void ClearDataSave()
    {
        dataSave.Clear();
    }
    private void StopAnimation()
    {
        object[] packages = new object[1];
        packages[0] = true;
        ConnectionManager.instance.SendAction(EventCodes.ActionStopAnimation, packages);
    }
    private void ActivateExtension(string objectName, bool active)
    {
        object[] packages = new object[2];
        packages[0] = objectName;
        packages[1] = active;
        ConnectionManager.instance.SendAction(EventCodes.ActionActivateExtension, packages);
    }
    private void ActivateBodyMoving(bool active)
    {
        object[] packages = new object[1];
        packages[0] = active;
        ConnectionManager.instance.SendAction(EventCodes.ActionBodyMoving, packages);
    }
    private void ChangeModel(string model, string pointName, string animator, string animation)
    {
        object[] packages = new object[4];
        packages[0] = model;
        packages[1] = pointName;
        packages[2] = animator;
        packages[3] = animation;
        ConnectionManager.instance.SendAction(EventCodes.ActionChangeModel, packages);
    }
    public void EnableButtons(bool enable)
    {
        var listButton = lessonUISpace.GetComponentsInChildren<ExerciseButton>();
        foreach (var b in listButton)
        {
            b.GetComponent<Button>().interactable = enable;
        }
    }
}
