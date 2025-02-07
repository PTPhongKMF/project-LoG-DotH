using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneMetadata : MonoBehaviour {
    private static SceneMetadata instance;
    public static SceneMetadata Instance {
        get => instance;
        private set => instance = value;
    }

    public bool isPlayerMovable;
    public string locationName;

    private void Awake() {
        // there can only be one of this instance script at one time, if another exist, destroy it
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }
    }
}
