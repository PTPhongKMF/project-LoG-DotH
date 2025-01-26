using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {
    PlayerControls playerControls;
    AnimatorManager animatorManager;

    public Vector2 movementInput;
    private float moveAmount;
    public float verticalInput;
    public float horizontalInput;

    private void Awake() {
        playerControls = new PlayerControls();
        animatorManager = GetComponent<AnimatorManager>();
    }

    private void OnEnable() {
        playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
        playerControls.Enable();
    }
    private void OnDisable() {
        playerControls.Disable();
    }

    public void HandleAllInput() {
        HandleMovementInput();
    }

    private void HandleMovementInput() {
        verticalInput = movementInput.y;
        horizontalInput = movementInput.x;
        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));
        animatorManager.UpdateAnimatorValues(0, moveAmount);
    }
}
