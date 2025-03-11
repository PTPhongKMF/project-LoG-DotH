using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour {
    public MeleeWeaponDamageCollider meleeDamageCollider;

    private void Awake() {
        meleeDamageCollider = GetComponentInChildren<MeleeWeaponDamageCollider>();
    }

    public void SetWeaponDamage(CharacterMovementManager characterWielding, WeaponItem weapon) {
        Debug.Log($"[Weapon Setup] Setting up weapon: {weapon.name}");
        Debug.Log($"[Weapon Setup] Base weapon damage: {weapon.damage}");
        Debug.Log($"[Weapon Setup] Attack modifiers:");
        Debug.Log($"  Light Attack 1: {weapon.light_Attack_01_Modifier}x");
        Debug.Log($"  Light Attack 2: {weapon.light_Attack_02_Modifier}x");
        Debug.Log($"  Light Attack 3: {weapon.light_Attack_03_Modifier}x");
        Debug.Log($"  Special Attack: {weapon.special_Attack_01_Modifier}x");

        meleeDamageCollider.characterCausingDamage = characterWielding;
        meleeDamageCollider.damageDealt = weapon.damage;

        meleeDamageCollider.light_Attack_01_Modifier = weapon.light_Attack_01_Modifier;
        meleeDamageCollider.light_Attack_02_Modifier = weapon.light_Attack_02_Modifier;
        meleeDamageCollider.light_Attack_03_Modifier = weapon.light_Attack_03_Modifier;
        meleeDamageCollider.special_Attack_01_Modifier = weapon.special_Attack_01_Modifier;
    }
}
