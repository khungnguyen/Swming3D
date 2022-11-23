using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingCollisonDetecter : MonoBehaviour
{
    public Action<Transform,bool> listener;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("RingTarget"))
        {
            listener?.Invoke(transform,true);
        }

    }
     private void OnTriggerExit(Collider collider)
    {
        if (collider.CompareTag("RingTarget"))
        {
            listener?.Invoke(transform,false);
        }

    }
}
