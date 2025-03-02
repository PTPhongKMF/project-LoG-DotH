using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "NPC/AI States/Attack")]
public class NpcAttackState : NpcAIState {
    [HideInInspector] public NpcAttackAction currentAttack;
    [HideInInspector] public bool willPerformCombo = false;

    protected bool hasPerformedAttack = false;
    protected bool hasPerformedCombo = false;

    [SerializeField] protected bool pivotAfterAttack = false;

    public override NpcAIState Tick(NpcCharacterManager npcCharacter) {
        if (npcCharacter.npcCombatManager.currentTarget == null)
            return SwitchState(npcCharacter, npcCharacter.idleState);

        if (npcCharacter.npcCombatManager.currentTarget.isDead)
            return SwitchState(npcCharacter, npcCharacter.idleState);

        npcCharacter.npcCombatManager.RotateTowardsTargetWhilstAttacking(npcCharacter);
        npcCharacter.characterAnimatorManager.UpdateAnimatorMovementValues(0, 0);

        if (willPerformCombo && !hasPerformedCombo) {
            if (currentAttack.comboAction != null) {

                hasPerformedCombo = true;
                currentAttack.comboAction.AttemptToPerformAction(npcCharacter);
            }
        }

        if (npcCharacter.isPerformingAction) return this;

        if (!hasPerformedAttack) {
            if (npcCharacter.npcCombatManager.actionRecoveryTimer > 0) return this;
            PerformAttack(npcCharacter);

            return this;
        }

        if (pivotAfterAttack) npcCharacter.npcCombatManager.PivotTowardsTarget(npcCharacter);

        return SwitchState(npcCharacter, npcCharacter.combatStanceState);
    }

    protected void PerformAttack(NpcCharacterManager npcCharacter) {
        hasPerformedAttack = true;
        currentAttack.AttemptToPerformAction(npcCharacter);
        npcCharacter.npcCombatManager.actionRecoveryTimer = currentAttack.actionRecoveryTime;
    }

    protected override void ResetStateFlags(NpcCharacterManager npcCharacter) {
        base.ResetStateFlags(npcCharacter);

        hasPerformedAttack = false;
        hasPerformedCombo = false;
    }
}
