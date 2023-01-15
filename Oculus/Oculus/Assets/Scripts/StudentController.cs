using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using System;
public class StudentController : MonoBehaviourPunCallbacks, IReceiver
{
    const string TAG = "[StudentController] ";
    public Animator animator;

    private Avatar animatorAvatar;

    public RigBuilder rig;

    public StudentExtension studentExtensions;

    public Transform bodyMovingCube;

    private int curLesson;

    public Action<bool> OnAnimationStateComplete;

    struct TransformInfor
    {
        public Vector3 position;
        public Quaternion rotation;
    }
    // public bool useF
    // Start is called before the first frame update
    void Awake()
    {
        StudentModelManager.instance.OnStudentCreate(this);
    }
    public static bool firstTime = false;
    void Start()
    {
        if (VRAppDebug.USE_DEBUG_VR_SINGLE_PREVIEW)
        {
            StartCoroutine(DelayEnableInteraction(2, true));
        }
        else
        {

            //   InitFirstPose();
        }
    }
    [PunRPC]
    public void InitFirstPose()
    {
        Utils.LogError(this, "Init first pose");
        //  if (!firstTime)
        {
            var starTransfrom = SpawnPointManager.instance.GetStudentSpawnPointByName("Lesson_1_Ex_All_Pos");
            SetAnimator("Case_One", true);
            transform.SetPositionAndRotation(starTransfrom.position, starTransfrom.rotation);
            firstTime = true;
            HideAllExtension();
        }
    }
    private bool delayActiveInteraction = false;
    private bool delayInactiveInteraction = false;

    public void RepositionNexAnim(string name)
    {
        //   CorrectTransform(name);
    } 
    public void NotifityEndAnimationState(AnimationEvent e)
    {

        // StartCoroutine(DelayEnableInteraction(0.5f,true));
        Utils.LogError(this,"Animation Event call" + e.animatorClipInfo.clip.name,delayActiveInteraction);
        if (delayActiveInteraction)
        {
            Utils.LogError(this,"Animation Event call Enable" + e.animatorClipInfo.clip.name);
            StartCoroutine(DelayEnableInteraction(0.1f, true));
            // EnableInteraction(true);
            delayActiveInteraction = false;
        }
        else if (delayInactiveInteraction)
        {
            Utils.LogError(this,"Animation Event call Disable" + e.animatorClipInfo.clip.name);
            StartCoroutine(DelayEnableInteraction(0.1f, false));
            //EnableInteraction(false);
            delayInactiveInteraction = false;
        }
        OnAnimationStateComplete?.Invoke(true);
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
        delayInactiveInteraction = false;
        if (enable)
        {
            OnAnimationStateComplete?.Invoke(true);
        }

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
                curLesson = (int)packages[0];
                StopAllCoroutines();
                HideAllExtension();
                EnableInteraction(false);
                SettupStudent(curLesson);
                CorrectTransform(ExerciseManager.instance.GetStartPoint());
                SetAnimator(ExerciseManager.instance.GetAnimator());
                Debug.LogError(TAG + "ActionSettingUpLesson " + curLesson);
                break;
            case EventCodes.ActionPlayAnimation:
                string animation = (string)packages[1];
                Debug.Log(TAG + "ActionPlayAnimation " + animation);
                StopAllCoroutines();
                EnableInteraction(false);
                TriggerAnimation(animation);
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
                    //EnableInteraction(true);
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
                    // EnableInteraction(false);
                }
                break;
            case EventCodes.ActionChangeController:
                Debug.Log(TAG + "ActionChangeController" + (string)packages[0]);
                EnableInteraction(false);
                SetAnimator((string)packages[0], true);
                break;
            case EventCodes.ActionCorrectTransform:
                Debug.Log(TAG + "ActionCorrectTransform" + (string)packages[0]);
                CorrectTransform((string)packages[0]);
                break;
            // case EventCodes.ActionStopAnimation:
            //     Debug.Log(TAG + "ActionStopAnimation");
            //     StartCoroutine(StopAnimation());
            //     break;
            case EventCodes.ActionActivateExtension:
                Debug.Log(TAG + "ActionActivateExtension");
                string transformName = (string)packages[0];
                bool active = (bool)packages[1];
                ActivateExtension(transformName, active);
                break;
            case EventCodes.ActionBodyMoving:
                bool enable = (bool)packages[0];
                ActivateBodyMoving(enable);
                break;
            case EventCodes.ActionChangeModel:

                break;


        }
    }
    private bool IsBodyMovingEnable = false;
    private string currentTransform = "";
    IEnumerator StopSwimInSecond(float second)
    {
        yield return new WaitForSeconds(second);
        animator.StopPlayback();
    }
    [PunRPC]
    public void SetAnimator(string animatorName, bool force = false)
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
                    }

                }
            }
            ReActivate();
            ResetAnimatorTriggers();
        }
        if (IsBodyMovingEnable)
        {
            // reset transform before playing animaiton
            ResetTransformAfterBodyMove();
            IsBodyMovingEnable = false;
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
    [PunRPC]
    public void TriggerAnimation(string trigger)
    {
        if (trigger != null && trigger.Length > 0)
        {
//            Utils.Log(this, "TriggerAnimation", "IsBodyMovingEnable", IsBodyMovingEnable);
            if (IsBodyMovingEnable)
            {
                // reset transform before playing animaiton
                ResetTransformAfterBodyMove();
                IsBodyMovingEnable = false;
            }
            BeforeTriggerAnim(trigger);
            animator.SetTrigger(trigger);
        }

    }
    private void ResetTransformAfterBodyMove()
    {
        if (!string.IsNullOrEmpty(currentTransform))
        {
            CorrectTransform(currentTransform);
        }
    }
    private string lastTrigger;
    private void BeforeTriggerAnim(string trigger)
    {
        string[] extension = ExerciseManager.instance.exercises.extension;
        if (studentExtensions != null)
        {
            studentExtensions.UpdateExtensions(extension, trigger, lastTrigger);
        }
        lastTrigger = trigger;
    }

    [PunRPC]
    private void ActivateBodyMoving(bool enable)
    {
        Utils.LogError("ActivateBodyMoving",enable);
        IsBodyMovingEnable = enable;
        bodyMovingCube.gameObject.SetActive(enable);
    }
    private void SettupStudent(int lesson)
    {
        if (studentExtensions != null)
        {
            studentExtensions.SetUpBaseOnLesson(lesson);
        }

    }

    private void HideAllExtension()
    {
        if (studentExtensions != null)
        {
            studentExtensions.HideAllExtension();
        }
        ActivateBodyMoving(false);

    }
    private void ReActivate()
    {
        transform.gameObject.SetActive(false);
        transform.gameObject.SetActive(true);
    }
    [PunRPC]
    public void CorrectTransform(string name)
    {
        Transform init = SpawnPointManager.instance.GetStudentSpawnPointByName(name);
        currentTransform = name;
        if (init != null)
        {
            Debug.Log(TAG + "CorrectTransform" + name);
            transform.SetPositionAndRotation(init.position, init.rotation);
            transform.localScale =init.localScale;
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
    private void ActivateExtension(string transformName, bool active)
    {
        if (studentExtensions != null)
        {
            studentExtensions.ActivateExtension(transformName, active);
        }


    }
    [PunRPC]
    public void EnableInteractionDelay(bool b) {
        delayActiveInteraction = b;
        Utils.Log(this,"Calling EnableInteractionDelay",this,delayActiveInteraction);
    }
     [PunRPC]
    public void EnableInteractionImmediate(bool b) {
       StartCoroutine(DelayEnableInteraction(0.1f, b));
        Utils.Log(this,"Calling EnableInteractionImmediate",b);
    }
    
}
