using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractionManager : MonoBehaviour {
    private PlayerMovementController player;

    private List<Interactable> currentInteractableActions;

    private void Awake() {
        player = GetComponent<PlayerMovementController>();
    }

    private void Start() {
        currentInteractableActions = new List<Interactable>();
    }

    private void FixedUpdate() {
        if (!PlayerUIManager.Instance.menuWindowIsOpen && !PlayerUIManager.Instance.popupWindowIsOpen)
            CheckForInteractable();
    }

    private void CheckForInteractable() {
        if (currentInteractableActions.Count == 0)
            return;

        if (currentInteractableActions[0] == null){
            currentInteractableActions.RemoveAt(0);
            return;
        }

        if (currentInteractableActions[0] != null)
            PlayerUIManager.Instance.playerUIPopupManager.SendPlayerMessagePopup(currentInteractableActions[0].interactableText);
    }

    private void RefreshInteractionList() {
        for (int i = currentInteractableActions.Count - 1; i > -1; i--) {
            if (currentInteractableActions[i] == null)
                currentInteractableActions.RemoveAt(i);
        }
    }

    public void AddInteractionToList(Interactable interactableObject) {
        RefreshInteractionList();

        if (!currentInteractableActions.Contains(interactableObject))
            currentInteractableActions.Add(interactableObject); 
    }

    public void RemoveInteractionFromList(Interactable interactableObject) {
        if (currentInteractableActions.Contains(interactableObject))
            currentInteractableActions.Remove(interactableObject);

        RefreshInteractionList();
    }


    public void Interact() {
        if (currentInteractableActions.Count == 0)
            return;

        if (currentInteractableActions[0] != null) {
            currentInteractableActions[0].Interact(player);
            RefreshInteractionList();
        }
    }
}
