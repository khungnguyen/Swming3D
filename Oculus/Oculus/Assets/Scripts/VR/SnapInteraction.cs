using UnityEngine;
using UnityEngine.Animations.Rigging;
using Oculus.Interaction;
using System;
using Photon.Pun;
using System.Collections;

public class SnapInteraction : MonoBehaviourPunCallbacks, IPointableElement
{
    // Start is called before the first frame update

    public MonoBehaviour constraintComp;

    public Transform snapTo;

    public GameObject visualGameObject;

    public bool lockTarget = false;

    private bool isSnap = true;

    public bool disableSnapFunct = false;

    private bool enableWeight;

    public event Action<PointerEvent> WhenPointerEventRaised;

    public void Awake()
    {
        bool usePhotonView = !(photonView != null && photonView.IsMine);
        disableSnapFunct = false;//!DectectVR.instancne.isVR || usePhotonView;
        if (!disableSnapFunct)
        {
            // Find themself in parent
            if (constraintComp == null || (!(constraintComp is TwoBoneIKConstraint) && !(constraintComp is MultiAimConstraint)))
            {
                constraintComp = transform.parent.GetComponent<TwoBoneIKConstraint>();
                if (constraintComp == null)
                {
                    constraintComp = transform.parent.GetComponent<MultiAimConstraint>();
                }
            }
           // activateConstraintComp(false);
            EnableRigWeight(false);
            //ádasd
        }

    }

    IEnumerator EnableWeightEnumrator(float delayTime)
    {
        //Wait for the specified delay time before continuing.
        yield return new WaitForSeconds(delayTime);
        EnableRigWeight();
       //Do the action after the delay time has finished.
    }
    public void EnableRigWeight(bool enable = true)
    {
        enableWeight = enable;
        if (!disableSnapFunct)
        {
            if (constraintComp is TwoBoneIKConstraint)
            {
                ((TwoBoneIKConstraint)constraintComp).weight = enable ? 1 : 0;
            }
            else if (constraintComp is MultiAimConstraint)
            {
                ((MultiAimConstraint)constraintComp).weight = enable ? 1 : 0; 
            }
        }
        EnableGrabableCube(enable);
    }
    //dssd
    void Start()
    {
       // StartCoroutine(EnableWeightEnumrator(2));
    }
    public void ProcessPointerEvent(PointerEvent evt)
    {
        if(!disableSnapFunct)
        {
            if (photonView.IsMine)
            {
                // Find themself in parent
                switch (evt.Type)
                {
                    case PointerEventType.Select:
                        OnSelect();
                        break;
                    case PointerEventType.Unselect:
                        OnUnselect();
                        break;
                    case PointerEventType.Move:
                        break;
                }
            }
        }
      

    }
    public void OnSelect()
    {
        isSnap = false;
       // activateConstraintComp(true);
    }
    public void OnUnselect()
    {
        isSnap = true;
        SnapUpdate();
        // activateConstraintComp(false);
    }
    /*
     * lockTarget = true so alway snap to target, target move, cube moves
     * lockTarget = false : cube will be static and target was locked
     */
    public void Update()
    {
        if (!disableSnapFunct)
        {
            
            if (isSnap)
            {
                if(lockTarget && enableWeight)
                {
                    // do nothing
                }
                else
                {
                    SnapUpdate();
                }
            }
        }
    }
    private void SnapUpdate()
    {
        if (Vector3.Distance(transform.position, snapTo.position) >= 0.01)
        {
            transform.position = snapTo.position;
        }
        if (Vector3.Distance(transform.rotation.eulerAngles, snapTo.rotation.eulerAngles) >= 0.01)
        {
            transform.rotation = snapTo.rotation;
        }
    }
    private void activateConstraintComp(bool active)
    {
        if (constraintComp is TwoBoneIKConstraint)
        {
            ((TwoBoneIKConstraint)constraintComp).enabled = active;
        }
        else if (constraintComp is MultiAimConstraint)
        {
            ((MultiAimConstraint)constraintComp).enabled = active;
        }
    }
    public void EnableGrabableCube(bool a)
    {
        visualGameObject.GetComponent<MeshRenderer>().enabled = a ;
       // GetComponent<MeshRenderer>().material.color = new Color(1.0f, 1.0f, 1.0f, a?1f:0f);
    }
    public override void OnDisable()
    {

    }
    //
}
