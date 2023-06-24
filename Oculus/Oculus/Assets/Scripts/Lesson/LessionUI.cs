using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;

public class LessionUI : MonoBehaviour, IButtonAction
{
    public GameObject lessonButtonPrefab;

    public GameObject PanleUserDecison;
    public GameObject panelChapter;

    public Transform scrollContent;
    public Transform dialogParent;

    public TMP_Text chapterName;
    public TMP_Text chapterTitle;

    public Transform lessonListTransform;

    public Transform layoutButton;

    [SerializeField]
    private BoundInAndOut boundAnimation;

    private bool goToExerciseInstedOfChapter = false;

    private LessonGroupType previousGroup;

    private bool useNewUI = VRAppDebug.USE_NEW_MENU_DESIGN;

    private bool useBackButton = true;
    enum Action
    {
        SelectChapter,
        SelectLesson,
        SelectLessonGroup,
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
            case Action.SelectChapter:
                previousGroup = Utils.String2Enum<LessonGroupType>(types[1]);
                CreateMenuLessonGroup(previousGroup);
                break;
            case Action.SelectLessonGroup:
                previousGroup = Utils.String2Enum<LessonGroupType>(types[1]);
                var previousLessonGoup = Utils.String2Enum<ExerciseGroupEnum>(types[2]);
                CreateLessonMenu(previousGroup, previousLessonGoup);
                break;
        }

    }
    IEnumerator AddContentFitter()
    {
        yield return new WaitForEndOfFrame();
        scrollContent.GetComponent<VerticalLayoutGroup>().enabled = false;
        yield return new WaitForEndOfFrame();
        scrollContent.GetComponent<VerticalLayoutGroup>().enabled = true;
        GetComponent<VerticalLayoutGroup>().enabled = false;
        yield return new WaitForEndOfFrame();
        GetComponent<VerticalLayoutGroup>().enabled = true;
    }
    private void ClearScrollContent()
    {
        Utils.DestroyTransformChildren(scrollContent);
        Utils.DestroyTransformChildren(layoutButton);
    }
    // Start is called before the first frame update
    void Start()
    {
        if (useNewUI)
        {
            Utils.DestroyTransformChildren(transform);
        }
        EnableTextAtChapter(false);

    }
    private void CreateChapterMenu(string a = null)
    {
        boundAnimation.PlayBoundEffect();
        EnableTextAtChapter(false);
        SendActionResetLesson();
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
                CreateChapterMenu();
            }).Show();
            for (var i = 0; i < LessonManager.instance.GetLessonGroups().Count; i++)
            {
                var item = LessonManager.instance.GetLessonGroups()[i];
                ButtonBaseRoom butt = (ButtonBaseRoom)dialogCP.AddButton();
                butt.SetText(LessonManager.LessonGroupName[(int)item.groupType]);
                butt.SetDescription("");
                butt.SetData(Action.SelectChapter.ToString() + "_" + item.groupType.ToString());
            }
        }
        else
        {

            ClearScrollContent();
            for (var i = 0; i < LessonManager.instance.GetLessonGroups().Count; i++)
            {
                var item = LessonManager.instance.GetLessonGroups()[i];
                var go = Instantiate(lessonButtonPrefab, scrollContent);
                var comp = go.GetComponent<LessonButton>();
                comp.SetText(LessonManager.LessonGroupName[(int)item.groupType]);
                comp.SetButtonInfo(Action.SelectChapter.ToString() + "_" + item.groupType.ToString());
                comp.SetTextSize(18);
                comp.OnClicked += OnClicked;

            }
            StartCoroutine(AddContentFitter());
        }

    }
    private void CreateLessonMenu(LessonGroupType groupType, ExerciseGroupEnum groupExercise = ExerciseGroupEnum.NA)
    {
        boundAnimation.PlayBoundEffect();
        EnableTextAtChapter(true);
        chapterName.text = LessonManager.LessonGroupName[(int)groupType];
        var lessonList = LessonManager.instance.GetLessons(groupType);
        var groups = lessonList.FindAll(e => e.groupInfo.Group == groupExercise);
        List<Exercises> avalableExercise = new();
        if (groups != null)
        {
            groups.ForEach(e =>
            {
                avalableExercise.Add(e.lesson);
            });
        }
        for (var i = 0; i < avalableExercise.Count; i++)
        {
            var item = avalableExercise[i];
            var go = Instantiate(lessonButtonPrefab, scrollContent);
            var comp = go.GetComponent<LessonButton>();
            Utils.Log(this, item.Exercise);
            comp.SetText(item.Exercise);
            comp.SetButtonInfo(Action.SelectLesson.ToString() + "_" + groupType.ToString() + "_" + i);
            comp.OnClicked += OnClicked;

        }
        //adding button back to Lession
        if (useBackButton)
        {
            var go = Instantiate(lessonButtonPrefab, layoutButton);
            var comp = go.GetComponent<LessonButton>();
            comp.SetText("BACK To LESSON List");
            comp.SetButtonInfo(Action.SelectLesson.ToString() + "_" + groupType.ToString() + "_");
            comp.OnClicked += CreateChapterMenu;
            comp.SetTextSize(16);
            comp.EnableBold();
        }

        StartCoroutine(AddContentFitter());
    }
    private void CreateMenuLessonGroup(LessonGroupType groupType)
    {
        boundAnimation.PlayBoundEffect();
        EnableTextAtChapter(true);
        chapterName.text = LessonManager.LessonGroupName[(int)groupType];
        var lessonList = LessonManager.instance.GetLessons(groupType);
        var hasManyGroup = false;
        var groups = lessonList.FindAll(e => e.groupInfo.Group == ExerciseGroupEnum.NA);
        if (groups.Count == 0)
        {
            hasManyGroup = true;
        }
        if (hasManyGroup)
        {

        }
        else
        {
            CreateLessonMenu(groupType);
            return;
        }
        List<ExerciseGroupStruct> groupEnum = new();
        Debug.LogError("System.Enum.GetValues(typeof(ExerciseGroupEnum)).Lengt" + System.Enum.GetValues(typeof(ExerciseGroupEnum)).Length);
        for (int i = 0; i < System.Enum.GetValues(typeof(ExerciseGroupEnum)).Length; i++)
        {
            if (i == (int)ExerciseGroupEnum.NA) continue;
            var _g = lessonList.FindAll(e => (int)(e.groupInfo.Group) == i);
            if (_g.Count > 0)
            {
                Debug.LogError("ExerciseGroupStruct" + _g);
                groupEnum.Add(_g.Find(e => (e.groupInfo.GroupButtonName != null && !e.groupInfo.GroupButtonName.Equals(""))).groupInfo);
            }
        }
        for (var i = 0; i < groupEnum.Count; i++)
        {
            var item = groupEnum[i];
            var go = Instantiate(lessonButtonPrefab, scrollContent);
            var comp = go.GetComponent<LessonButton>();
            Utils.Log(this, item.GroupButtonName);
            comp.SetText(item.GroupButtonName);
            comp.SetButtonInfo(Action.SelectLessonGroup.ToString() + "_" + groupType.ToString() + "_" + (int)item.Group + "_" + i);
            comp.OnClicked += OnClicked;

        }
        //adding button back to Lession
        if (useBackButton)
        {
            var go = Instantiate(lessonButtonPrefab, layoutButton);
            var comp = go.GetComponent<LessonButton>();
            comp.SetText("BACK To LESSON List");
            comp.SetButtonInfo(Action.SelectLesson.ToString() + "_" + groupType.ToString() + "_");
            comp.OnClicked += CreateChapterMenu;
            comp.SetTextSize(16);
            comp.EnableBold();
        }

        StartCoroutine(AddContentFitter());
    }
    private void OnLessonGroupSelected()
    {

    }
    private void OnLessonSelected(LessonGroupType groupType, int i)
    {
        transform.gameObject.SetActive(false);
        var lessonList = LessonManager.instance.GetExerciseList(groupType);
        var lessonIndex = i;
        SendActionInitLesson(groupType, lessonIndex);
        PanleUserDecison.GetComponent<ExerciseActionControl>().Show();
        ExerciseManager.instance.SetExercises(lessonList[lessonIndex], lessonIndex, groupType);


    }
    public void SendActionInitLesson(LessonGroupType groupType, int index)
    {
        object[] packages = new object[2];
        packages[0] = (int)groupType;
        packages[1] = index;
        ConnectionManager.instance.SendAction(EventCodes.ActionInitLesson, packages, ReceiverGroup.All);
    }
    public void SendActionResetLesson()
    {
        object[] packages = new object[1];
        ConnectionManager.instance.SendAction(EventCodes.ActionResetLesson, packages, ReceiverGroup.All);
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
            Utils.Log(this, "Call me here OnEnable");
            CreateMenuLessonGroup(previousGroup);

        }
        else
        {
            CreateChapterMenu();
        }
        gameObject.SetActive(true);
        boundAnimation.PlayBoundEffect();

    }

    public void EnableToGoToExercis()
    {
        goToExerciseInstedOfChapter = true;
    }
    private void EnableTextAtChapter(bool b)
    {
        chapterName.gameObject.SetActive(b);
        lessonListTransform.gameObject.SetActive(b);
        if (!b)
        {
            chapterTitle.text = "LESSON LIST";
        }
        else
        {
            chapterTitle.text = "LESSON";
        }
    }

}
