using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using System;
public enum LessonID
{
    Lesson_1,
    Lesson_2
}
public class StudentController : MonoBehaviourPunCallbacks, IReciever
{
    public Animator animator;

    public RuntimeAnimatorController[] animatorData;

    public Avatar animatorAvatar;

    public RigBuilder rig;

    public Transform kickBoard;
    public Transform boardHolderWrong;
    public Transform boardHolderRight;
    public Transform boardHolderSwim;

    public Transform replaceModelPoint;

    public GameObject currentModel;
    private GameObject replaceModel;
    private Avatar curAvatar;

    private int curLesson;

    struct TransformInfor
    {
        public Vector3 position;
        public Quaternion rotation;
    }
    // Start is called before the first frame update
    void Start()
    {
        var starTransfrom = SpawnPointManager.instance.GetStudentSpawnPointByName("Lesson_1_Ex_All_Pos");
        transform.SetPositionAndRotation(starTransfrom.position, starTransfrom.rotation);
    }
    private bool delayActiveInteraction = false;
    private bool delayInactiveInteraction = false;
    public void NotifityEndAnimationState(AnimationEvent e)
    {

        // StartCoroutine(DelayEnableInteraction(0.5f,true));
        if (delayActiveInteraction)
        {
            // Debug.LogError("Animation Event call" + e.time + e.animatorClipInfo.clip.name);
            StartCoroutine(DelayEnableInteraction(0.5f, true));
            delayActiveInteraction = false;
        }
        else if (delayInactiveInteraction)
        {
            StartCoroutine(DelayEnableInteraction(0.5f, false));
            delayActiveInteraction = false;
        }
    }
    public void EnableInteraction(bool enable)
    {
        rig.enabled = enable;
        SnapInteraction[] listSnap = transform.GetComponentsInChildren<SnapInteraction>(true);
        foreach (var snap in listSnap)
        {
            snap.EnableRigWeight(enable);
        }
    }
    IEnumerator DelayEnableInteraction(float delayTime, bool enable)
    {
        delayActiveInteraction = false;
        delayInactiveInteraction = false;
        yield return new WaitForSeconds(delayTime);
        EnableInteraction(enable);
        Debug.Log("DelayEnableInteraction = " + enable);

    }
    public void OnActionReciver(EventCodes theEvent, object[] packages)
    {

        switch (theEvent)
        {
            case EventCodes.ActionSettingUpLesson:
                transform.gameObject.SetActive(false);
                transform.gameObject.SetActive(true);
                //animator.enabled = true;
                curLesson = (int)packages[0];
                SettupStudent(curLesson);
                InitTransform(ExerciseManager.instance.GetStartPoint());
                SetAnimator(ExerciseManager.instance.GetAnimator());
                Debug.LogError("Setting Up for lesson");
                break;
            case EventCodes.ActionPlayAnimation:
                string animation = (string)packages[1];
                bool useReplaceModel = (bool)packages[0];
                //animator.avatar = animatorAvatar;
                Debug.Log("Play Animaton" + animation);

                EnableInteraction(false);
                if (useReplaceModel)
                {
                    TriggerReplaceAnimation(animation);
                }
                else
                {
                    TriggerAnimation(animation);
                }
                UpdateStudentBehavior(animation);
                break;
            case EventCodes.ActionStartExercise:
                int exerciseIndex = (int)packages[1];
                ExerciseUnit unit = ExerciseManager.instance.GetExercise(exerciseIndex);
                string lessonName = unit.lessonName;
                string startExerciseAnimation = unit.startExerciseAnimation;
                // string startPointName = unit.startPointName;
                // InitTransform(startPointName);
                Debug.Log("Lesson Accept" + lessonName);
                SetAnimator(ExerciseManager.instance.GetAnimator());
                TriggerAnimation(startExerciseAnimation);
                break;
            case EventCodes.ActionNextExercise:
                EnableInteraction(false);
                int excerciseIndex = (int)packages[1];
                ExerciseUnit muyUnit = ExerciseManager.instance.GetExercise(excerciseIndex);
                Debug.Log("Next lession is  " + muyUnit.lessonName);
                InitTransform(muyUnit.startPointName);
                TriggerAnimation(muyUnit.startAnimation);
                break;
            case EventCodes.ActionEnableInteractable:
                //Delay after finishing Animation
                if ((bool)packages[1])
                {
                    Debug.Log("Enable Interaction after Animation Compleated");
                    delayActiveInteraction = true;
                }
                else
                {
                    Debug.Log("Enable Interaction immediately");
                    EnableInteraction(true);
                }
                break;
            case EventCodes.ActionDisableInteractable:
                if ((bool)packages[1])
                {
                    Debug.Log("Disable Interaction after Animation Compleated");
                    delayInactiveInteraction = true;
                }
                else
                {
                    Debug.Log("Disable Interaction immediately");
                    EnableInteraction(false);
                }
                break;
            case EventCodes.ActionReplaceModel:
                string modelName = (string)packages[0];
                GameObject model = ModelHolder.instance.FindModelByName(modelName);
                if (model != null)
                {
                    replaceModel = Instantiate(model);
                    replaceModel.transform.SetParent(transform);
                    replaceModel.transform.SetPositionAndRotation(replaceModelPoint.position, replaceModelPoint.rotation);
                    SwapModel(true);
                }
                break;
            case EventCodes.ActionResetModel:
                SwapModel(false);
                Destroy(replaceModel);
                break;

        }
    }
    private void UpdateStudentBehavior(string behavior)
    {
        if (behavior == "NotFollowDistance")
        {
            //  StartCoroutine(StopSwimInSecond(2));
        }
    }
    IEnumerator StopSwimInSecond(float second)
    {
        yield return new WaitForSeconds(second);
        animator.StopPlayback();
    }
    private void SetAnimator(string animatorName)
    {
        // if (animatorName != animator.runtimeAnimatorController.name)
        {
            var find = (new List<RuntimeAnimatorController>(animatorData)).Find(e => e.name == animatorName);
            if (find != null)
            {

                animator.runtimeAnimatorController = find;
                Debug.Log("Reset Animator to" + animatorName);
            }
            else
            {
                Debug.LogError("Couldn't find Animator Controller " + animatorName);
            }
        }
    }

    public override void OnEnable()
    {
        ConnectionManager.AddCallbackTarget(this);
    }
    public override void OnDisable()
    {
        ConnectionManager.RemoveCallBackTarget(this);
    }
    private void InitTransform(string name)
    {
        Transform init = SpawnPointManager.instance.GetStudentSpawnPointByName(name);
        if (init != null)
        {
            transform.SetPositionAndRotation(init.position, init.rotation);
        }
    }
    private void TriggerAnimation(string trigger)
    {
        if (trigger != null && trigger.Length > 0)
        {
            BeforeTriggerAnim(trigger);
            animator.SetTrigger(trigger);
        }

    }
    private void BeforeTriggerAnim(string trigger)
    {
        if (trigger == "PositionWrong")
        {
            kickBoard.GetComponent<SnapTo>().setTarget(boardHolderWrong);

        }
        else
        {
            kickBoard.GetComponent<SnapTo>().setTarget(boardHolderRight);
        }
    }
    private void SwapModel(bool useReplace)
    {
        if (replaceModel != null)
        {
            currentModel.SetActive(!useReplace);
            replaceModel.SetActive(useReplace);
        }
        else
        {
            currentModel.SetActive(true);
        }

    }
    private void TriggerReplaceAnimation(string trigger)
    {
        if (trigger != null && trigger.Length > 0)
        {
            replaceModel?.GetComponent<Animator>().SetTrigger(trigger);
        }

    }
    private void SettupStudent(int lesson)
    {
        if (lesson == (int)LessonID.Lesson_1)
        {
            kickBoard.gameObject.SetActive(false);
        }
        if (lesson == (int)LessonID.Lesson_2)
        {
            kickBoard.gameObject.SetActive(true);
        }
    }
}
