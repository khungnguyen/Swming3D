using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DragnDrop : MonoBehaviour
{
    [SerializeField]
    private InputAction mouseClick;
    private Camera mainCamera;

    [SerializeField]
    private float mouseDragPhysicSpeed = 10f;

    [SerializeField]
    private float mouseDragSpeed = 0.1f;

    private WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();
    private Vector3 refVelocity = Vector3.zero;

    private void Awake()
    {
        mainCamera = Camera.main;
    }
    private void OnEnable()
    {
        mouseClick.Enable();
        mouseClick.performed += MousePressed;
    }
    private void OnDisable()
    {
        mouseClick.performed -= MousePressed;
        mouseClick.Disable();
    }
    void OnGUI()
    {
        Vector3 point = new Vector3();
        Event currentEvent = Event.current;
        Vector2 mousePos = new Vector2();

        // Get the mouse position from Event.
        // Note that the y position from Event is inverted.
        mousePos.x = currentEvent.mousePosition.x;
        mousePos.y = mainCamera.pixelHeight - currentEvent.mousePosition.y;

        point = mainCamera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, mainCamera.nearClipPlane));

        GUILayout.BeginArea(new Rect(20, 20, 250, 120));
        GUILayout.Label("Screen pixels: " + mainCamera.pixelWidth + ":" + mainCamera.pixelHeight);
        GUILayout.Label("Mouse position: " + mousePos);
        GUILayout.Label("World position: " + point.ToString("F3"));
        GUILayout.EndArea();
    }
    private void MousePressed(InputAction.CallbackContext context)
    {
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        Debug.Log("Ray"+ ray +"Mouse Pos" + Mouse.current.position.ReadValue());
        RaycastHit hit;
        if(Physics.Raycast(ray,out hit))
        {
            if(hit.collider!=null)
            {
                StartCoroutine(DragUpdate(hit.collider.gameObject));
            }
        }

    }
    private IEnumerator DragUpdate(GameObject clickedObject)
    {
        clickedObject.TryGetComponent<Rigidbody>(out var rb);
        clickedObject.TryGetComponent<SnapObject>(out var snapOb);
        float initialDistance = Vector3.Distance(clickedObject.transform.position, mainCamera.transform.position);
       
        while (mouseClick.ReadValue<float>() != 0)
        {
            Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            Debug.Log(ray.GetPoint(initialDistance));
            snapOb?.EnableSnap(false);
            if (rb != null)
            {
                Vector3 direction = ray.GetPoint(initialDistance) - clickedObject.transform.position;
                rb.velocity = direction * mouseDragPhysicSpeed;
                yield return waitForFixedUpdate;
            }
            else
            {
                clickedObject.transform.position = Vector3.SmoothDamp(clickedObject.transform.position, ray.GetPoint(initialDistance), ref refVelocity, mouseDragSpeed);
                yield return null;
            }
        }
    }
}
