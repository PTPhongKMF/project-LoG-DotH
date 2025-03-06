using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class PlayerMovementController : CharacterMovementManager {
    [HideInInspector] public PlayerLocomotionController playerLocomotionController;
    [HideInInspector] public PlayerAnimatorController playerAnimatorController;
    [HideInInspector] public PlayerStatsManager playerStatsManager;
    [HideInInspector] public PlayerInventoryManager playerInventoryManager;
    [HideInInspector] public PlayerEquipmentManager playerEquipmentManager;
    [HideInInspector] public PlayerCombatManager playerCombatManager;
    [HideInInspector] public PlayerSoundFXController playerSoundFXController;
    [HideInInspector] public PlayerInteractionManager playerInteractionManager;

    public bool isUsingLeftHand = false;
    public bool isUsingRightHand = false;

    // debug / testing
    public bool testing = false;

    private Vector3? setSpawnPoint = null;  // Nullable Vector3 to store custom spawn point

    protected override void Awake() {
        base.Awake();
        DontDestroyOnLoad(this);

        playerLocomotionController = GetComponent<PlayerLocomotionController>();
        playerAnimatorController = GetComponent<PlayerAnimatorController>();
        playerStatsManager = GetComponent<PlayerStatsManager>();
        playerInventoryManager = GetComponent<PlayerInventoryManager>();
        playerEquipmentManager = GetComponent<PlayerEquipmentManager>();
        playerCombatManager = GetComponent<PlayerCombatManager>();
        playerSoundFXController = GetComponent<PlayerSoundFXController>();
        playerInteractionManager = GetComponent<PlayerInteractionManager>();
    }

    protected override void Start() {
        base.Start();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        PlayerUIManager.Instance.playerUILoadingScreen.ActivateLoadingScreen();
        StartCoroutine(DelayedTeleport());
    }

    private IEnumerator DelayedTeleport() {
        // Initial delay to let scene load
        yield return new WaitForSeconds(0.5f);

        // Initial teleport
        Vector3 targetPosition;
        if (setSpawnPoint.HasValue) {
            targetPosition = setSpawnPoint.Value;
            setSpawnPoint = null;  // Clear the custom spawn point after use
        } else if (SceneData.Instance != null) {
            targetPosition = SceneData.Instance.playerDefaultSpawnPoint;
        } else {
            yield break; // No valid spawn point
        }

        transform.position = targetPosition;

        // Wait a frame to let physics update
        yield return new WaitForFixedUpdate();

        // Start checking if player is grounded
        float maxWaitTime = 5f; // Maximum time to wait for grounding
        float elapsedTime = 0f;
        float checkInterval = 0.1f; // How often to check and retry

        while (!isGrounded && elapsedTime < maxWaitTime) {
            // Slightly raise the position and try again
            transform.position = targetPosition + Vector3.up * 0.5f;
            
            elapsedTime += checkInterval;
            yield return new WaitForSeconds(checkInterval);
        }

        // Final safety check - if still not grounded, try one last time with more height
        if (!isGrounded) {
            transform.position = targetPosition + Vector3.up * 1f;
        }
    }

    public void SetCustomSpawnPoint(Vector3 spawnPoint) {
        setSpawnPoint = spawnPoint;
    }

    protected override void Update() {
        base.Update();
        if (playerLocomotionController != null) {
            playerLocomotionController.HandleAllMovement();
        }

        if (testing) {
            testing = false;
            PlayerUIManager.Instance.eatingMinigame.StartMinigame((bool won) => {
                if (won) {
                    Debug.Log("Player won the eating minigame!");
                } else {
                    Debug.Log("Player lost the eating minigame!");
                }
            });
        }
    }

    protected override void LateUpdate() {
        base.LateUpdate();
        PlayerCameraManager.Instance.HandleAllCameraAction();
    }

    public override IEnumerator ProcessDeathEvent(bool manuallySelectDeathAnimation = false) {
        PlayerUIManager.Instance.playerUIPopupManager.ShowYouDiedPopup();

        return base.ProcessDeathEvent(manuallySelectDeathAnimation);
    }

    public override void ReviveCharacter() {
        base.ReviveCharacter();

        playerStatsManager.CurrentHealth = playerStatsManager.totalHealth;
        playerStatsManager.CurrentStam = playerStatsManager.totalStam;

        // teleport to last checkpoint (maybe)

        // play revive animation (maybe)
    }

    public void SetPlayerActionHand(bool isRightHand) {
        if (isRightHand) {
            isUsingLeftHand = false;
            isUsingRightHand = true;
        } else {
            isUsingLeftHand = true;
            isUsingRightHand = false;
        }
    }
}
