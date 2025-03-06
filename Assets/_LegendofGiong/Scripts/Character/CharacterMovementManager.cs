using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovementManager : MonoBehaviour {
    [HideInInspector] public CharacterController characterController;
    [HideInInspector] public CharacterStatsManager characterStatsManager;
    [HideInInspector] public CharacterEffectsManager characterEffectsManager;
    [HideInInspector] public CharacterAnimatorManager characterAnimatorManager;
    [HideInInspector] public CharacterCombatManager characterCombatManager;
    [HideInInspector] public CharacterSoundFXManager characterSoundFXManager;
    [HideInInspector] public Animator animator;

    public bool isArmed = false;
    public bool revertIsArmed = false;
    public bool isDead = false;

    public bool isPerformingAction = false;
    public bool canMove = true;
    public bool canRotate = true;
    public bool canDodge = true;
    public bool canJump = true;
    public bool isJumping = false;
    public bool isGrounded = true;
    private bool isMoving = false;
    public bool isAttacking = false;

    public CharacterGroup characterGroup;

    public bool IsMoving {
        get => isMoving; 
        set {
            isMoving = value;
            animator.SetBool("isMoving", value);
        } 
    }

    protected virtual void Awake() {
        characterController = GetComponent<CharacterController>();
        characterStatsManager = GetComponent<CharacterStatsManager>();
        characterEffectsManager = GetComponent<CharacterEffectsManager>();
        characterAnimatorManager = GetComponent<CharacterAnimatorManager>();
        characterCombatManager = GetComponent<CharacterCombatManager>();
        characterSoundFXManager = GetComponent<CharacterSoundFXManager>();
        animator = GetComponent<Animator>();
    }

    protected virtual void Start() {
    }

    protected virtual void Update() {
        animator.SetBool("isGrounded", isGrounded);
    }

    protected virtual void FixedUpdate() {
        
    }

    protected virtual void LateUpdate() {

    }

    public virtual IEnumerator ProcessDeathEvent(bool manuallySelectDeathAnimation = false) {
        isDead = true;

        if (!manuallySelectDeathAnimation) {
            characterAnimatorManager.PlayTargetActionAnimation("Death", AnimationSettings.IsPerformingAction | AnimationSettings.IsGrounded);
        }
        characterSoundFXManager.PlayDeathSFX();

        yield return new WaitForSeconds(5);
    }

    public virtual void ReviveCharacter() {

    }
}
