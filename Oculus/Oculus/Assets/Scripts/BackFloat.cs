using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackFloat : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider collider)
    {
       Utils.LogError(this,"BackFloat Trigger Collider Enter");
       gameObject.SetActive(false);
    }
     private void OnTriggerExit(Collider collider)
    {
       Utils.LogError(this,"BackFloat Trigger Collider Exit");
    }
}
