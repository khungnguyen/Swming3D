using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenSwicher : MonoBehaviour
{
    [SerializeField]
    private GameObject[] Desktop;

    [SerializeField]
    private GameObject[] XR;
    // Start is called before the first frame update
    private void Awake()
    {
       foreach(var d in Desktop)
        {
            d.SetActive(false);
        }
        foreach (var d in XR)
        {
            d.SetActive(false);
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
