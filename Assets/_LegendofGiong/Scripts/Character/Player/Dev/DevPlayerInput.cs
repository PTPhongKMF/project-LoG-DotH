using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevPlayerInput : MonoBehaviour {
    [SerializeField] private bool inputEnabled;

    private void Start() {
        if (PlayerInputController.Instance != null) {
            PlayerInputController.Instance.enabled = inputEnabled;
        }
    }

    private void OnValidate() {
        if (PlayerInputController.Instance != null) {
            PlayerInputController.Instance.enabled = inputEnabled;
        }
    }
}
