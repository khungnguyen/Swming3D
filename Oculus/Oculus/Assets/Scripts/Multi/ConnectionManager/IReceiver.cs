using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IReceiver {
    void OnActionReceiver(EventCodes theEvent,object[] packages);
}
 
 