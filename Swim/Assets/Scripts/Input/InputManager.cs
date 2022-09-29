using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    // Start is called before the first frame update

    private Inputs inputs;
    private static InputManager _instance;
    public static InputManager instance {
        get { return _instance;}
    }
    private void Awake()
    {
        if(_instance != null && _instance != this) {
            Destroy(this.gameObject);
        }
        else {
            _instance = this;
        }
        this.inputs = new Inputs();
    }
    private void OnEnable()
    {
        this.inputs.Enable();
    }
    void OnDisable()
    {
        this.inputs.Disable();
    }
    public Vector2 GetMovement()
    {
        return this.inputs.Player.Movement.ReadValue<Vector2>();
    }
    public Vector2 GetMouseDelta()
    {
        return this.inputs.Player.Look.ReadValue<Vector2>();
    }
    public bool IsJump() {
        return this.inputs.Player.Jump.triggered;
    }
}
