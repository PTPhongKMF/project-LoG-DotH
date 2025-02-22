using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldCharacterEffectsManager : MonoBehaviour {
    private static WorldCharacterEffectsManager instance;
    public static WorldCharacterEffectsManager Instance {
        get => instance;
        private set => instance = value;
    }

    public TakeDamageEffect takeDamageEffect;

    [SerializeField] private List<InstantCharacterEffect> instantEffects;

    private void Awake() {
        // there can only be one of this instance script at one time, if another exist, destroy it
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }

        GenerateEffectId();
    }

    private void GenerateEffectId() {
        for (int i = 0; i < instantEffects.Count; i++) {
            instantEffects[i].instantEffectId = i;
        }
    }
}
