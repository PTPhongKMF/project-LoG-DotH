using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : CharacterMovementManager {
    private PlayerLocomotionController playerLocomotionController;

    protected override void Awake() {
        base.Awake();
        playerLocomotionController = GetComponent<PlayerLocomotionController>();
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
