using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponItemAction : ScriptableObject {
    public int actionId;

    public virtual void AttemptToPerformAction(PlayerMovementController playerPerforming, WeaponItem weaponPerforming) {

    }
}
