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
    public float jumpHeight = 1f;
    public float jumpSpeed = 7f;
    public Vector3 jumpDirection;

    public float aerialSpeed = 5f;
    private Vector3 aerialDirection;

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
        HandleJumpMovement();
        HandleAerialMovement();
    }

    private void HandleGroundedMovement() {
        if (!playerMovementController.canMove) return;

        moveDirection = GetDirection(horizontalMovement, verticalMovement);

        if (playerMovementController.isAttacking) {
            playerMovementController.characterController.Move(moveDirection * runSpeed * Time.deltaTime);
            return;
        }

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

    private void HandleAerialMovement() {
        if (!playerMovementController.isGrounded) {
            aerialDirection = GetDirection(horizontalMovement, verticalMovement);

            playerMovementController.characterController.Move(aerialDirection * aerialSpeed * Time.deltaTime);
        }
    }

    private void HandleJumpMovement() {
        if (playerMovementController.isJumping) {
            playerMovementController.characterController.Move(jumpDirection * jumpSpeed * Time.deltaTime);
        }
    }

    private void HandleRotation() {
        if (!playerMovementController.canRotate) return;
        if (playerMovementController.isArmed) return; 

        targetRotationDirection = Vector3.zero;
        targetRotationDirection = GetDirection(horizontalMovement, verticalMovement);

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

        if (!playerMovementController.isGrounded) return;

        if (horizontalMovement != 0 || verticalMovement != 0) {
            dodgeDirection = GetDirection(horizontalMovement, verticalMovement);
        } else {
            dodgeDirection = transform.forward; // the direction of the player current facing toward
        }

        Quaternion playerRotation = Quaternion.LookRotation(dodgeDirection);
        playerMovementController.transform.rotation = playerRotation;

        playerMovementController.playerAnimatorController.PlayTargetActionAnimation("B_RollForward", AnimationSettings.IsPerformingAction | AnimationSettings.IsGrounded);
        playerStatsManager.CurrentStam -= dodgeStamCost;
    }

    public void AttemptToPerformJump() {
        if (!playerMovementController.canJump) return; // player is busy
        if (playerMovementController.isJumping) return;
        if (!playerMovementController.isGrounded) return;

        playerMovementController.playerAnimatorController.PlayTargetActionAnimation("Jump_Up", AnimationSettings.IsPerformingAction | AnimationSettings.IsJumping |
                                                                                               AnimationSettings.CanRotate);
        jumpDirection = GetDirection(horizontalMovement, verticalMovement);

        if (jumpDirection != Vector3.zero) {
            switch (PlayerInputController.Instance.moveValue) {
                case 1f:
                    jumpDirection *= 1f;
                    break;
                case 0.5f:
                    jumpDirection *= 0.5f;
                    break;
                case 1.5f:
                    jumpDirection *= 1.5f;
                    break;
            }
        }
        ApplyJumpingVelocity();
    }

    public void ApplyJumpingVelocity() {
        yVelocity.y = Mathf.Sqrt(jumpHeight * -2 * gravityForce);
    }

    private void GetMovementInput() {
        horizontalMovement = PlayerInputController.Instance.HorizontalMovementInput();
        verticalMovement = PlayerInputController.Instance.VerticalMovementInput();
    }

    private Vector3 GetDirection(float horizontalValue, float verticalValue) {
        Vector3 direction = Vector3.zero;
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
