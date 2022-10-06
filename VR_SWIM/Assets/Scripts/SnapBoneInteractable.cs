
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class SnapBoneInteractable : XRGrabInteractableExtends
{
    [SerializeField]
    private Transform targetBone;

    [SerializeField]
    private TwoBoneIKConstraint rigging;
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

       // rigging.enabled = false;
    }
    public void DisableSnap()
    {
        IsSnapped = false;
        //rigging.enabled = true;
    }
    void Update()
    {
        if(IsSnapped && targetBone!= null)
        {
          //  transform.position = targetBone.position;
           // transform.rotation = targetBone.rotation;
           
        }
    }
}
