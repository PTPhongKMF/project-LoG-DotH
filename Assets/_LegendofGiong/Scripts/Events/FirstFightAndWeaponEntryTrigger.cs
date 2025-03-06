using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstFightAndWeaponEntryTrigger : MonoBehaviour {
    [SerializeField] private Collider triggerCollider;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip entryAudio;

    private void Awake() {
        if (audioSource == null)
            audioSource = GetComponentInParent<AudioSource>();
    }

    public virtual void OnTriggerEnter(Collider other) {
        PlayerMovementController player = other.GetComponent<PlayerMovementController>();
        PlayerUIManager.Instance.playerUIPopupManager.ToggleFirstFightPopup();

        audioSource.PlayOneShot(entryAudio, 1);
        gameObject.SetActive(false);
    }
}
