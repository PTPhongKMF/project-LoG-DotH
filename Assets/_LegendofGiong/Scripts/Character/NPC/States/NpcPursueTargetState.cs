using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "NPC/AI States/Pursue Target")]
public class NpcPursueTargetState : NpcAIState {
    public override NpcAIState Tick(NpcCharacterManager npcCharacter) {
        if (npcCharacter.isPerformingAction) return this;

        if (npcCharacter.npcCombatManager.currentTarget == null) 
            return SwitchState(npcCharacter, npcCharacter.idleState);

        if (!npcCharacter.navMeshAgent.enabled)
            npcCharacter.navMeshAgent.enabled = true;

        if (npcCharacter.npcCombatManager.viewableAngle <  npcCharacter.npcCombatManager.minimumFOV
            || npcCharacter.npcCombatManager.viewableAngle > npcCharacter.npcCombatManager.maximumFOV)
            npcCharacter.npcCombatManager.PivotTowardsTarget(npcCharacter);

        npcCharacter.npcLocomotionManager.RotateTowardAgent(npcCharacter);

        if (npcCharacter.npcCombatManager.distanceFromTarget <= npcCharacter.navMeshAgent.stoppingDistance)
            return SwitchState(npcCharacter, npcCharacter.combatStanceState);

        // too far, comback

        NavMeshPath path = new NavMeshPath();
        npcCharacter.navMeshAgent.CalculatePath(npcCharacter.npcCombatManager.currentTarget.transform.position, path);
        npcCharacter.navMeshAgent.SetPath(path);

        return this;
    }
}
