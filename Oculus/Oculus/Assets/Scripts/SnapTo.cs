using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapTo : MonoBehaviour
{
    // Start is called before the first frame 
    private Transform target;

    private bool keepUpdating = false;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (target != null && this.keepUpdating)
        {
            transform.SetPositionAndRotation(target.position, target.rotation);
        }
    }
    public void setTarget(Transform target,bool keepUpdating = true)
    {
        this.target = target;
        this.keepUpdating = keepUpdating;
        if (target != null)
        {
            transform.SetPositionAndRotation(target.position, target.rotation);
        }
    }
}
