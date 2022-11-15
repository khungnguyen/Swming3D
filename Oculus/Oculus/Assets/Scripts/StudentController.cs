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
public class StudentController : MonoBehaviourPunCallbacks, IReceiver
{
    const string TAG = "[StudentController] ";
    public Animator animator;

    private Avatar animatorAvatar;

    public RigBuilder rig;

    public Transform kickBoard;
    public Transform boardHolderWrong;
    public Transform boardHolderRight;
    public Transform boardHolderSplashKick;
    public Transform boardHolderSwim;
    public Transform replaceModelPoint;
    public GameObject currentModel;
    private GameObject replaceModel;

    public Transform bodyMovingCube;

    private int curLesson;

    struct TransformInfor
    {
        public Vector3 position;
        public Quaternion rotation;
    }
    // Start is called before the first frame update
    void Start()
    {
        if (VRAppDebug.USE_DEBUG_VR_SINGLE_PREVIEW)
        {
            StartCoroutine(DelayEnableInteraction(2, true));
        }
        else
        {
            var starTransfrom = SpawnPointManager.instance.GetStudentSpawnPointByName("Lesson_1_Ex_All_Pos");
            SetAnimator("Case_One");
            transform.SetPositionAndRotation(starTransfrom.position, starTransfrom.rotation);
        }

    }
    private bool delayActiveInteraction = false;
    private bool delayInactiveInteraction = false;
    public void NotifityEndAnimationState(AnimationEvent e)
    {

        // StartCoroutine(DelayEnableInteraction(0.5f,true));
        if (delayActiveInteraction)
        {
            Debug.LogError("Animation Event call Enable" + e.animatorClipInfo.clip.name);
            StartCoroutine(DelayEnableInteraction(0.2f, true));
            delayActiveInteraction = false;
        }
        else if (delayInactiveInteraction)
        {
            Debug.LogError("Animation Event call Disable" + e.animatorClipInfo.clip.name);
            StartCoroutine(DelayEnableInteraction(0.2f, false));
            delayInactiveInteraction = false;
        }
    }
    public void EnableInteraction(bool enable)
    {
        rig.enabled = enable;
        IInteraction[] listInteractionObject = transform.GetComponentsInChildren<IInteraction>(true);
        foreach (var mobject in listInteractionObject)
        {
            mobject.EnableInteraction(enable);
        }
        delayActiveInteraction = false;
        delayActiveInteraction = false;
        delayInactiveInteraction = false;
    }
    IEnumerator DelayEnableInteraction(float delayTime, bool enable)
    {
        delayActiveInteraction = false;
        delayInactiveInteraction = false;
        yield return new WaitForSeconds(delayTime);
        EnableInteraction(enable);
        Debug.Log(TAG + "DelayEnableInteraction = " + enable);

    }
    public void OnActionReceiver(EventCodes theEvent, object[] packages)
    {

        switch (theEvent)
        {
            case EventCodes.ActionSettingUpLesson:
                //animator.enabled = true;
                curLesson = (int)packages[0];
                StopAllCoroutines();
                EnableInteraction(false);
                SettupStudent(curLesson);
                CorrectTransform(ExerciseManager.instance.GetStartPoint());
                SetAnimator(ExerciseManager.instance.GetAnimator());
                Debug.LogError(TAG + "ActionSettingUpLesson " + curLesson);
                break;
            case EventCodes.ActionPlayAnimation:
                string animation = (string)packages[1];
                bool useReplaceModel = (bool)packages[0];
                //animator.avatar = animatorAvatar;
                Debug.Log(TAG + "ActionPlayAnimation " + animation);
                StopAllCoroutines();
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
                Debug.Log(TAG + "ActionStartExercise" + lessonName);
                //SetAnimator(ExerciseManager.instance.GetAnimator());
                TriggerAnimation(startExerciseAnimation);
                break;
            case EventCodes.ActionNextExercise:
                EnableInteraction(false);
                int excerciseIndex = (int)packages[0];
                ExerciseUnit muyUnit = ExerciseManager.instance.GetExercise(excerciseIndex);
                Debug.Log(TAG + "ActionNextExercise" + muyUnit.lessonName);
                SetAnimator(muyUnit.startController);
                CorrectTransform(muyUnit.startPointName);
                TriggerAnimation(muyUnit.startAnimation);
                break;
            case EventCodes.ActionEnableInteractable:
                //Delay after finishing Animation
                delayInactiveInteraction = false;
                if ((bool)packages[1])
                {
                    Debug.Log(TAG + "Enable Interaction after Animation Compleated");
                    delayActiveInteraction = true;
                }
                else
                {
                    Debug.Log(TAG + "Enable Interaction immediately");
                    StartCoroutine(DelayEnableInteraction(0.1f, true));
                }
                break;
            case EventCodes.ActionDisableInteractable:
                delayActiveInteraction = false;
                if ((bool)packages[1])
                {
                    Debug.Log(TAG + "Disable Interaction after Animation Compleated");
                    delayInactiveInteraction = true;
                }
                else
                {
                    Debug.Log(TAG + "Disable Interaction immediately");
                    StartCoroutine(DelayEnableInteraction(0.1f, false));
                }
                break;
            //Unused Cases
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
            //End Unused Cases
            case EventCodes.ActionChangeController:
                Debug.Log(TAG + "ActionChangeController" + (string)packages[0]);
                SetAnimator((string)packages[0], true);
                break;
            case EventCodes.ActionCorrectTransform:
                Debug.Log(TAG + "ActionCorrectTransform" + (string)packages[0]);
                CorrectTransform((string)packages[0]);
                break;
            case EventCodes.ActionStopAnimation:
                Debug.Log(TAG + "ActionStopAnimation");
                StartCoroutine(StopAnimation());
                break;

        }
    }
    private void UpdateStudentBehavior(string behavior)
    {
        if (curLesson == (int)LessonID.Lesson_2)
        {
            if (behavior == "Swim" || behavior == "Walk")
            {
                //  StartCoroutine(StopSwimInSecond(2));
                boardHolderSwim.gameObject.SetActive(true);
                kickBoard.gameObject.SetActive(false);
            }
            else
            {
                boardHolderSwim.gameObject.SetActive(false);
                kickBoard.gameObject.SetActive(true);
            }
        }

    }
    IEnumerator StopSwimInSecond(float second)
    {
        yield return new WaitForSeconds(second);
        animator.StopPlayback();
    }
    private void SetAnimator(string animatorName, bool force = false)
    {
        if (animatorName != null && animatorName.Length > 0 || force)
        {
            if (animatorName != animator?.runtimeAnimatorController?.name || force)
            {
                AnimationDataItem find = AnimationDataHolder.instance.GetAnimationDataByControllerName(animatorName);
                {
                    if (find.controller != null)
                    {
                        animator.runtimeAnimatorController = find.controller;
                        Debug.Log(TAG + "Reset Animator to" + animatorName);
                    }
                  //  Debug.Log(TAG + "Reset Avatar to" + find.avatar.name);
                    if (find.avatar != null)
                    {
                        Debug.Log(TAG + "Reset Avatar to" + find.avatar);
                        animator.avatar = find.avatar;
                        Debug.Log(TAG + "Reset Avatar to" + find.avatar.name);
                    }

                }
            }
            ReActivate();
            ResetAnimatorTriggers();
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
    private void TriggerAnimation(string trigger)
    {
        if (trigger != null && trigger.Length > 0)
        {
            BeforeTriggerAnim(trigger);
            animator.SetTrigger(trigger);
        }

    }
    private string lastAnimation;
    private void BeforeTriggerAnim(string trigger)
    {
        if (trigger == "StopAnim")
        {
            if (lastAnimation == "PositionWrong")
            {
                kickBoard.GetComponent<SnapTo>().setTarget(boardHolderWrong);

            }
            else if (lastAnimation == "SplashKickWrong")
            {
                kickBoard.GetComponent<SnapTo>().setTarget(boardHolderSplashKick);

            }
        }
        else if (trigger == "PositionWrong")
        {
            kickBoard.GetComponent<SnapTo>().setTarget(boardHolderWrong);

        }
        else if (trigger == "SplashKickWrong")
        {
            kickBoard.GetComponent<SnapTo>().setTarget(boardHolderSplashKick);

        }
        else if (curLesson == (int)LessonID.Lesson_2 && trigger == "EnableKickBoard")
        {
            kickBoard.gameObject.SetActive(true);
        }
        else
        {
            kickBoard.GetComponent<SnapTo>().setTarget(boardHolderRight);
        }
        if (trigger == "LoseControl")
        {
            bodyMovingCube.gameObject.SetActive(true);
        }
        else {
            bodyMovingCube.gameObject.SetActive(false);
        }

        lastAnimation = trigger;
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
            boardHolderSwim.gameObject.SetActive(false);
            kickBoard.gameObject.SetActive(false);
        }
        if (lesson == (int)LessonID.Lesson_2)
        {
            kickBoard.gameObject.SetActive(false);
            boardHolderSwim.gameObject.SetActive(false);
        }
    }
    private void ReActivate()
    {
        transform.gameObject.SetActive(false);
        transform.gameObject.SetActive(true);
    }
    private void CorrectTransform(string name)
    {
        Transform init = SpawnPointManager.instance.GetStudentSpawnPointByName(name);
        if (init != null)
        {
            Debug.Log(TAG + "CorrectTransform" + name);
            transform.SetPositionAndRotation(init.position, init.rotation);
        }
    }
    private void ResetAnimatorTriggers()
    {
        foreach (var trigger in animator.parameters)
        {
            if (trigger.type == AnimatorControllerParameterType.Trigger)
            {
                animator.ResetTrigger(trigger.name);
            }
        }
    }
    private IEnumerator StopAnimation()
    {
        var stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        Debug.Log("animator.layerCount" + animator.layerCount);
        Debug.Log("animator.GetLayerName" + animator.GetLayerName(0));
        yield return IsCurrentAnimationPlaying();
        animator.StopPlayback();
    }
    private IEnumerator IsCurrentAnimationPlaying()
    {
        var stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        while (true)
        {
            yield return null;
            Debug.Log("stateInfo.normalizedTime" + stateInfo.ToString() + "-" + stateInfo.normalizedTime % 1);
            //  if (stateInfo.normalizedTime % 1.0f == 0f)
            {
                Debug.Log("Anim mation finish");
                break;
            }
        }
    }
}
