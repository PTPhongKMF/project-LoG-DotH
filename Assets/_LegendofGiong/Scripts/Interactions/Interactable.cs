using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour {
    public string interactableText;
    [SerializeField] protected Collider interactableCollider;

    protected virtual void Awake() {
        if (interactableCollider == null)
            interactableCollider = GetComponent<Collider>();
    }

    protected virtual void Start() {

    }

    public virtual void Interact(PlayerMovementController player) {
        interactableCollider.enabled = false;
        player.playerInteractionManager.RemoveInteractionFromList(this);
        PlayerUIManager.Instance.playerUIPopupManager.CloseAllPopupWindows();

        Debug.Log("You have interacted!");
    }

    public virtual void OnTriggerEnter(Collider other) {
        PlayerMovementController player = other.GetComponent<PlayerMovementController>();

        if (player != null) {
            player.playerInteractionManager.AddInteractionToList(this);
        }
    }

    public virtual void OnTriggerExit(Collider other) {
        PlayerMovementController player = other.GetComponent<PlayerMovementController>();

        if (player != null) {
            player.playerInteractionManager.RemoveInteractionFromList(this);
            PlayerUIManager.Instance.playerUIPopupManager.CloseAllPopupWindows();
        }
    }
}
