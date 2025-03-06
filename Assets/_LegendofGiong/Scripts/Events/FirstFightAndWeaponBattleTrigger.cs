using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstFightAndWeaponBattleTrigger : MonoBehaviour {
    [SerializeField] private Collider triggerCollider;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip battleAudio;

    private void Awake() {
        // Only get component if not already assigned in inspector
        if (audioSource == null) {
            audioSource = GetComponentInParent<AudioSource>();
        }
    }

    public virtual void OnTriggerEnter(Collider other) {
        SceneData.Instance.hasStartFirstFightAndWeapon = true;
        FirstFightAndWeapon.Instance.ToggleTrapConstruct();

        audioSource.PlayOneShot(battleAudio, 1);
        gameObject.SetActive(false);
    }
}
