using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcCharacterSpawner : MonoBehaviour {
    [SerializeField] private GameObject npcCharacter;
    private GameObject instantiatedNpcCharacter;

    private void Awake() {
    }

    private void Start() {
        WorldNPCManager.Instance.SpawnCharacter(this);
        gameObject.SetActive(false);
    }

    public void AttemptToSpawn() {
        if (npcCharacter != null) {
            instantiatedNpcCharacter = Instantiate(npcCharacter);
            instantiatedNpcCharacter.GetComponent<NpcCharacterManager>().Spawn(transform.position, transform.rotation);
        }
    }
}
