using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CinemachinePOVExtension : CinemachineExtension
{
    [SerializeField]
    private float horizontalSpeed = 10f;
    [SerializeField]
    private float verticalSpeed = 10f;
    [SerializeField]
    private float claimAngle = 80f;
    private InputManager inputs;
    private Vector3 startingRotation;
    protected override void Awake() {
        this.inputs = InputManager.instance;
        base.Awake();
    }
    protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if(vcam.Follow) {
            if(stage == CinemachineCore.Stage.Aim) {
                if(startingRotation == null) {
                    startingRotation = transform.localRotation.eulerAngles;
                    Vector2 deltaInput = inputs.GetMouseDelta();
                    startingRotation.x+=deltaInput.x *horizontalSpeed*Time.deltaTime;
                    startingRotation.y+=deltaInput.y *verticalSpeed*Time.deltaTime;
                    startingRotation.y = Mathf.Clamp(startingRotation.y,-claimAngle,claimAngle);
                    state.RawOrientation = Quaternion.Euler(startingRotation.y,startingRotation.x,0f);
                }
            } 
        }
    }
}
