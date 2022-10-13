using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IReciever {
    void OnActionReciver(EventCodes theEvent,object[] packages);
}
 