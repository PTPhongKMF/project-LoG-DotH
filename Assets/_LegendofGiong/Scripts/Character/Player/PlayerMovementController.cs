using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : CharacterMovementManager {
    [HideInInspector] public PlayerLocomotionController playerLocomotionController;
    [HideInInspector] public PlayerAnimatorController playerAnimatorController;

    protected override void Awake() {
        base.Awake();
        playerLocomotionController = GetComponent<PlayerLocomotionController>();
        playerAnimatorController = GetComponent<PlayerAnimatorController>();
    }

    protected override void Update() {
        base.Update();
        playerLocomotionController.HandleAllMovement(); 
    }

    protected override void LateUpdate() {
        base.LateUpdate();
        PlayerCameraManager.Instance.HandleAllCameraAction();
    }
}
