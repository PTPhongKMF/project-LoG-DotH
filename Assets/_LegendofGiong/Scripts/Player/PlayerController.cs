using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    private InputManager inputManager;
    //private CameraController cameraController;
    private PlayerLocomotion playerLocomotion;

    private void Awake() {
        inputManager = GetComponent<InputManager>();
        //cameraController = FindObjectOfType<CameraController>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
    }

    private void Update() { 
        inputManager.HandleAllInput();
    }

    private void FixedUpdate() {
        playerLocomotion.HandleAllMovement();
    }

    private void LateUpdate() {
        //cameraController.HandleAllCameraMovement();
    }
}
