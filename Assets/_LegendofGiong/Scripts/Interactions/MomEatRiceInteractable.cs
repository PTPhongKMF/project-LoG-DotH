using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MomEatRiceInteractable : Interactable {
    private bool isMinigameRunning = false;

    public override void Interact(PlayerMovementController player) {
        if (isMinigameRunning) return;

        base.Interact(player);
        isMinigameRunning = true;

        // Start the minigame
        PlayerUIManager.Instance.eatingMinigame.StartMinigame((bool won) => {
            isMinigameRunning = false;

            if (won) {
                Debug.Log("Mom proud!");
                int rewardPoints = Random.Range(1, 10);
                player.playerStatsManager.AddLevelPoint(rewardPoints);
                PlayerUIManager.Instance.minigameManager.ShowResultMinigame("you_won", rewardPoints);
            } else {
                PlayerUIManager.Instance.minigameManager.ShowResultMinigame("you_lost");
            }

            // Add random cooldown between 5-10 seconds
            StartCoroutine(ReEnableInteraction(Random.Range(5f, 10f)));
        });
    }

    private IEnumerator ReEnableInteraction(float cooldownTime) {
        yield return new WaitForSeconds(cooldownTime);
        interactableCollider.enabled = true;
    }

    public override void OnTriggerExit(Collider other) {
        base.OnTriggerExit(other);
        interactableCollider.enabled = true;
    }
}
