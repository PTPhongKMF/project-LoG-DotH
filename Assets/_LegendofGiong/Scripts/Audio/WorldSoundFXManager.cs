using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSoundFXManager : MonoBehaviour {
    private static WorldSoundFXManager instance;
    public static WorldSoundFXManager Instance {
        get => instance;
        private set => instance = value;
    }

    public AudioClip dodgeSFX;

    public AudioClip[] drawBladeSfx;
    public AudioClip[] bladeDamageSfx;
    public AudioClip[] weaponSwingSfx; // Array for different weapon swing sounds
    public AudioClip[] punchSwingSfx; // Array for different punch swing sounds

    public AudioClip[] deathSfx;
    public AudioClip[] attackGruntSfx;
    public AudioClip[] damageGruntSfx;

    // Footstep sounds for different surfaces
    [Header("Footstep Sounds")]
    public AudioClip[] walkFootstepSfx;
    public AudioClip[] runFootstepSfx;
    public AudioClip[] sprintFootstepSfx;

    private void Awake() {
        // there can only be one of this instance script at one time, if another exist, destroy it
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    public AudioClip ChooseRandomSoundFXFromArray(AudioClip[] array) {
        if (array == null || array.Length == 0) return null;
        int index = Random.Range(0, array.Length);
        return array[index];
    }
}
