using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;

public class LessionUI : MonoBehaviour, IButtonAction
{
    public GameObject lessonButtonPrefab;

    public GameObject PanleUserDecison;

    public Transform scrollContent;

    public GameObject lessonDialog;

    public Transform dialogParent;

    public bool useNewUI = true;
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
        ClearScrollContent();
        string[] types = action.Split("_");
        switch (Utils.String2Enum<Action>(types[0]))
        {
            case Action.SelectLesson:
                OnLessonSelected(Utils.String2Enum<LessonGroupType>(types[1]), int.Parse(types[2]));
                break;
            case Action.SelectGroup:
                CreateLessonMenu(Utils.String2Enum<LessonGroupType>(types[1]));
                break;
        }
    }
    private void ClearScrollContent()
    {
        for (var i = scrollContent.childCount - 1; i >= 0; i--)
        {
            Destroy(scrollContent.GetChild(i).gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {


    }
    private void CreateLessonGroupMenu()
    {

        if (useNewUI)
        {
            var dialogGO = Instantiate(lessonDialog, dialogParent);
            DialogScroll dialogCP = dialogGO.GetComponent<DialogScroll>();
            DialogOption option = new DialogOption { title = "Lesson Group", description = "Lessons have same period", hideButton = true };
            dialogCP.Init(option, (object select) =>
            {
                dialogCP.Hide(() =>
                {
                    OnClicked((string)select);
                });

            }, null, (object cancel) =>
            {
                CreateLessonGroupMenu();
            });
            for (var i = 0; i < LessonManager.instance.GetLessonGroups().Count; i++)
            {
                var item = LessonManager.instance.GetLessonGroups()[i];
                ButtonBaseRoom butt = (ButtonBaseRoom)dialogCP.AddButton();
                butt.SetText("Lesson Group " + item.groupType);
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
                comp.SetText("Lesson Group " + item.groupType);
                comp.SetButtonInfo(Action.SelectGroup.ToString() + "_" + item.groupType.ToString());
                comp.OnClicked += OnClicked;

            }
        }

    }
    private void CreateLessonMenu(LessonGroupType groupType)
    {
        var data = LessonManager.instance.GetLessons(groupType);
        if (useNewUI)
        {
            var dialogGO = Instantiate(lessonDialog, dialogParent);
            DialogScroll dialogCP = dialogGO.GetComponent<DialogScroll>();
            DialogOption option = new DialogOption { title = "Lesson Group", description = "Lessons have same period", hideButton = true };
            dialogCP.Init(option, (object select) =>
            {
                OnClicked((string)select);
                dialogCP.Hide();
            }, null, (object cancel) =>
            {
                dialogCP.Hide(()=>{
                    CreateLessonGroupMenu();
                });
            });
            for (var i = 0; i < data.Count; i++)
            {
                var item = data[i];
                ButtonBaseRoom butt = (ButtonBaseRoom)dialogCP.AddButton();
                butt.SetText(item.Exercise);
                butt.SetDescription("Coming soon");
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
        CreateLessonGroupMenu();
    }
}
