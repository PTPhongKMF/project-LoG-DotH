using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "NPC/AI States/Idle")]
public class NpcIdleState : NpcAIState {


    public override NpcAIState Tick(NpcCharacterManager npcCharacter) {
        if (npcCharacter.characterCombatManager.currentTarget != null) {
            return SwitchState(npcCharacter, npcCharacter.pursueTargetState);
        } else {
            npcCharacter.npcCombatManager.FindATargetViaLineOfSight(npcCharacter);
            return this;
        }
    }
}
