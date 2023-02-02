using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StudentControllerBackFloat : StudentController
{
    [SerializeField] private BackFloat backFloatComp;
    private bool playFlashingOnce = true;
    protected override void Start()
    {
        backFloatComp.OnInteraction += OnBackFloatInteract;
        base.Start();
    }
    public void BackFloatFlashing(AnimationEvent e)
    {
        if (backFloatComp != null)
        {
            if (playFlashingOnce)
            {
                playFlashingOnce = false;
                backFloatComp.BackFloatFlashing();
                EnableInteraction(false);
            }

        }

    }
    public override void NotifityEndAnimationState(AnimationEvent e)
    {
        if (!playFlashingOnce)
        {

            base.NotifityEndAnimationState(e);
        }
        else
        {
            OnAnimationStateComplete?.Invoke(true);
        }

    }
    private void OnBackFloatInteract()
    {
        EnableInteraction(true);
    }
}
