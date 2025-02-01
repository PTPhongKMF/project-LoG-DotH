using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSoundFXManager : MonoBehaviour {
    private static WorldSoundFXManager instance;
    public static WorldSoundFXManager Instance {
        get => instance;
        private set => instance = value;
    }

    public AudioClip dodgeSFX;

    private void Awake() {
        // there can only be one of this instance script at one time, if another exist, destroy it
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }
}
