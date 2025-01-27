using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorController : MonoBehaviour {
    private Animator animator;

    private int horizontal;
    private int vertical;

    private void Awake() {
        animator = GetComponent<Animator>();
        horizontal = Animator.StringToHash("Horizontal");
        vertical = Animator.StringToHash("Vertical");
    }

    public void UpdateAnimatorValues(float horizontalMovement, float verticalMovement) {
        float snappedHorizontal = SnapMovementValue(0.55f, 0.5f, 1f, horizontalMovement);
        float snappedVertical = SnapMovementValue(0.55f, 0.5f, 1f, verticalMovement);

        animator.SetFloat(horizontal, snappedHorizontal, 0.1f, Time.deltaTime);
        animator.SetFloat(vertical, snappedVertical, 0.1f, Time.deltaTime);
    }

    private float SnapMovementValue(float threshold, float snapValue1, float snapValue2, float movementValue) {
        if (movementValue > 0 && movementValue < threshold) {
            return snapValue1;
        } else if (movementValue > threshold) {
            return snapValue2;
        } else if (movementValue < 0 && movementValue > -threshold) {
            return -snapValue1;
        } else if (movementValue < -threshold) {
            return -snapValue2;
        } else {
            return 0f;
        }
    }
}
