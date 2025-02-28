using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class B_RollRight_Behaviour : StateMachineBehaviour {
    PlayerLocomotionController playerLocomotionController;

    [SerializeField] private float peakSpeedMultiplier = 1f;   // 100% at peak
    [SerializeField] private float finalSpeed = 0.01f;         // 1% end speed

    // Frame timing conversion (based on 45 total frames):
    // Frame 6  = 6/45  about 0.133
    // Frame 28 = 28/45 about 0.622
    // Frame 30 = 30/45 about 0.667
    [SerializeField] private float startMovementPoint = 0.133f;  // Frame 6
    [SerializeField] private float decelerationStart = 0.622f;  // Frame 28
    [SerializeField] private float decelerationEnd = 0.667f;    // Frame 30

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (playerLocomotionController == null) {
            playerLocomotionController = animator.GetComponent<PlayerLocomotionController>();
        }
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        float normalizedTime = stateInfo.normalizedTime % 1;
        float speedMultiplier;

        if (normalizedTime < startMovementPoint) {
            speedMultiplier = 0f;
        } else if (normalizedTime < decelerationStart) {
            speedMultiplier = peakSpeedMultiplier;
        } else if (normalizedTime < decelerationEnd) {
            float t = (normalizedTime - decelerationStart) / (decelerationEnd - decelerationStart);
            speedMultiplier = Mathf.Lerp(peakSpeedMultiplier, finalSpeed, t);
        } else {
            speedMultiplier = finalSpeed;
        }

        float adjustedSpeed = playerLocomotionController.dodgeSpeed * speedMultiplier;
        Vector3 rightDirection = playerLocomotionController.playerMovementController.transform.right;

        playerLocomotionController.playerMovementController.characterController.Move(
            rightDirection * adjustedSpeed * Time.deltaTime
        );
    }
} 