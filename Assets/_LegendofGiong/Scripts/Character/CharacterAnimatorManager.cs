using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimatorManager : MonoBehaviour {
    CharacterMovementManager characterMovementManager;

    public bool revertAnimatorRootMotionSetting = false;

    protected virtual void Awake() {
        characterMovementManager = GetComponent<CharacterMovementManager>();
    }

    public void UpdateAnimatorMovementValues(float horizontalValue, float verticalValue) {
        characterMovementManager.animator.SetFloat("Horizontal", horizontalValue, 0.1f, Time.deltaTime);
        characterMovementManager.animator.SetFloat("Vertical", verticalValue, 0.1f, Time.deltaTime);
    }

    public virtual void PlayTargetActionAnimation
        (string targetAnimation, bool isPerformingAction = true, bool canMove = false, bool canRotate = false, bool canDodge = false, 
        bool? applyRootMotion = null, bool revertApplyRootMotion = false) {
        if (applyRootMotion.HasValue) {
            characterMovementManager.animator.applyRootMotion = applyRootMotion.Value;

            if (revertApplyRootMotion) revertAnimatorRootMotionSetting = true;
        }

        characterMovementManager.animator.CrossFade(targetAnimation, 0.2f);
        characterMovementManager.isPerformingAction = isPerformingAction;
        characterMovementManager.canMove = canMove;
        characterMovementManager.canRotate = canRotate;
        characterMovementManager.canDodge = canDodge;
    }

    public virtual void RevertAnimatorRootMotion() {
        if (revertAnimatorRootMotionSetting) {
            revertAnimatorRootMotionSetting = false;
            characterMovementManager.animator.applyRootMotion = !characterMovementManager.animator.applyRootMotion;
        }
    }
}
