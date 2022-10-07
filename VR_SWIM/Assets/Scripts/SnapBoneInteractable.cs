
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Animations;
[System.Serializable]
public struct Bones
{
    [SerializeField]
    public MultiAimConstraint constraint;
    [SerializeField]
    public TwoBoneIKConstraint ikbone;
    public void enabled()
    {
        if(constraint!=null)
        {
            constraint.enabled = true;
        }
        if (ikbone != null)
        {
            ikbone.enabled = true;
        }
    }
    public void disable()
    {
        if (constraint != null)
        {
            constraint.enabled = false;
        }
        if (ikbone != null)
        {
            ikbone.enabled = false;
        }
    }
}
public class SnapBoneInteractable : XRGrabInteractableExtends
{
    [SerializeField]
    private Transform targetBone;

    public Bones rigging;
    // Start is called before the first frame update
    private bool IsSnapped = true;
    void Start()
    {
        if (targetBone == null)
        {
            Debug.LogError("Missing targetBone");
        }
    }
    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        
        base.OnSelectEntered(args);
        DisableSnap();
    }
    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        EnableSnap();
    }

    public void EnableSnap()
    {
        IsSnapped = true;
        if (useRig)
            rigging.enabled();
    }
    public void DisableSnap()
    {
        IsSnapped = false;
        if(useRig)
        rigging.disable();
    }
    void LateUpdate()
    {
        if(useSnap && IsSnapped && targetBone!= null)
        {
           transform.position = targetBone.position;
           transform.rotation = targetBone.rotation;
           
        }
    }
    private bool useSnap = true;
    private bool useRig = true;
}
