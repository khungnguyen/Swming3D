using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BenInteractable : SnapInteraction
{
    [SerializeField] List<SnapInteraction> interactionList;

    List<SnapInteraction> nolock;
    void Start()
    {
        nolock = interactionList.FindAll(e => e.lockTarget);
    }
    public override void OnSelect()
    {
        foreach (var mobject in nolock)
        {
            mobject.lockTarget = false;
        }
        foreach (var mobject in interactionList)
        {
            mobject.EnableInteraction(false);
        }
        base.OnSelect();
        Utils.LogError(this, "BenInteractable OnSelect");
    }
    public override void OnUnselect()
    {
        
        foreach (var mobject in interactionList)
        {
            mobject.EnableInteraction(true);
        }
        foreach (var mobject in nolock)
        {
            mobject.lockTarget = true;
        }
        base.OnUnselect();
        Utils.LogError(this, "BenInteractable OnUnselect");
    }
    IEnumerator EnableWeightEnumrator(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);

    }
}
