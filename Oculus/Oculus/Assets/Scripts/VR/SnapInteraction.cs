using UnityEngine;
using UnityEngine.Animations.Rigging;
using Oculus.Interaction;
using System;

public class SnapInteraction : MonoBehaviour, IPointableElement
{
    // Start is called before the first frame update

    public MonoBehaviour constraintComp;
    public Transform snapTo;

    private bool isSnap = true;
    public bool disableSnape = false;
    public event Action<PointerEvent> WhenPointerEventRaised;
    public void Awake()
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
        activateConstraintComp(false);
       ((TwoBoneIKConstraint)constraintComp).weight = 0;//fds
       
    }
    void Start()
    {
       
    }
    public void ProcessPointerEvent(PointerEvent evt)
    {
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
    public void OnSelect()
    {
        isSnap = false;
        activateConstraintComp(true);
    }
    public void OnUnselect()
    {
        isSnap = true;
        activateConstraintComp(false);
    }
    public void LateUpdate()
    {
        if (isSnap)
        {
            if(Vector3.Distance(transform.position, snapTo.position)>=0.01)
            {
                transform.position = snapTo.position;
            }
            if (Vector3.Distance(transform.rotation.eulerAngles, snapTo.rotation.eulerAngles) >= 0.01)
            {
                transform.rotation = snapTo.rotation;
            }

        }
    }
    private void activateConstraintComp(bool active)
    {
        if (constraintComp is TwoBoneIKConstraint)
        {
            ((TwoBoneIKConstraint)constraintComp).enabled = active;
            ((TwoBoneIKConstraint)constraintComp).weight = 1;
        }
        else if (constraintComp is MultiAimConstraint)
        {
            ((MultiAimConstraint)constraintComp).enabled = active;
        }
    }
    public void visible(bool a)
    {
        transform.gameObject.SetActive(a);
    }

}
