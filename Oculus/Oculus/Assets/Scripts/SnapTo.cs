using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapTo : MonoBehaviour
{
    // Start is called before the first frame 
    private Transform target;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(target != null) {
          transform.SetPositionAndRotation(target.position,target.rotation);
        }
    }
    public void setTarget(Transform target) {
        this.target = target;
        transform.SetPositionAndRotation(target.position,target.rotation);
        
    }
}
