using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {
    private PlayerControls playerControls;
    private AnimatorController animatorController;

    public Vector2 movementInput;
    public Vector2 cameraInput;

    private float moveAmount;
    public float verticalInput;
    public float horizontalInput;
    public float cameraInputX;
    public float cameraInputY;

    private void Awake() {
        playerControls = new PlayerControls();
        animatorController = GetComponent<AnimatorController>();
    }

    private void OnEnable() {
        playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
        playerControls.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();

        playerControls.Enable();
    }
    private void OnDisable() {
        playerControls.Disable();
    }

    public void HandleAllInput() {
        HandleMovementInput();
    }

    private void HandleMovementInput() {
        horizontalInput = movementInput.x;
        verticalInput = movementInput.y;
        
        cameraInputX = cameraInput.x;
        cameraInputY = cameraInput.y;

        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));
        animatorController.UpdateAnimatorValues(0, moveAmount);
    }
}
