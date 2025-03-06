using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeaponDamageCollider : DamageCollider {
    public CharacterMovementManager characterCausingDamage;

    public float light_Attack_01_Modifier;
    public float light_Attack_02_Modifier;
    public float light_Attack_03_Modifier;

    public float special_Attack_01_Modifier;

    protected override void Awake() {
        base.Awake();

        damageCollider.enabled = false;
    }

    protected override void OnTriggerEnter(Collider other) {
        CharacterMovementManager damageTarget = other.GetComponentInParent<CharacterMovementManager>();

        if (damageTarget != null) {
            if (damageTarget == characterCausingDamage) return;
            
            if (!WorldLayerUtilityManager.Instance.CanIDamageThisTarget(characterCausingDamage.characterGroup, damageTarget.characterGroup)) return;

            contactPoint = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
            DealDamageToTarget(damageTarget);
        }
    }

    protected override void DealDamageToTarget(CharacterMovementManager damageTarget) {
        if (damagedCharacter.Contains(damageTarget)) return;

        damagedCharacter.Add(damageTarget);

        TakeDamageEffect damageEffect = Instantiate(WorldCharacterEffectsManager.Instance.takeDamageEffect);
        damageEffect.characterCausingDamage = characterCausingDamage;
        damageEffect.damageDealt = damageDealt;
        damageEffect.contactPoint = contactPoint;

        Debug.Log($"[Attack Modifier] Current attack type: {characterCausingDamage.characterCombatManager.currentAttackType}");
        Debug.Log($"[Attack Modifier] Base damage before modifier: {damageEffect.damageDealt}");

        switch (characterCausingDamage.characterCombatManager.currentAttackType) {
            case AttackType.LightAttack01:
                Debug.Log($"[Attack Modifier] Applying Light Attack 1 modifier: {light_Attack_01_Modifier}x");
                ApplyAttackDamageModifiers(light_Attack_01_Modifier, damageEffect);
                break;
            case AttackType.LightAttack02:
                Debug.Log($"[Attack Modifier] Applying Light Attack 2 modifier: {light_Attack_02_Modifier}x");
                ApplyAttackDamageModifiers(light_Attack_02_Modifier, damageEffect);
                break;
            case AttackType.LightAttack03:
                Debug.Log($"[Attack Modifier] Applying Light Attack 3 modifier: {light_Attack_03_Modifier}x");
                ApplyAttackDamageModifiers(light_Attack_03_Modifier, damageEffect);
                break;
            case AttackType.SpecialAttack01:
                Debug.Log($"[Attack Modifier] Applying Special Attack modifier: {special_Attack_01_Modifier}x");
                ApplyAttackDamageModifiers(special_Attack_01_Modifier, damageEffect);
                break;
        }

        Debug.Log($"[Attack Modifier] Final damage after modifier: {damageEffect.damageDealt}");
        damageTarget.characterEffectsManager.ProcessInstantEffect(damageEffect);
    }

    private void ApplyAttackDamageModifiers(float modifier, TakeDamageEffect damage) {
        damage.damageDealt *= modifier;
    }
}
