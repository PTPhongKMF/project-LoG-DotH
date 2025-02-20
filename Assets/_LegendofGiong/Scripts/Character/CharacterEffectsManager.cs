using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEffectsManager : MonoBehaviour {
    CharacterStatsManager characterStatsManager;

    protected virtual void Awake() {
        characterStatsManager = GetComponent<CharacterStatsManager>();
    }

    public virtual void ProcessInstantEffect(InstantCharacterEffect instantEffect) {
        instantEffect.ProcessEffect(characterStatsManager);
    }
}
