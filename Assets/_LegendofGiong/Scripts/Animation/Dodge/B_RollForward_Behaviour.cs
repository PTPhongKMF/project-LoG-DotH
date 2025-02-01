using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class B_RollForward_Behaviour : StateMachineBehaviour {
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

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (playerLocomotionController == null) {
            playerLocomotionController = animator.GetComponent<PlayerLocomotionController>();
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        float normalizedTime = stateInfo.normalizedTime % 1;
        float speedMultiplier;

        if (normalizedTime < startMovementPoint) {
            // No movement until frame 6
            speedMultiplier = 0f;
        } else if (normalizedTime < decelerationStart) {
            // Full speed from frame 6 to 28
            speedMultiplier = peakSpeedMultiplier;
        } else if (normalizedTime < decelerationEnd) {
            // Sharp deceleration from frame 28 to 30
            float t = (normalizedTime - decelerationStart) / (decelerationEnd - decelerationStart);
            speedMultiplier = Mathf.Lerp(peakSpeedMultiplier, finalSpeed, t);
        } else {
            // Maintain minimal speed from frame 30 to end
            speedMultiplier = finalSpeed;
        }

        float adjustedSpeed = playerLocomotionController.dodgeSpeed * speedMultiplier;

        playerLocomotionController.playerMovementController.characterController.Move(
            playerLocomotionController.dodgeDirection * adjustedSpeed * Time.deltaTime
        );
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
