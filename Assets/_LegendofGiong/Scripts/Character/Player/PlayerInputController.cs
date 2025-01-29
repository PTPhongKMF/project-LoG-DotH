using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInputController : MonoBehaviour {
    private static PlayerInputController instance;
    public static PlayerInputController Instance {
        get => instance;
        private set => instance = value;
    }

    PlayerControls playerControls;

    [SerializeField] private Vector2 movementInput; // keyboard WASD input
    public float HorizontalMovementInput() => movementInput.x;
    public float VerticalMovementInput() => movementInput.y;
    public float moveAmount;

    [SerializeField] private Vector2 lookInput; // mouse input
    public float HorizontalLookInput() => lookInput.x;
    public float VerticalLookInput() => lookInput.y;


    private void Awake() {
        // there can only be one of this instance script at one time, if another exist, destroy it
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }

        // run this whenever on a new screen after changed
        SceneManager.activeSceneChanged += OnSceneChange;
    }

    private void Start() {
        instance.enabled = false;
    }

    private void OnEnable() {
        if (playerControls == null) {
            playerControls = new PlayerControls();

            playerControls.Player.Move.performed += i => movementInput = i.ReadValue<Vector2>();
            playerControls.Player.Move.canceled += i => movementInput = i.ReadValue<Vector2>();

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
        HandlePlayerMovementInput();
        HandleCameraLookInput();
    }

    private void HandlePlayerMovementInput() {
        if (movementInput.x != 0 || movementInput.y != 0) {
            moveAmount = 1f; // is moving
        } else {
            moveAmount = 0f; // is idle
        }
    }

    private void HandleCameraLookInput() {

    }

    private void OnSceneChange(Scene previousScene, Scene currentScene) {
        SceneMetadata metadata = FindObjectOfType<SceneMetadata>();

        if (metadata.isPlayerMovable) {
            instance.enabled = true;
        } else {
            instance.enabled = false;
        }
    }
}
