using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    InputManager inputManager;
    PlayerLocomotion playerLocomotion;

    public void Awake() {
        inputManager = GetComponent<InputManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
    }

    public void Update() { 
        inputManager.HandleAllInput();
    }

    public void FixedUpdate() {
        playerLocomotion.HandleAllMovement();
    }
}
