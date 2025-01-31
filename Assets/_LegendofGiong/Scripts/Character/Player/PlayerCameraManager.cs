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

    public float horizontalLookSensitivity = 50f;
    public float verticalLookSensitivity = 30f;
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

        // rotate the viewpoint left and right so player know what direction is forward
        transform.rotation = Quaternion.Euler(0, currentHorizontalAngle, 0);

        // rotate the viewpoint up/down/left/right by adjusting the viewpoint child object of the player object
        viewpointObject.rotation = Quaternion.Euler(currentVerticalAngle, currentHorizontalAngle, 0);
    }
}
