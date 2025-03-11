using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundRepeater : MonoBehaviour {
    private CharacterMovementManager character;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip sound;
    [SerializeField] private float minPitch = 0.9f;
    [SerializeField] private float maxPitch = 1.1f;
    [SerializeField] private float minRepeatTime = 5f;
    [SerializeField] private float maxRepeatTime = 20f;

    private Coroutine soundRoutine;

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
        character = GetComponent<CharacterMovementManager>();
    }

    private void Start() {
        soundRoutine = StartCoroutine(PlaySoundRoutine());
    }

    private IEnumerator PlaySoundRoutine() {
        while (true) {
            // Check if character is dead, if so, stop the coroutine
            if (character != null && character.isDead) {
                break;
            }

            // Randomize pitch
            audioSource.pitch = Random.Range(minPitch, maxPitch);
            
            // Play the sound
            audioSource.PlayOneShot(sound);

            // Wait for random time before next play
            float waitTime = Random.Range(minRepeatTime, maxRepeatTime);
            yield return new WaitForSeconds(waitTime);
        }
    }

    // Optional: Add a method to manually stop the sound routine
    public void StopSoundRoutine() {
        if (soundRoutine != null) {
            StopCoroutine(soundRoutine);
            soundRoutine = null;
        }
    }
}
