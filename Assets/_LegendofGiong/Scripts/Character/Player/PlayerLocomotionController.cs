using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotionController : CharacterLocomotionManager {
    public PlayerMovementController playerMovementController;
    public PlayerStatsManager playerStatsManager;

    private float horizontalMovement;
    private float verticalMovement;

    private Vector3 moveDirection;
    private Vector3 targetRotationDirection;
    [HideInInspector] public float walkSpeed = 2f;
    [HideInInspector] public float runSpeed = 7f;
    [HideInInspector] public float sprintSpeed = 10f;
    [HideInInspector] public float rotationSpeed = 15f;

    public Vector3 dodgeDirection;
    [HideInInspector] public float dodgeSpeed = 7f;

    [HideInInspector] public float dodgeStamCost = 35f;

    protected override void Awake() {
        base.Awake();

        playerMovementController = GetComponent<PlayerMovementController>();
        playerStatsManager = GetComponent<PlayerStatsManager>();
    }

    public void HandleAllMovement() {
        GetMovementInput();
        HandleGroundedMovement();
        HandleRotation();
    }

    private void HandleGroundedMovement() {
        if (!playerMovementController.canMove) return;

        moveDirection = GetGroundedDirection(moveDirection, horizontalMovement, verticalMovement);

        if (true) {
            switch (PlayerInputController.Instance.moveValue) {
                case 1f:
                    playerMovementController.characterController.Move(moveDirection * runSpeed * Time.deltaTime);
                    break;
                case 0.5f:
                    playerMovementController.characterController.Move(moveDirection * walkSpeed * Time.deltaTime);
                    break;
                case 1.5f:
                    if (playerStatsManager.CurrentStam <= 0) {
                        PlayerInputController.Instance.sprintInput = false;
                        return;
                    }

                    playerMovementController.characterController.Move(moveDirection * sprintSpeed * Time.deltaTime);
                    playerStatsManager.CurrentStam -= CalculateSprintStamCost() * Time.deltaTime;
                    break;
            }
        }
    }

    private void HandleRotation() {
        if (!playerMovementController.canRotate) return;

        targetRotationDirection = Vector3.zero;
        targetRotationDirection = GetGroundedDirection(targetRotationDirection, horizontalMovement, verticalMovement);

        if (targetRotationDirection == Vector3.zero) {
            targetRotationDirection = transform.forward;
        }

        if (moveDirection != Vector3.zero) {
            Quaternion newRotation = Quaternion.LookRotation(targetRotationDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, rotationSpeed * Time.deltaTime);
        }
    }

    public void AttemptToPerformDodge() {
        if (!playerMovementController.canDodge | playerStatsManager.CurrentStam <= 0) 
            return; // player is busy or out of stam, do not dodge

        if (horizontalMovement != 0 || verticalMovement != 0) {
            dodgeDirection = GetGroundedDirection(dodgeDirection, horizontalMovement, verticalMovement);
        } else {
            dodgeDirection = transform.forward; // the direction of the player current facing toward
        }

        Quaternion playerRotation = Quaternion.LookRotation(dodgeDirection);
        playerMovementController.transform.rotation = playerRotation;

        playerMovementController.playerAnimatorController.PlayTargetActionAnimation("B_RollForward");
        playerStatsManager.CurrentStam -= dodgeStamCost;
    }

    private void GetMovementInput() {
        horizontalMovement = PlayerInputController.Instance.HorizontalMovementInput();
        verticalMovement = PlayerInputController.Instance.VerticalMovementInput();
    }

    private Vector3 GetGroundedDirection(Vector3 direction, float horizontalValue, float verticalValue) {
        direction = PlayerCameraManager.Instance.transform.right * horizontalValue;
        direction = direction + PlayerCameraManager.Instance.transform.forward * verticalValue;
        direction.Normalize();
        direction.y = 0;

        return direction;
    }

    private float CalculateSprintStamCost() {
        return 5f + (playerStatsManager.totalStam * 0.1f);
    }
}
