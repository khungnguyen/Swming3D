using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class StudentController : MonoBehaviourPunCallbacks, IReciever
{
    public Animator animator;

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

    public void EndIdle(AnimationEvent e)
    {
        //Debug.LogError("Animation Event call" + e.time + e.animatorClipInfo.clip.name);
      //  EnableInteraction(true);
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
    public void DisableInteraction()
    {

    }
    IEnumerator DelayActiveRig (float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        EnableInteraction(true);
    }
    public void OnActionReciver(EventCodes theEvent, object[] packages)
    {

        switch(theEvent)
        {
            case EventCodes.ActionPlayAnimation:
                string animation = (string)packages[1];
                //animator.avatar = animatorAvatar;
                Debug.Log("Play Animaton" + animation);
                EnableInteraction(false);
                animator.SetBool(animation, true);
                break;
            case EventCodes.ActionYes:
                string lessonName = (string)packages[1];
                string startAnimaiton = (string)packages[2];
                Debug.Log("Lesson Accept" + lessonName);
                EnableInteraction(true);
                
                //  animator.SetBool(startAnimaiton, true);
                break;
            case EventCodes.ActionNo:
                int lessonIndex = (int)packages[1];
                ExerciseUnit lesson = ExerciseManager.instance.GetExercise(lessonIndex);
                Debug.Log("Next lession is  " + lesson.lessonName);
                EnableInteraction(false);
                animator.SetBool(lesson.starAnimation, true);
                transform.position = curTransform.position;
                transform.rotation = curTransform.rotation;
                break;

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
}
