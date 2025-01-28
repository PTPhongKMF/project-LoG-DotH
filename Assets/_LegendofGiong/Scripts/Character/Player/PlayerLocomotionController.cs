using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotionController : CharacterLocomotionManager {
    private PlayerMovementController playerMovementController;

    public float horizontalMovement; 
    public float verticalMovement;

    private Vector3 moveDirection;
    private Vector3 targetRotationDirection;

    //[SerializeField] private float walkSpeed = 2f;
    [SerializeField] private float runSpeed = 7f;
    //[SerializeField] private float sprintSpeed = 10f;
    [SerializeField] private float rotationSpeed = 15f;

    protected override void Awake() {
        base.Awake();

        playerMovementController = GetComponent<PlayerMovementController>();
    }

    public void HandleAllMovement() {
        HandleGroundedMovement();
        HandleRotation();
    }

    private void HandleGroundedMovement() {
        (horizontalMovement, verticalMovement) = PlayerInputController.Instance.GetHorizontalAndVerticalInputs();

        moveDirection = PlayerCamera.Instance.transform.right * horizontalMovement;
        moveDirection = moveDirection + PlayerCamera.Instance.transform.forward * verticalMovement; 
        moveDirection.Normalize();
        moveDirection.y = 0;

        if (PlayerInputController.Instance.moveAmount == 1f) {
            playerMovementController.characterController.Move(moveDirection * runSpeed * Time.deltaTime);
        }
    }

    private void HandleRotation() {
        targetRotationDirection = Vector3.zero;
        targetRotationDirection = PlayerCamera.Instance.cameraObject.transform.right * horizontalMovement;
        targetRotationDirection = targetRotationDirection + PlayerCamera.Instance.cameraObject.transform.forward * verticalMovement;
        targetRotationDirection.Normalize();
        targetRotationDirection.y = 0;

        if (targetRotationDirection == Vector3.zero) {
            targetRotationDirection = transform.forward;
        }

        Quaternion newRotation = Quaternion.LookRotation(targetRotationDirection);
        Quaternion targetRotation = Quaternion.Slerp(transform.rotation, newRotation, rotationSpeed  * Time.deltaTime);
        transform.rotation = targetRotation;
    }
}
