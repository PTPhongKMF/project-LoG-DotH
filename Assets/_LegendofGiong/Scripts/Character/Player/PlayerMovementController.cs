using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : CharacterMovementManager {
    [HideInInspector] public PlayerLocomotionController playerLocomotionController;
    [HideInInspector] public PlayerAnimatorController playerAnimatorController;
    [HideInInspector] public PlayerStatsManager playerStatsManager;
    [HideInInspector] public PlayerInventoryManager playerInventoryManager;
    [HideInInspector] public PlayerEquipmentManager playerEquipmentManager;
    [HideInInspector] public PlayerCombatManager playerCombatManager;

    public bool isUsingLeftHand = false;
    public bool isUsingRightHand = false;

    // debug / testing
    public bool switchRight = false;

    protected override void Awake() {
        base.Awake();
        playerLocomotionController = GetComponent<PlayerLocomotionController>();
        playerAnimatorController = GetComponent<PlayerAnimatorController>();
        playerStatsManager = GetComponent<PlayerStatsManager>();
        playerInventoryManager = GetComponent<PlayerInventoryManager>();
        playerEquipmentManager = GetComponent<PlayerEquipmentManager>();
        playerCombatManager = GetComponent<PlayerCombatManager>();
    }

    protected override void Start() {
        base.Start();
    }

    protected override void Update() {
        base.Update();
        playerLocomotionController.HandleAllMovement();

        if (switchRight) {
            switchRight = false;
            playerEquipmentManager.SwitchRightWeapon();
        }
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

    public void SetPlayerActionHand(bool isRightHand) {
        if (isRightHand) {
            isUsingLeftHand = false;
            isUsingRightHand = true;
        } else {
            isUsingLeftHand = true;
            isUsingRightHand = false;
        }
    }
}
