using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private float playerSpeed = 2.0f;
    private float jumpHeight = 1.0f;
    private float gravityValue = -9.81f;

    private InputManager inputs;

    private Transform cameraTransform;

    private float velocityRotation;
    private float smoothAngleTime = 0.1f;
    private void Start()
    {
        this.controller = this.gameObject.GetComponent<CharacterController>();
        this.inputs = InputManager.instance;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        this.cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        this.groundedPlayer = this.controller.isGrounded;
        if (groundedPlayer && this.playerVelocity.y < 0)
        {
            this.playerVelocity.y = 0f;
        }
        Vector3 inputMovement = this.inputs.GetMovement();
        Vector3 direction = new Vector3(inputMovement.x, 0, inputMovement.y);
        // float angle = Mathf.Atan2(direction.x,direction.y)*Mathf.Rad2Deg;
        // angle = Mathf.SmoothDampAngle(transform.eulerAngles.y,angle,ref velocityRotation,smoothAngleTime);
        direction = cameraTransform.forward * direction.z + cameraTransform.right * direction.x;
        direction.y =0f;
        this.controller.Move(direction.normalized * Time.deltaTime * this.playerSpeed);
        
        // this.transform.rotation = Quaternion.Euler(0f,angle,0f);
        if (direction != Vector3.zero)
        {
            this.gameObject.transform.forward = direction;
        }

        // Changes the height position of the player..
        if (this.inputs.IsJump() && this.groundedPlayer)
        {
            this.playerVelocity.y += Mathf.Sqrt(this.jumpHeight * -3.0f * this.gravityValue);
        }

        this.playerVelocity.y += this.gravityValue * Time.deltaTime;
        this.controller.Move(this.playerVelocity * Time.deltaTime);
    }
}
