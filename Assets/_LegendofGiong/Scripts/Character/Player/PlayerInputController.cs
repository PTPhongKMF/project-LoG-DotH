using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class PlayerInputController : MonoBehaviour {
    private static PlayerInputController instance;
    public static PlayerInputController Instance {
        get => instance;
        private set => instance = value;
    }

    [SerializeField] private GameObject player;
    private PlayerControls playerControls;
    [HideInInspector] public PlayerMovementController playerMovementController;

    [SerializeField] private Vector2 movementInput; // keyboard WASD input
    public float HorizontalMovementInput() => movementInput.x;
    public float VerticalMovementInput() => movementInput.y;

    [HideInInspector] public bool walkInput = false;
    [HideInInspector] public bool sprintInput = false;
    private IInputInteraction dodgeSprintInputContext;
    [HideInInspector] public bool jumpInput = false;

    public float moveValue;
    private float idleValue = 0f;
    private float walkValue = 0.5f;
    private float runValue = 1f;
    private float sprintValue = 1.5f;

    [SerializeField] private Vector2 lookInput; // mouse input
    public float HorizontalLookInput() => lookInput.x;
    public float VerticalLookInput() => lookInput.y;

    private void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }

        playerMovementController = player.GetComponent<PlayerMovementController>();
        SceneManager.activeSceneChanged += OnSceneChange;
    }

    private void Start() {

    }

    private void OnEnable() {
        if (playerControls == null) {
            playerControls = new PlayerControls();
            playerControls.Player.Move.performed += i => movementInput = i.ReadValue<Vector2>();
            playerControls.Player.Move.canceled += i => movementInput = i.ReadValue<Vector2>();
            playerControls.Player.Walk.performed += i => walkInput = !walkInput;
            playerControls.Player.DodgeSprint.performed += i => dodgeSprintInputContext = i.interaction;
            playerControls.Player.Jump.performed += i => jumpInput = true;

            playerControls.Camera.Look.performed += i => lookInput = i.ReadValue<Vector2>();
            playerControls.Camera.Look.canceled += i => lookInput = i.ReadValue<Vector2>();
        }
        playerControls.Enable();
    }

    private void OnDestroy() {
        SceneManager.activeSceneChanged -= OnSceneChange;
    }

    private void OnApplicationFocus(bool focus) {
        if (enabled) {
            if (focus) {
                playerControls.Enable();
            } else {
                playerControls.Disable();
            }
        }
    }

    private void Update() {
        HandleAllInputs();
    }

    private void HandleAllInputs() {
        HandlePlayerMovementInput();
        HandleCameraLookInput();
        HandleDodgeSprintInput();
    }

    // MOVEMENT
    private void HandlePlayerMovementInput() {
        if (movementInput.x != 0 || movementInput.y != 0) {
            if (sprintInput) {
                moveValue = sprintValue;
            } else if (walkInput) {
                moveValue = walkValue;
            } else {
                moveValue = runValue;
            }
        } else {
            moveValue = idleValue;
            sprintInput = false;
        }

        // use 0 and moveAmount to prevent strafing, only enable strafing when in combat
        playerMovementController.playerAnimatorController.UpdateAnimatorMovementValues(0, moveValue);
    }

    private void HandleCameraLookInput() {
        // maybe some implement in the future?
    }

    // ACTION
    private void HandleDodgeSprintInput() {
        if (dodgeSprintInputContext is TapInteraction) {
            dodgeSprintInputContext = default;
            playerMovementController.playerLocomotionController.AttemptToPerformDodge();
        } else if (dodgeSprintInputContext is HoldInteraction) {
            dodgeSprintInputContext = default;
            sprintInput = true;
        }
    }

    private void HandleJumpInput() {
        if (jumpInput) {
            jumpInput = false;

            playerMovementController.playerLocomotionController.AttemptToPerformJump();
        }
    }

    // OTHER
    private void OnSceneChange(Scene previousScene, Scene currentScene) {
        instance.enabled = SceneMetadata.Instance.isPlayerMovable;
    }
}
