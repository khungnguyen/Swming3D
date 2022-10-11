using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionReceiver : MonoBehaviour, IReciever
{
    public void OnActionReciver(EventCodes theEvent, object[] packages)
    {
      //  throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    public void OnEnable()
    {
        ConnectionManager.AddCallbackTarget(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
