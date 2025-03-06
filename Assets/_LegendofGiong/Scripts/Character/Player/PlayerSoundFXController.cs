using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundFXController : CharacterSoundFXManager {
    private PlayerMovementController playerMovementController;
    private PlayerLocomotionController playerLocomotionController;
    
    private float footstepTimer = 0f;
    private float baseStepInterval = 0.5f; // Base interval for walking
    
    protected override void Awake() {
        base.Awake();
        playerMovementController = GetComponent<PlayerMovementController>();
        playerLocomotionController = GetComponent<PlayerLocomotionController>();
    }

    private void Update() {
        HandleFootsteps();
    }

    private void HandleFootsteps() {
        if (!playerMovementController.isGrounded || !playerMovementController.IsMoving) {
            footstepTimer = 0f;
            return;
        }

        // Calculate step interval based on movement speed
        float currentSpeed = PlayerInputController.Instance.moveValue;
        float stepInterval = GetStepInterval(currentSpeed);

        footstepTimer += Time.deltaTime;
        if (footstepTimer >= stepInterval) {
            PlayFootstepSound(currentSpeed);
            footstepTimer = 0f;
        }
    }

    private float GetStepInterval(float moveValue) {
        // Adjust intervals based on movement speed
        if (moveValue == 1.5f) { // Sprinting
            return baseStepInterval * 0.4f; // Faster steps while sprinting
        }
        else if (moveValue == 1f) { // Running
            return baseStepInterval * 0.6f; // Moderate step rate
        }
        else { // Walking
            return baseStepInterval; // Normal walking pace
        }
    }

    private void PlayFootstepSound(float moveValue) {
        AudioClip[] footstepArray;
        float volume = 0.3f; // Lowered base volume

        // Select appropriate footstep array based on movement speed
        if (moveValue == 1.5f) { // Sprinting
            footstepArray = WorldSoundFXManager.Instance.sprintFootstepSfx;
            volume = 0.3f; // 30% volume for sprint
        }
        else if (moveValue == 1f) { // Running
            footstepArray = WorldSoundFXManager.Instance.runFootstepSfx;
            volume = 0.2f; // 20% volume for running
        }
        else { // Walking
            footstepArray = WorldSoundFXManager.Instance.walkFootstepSfx;
            volume = 0.15f; // 15% volume for walking
        }

        AudioClip footstepSound = WorldSoundFXManager.Instance.ChooseRandomSoundFXFromArray(footstepArray);
        if (footstepSound != null) {
            PlaySoundFX(footstepSound, volume, true, 0.1f);
        }
    }
}
