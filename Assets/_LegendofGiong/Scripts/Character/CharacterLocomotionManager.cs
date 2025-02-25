using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLocomotionManager : MonoBehaviour {
    private CharacterMovementManager characterMovementManager;

    [SerializeField] private LayerMask groundLayer;
    [SerializeField] protected float gravityForce = -40f;
    [SerializeField] private float groundCheckSphereRadius = 0.3f;
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

        if (characterMovementManager.isGrounded) {
            if (yVelocity.y < 0f) {
                inAirTimer = 0;
                hasFallingVelocityBeenSet = false;
                yVelocity.y = groundedYVelocity;
            }
        } else {
            // if we not jumping and falling velocity has not been set
            if (!characterMovementManager.isJumping && !hasFallingVelocityBeenSet) { 
                hasFallingVelocityBeenSet = true;
                yVelocity.y = fallStartYVelocity;
            }

            inAirTimer += Time.deltaTime;
            characterMovementManager.animator.SetFloat("InAirTimer", inAirTimer);
            yVelocity.y += gravityForce * Time.deltaTime;
        }
        characterMovementManager.characterController.Move(yVelocity * Time.deltaTime);
    }

    protected void HandleGroundCheck() {
        characterMovementManager.isGrounded = Physics.CheckSphere(characterMovementManager.transform.position, groundCheckSphereRadius, groundLayer);
    }

    //protected void OnDrawGizmosSelected() {
    //    Gizmos.DrawSphere(characterMovementManager.transform.position, groundCheckSphereRadius);
    //}
}
