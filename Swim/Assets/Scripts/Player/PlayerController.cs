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
    private void Start()
    {
        this.controller = this.gameObject.GetComponent<CharacterController>();
        this.inputs = InputManager.instance;
        Cursor.visible = false;
        // Cursor.lockState = CursorLockMode.Locked;
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
        Vector3 move = new Vector3(inputMovement.x, 0, inputMovement.y);
        move = cameraTransform.forward * move.z + cameraTransform.right * move.x;
        move.y =0f;
        this.controller.Move(move * Time.deltaTime * this.playerSpeed);

        if (move != Vector3.zero)
        {
            this.gameObject.transform.forward = move;
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
