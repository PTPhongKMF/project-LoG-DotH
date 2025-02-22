using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Character Effects/Instant Effect/Take Damage")]
public class TakeDamageEffect : InstantCharacterEffect {
    public CharacterMovementManager characterCausingDamage;

    public float damageDealt = 0f; // pure dmg
    private float finalDamageDealt = 0f; // final dmg after calculate resist/armor/...

    public bool playDamageAnimation = true;
    public bool manuallySelectDamageAnimation = false;
    public string damageAnimation;

    public bool playDamageSfx = true;

    public float angleHitFrom;
    public Vector3 contactPoint;

    public override void ProcessEffect(CharacterMovementManager character) {
        base.ProcessEffect(character);

        if (character.isDead) return;
        //if (character.isInvulnerability) return;

        CalculateDamage(character);
    }

    private void CalculateDamage(CharacterMovementManager character) {
        if (characterCausingDamage != null) { 
            
        }

        finalDamageDealt = damageDealt;
        character.characterStatsManager.CurrentHealth -= finalDamageDealt;
    }
}
