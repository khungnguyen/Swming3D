using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BenInteractable : SnapInteraction
{
      [SerializeField] List<SnapInteraction> interactionList;
    public override void OnSelect()
    {
        foreach (var mobject in interactionList)
        {
            mobject.EnableInteraction(false);
        }
        base.OnSelect();
    }
    public override void OnUnselect()
    {
        base.OnUnselect();
        foreach (var mobject in interactionList)
        {
            mobject.EnableInteraction(true);
        }
    }
}
