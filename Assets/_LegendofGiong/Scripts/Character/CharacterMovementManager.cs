using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovementManager : MonoBehaviour {
    [HideInInspector] public CharacterController characterController;
    [HideInInspector] public CharacterStatsManager characterStatsManager;
    [HideInInspector] public CharacterEffectsManager characterEffectsManager;
    [HideInInspector] public Animator animator;

    public bool isDead = false;

    public bool isPerformingAction = false;
    public bool canMove = true;
    public bool canRotate = true;
    public bool canDodge = true;
    public bool canJump = true;
    public bool isJumping = false;
    public bool isGrounded = true;

    protected virtual void Awake() {
        DontDestroyOnLoad(this);

        characterController = GetComponent<CharacterController>();
        characterStatsManager = GetComponent<CharacterStatsManager>();
        characterEffectsManager = GetComponent<CharacterEffectsManager>();
        animator = GetComponent<Animator>();
    }

    protected virtual void Start() {

    }

    protected virtual void Update() {
        animator.SetBool("isGrounded", isGrounded);
    }

    protected virtual void LateUpdate() {

    }
}
