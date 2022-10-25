using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockRotation : MonoBehaviour
{
    // Start is called before the first frame update
    [Range(-180, 0)]
    public float rotateXMin = -60f;
    [Range(0, 180)]
    public float rotateXMax = 60f;

    public Transform target;

    private float XMin,XMax,YMin,YMax,ZMin,ZMax;
    void Start()
    {
        // Fuck
        Vector3 objectRoation = target.rotation.eulerAngles;
        XMin = objectRoation.x + rotateXMin;
        XMax = objectRoation.x + rotateXMax;
        // Fuck
    }
    void LateUpdate()
    {
        if (target != null)
        {
            Vector3 objectRoation = target.rotation.eulerAngles;
            objectRoation.x = objectRoation.x>180?objectRoation.x-360:objectRoation.x;
            // objectRoation.y = objectRoation.y>180?objectRoation.y-360:objectRoation.y;
            // objectRoation.z = objectRoation.z>180?objectRoation.z-360:objectRoation.z;
            objectRoation.x = Mathf.Clamp(objectRoation.x,XMin,XMax);
            // objectRoation.y = Mathf.Clamp(objectRoation.y,roateXMin,roateXMax);
            // objectRoation.z = Mathf.Clamp(objectRoation.z,roateXMin,roateXMax);
            Debug.Log(objectRoation);
            target.rotation = Quaternion.Euler(objectRoation);
        }

    }
}
