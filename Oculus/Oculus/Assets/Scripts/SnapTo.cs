using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapTo : MonoBehaviour
{
    // Start is called before the first frame 
    private Transform target;
    private bool keepUpdate = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(target != null && keepUpdate) {
          transform.SetPositionAndRotation(target.position,target.rotation);
        }
    }
    public void setTarget(Transform target,bool keepUpdate = true) {
        this.target = target;
        this.keepUpdate = keepUpdate;
        transform.SetPositionAndRotation(target.position,target.rotation);
        
    }
}
