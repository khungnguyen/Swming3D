using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapObject : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private Transform target2Snap;

    [SerializeField]
    GameObject parent;

    InputManager input;
    private bool isSnapped = true;
    void Start()
    {
        input = InputManager.instance;
        parent = transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        // if (target2Snap != null && isSnapped)
        // {
        //     transform.position = target2Snap.position;
        // }
        // else if (input.IsLeftMousePress())
        // {
        //     isSnapped = false;
        // }
        // else
        // {
        //     isSnapped = true;
        // }
    }
    public void EnableSnap(bool enable)
    {
        isSnapped = enable;
    }
}
