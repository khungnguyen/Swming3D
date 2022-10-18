using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using System;

public class StudentController : MonoBehaviourPunCallbacks, IReciever
{
    public Animator animator;

    public RuntimeAnimatorController[] animatorData;

    public Avatar animatorAvatar;

    public RigBuilder rig;

    private Avatar curAvatar;


    struct TransformInfor
    {
        public Vector3 position;
        public Quaternion rotation;
    }
    private TransformInfor curTransform;
    // Start is called before the first frame update
    void Start()
    {
        curTransform.position = transform.position;
        curTransform.rotation = transform.rotation;
    }
    private bool delayActiveInteraction = false;
    private bool delayInactiveInteraction = false;
    public void NotifityEndAnimationState(AnimationEvent e)
    {
        Debug.LogError("Animation Event call" + e.time + e.animatorClipInfo.clip.name);
        if(delayActiveInteraction){
            StartCoroutine(DelayEnableInteraction(0.5f,true));
            delayActiveInteraction = false;
        }
        else if(delayInactiveInteraction){
            StartCoroutine(DelayEnableInteraction(0.5f,false));
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
    IEnumerator DelayEnableInteraction(float delayTime,bool enable)
    {
        delayActiveInteraction = false;
        delayInactiveInteraction= false;
        yield return new WaitForSeconds(delayTime);
        EnableInteraction(true);
    }
    public void OnActionReciver(EventCodes theEvent, object[] packages)
    {

        switch (theEvent)
        {
            case EventCodes.ActionPlayAnimation:
                string animation = (string)packages[1];
                //animator.avatar = animatorAvatar;
                Debug.Log("Play Animaton" + animation);
                EnableInteraction(false);
                TriggerAnimation(animation);
                UpdateStudentBehavior(animation);
                break;
            case EventCodes.ActionYes:
                string lessonName = (string)packages[1];
                string animator = (string)packages[3];
                string startExerciseAnimation = (string)packages[4];
                Debug.Log("Lesson Accept" + lessonName);
                SetAnimator(animator);
                if(startExerciseAnimation!=null){
                    TriggerAnimation(startExerciseAnimation);
                }
                else {
                    StartCoroutine(DelayEnableInteraction(0.5f,true));
                }
                break;
            case EventCodes.ActionNo:
                EnableInteraction(false);
                ResetTransform();
                int lessonIndex = (int)packages[1];
                ExerciseUnit lesson = ExerciseManager.instance.GetExercise(lessonIndex);
                Debug.Log("Next lession is  " + lesson.lessonName);
                TriggerAnimation(lesson.startAnimation);
                break;
            case EventCodes.ActionEnableInteractable:
                //Delay after finishing Animation
                if((bool)packages[1]) {
                    // StartCoroutine(IsCurrentAnimationCompleted(()=>{
                    //     EnableInteraction(true);
                        Debug.Log("Enable Interaction after Animation Compleated"); 
                    // }));
                    delayActiveInteraction = true;
                }
                else {
                    Debug.Log("Enable Interaction immediately"); 
                    EnableInteraction(true);
                }
                break;
            case EventCodes.ActionDisableInteractable:
                if((bool)packages[1]) {
                    // StartCoroutine(IsCurrentAnimationCompleted(()=>{
                    //     EnableInteraction(false);
                    Debug.Log("Disable Interaction after Animation Compleated"); 
                    // }));
                    delayInactiveInteraction = true;
                }
                else {
                    Debug.Log("Disable Interaction immediately"); 
                    EnableInteraction(false);
                }
                break;

        }
    }
    private void UpdateStudentBehavior(string behavior)
    {
        if (behavior == "NotFollowDistance")
        {
            StartCoroutine(StopSwimInSecond(2));
        }
    }
    IEnumerator StopSwimInSecond(float second)
    {
        yield return new WaitForSeconds(second);
        animator.StopPlayback();
    }
    private void SetAnimator(string animatorName)
    {
        if (animatorName != animator.runtimeAnimatorController.name)
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
    private void ResetTransform()
    {
        transform.SetPositionAndRotation(curTransform.position,curTransform.rotation);
    } 
    private void TriggerAnimation(string trigger) {
       animator.SetTrigger(trigger);
    }
    private IEnumerator IsCurrentAnimationCompleted(Action onCompleted) {
        Debug.Log("IsCurrentAnimationCompleted =" +animator.GetCurrentAnimatorStateInfo(0).IsName("StandJump") +"-" +animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
        while(animator.GetCurrentAnimatorStateInfo(0).IsName("StandJump") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f) {
          yield return new WaitForFixedUpdate();
          Debug.Log("Iam running" + animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
        }
       if(onCompleted!= null) {
            onCompleted();
       }
    }
}
