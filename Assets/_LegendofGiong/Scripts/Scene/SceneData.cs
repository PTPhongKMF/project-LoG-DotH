using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneData : MonoBehaviour {
    private static SceneData instance;
    public static SceneData Instance {
        get => instance;
        private set => instance = value;
    }
    public bool isSafeToSave;
    public bool isPlayerMovable;
    public Vector3 playerDefaultSpawnPoint = Vector3.zero;

    public string locationName;

    public GameObject[] allNPCInScene;

    [Header("Starter_Villiage")]
    public bool hasStartFirstFightAndWeapon = false;
    public bool hasFirstFightAndWeapon = false;


    private void Awake() {
        // there can only be one of this instance script at one time, if another exist, destroy it
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }
    }
}
