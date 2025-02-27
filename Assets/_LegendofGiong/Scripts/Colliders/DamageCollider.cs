using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCollider : MonoBehaviour {
    protected Collider damageCollider;

    public float damageDealt = 0f; // pure dmg

    protected Vector3 contactPoint;

    protected List<CharacterMovementManager> damagedCharacter = new();

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Character") ||
            other.gameObject.layer == LayerMask.NameToLayer("Player")) {
            CharacterMovementManager damageTarget = other.GetComponentInParent<CharacterMovementManager>();

            if (damageTarget != null) {
                contactPoint = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);

                DealDamageToTarget(damageTarget);
            }
        }
    }

    protected virtual void DealDamageToTarget(CharacterMovementManager damageTarget) {
        if (damagedCharacter.Contains(damageTarget)) return;

        damagedCharacter.Add(damageTarget);

        TakeDamageEffect damageEffect = Instantiate(WorldCharacterEffectsManager.Instance.takeDamageEffect);
        damageEffect.damageDealt = damageDealt;
        damageEffect.contactPoint = contactPoint;

        damageTarget.characterEffectsManager.ProcessInstantEffect(damageEffect);
    }

    public virtual void EnableDamageCollider() {
        damageCollider.enabled = true;
    }

    public virtual void DisableDamageCollider() {
        damageCollider.enabled = false;
        damagedCharacter.Clear();
    }
}
