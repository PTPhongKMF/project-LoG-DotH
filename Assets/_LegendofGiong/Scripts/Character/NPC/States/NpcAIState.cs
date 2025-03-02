using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcAIState : ScriptableObject {
    public virtual NpcAIState Tick(NpcCharacterManager npcCharacter) {

        return this;
    }

    protected virtual NpcAIState SwitchState(NpcCharacterManager npcCharacter, NpcAIState newState) {
        ResetStateFlags(npcCharacter);
        return newState;
    }

    protected virtual void ResetStateFlags(NpcCharacterManager npcCharacter) {

    }
}
