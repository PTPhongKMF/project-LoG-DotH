using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour {
    private MeleeWeaponDamageCollider meleeDamageCollider;

    private void Awake() {
        meleeDamageCollider = GetComponentInChildren<MeleeWeaponDamageCollider>();
    }

    public void SetWeaponDamage(CharacterMovementManager characterWielding, WeaponItem weapon) {
        meleeDamageCollider.characterCausingDamage = characterWielding;
        meleeDamageCollider.damageDealt = weapon.damage;
    }
}
