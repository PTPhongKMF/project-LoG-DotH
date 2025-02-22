using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantCharacterEffect : ScriptableObject {
    public int instantEffectId;

    public virtual void ProcessEffect(CharacterMovementManager character) {

    }
}
