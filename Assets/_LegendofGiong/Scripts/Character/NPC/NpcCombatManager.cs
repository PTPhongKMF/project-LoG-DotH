using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class NpcCombatManager : CharacterCombatManager {
    public float distanceFromTarget;
    public float viewableAngle;
    public Vector3 targetsDirection;

    public float actionRecoveryTimer = 0;

    [SerializeField] private float detectionRadius = 15f;
    public float minimumFOV = -35f;
    public float maximumFOV = 35f;

    public float attackRotationSpeed = 25f;


    public void FindATargetViaLineOfSight(NpcCharacterManager npcCharacter) {
        if (currentTarget != null) return;

        Collider[] colliders = Physics.OverlapSphere(npcCharacter.transform.position, detectionRadius, WorldLayerUtilityManager.Instance.GetCharacterLayers());

        int collidersFound = colliders.Length;
        for (int i = 0; i < collidersFound; i++) {
            CharacterMovementManager targetCharacter = colliders[i].transform.GetComponent<CharacterMovementManager>();
            if (targetCharacter == null) continue;
            if (targetCharacter == npcCharacter) continue;
            if (targetCharacter.isDead) continue;

            if (WorldLayerUtilityManager.Instance.CanIDamageThisTarget(npcCharacter.characterGroup, targetCharacter.characterGroup)) {
                Vector3 targetDirection = targetCharacter.transform.position - npcCharacter.transform.position;
                float angleOfPotentialTarget = Vector3.Angle(targetDirection, npcCharacter.transform.forward);

                if (angleOfPotentialTarget > minimumFOV && angleOfPotentialTarget < maximumFOV) {
                    if (Physics.Linecast(npcCharacter.characterCombatManager.lockOnTransform.position,
                                        targetCharacter.characterCombatManager.lockOnTransform.position,
                                        WorldLayerUtilityManager.Instance.GetEnvironmentLayers())) {

                    } else {
                        targetsDirection = targetCharacter.transform.position - transform.position;
                        viewableAngle = WorldLayerUtilityManager.Instance.GetAngleOfTarget(transform, targetsDirection);
                        npcCharacter.characterCombatManager.currentTarget = targetCharacter;
                        PivotTowardsTarget(npcCharacter);
                    }
                }
            }
        }
    }

    public void PivotTowardsTarget(NpcCharacterManager npcCharacter) {
        if (npcCharacter.isPerformingAction) return;

        if (viewableAngle >= 45 && viewableAngle <= 135)
            npcCharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn_90_R", AnimationSettings.CanDodge | AnimationSettings.CanMove | AnimationSettings.CanRotate | AnimationSettings.IsGrounded | AnimationSettings.ApplyRootMotion);
        else if (viewableAngle <= -45 && viewableAngle >= -135)
            npcCharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn_90_L", AnimationSettings.CanDodge | AnimationSettings.CanMove | AnimationSettings.CanRotate | AnimationSettings.IsGrounded | AnimationSettings.ApplyRootMotion);

        if (viewableAngle >= 136 && viewableAngle <= 180)
            npcCharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn_180_R", AnimationSettings.CanDodge | AnimationSettings.CanMove | AnimationSettings.CanRotate | AnimationSettings.IsGrounded | AnimationSettings.ApplyRootMotion);
        else if (viewableAngle <= -136 && viewableAngle >= -180)
            npcCharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn_180_L", AnimationSettings.CanDodge | AnimationSettings.CanMove | AnimationSettings.CanRotate | AnimationSettings.IsGrounded | AnimationSettings.ApplyRootMotion);
    }

    public void HandleRecoveryTimer(NpcCharacterManager npcCharacter) {
        if (actionRecoveryTimer > 0) {
            if (!npcCharacter.isPerformingAction) {
                actionRecoveryTimer -= Time.deltaTime;
            }
        }
    }

    public void RotateTowardsAgent(NpcCharacterManager npcCharacter) {
        if (npcCharacter.IsMoving)
            npcCharacter.transform.rotation = npcCharacter.navMeshAgent.transform.rotation;
    }

    public void RotateTowardsTargetWhilstAttacking(NpcCharacterManager npcCharacter) {
        if (currentTarget == null) return;

        if (!npcCharacter.canRotate) return;

        if (!npcCharacter.isPerformingAction) return;

        Vector3 targetDirection = currentTarget.transform.position - npcCharacter.transform.position;
        targetDirection.y = 0;
        targetDirection.Normalize();

        if (targetDirection == Vector3.zero) targetDirection = npcCharacter.transform.forward;

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

        npcCharacter.transform.rotation = Quaternion.Slerp(npcCharacter.transform.rotation, targetRotation, attackRotationSpeed * Time.deltaTime);
    }

}

