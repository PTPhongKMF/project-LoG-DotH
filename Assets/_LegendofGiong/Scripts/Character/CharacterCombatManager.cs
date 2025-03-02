using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCombatManager : MonoBehaviour {
    protected CharacterMovementManager currentCharacter;
    public CharacterMovementManager currentTarget;

    public AttackType currentAttackType;

    public string lastAttackAnimationPerformed;

    public Transform lockOnTransform;

    protected virtual void Awake() {
        currentCharacter = GetComponent<CharacterMovementManager>();
        lockOnTransform = GetComponentInChildren<LockOnTransform>().transform;
    }
}
