using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : CharacterMovementManager {
    [HideInInspector] public PlayerLocomotionController playerLocomotionController;
    [HideInInspector] public PlayerAnimatorController playerAnimatorController;
    [HideInInspector] public PlayerStatsManager playerStatsManager;
    [HideInInspector] public PlayerInventoryManager playerInventoryManager;

    protected override void Awake() {
        base.Awake();
        playerLocomotionController = GetComponent<PlayerLocomotionController>();
        playerAnimatorController = GetComponent<PlayerAnimatorController>();
        playerStatsManager = GetComponent<PlayerStatsManager>();
        playerInventoryManager = GetComponent<PlayerInventoryManager>();
    }

    protected override void Start() {

    }

    protected override void Update() {
        base.Update();
        playerLocomotionController.HandleAllMovement(); 
    }

    protected override void LateUpdate() {
        base.LateUpdate();
        PlayerCameraManager.Instance.HandleAllCameraAction();
    }

    public override IEnumerator ProcessDeathEvent(bool manuallySelectDeathAnimation = false) {
        PlayerUIManager.Instance.playerUIPopupManager.ShowYouDiedPopup();

        return base.ProcessDeathEvent(manuallySelectDeathAnimation);
    }

    public override void ReviveCharacter() {
        base.ReviveCharacter();

        playerStatsManager.CurrentHealth = playerStatsManager.totalHealth;
        playerStatsManager.CurrentStam = playerStatsManager.totalStam;

        // teleport to last checkpoint (maybe)

        // play revive animation (maybe)
    }
}
