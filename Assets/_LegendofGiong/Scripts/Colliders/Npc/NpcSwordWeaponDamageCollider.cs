using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcSwordWeaponDamageCollider : DamageCollider {
    private NpcCharacterManager characterCausingDamage;

    protected override void Awake() {
        base.Awake();

        characterCausingDamage = GetComponentInParent<NpcCharacterManager>();
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

        damageTarget.characterEffectsManager.ProcessInstantEffect(damageEffect);
    }

}
