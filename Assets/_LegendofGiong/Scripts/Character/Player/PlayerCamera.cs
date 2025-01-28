using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCamera : MonoBehaviour {
    private static PlayerCamera instance;
    public static PlayerCamera Instance {
        get {
            return instance;
        }
        private set {
            instance = value;
        }
    }

    public Camera cameraObject;

    private void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    private void Start() {
        DontDestroyOnLoad(gameObject);
    }
}
