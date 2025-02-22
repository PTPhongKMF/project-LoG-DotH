using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEffectsManager : MonoBehaviour {
    CharacterMovementManager characterMovementManager;

    protected virtual void Awake() {
        characterMovementManager = GetComponent<CharacterMovementManager>();
    }

    public virtual void ProcessInstantEffect(InstantCharacterEffect instantEffect) {
        instantEffect.ProcessEffect(characterMovementManager);
    }
}
