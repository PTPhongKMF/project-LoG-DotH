using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSoundFXManager : MonoBehaviour {
    private AudioSource audioSource;

    protected virtual void Awake() {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySoundFX(AudioClip soundFX, float volume = 1, bool randomizePitch = true, float pitchRandom = 0.1f) {
        // Store original pitch
        float originalPitch = audioSource.pitch;

        // Set random pitch if needed
        if (randomizePitch) {
           audioSource.pitch = 1 + Random.Range(-pitchRandom, pitchRandom);
        }

        // Play the sound
        audioSource.PlayOneShot(soundFX, volume);

        // Reset pitch back to original
        audioSource.pitch = originalPitch;
    }

    public void PlayDodgeSFX() {
        audioSource.PlayOneShot(WorldSoundFXManager.Instance.dodgeSFX);
    }

    public void PlayWeaponSwingSFX() {
        if (WorldSoundFXManager.Instance.weaponSwingSfx != null && WorldSoundFXManager.Instance.weaponSwingSfx.Length > 0) {
            AudioClip swingSound = WorldSoundFXManager.Instance.ChooseRandomSoundFXFromArray(WorldSoundFXManager.Instance.weaponSwingSfx);
            PlaySoundFX(swingSound, 0.5f); // Slightly lower volume for swing sounds
        }
    }

    public void PlayPunchSwingSFX() {
        if (WorldSoundFXManager.Instance.punchSwingSfx != null && WorldSoundFXManager.Instance.punchSwingSfx.Length > 0) {
            AudioClip punchSound = WorldSoundFXManager.Instance.ChooseRandomSoundFXFromArray(WorldSoundFXManager.Instance.punchSwingSfx);
            PlaySoundFX(punchSound, 0.5f); // Slightly lower volume for punch sounds
        }
    }

    public void PlayAttackGruntSFX() {
        AudioClip gruntSound = WorldSoundFXManager.Instance.ChooseRandomSoundFXFromArray(WorldSoundFXManager.Instance.attackGruntSfx);
        PlaySoundFX(gruntSound, 0.8f); // Slightly lower volume for punch sounds
    }

    public void PlayDamageGruntSFX() {
        AudioClip gruntSound = WorldSoundFXManager.Instance.ChooseRandomSoundFXFromArray(WorldSoundFXManager.Instance.damageGruntSfx);
        PlaySoundFX(gruntSound, 0.7f); // Slightly lower volume for punch sounds
    }

    public void PlayDeathSFX() {
        AudioClip deathSound = WorldSoundFXManager.Instance.ChooseRandomSoundFXFromArray(WorldSoundFXManager.Instance.deathSfx);
        PlaySoundFX(deathSound);
    }

    public void PlayDrawBladeSFX() {
        AudioClip drawSound = WorldSoundFXManager.Instance.ChooseRandomSoundFXFromArray(WorldSoundFXManager.Instance.drawBladeSfx);
        PlaySoundFX(drawSound, 0.5f);
    }
}
