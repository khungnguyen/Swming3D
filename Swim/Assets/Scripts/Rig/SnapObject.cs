using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapObject : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    Transform target2Snap;
    InputManager input;
    void Start()
    {
        input = InputManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (target2Snap && input)
        {
            if(input.IsLeftMousePress())
            {
                Debug.Log("Left mouse pressed");
            }
            else
            {
                transform.position = target2Snap.position;
            }

        }
    }
}
