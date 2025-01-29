using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldManager : MonoBehaviour {
    private static WorldManager instance;
    public static WorldManager Instance {
        get => instance;
        private set => instance = value;
    }

    [SerializeField] private int worldSceneIndex;

    private void Awake() {
        // there can only be one of this instance script at one time, if another exist, destroy it
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    private void Start() {
        
    }

    public IEnumerator LoadNewGame() {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(worldSceneIndex);

        yield return null;
    }
}
