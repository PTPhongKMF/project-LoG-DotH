using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldNPCManager : MonoBehaviour {
    private static WorldNPCManager instance;
    public static WorldNPCManager Instance {
        get => instance;
        private set => instance = value;
    }

    [SerializeField] private GameObject[] npcCharacters;
    [SerializeField] private List<GameObject> spawnedInCharacters;

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
        StartCoroutine(WaitForSceneToLoad());
    }

    private IEnumerator WaitForSceneToLoad() {
        while (!SceneManager.GetActiveScene().isLoaded) {
            yield return null;
        }
        SpawnAllCharacters();
    }

    private void SpawnAllCharacters() {
        GetAllNPCInScene();

        int arrayLength = npcCharacters.Length;
        for (int i = 0; i < arrayLength; i++) {
            GameObject instantiatedCharacter = Instantiate(npcCharacters[i]);
            instantiatedCharacter.GetComponent<NpcCharacterManager>().Spawn();
            spawnedInCharacters.Add(instantiatedCharacter);
        }
    }

    private void GetAllNPCInScene() {
        npcCharacters = SceneData.Instance.allNPCInScene;
    }

    private void DespawnAllCharacters() {
        int arrayLength = spawnedInCharacters.Count;
        for (int i = 0; i < arrayLength; i++) {
            spawnedInCharacters[i].GetComponent<NpcCharacterManager>().Despawn();
        }
    }
}
