using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLocomotionManager : MonoBehaviour {
    private CharacterMovementManager characterMovementManager;

    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckSphereRadius = 1f;
    [SerializeField] protected Vector3 yVelocity; // force at which pull character up or down (jumping or falling)
    [SerializeField] protected float groundedYVelocity = -20f; // force at which character is sticking to the ground while isGrounded
    [SerializeField] protected float fallStartYVelocity = -5f; // force which character begin to fall (rise as fall longer)
    protected bool hasFallingVelocityBeenSet = false;
    protected float inAirTimer = 0f;

    protected virtual void Awake() {
        characterMovementManager = GetComponent<CharacterMovementManager>();
    }

    protected virtual void Update() {
        HandleGroundCheck();
    }

    protected void HandleGroundCheck() {
        characterMovementManager.isGrounded = Physics.CheckSphere(characterMovementManager.transform.position, groundCheckSphereRadius, groundLayer);
    }

    protected void OnDrawGizmosSelected() {
        Gizmos.DrawSphere(characterMovementManager.transform.position, groundCheckSphereRadius);
    }
}
