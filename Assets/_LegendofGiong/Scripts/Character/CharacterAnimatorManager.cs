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

    //public virtual void PlayTargetActionAnimation(string targetAnimation, bool isPerformingAction = true,
    //                                              bool canMove = false, bool canRotate = false, bool canDodge = false,
    //                                              bool canJump = false, bool isJumping = false,
    //                                              bool? applyRootMotion = null, bool revertApplyRootMotion = false) {
    //    if (applyRootMotion.HasValue) {
    //        characterMovementManager.animator.applyRootMotion = applyRootMotion.Value;

    //        if (revertApplyRootMotion) revertAnimatorRootMotionSetting = true;
    //    }

    //    characterMovementManager.animator.CrossFade(targetAnimation, 0.2f);
    //    characterMovementManager.isPerformingAction = isPerformingAction;
    //    characterMovementManager.canMove = canMove;
    //    characterMovementManager.canRotate = canRotate;
    //    characterMovementManager.canDodge = canDodge;
    //    characterMovementManager.canJump = canJump;
    //}
    public virtual void PlayTargetActionAnimation(string targetAnimation, AnimationSettings settings) {
        characterMovementManager.isPerformingAction = settings.HasFlag(AnimationSettings.IsPerformingAction);
        characterMovementManager.canMove = settings.HasFlag(AnimationSettings.CanMove);
        characterMovementManager.canRotate = settings.HasFlag(AnimationSettings.CanRotate);
        characterMovementManager.canDodge = settings.HasFlag(AnimationSettings.CanDodge);
        characterMovementManager.canJump = settings.HasFlag(AnimationSettings.CanJump);
        characterMovementManager.isJumping = settings.HasFlag(AnimationSettings.IsJumping);
        characterMovementManager.isGrounded = settings.HasFlag(AnimationSettings.IsGrounded);
        characterMovementManager.animator.applyRootMotion = settings.HasFlag(AnimationSettings.ApplyRootMotion);
        revertAnimatorRootMotionSetting = settings.HasFlag(AnimationSettings.RevertApplyRootMotion);

        characterMovementManager.animator.CrossFade(targetAnimation, 0.2f);
    }

    public virtual void PlayTargetAttackActionAnimation(string targetAnimation, AttackType attackType, AnimationSettings settings) {
        characterMovementManager.characterCombatManager.currentAttackType = attackType;
        
        characterMovementManager.isPerformingAction = settings.HasFlag(AnimationSettings.IsPerformingAction);
        characterMovementManager.canMove = settings.HasFlag(AnimationSettings.CanMove);
        characterMovementManager.canRotate = settings.HasFlag(AnimationSettings.CanRotate);
        characterMovementManager.canDodge = settings.HasFlag(AnimationSettings.CanDodge);
        characterMovementManager.canJump = settings.HasFlag(AnimationSettings.CanJump);
        characterMovementManager.isJumping = settings.HasFlag(AnimationSettings.IsJumping);
        characterMovementManager.isGrounded = settings.HasFlag(AnimationSettings.IsGrounded);
        characterMovementManager.isAttacking = settings.HasFlag(AnimationSettings.IsAttacking);
        characterMovementManager.animator.applyRootMotion = settings.HasFlag(AnimationSettings.ApplyRootMotion);
        revertAnimatorRootMotionSetting = settings.HasFlag(AnimationSettings.RevertApplyRootMotion);

        characterMovementManager.animator.CrossFade(targetAnimation, 0.2f);
    }

    public virtual void RevertAnimatorRootMotion() {
        if (revertAnimatorRootMotionSetting) {
            revertAnimatorRootMotionSetting = false;
            characterMovementManager.animator.applyRootMotion = !characterMovementManager.animator.applyRootMotion;
        }
    }
}
