using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatManager : CharacterCombatManager {
    private PlayerMovementController playerCharacter;

    public WeaponItem currentWeaponBeingUsed;

    protected override void Awake() {
        base.Awake();

        playerCharacter = GetComponent<PlayerMovementController>();
    }

    public void PerformWeaponBasedAction(WeaponItemAction weaponAction, WeaponItem weaponPerforming) {
        weaponAction.AttemptToPerformAction(playerCharacter, weaponPerforming);
    }
}
