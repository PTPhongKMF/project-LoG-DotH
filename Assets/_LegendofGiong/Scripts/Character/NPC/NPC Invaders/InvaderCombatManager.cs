using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvaderCombatManager : NpcCombatManager {
    [SerializeField] NpcSwordWeaponDamageCollider swordWeaponDamageCollider;

    [SerializeField] int baseDamage = 25;
    [SerializeField] float attack_01_damage_modifier = 1f;
    [SerializeField] float attack_02_damage_modifier = 1.2f;
    [SerializeField] float attack_03_damage_modifier = 1.5f;
    [SerializeField] float attack_23_damage_modifier = 2f;

    public void SetAttack01Damage() {
        swordWeaponDamageCollider.damageDealt = baseDamage * attack_01_damage_modifier;
    }
    public void SetAttack02Damage() {
        swordWeaponDamageCollider.damageDealt = baseDamage * attack_02_damage_modifier;
    }
    public void SetAttack03Damage() {
        swordWeaponDamageCollider.damageDealt = baseDamage * attack_03_damage_modifier;
    }
    public void SetAttack23Damage() {
        swordWeaponDamageCollider.damageDealt = baseDamage * attack_23_damage_modifier;
    }

    public void OpenDamageCollider() {
        swordWeaponDamageCollider.EnableDamageCollider();
    }
    public void CloseDamageCollider() {
        swordWeaponDamageCollider.DisableDamageCollider();
    }
}
