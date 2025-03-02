using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour {
    public MeleeWeaponDamageCollider meleeDamageCollider;

    private void Awake() {
        meleeDamageCollider = GetComponentInChildren<MeleeWeaponDamageCollider>();
    }

    public void SetWeaponDamage(CharacterMovementManager characterWielding, WeaponItem weapon) {
        meleeDamageCollider.characterCausingDamage = characterWielding;
        meleeDamageCollider.damageDealt = weapon.damage;

        meleeDamageCollider.light_Attack_01_Modifier = weapon.light_Attack_01_Modifier;
        meleeDamageCollider.light_Attack_02_Modifier = weapon.light_Attack_02_Modifier;
        meleeDamageCollider.light_Attack_03_Modifier = weapon.light_Attack_03_Modifier;
        meleeDamageCollider.special_Attack_01_Modifier = weapon.special_Attack_01_Modifier;
    }
}
