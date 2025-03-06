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

    public bool isPerformingLoadingOperation = false;

    [SerializeField] private List<NpcCharacterSpawner> npcCharacterSpawners;
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
    }

    public void SpawnCharacter(NpcCharacterSpawner npcCharacterSpawner) {
        npcCharacterSpawners.Add(npcCharacterSpawner);
        npcCharacterSpawner.AttemptToSpawn(); 
    }

    private void DespawnAllCharacters() {
        int arrayLength = spawnedInCharacters.Count;
        for (int i = 0; i < arrayLength; i++) {
            spawnedInCharacters[i].GetComponent<NpcCharacterManager>().Despawn();
        }
    }
}
