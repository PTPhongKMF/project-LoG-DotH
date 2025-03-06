using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessengerInteractable : Interactable {
    private Animator animator;
    private string[] anim = new string[]{ "talk1", "talk2" };

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip notYet;
    [SerializeField] private AudioClip ready;

    protected override void Awake() {
        base.Awake();

        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    public override void Interact(PlayerMovementController player) {
        base.Interact(player);

        if (player.playerStatsManager.GetLevelPoint() < 20) {
            audioSource.PlayOneShot(notYet, 1);
            animator.CrossFade(anim[Random.Range(0, 2)], 0.2f);
        } else {
            audioSource.PlayOneShot(ready, 1);
            StartCoroutine(Delayshowing(ready.length));
        }

        StartCoroutine(ReEnableInteraction(Random.Range(5f, 10f)));
    }

    private IEnumerator ReEnableInteraction(float cooldownTime) {
        yield return new WaitForSeconds(cooldownTime + 10f);
        interactableCollider.enabled = true;
    }

    private IEnumerator Delayshowing(float delay) {
        yield return new WaitForSeconds(delay);
        PlayerUIManager.Instance.playerUIPopupManager.ToggleToWarPopup();
    }

    public override void OnTriggerExit(Collider other) {
        base.OnTriggerExit(other);
        interactableCollider.enabled = true;
    }
}
