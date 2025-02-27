using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Character Actions/Weapon Actions/Test Action")]
public class WeaponItemAction : ScriptableObject {
    public int actionId;

    public virtual void AttemptToPerformAction(PlayerMovementController playerPerforming, WeaponItem weaponPerforming) {
        playerPerforming.playerCombatManager.currentWeaponBeingUsed = Instantiate(weaponPerforming);
    }
}
