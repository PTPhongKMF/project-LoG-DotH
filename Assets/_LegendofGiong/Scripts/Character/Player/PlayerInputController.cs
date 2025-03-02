using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using static UnityEngine.Rendering.DebugUI;

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

    [SerializeField] private bool lmb_Input = false;

    [SerializeField] private Vector2 armedMoveValue;
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
            playerControls.Player.ArmingButton.performed += i => {
                //playerMovementController.isArmed = sprintInput ? false : !playerMovementController.isArmed
                if (sprintInput) return;
                playerMovementController.isArmed = !playerMovementController.isArmed;

                if (!playerMovementController.isArmed) {
                    playerMovementController.playerEquipmentManager.UnloadWeaponOnBothHands();
                    PlayerUIManager.Instance.playerUIHudManager.allWeaponSlotsCanvasGroup.alpha = 0;
                } else {
                    playerMovementController.playerEquipmentManager.LoadWeaponOnBothHands();
                    PlayerUIManager.Instance.playerUIHudManager.allWeaponSlotsCanvasGroup.alpha = 1f;
                }
            };
            playerControls.Player.QuickSlot1.performed += i => playerMovementController.playerEquipmentManager.SwitchRightWeaponToIndex(0);
            playerControls.Player.QuickSlot2.performed += i => playerMovementController.playerEquipmentManager.SwitchRightWeaponToIndex(1);
            playerControls.Player.QuickSlot3.performed += i => playerMovementController.playerEquipmentManager.SwitchRightWeaponToIndex(2);
            playerControls.Player.QuickSlot4.performed += i => { };
            playerControls.Player.QuickSlot5.performed += i => { };

            playerControls.Player.LeftMouseButton.performed += i => lmb_Input = true;

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
        HandleDodgeSprintInput();
        HandleJumpInput();
        HandleLmbInput();
    }

    // MOVEMENT
    private void HandlePlayerMovementInput() {
        if (movementInput.x != 0 || movementInput.y != 0) {
            if (sprintInput) {
                moveValue = sprintValue;
            } else if (walkInput) {
                if (playerMovementController.revertIsArmed) {
                    playerMovementController.revertIsArmed = false;
                    playerMovementController.isArmed = true;
                }
                moveValue = walkValue;
                armedMoveValue = GetArmedWalkValue(movementInput);
            } else {
                if (playerMovementController.revertIsArmed) {
                    playerMovementController.revertIsArmed = false;
                    playerMovementController.isArmed = true;
                }
                moveValue = runValue;
                armedMoveValue = movementInput;
            }
        } else {
            if (playerMovementController.revertIsArmed) {
                playerMovementController.revertIsArmed = false;
                playerMovementController.isArmed = true;
            }
            moveValue = idleValue;
            sprintInput = false;
            armedMoveValue = Vector2.zero;
        }

        if (moveValue != 0) playerMovementController.IsMoving = true;
        else playerMovementController.IsMoving = false;

        if (playerMovementController.isArmed && !sprintInput) {
            playerMovementController.playerAnimatorController.UpdateAnimatorMovementValues(armedMoveValue.x, armedMoveValue.y);
        } else {
            // use 0 and moveAmount to prevent strafing, only enable strafing when in combat
            playerMovementController.playerAnimatorController.UpdateAnimatorMovementValues(0, moveValue);
        }
    }

    private Vector2 GetArmedWalkValue(Vector2 movementInput) {
        Vector2 armedValue = Vector2.zero;
        switch ((Mathf.RoundToInt(movementInput.x * 10f), Mathf.RoundToInt(movementInput.y * 10f))) {
            case (0, 10):
                armedValue = new Vector2(0, 0.5f);
                break;
            case (-7, 7):
                armedValue = new Vector2(-0.2f, 0.2f);
                break;
            case (7, 7):
                armedValue = new Vector2(0.2f, 0.2f);
                break;
            case (0, -10):
                armedValue = new Vector2(0, -0.5f);
                break;
            case (-7, -7):
                armedValue = new Vector2(-0.2f, -0.2f);
                break;
            case (7, -7):
                armedValue = new Vector2(0.2f, -0.2f);
                break;
            case (-10, 0):
                armedValue = new Vector2(-0.5f, 0);
                break;
            case (10, 0):
                armedValue = new Vector2(0.5f, 0);
                break;
        }

        return armedValue;
    }

    // ACTION
    private void HandleDodgeSprintInput() {
        if (dodgeSprintInputContext is TapInteraction) {
            dodgeSprintInputContext = default;
            playerMovementController.playerLocomotionController.AttemptToPerformDodge();
        } else if (dodgeSprintInputContext is HoldInteraction) {
            if (playerMovementController.isArmed) {
                playerMovementController.revertIsArmed = true;
                playerMovementController.isArmed = false;
            }
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

    private void HandleLmbInput() {
        if (lmb_Input) {
            lmb_Input = false;

            // if we on ui dont do anyhthing, just return

            if (playerMovementController.isArmed) {
                playerMovementController.SetPlayerActionHand(true);
                playerMovementController.playerCombatManager.PerformWeaponBasedAction(playerMovementController.playerInventoryManager.currentRightHandWeapon
                    .lmb_Action, playerMovementController.playerInventoryManager.currentRightHandWeapon);
            }
        }
    }

    // OTHER
    private void OnSceneChange(Scene previousScene, Scene currentScene) {
        instance.enabled = SceneData.Instance.isPlayerMovable;
    }
}
