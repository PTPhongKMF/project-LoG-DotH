using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovementManager : MonoBehaviour {
    [HideInInspector] public CharacterController characterController;
    [HideInInspector] public Animator animator;

    public bool isPerformingAction = false;

    protected virtual void Awake() {
        DontDestroyOnLoad(this);

        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
    }

    protected virtual void Update() {
        
    }

    protected virtual void LateUpdate() {

    }
}
