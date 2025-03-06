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
        PlayDamageSfx(character);
        PlayDamageVfx(character);
    }

    private void CalculateDamage(CharacterMovementManager character) {
        Debug.Log($"[Damage Calculation] Base weapon damage: {damageDealt}");
        
        if (characterCausingDamage != null) { 
            float attackMultiplier = characterCausingDamage.characterStatsManager.totalAttack / 50f;
            Debug.Log($"[Damage Calculation] Attacker's total attack: {characterCausingDamage.characterStatsManager.totalAttack}");
            Debug.Log($"[Damage Calculation] Attack multiplier: {attackMultiplier}");
            
            // Apply attacker's attack stats to the base damage
            damageDealt *= attackMultiplier;
            Debug.Log($"[Damage Calculation] Final damage after attack multiplier: {damageDealt}");
        }

        finalDamageDealt = damageDealt;
        Debug.Log($"[Damage Calculation] Final damage dealt to {character.name}: {finalDamageDealt}");
        character.characterStatsManager.CurrentHealth -= finalDamageDealt;
    }

    private void PlayDamageVfx(CharacterMovementManager character) {
        character.characterEffectsManager.PlayBloodSplatterVfx(contactPoint);
    }

    private void PlayDamageSfx(CharacterMovementManager character) {
        AudioClip damageSfx = WorldSoundFXManager.Instance.ChooseRandomSoundFXFromArray(WorldSoundFXManager.Instance.bladeDamageSfx);

        character.characterSoundFXManager.PlaySoundFX(damageSfx, 0.6f);
        character.characterSoundFXManager.PlayDamageGruntSFX();
    }
}
