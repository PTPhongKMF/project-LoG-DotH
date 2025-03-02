using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorController : CharacterAnimatorManager {
    private PlayerMovementController playerMovementController;

    protected override void Awake() {
        base.Awake();

        playerMovementController = GetComponent<PlayerMovementController>();
    }


    public override void EnableCanDoCombo() {
        playerMovementController.playerCombatManager.canCombo = true;
    }

    public override void DisableCanDoCombo() {
        playerMovementController.playerCombatManager.canCombo = false;
    }
}
