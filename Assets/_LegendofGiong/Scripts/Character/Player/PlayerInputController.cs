using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInputController : MonoBehaviour {
    private static PlayerInputController instance;
    public static PlayerInputController Instance {
        get {
            return instance;
        }
        private set {
            instance = value;
        }
    }

    PlayerControls playerControls;

    [SerializeField] private Vector2 movementInput;
    [SerializeField] private float horizontalInput;
    [SerializeField] private float verticalInput;
    public float moveAmount;


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

    private void OnSceneChange(Scene previousScene, Scene currentScene) {
        SceneMetadata metadata = FindObjectOfType<SceneMetadata>();

        if (metadata.isPlayerMovable) {
            instance.enabled = true; 
        } else {
            instance.enabled = false; 
        }
    }

    private void OnEnable() {
        if (playerControls == null) {
            playerControls = new PlayerControls();

            playerControls.Player.Move.performed += i => movementInput = i.ReadValue<Vector2>();
            playerControls.Player.Move.canceled += i => movementInput = i.ReadValue<Vector2>();
        }

        playerControls.Enable();
    }

    private void OnDestroy() {
        SceneManager.activeSceneChanged -= OnSceneChange;
    }

    private void Update() {
        HandleMovementInput();
    }

    private void HandleMovementInput() {
        horizontalInput = movementInput.x;
        verticalInput = movementInput.y;

        if (horizontalInput != 0 || verticalInput != 0) {
            moveAmount = 1f; // moving
        } else {
            moveAmount = 0f; // idle
        }
    }

    public (float, float) GetHorizontalAndVerticalInputs() {
        return (horizontalInput, verticalInput);
    }
}
