using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "NPC/AI States/Combat Stance")]
public class NpcCombatStanceState : NpcAIState {
    public List<NpcAttackAction> npcAttacks;        // list of all attack
    protected List<NpcAttackAction> potentialAttacks; // list of possible attack to perform

    private NpcAttackAction currentAttack;
    private NpcAttackAction previousAttack;
    protected bool hasAttack = false;

    [SerializeField] protected bool canPerformCombo = false;
    [SerializeField] protected int chanceToPerformCombo = 25;
    protected bool hasRolledForComboChance = false;

    [SerializeField] public float maximumEngagementDistance = 5;

    public override NpcAIState Tick(NpcCharacterManager npcCharacter) {
        if (npcCharacter.isPerformingAction) return this;

        if (!npcCharacter.navMeshAgent.enabled) npcCharacter.navMeshAgent.enabled = true;

        if (!npcCharacter.IsMoving)
            if (npcCharacter.npcCombatManager.viewableAngle < -30 || npcCharacter.npcCombatManager.viewableAngle > 30)
                npcCharacter.npcCombatManager.PivotTowardsTarget(npcCharacter);

        npcCharacter.npcCombatManager.RotateTowardsAgent(npcCharacter);

        if (npcCharacter.npcCombatManager.currentTarget == null)
            return SwitchState(npcCharacter, npcCharacter.idleState);

        if (!hasAttack) {
            GetNewAttack(npcCharacter);
        } else {
            npcCharacter.attackState.currentAttack = currentAttack;
            return SwitchState(npcCharacter, npcCharacter.attackState);
        }

        if (npcCharacter.npcCombatManager.distanceFromTarget > maximumEngagementDistance)
            return SwitchState(npcCharacter, npcCharacter.pursueTargetState);

        NavMeshPath path = new NavMeshPath();
        npcCharacter.navMeshAgent.CalculatePath(npcCharacter.npcCombatManager.currentTarget.transform.position, path);
        npcCharacter.navMeshAgent.SetPath(path);

        return this;
    }

    protected virtual void GetNewAttack(NpcCharacterManager npcCharacter) {
        potentialAttacks = new List<NpcAttackAction>();

        foreach (NpcAttackAction potentialAttack in npcAttacks) {
            if (potentialAttack.minimumAttackDistance > npcCharacter.npcCombatManager.distanceFromTarget)
                continue;

            if (potentialAttack.maximumAttackDistance < npcCharacter.npcCombatManager.distanceFromTarget)
                continue;

            if (potentialAttack.minimumAttackAngle > npcCharacter.npcCombatManager.viewableAngle)
                continue;

            if (potentialAttack.maximumAttackAngle < npcCharacter.npcCombatManager.viewableAngle)
                continue;

            potentialAttacks.Add(potentialAttack);
        }

        if (potentialAttacks.Count <= 0) return;

        int totalWeight = 0;

        foreach (NpcAttackAction attack in potentialAttacks) {
            totalWeight += attack.attackWeight;
        }
        int randomWeightValue = Random.Range(1,  totalWeight + 1);
        int processedWeight = 0;

        foreach (NpcAttackAction attack in potentialAttacks) {
            processedWeight += attack.attackWeight;

            if (randomWeightValue <= processedWeight) {
                currentAttack = attack;
                previousAttack = currentAttack;
                hasAttack = true;
                return;
            }
        }
    }

    protected virtual bool RollForOutcomeChance(int outcomeChance) {
        bool outcome = false;
        int randomPercentage = Random.Range(0, 100);

        if (randomPercentage < outcomeChance) outcome = true;

        return outcome;
    }

    protected override void ResetStateFlags(NpcCharacterManager npcCharacter) {
        base.ResetStateFlags(npcCharacter);

        hasRolledForComboChance = false;
        hasAttack = false;
    }
}
