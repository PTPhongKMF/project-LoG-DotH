using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundRepeater : MonoBehaviour {
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip sound;
    [SerializeField] private float minPitch = 0.9f;
    [SerializeField] private float maxPitch = 1.1f;
    [SerializeField] private float minRepeatTime = 5f;
    [SerializeField] private float maxRepeatTime = 20f;

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start() {
        StartCoroutine(PlaySoundRoutine());
    }

    private IEnumerator PlaySoundRoutine() {
        while (true) {
            // Randomize pitch
            audioSource.pitch = Random.Range(minPitch, maxPitch);
            
            // Play the sound
            audioSource.PlayOneShot(sound);

            // Wait for random time before next play
            float waitTime = Random.Range(minRepeatTime, maxRepeatTime);
            yield return new WaitForSeconds(waitTime);
        }
    }
}
