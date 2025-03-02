using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcLocomotionManager : CharacterLocomotionManager {


    public void RotateTowardAgent(NpcCharacterManager npcCharacter) {
        if (npcCharacter.IsMoving) {
            npcCharacter.transform.rotation = npcCharacter.navMeshAgent.transform.rotation;
        }
    }
}
