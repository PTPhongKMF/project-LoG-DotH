using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCameraManager : MonoBehaviour {
    private static PlayerCameraManager instance;
    public static PlayerCameraManager Instance {
        get => instance;
        private set => instance = value;
    }

    public Transform viewpointObject;
    public Camera cameraObject;
    [SerializeField] private PlayerMovementController playerMovementController;

    public float horizontalLookSensitivity = 20f;
    public float verticalLookSensitivity = 10f;
    [SerializeField] private float minVerticalAngle = -89f;
    [SerializeField] private float maxVerticalAngle = 89f;

    [SerializeField] private float currentHorizontalAngle;
    [SerializeField] private float currentVerticalAngle;

    private void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    private void Start() {
    }

    public void HandleAllCameraAction() {
        HandleCameraRotation();
    }

    private void HandleCameraRotation() {
        currentHorizontalAngle += PlayerInputController.Instance.HorizontalLookInput() * horizontalLookSensitivity * Time.deltaTime;
        currentVerticalAngle -= PlayerInputController.Instance.VerticalLookInput() * verticalLookSensitivity * Time.deltaTime;

        currentVerticalAngle = Mathf.Clamp(currentVerticalAngle, minVerticalAngle, maxVerticalAngle);

        if (playerMovementController != null && playerMovementController.isArmed) {
            // When armed, rotate both the camera and the player
            transform.rotation = Quaternion.Euler(0, currentHorizontalAngle, 0);
            playerMovementController.transform.rotation = Quaternion.Euler(0, currentHorizontalAngle, 0);
        } else {
            // Normal free-look camera when not armed
            transform.rotation = Quaternion.Euler(0, currentHorizontalAngle, 0);
        }

        viewpointObject.rotation = Quaternion.Euler(currentVerticalAngle, currentHorizontalAngle, 0);
    }
}
