using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeaponDamageCollider : DamageCollider {
    public CharacterMovementManager characterCausingDamage;

    public float light_Attack_01_Modifier;

    protected override void Awake() {
        base.Awake();

        damageCollider.enabled = false;
    }

    protected override void OnTriggerEnter(Collider other) {
        CharacterMovementManager damageTarget = other.GetComponentInParent<CharacterMovementManager>();


        if (damageTarget != null) {
            if (damageTarget == characterCausingDamage) return;

            contactPoint = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
            DealDamageToTarget(damageTarget);
        }
    }

    protected override void DealDamageToTarget(CharacterMovementManager damageTarget) {
        if (damagedCharacter.Contains(damageTarget)) return;

        damagedCharacter.Add(damageTarget);

        TakeDamageEffect damageEffect = Instantiate(WorldCharacterEffectsManager.Instance.takeDamageEffect);
        damageEffect.damageDealt = damageDealt;
        damageEffect.contactPoint = contactPoint;

        switch (characterCausingDamage.characterCombatManager.currentAttackType) {
            case AttackType.LightAttack01:
                ApplyAttackDamageModifiers(light_Attack_01_Modifier, damageEffect);
                break;
        }

        damageTarget.characterEffectsManager.ProcessInstantEffect(damageEffect);
    }

    private void ApplyAttackDamageModifiers(float modifier, TakeDamageEffect damage) {
        damage.damageDealt *= modifier;
    }
}
