using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using UnityEngine.UI;

public class LessionUI : MonoBehaviour, IButtonAction
{
    public GameObject lessonButtonPrefab;

    public GameObject PanleUserDecison;

    public Transform scrollContent;
    public Transform dialogParent;

    private bool goToExerciseInstedOfChapter = false;

    private LessonGroupType previousGroup;

    private bool useNewUI = VRAppDebug.USE_NEW_MENU_DESIGN;
    enum Action
    {
        SelectGroup,
        SelectLesson
    }
    /**
    * action : SELECT_GROUP_LessonGroupType_Index to array
    */
    public void OnClicked(string action)
    {
        if (!useNewUI)
        {
            ClearScrollContent();
        }
        string[] types = action.Split("_");
        switch (Utils.String2Enum<Action>(types[0]))
        {
            case Action.SelectLesson:
                OnLessonSelected(Utils.String2Enum<LessonGroupType>(types[1]), int.Parse(types[2]));
                break;
            case Action.SelectGroup:
                previousGroup = Utils.String2Enum<LessonGroupType>(types[1]);
                CreateLessonMenu(previousGroup);
                break;
        }
    }
    IEnumerator AddContentFitter()
    {
        yield return new WaitForEndOfFrame();
        scrollContent.GetComponent<VerticalLayoutGroup>().enabled = false;
        scrollContent.GetComponent<VerticalLayoutGroup>().enabled = true;
        GetComponent<VerticalLayoutGroup>().enabled = false;
        GetComponent<VerticalLayoutGroup>().enabled = true;
    }
    private void ClearScrollContent()
    {
        Utils.DestroyTransformChildren(scrollContent);
    }
    // Start is called before the first frame update
    void Start()
    {
        if (useNewUI)
        {
            Utils.DestroyTransformChildren(transform);
        }

    }
    private void CreateLessonGroupMenu()
    {

        if (useNewUI)
        {
            // var dialogGO = Instantiate(lessonDialog, dialogParent);
            DialogScroll dialogCP = ResourceManager.instance.CreateDialog<DialogScroll>(DialogType.DialogScrollMenu, dialogParent);
            // DialogScroll dialogCP = dialogGO.GetComponent<DialogScroll>();
            DialogOption option = new DialogOption { title = "Exercises Group", description = "Exercises have same period", hideButton = true };
            dialogCP.Init(option, (object select) =>
            {
                dialogCP.Hide(() =>
                {
                    OnClicked((string)select);
                });

            }, null, (object cancel) =>
            {
                CreateLessonGroupMenu();
            }).Show();
            for (var i = 0; i < LessonManager.instance.GetLessonGroups().Count; i++)
            {
                var item = LessonManager.instance.GetLessonGroups()[i];
                ButtonBaseRoom butt = (ButtonBaseRoom)dialogCP.AddButton();
                butt.SetText(LessonManager.LessonGroupName[(int)item.groupType]);
                butt.SetDescription("");
                butt.SetData(Action.SelectGroup.ToString() + "_" + item.groupType.ToString());
            }
        }
        else
        {
            for (var i = 0; i < LessonManager.instance.GetLessonGroups().Count; i++)
            {
                var item = LessonManager.instance.GetLessonGroups()[i];
                var go = Instantiate(lessonButtonPrefab, scrollContent);
                var comp = go.GetComponent<LessonButton>();
                comp.SetText(LessonManager.LessonGroupName[(int)item.groupType]);
                comp.SetButtonInfo(Action.SelectGroup.ToString() + "_" + item.groupType.ToString());
                comp.OnClicked += OnClicked;

            }
            StartCoroutine(AddContentFitter());
        }

    }
    private void CreateLessonMenu(LessonGroupType groupType)
    {
        var data = LessonManager.instance.GetLessons(groupType);
        if (useNewUI)
        {
            // var dialogGO = Instantiate(lessonDialog, dialogParent);
            // DialogScroll dialogCP = dialogGO.GetComponent<DialogScroll>();
            DialogScroll dialogCP = ResourceManager.instance.CreateDialog<DialogScroll>(DialogType.DialogScrollMenu, dialogParent);
            DialogOption option = new() { title = "All Exercises", description = "Specific exercise", hideButton = true };
            dialogCP.Init(option, (object select) =>
            {
                OnClicked((string)select);
                dialogCP.Hide();
            }, null, (object cancel) =>
            {
                ScenesManager.instance.GoTo(SCREEN.MainMenu, true);
            }).Show();
            for (var i = 0; i < data.Count; i++)
            {
                var item = data[i];
                ButtonBaseRoom butt = (ButtonBaseRoom)dialogCP.AddButton();
                butt.SetText(item.Exercise);
                // butt.SetDescription("Coming soon");
                butt.SetData(Action.SelectLesson.ToString() + "_" + groupType.ToString() + "_" + i);

            }
        }
        else
        {
            for (var i = 0; i < data.Count; i++)
            {
                var item = data[i];
                var go = Instantiate(lessonButtonPrefab, scrollContent);
                var comp = go.GetComponent<LessonButton>();
                Utils.Log(this, item.Exercise);
                comp.SetText(item.Exercise);
                comp.SetButtonInfo(Action.SelectLesson.ToString() + "_" + groupType.ToString() + "_" + i);
                comp.OnClicked += OnClicked;

            }
            StartCoroutine(AddContentFitter());
        }

    }
    private void OnLessonSelected(LessonGroupType groupType, int i)
    {

        var lessonList = LessonManager.instance.GetLessons(groupType);
        var lessonIndex = i;
        SendActionInitLesson(groupType, lessonIndex);
        PanleUserDecison.SetActive(true);
        ExerciseManager.instance.SetExercises(lessonList[lessonIndex], lessonIndex);
        transform.gameObject.SetActive(false);
    }
    public void SendActionInitLesson(LessonGroupType groupType, int index)
    {
        object[] packages = new object[2];
        packages[0] = (int)groupType;
        packages[1] = index;
        ConnectionManager.instance.SendAction(EventCodes.ActionInitLesson, packages, ReceiverGroup.Others);
    }
    public void OnEnable()
    {
        if (goToExerciseInstedOfChapter)
        {
            goToExerciseInstedOfChapter = false;
            if (!useNewUI)
            {
                ClearScrollContent();
            }
            Utils.Log(this,"Call me here OnEnable");
            CreateLessonMenu(previousGroup);

        }
        else
        {
            CreateLessonGroupMenu();
        }

    }

    public void EnableToGoToExercis()
    {
        goToExerciseInstedOfChapter = true;
    }

}
