using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldLayerUtilityManager : MonoBehaviour {
    private static WorldLayerUtilityManager instance;
    public static WorldLayerUtilityManager Instance {
        get => instance;
        private set => instance = value;
    }

    [SerializeField] LayerMask characterLayers;
    [SerializeField] LayerMask environmentLayers;

    private void Awake() {
        // there can only be one of this instance script at one time, if another exist, destroy it
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    public LayerMask GetCharacterLayers() { return characterLayers; }

    public LayerMask GetEnvironmentLayers() { return environmentLayers; }

    public float GetAngleOfTarget(Transform charactertransform, Vector3 targetsDirection) {
        targetsDirection.y = 0;
        float viewableAngle =Vector3.Angle(charactertransform.forward, targetsDirection);
        Vector3 cross = Vector3.Cross(charactertransform.forward, targetsDirection);

        if (cross.y < 0) viewableAngle = -viewableAngle;

        return viewableAngle;
    }

    public bool CanIDamageThisTarget(CharacterGroup attacker, CharacterGroup target) {
        if (attacker == CharacterGroup.Player) {
            switch (target) {
                case CharacterGroup.Player: return false;
                case CharacterGroup.Ally: return false;
                case CharacterGroup.Enemy: return true;
                case CharacterGroup.Neutral: return true;
                case CharacterGroup.Invader: return true;
            }
        }

        if (attacker == CharacterGroup.Ally) {
            switch (target) {
                case CharacterGroup.Player: return false;
                case CharacterGroup.Ally: return false;
                case CharacterGroup.Enemy: return true;
                case CharacterGroup.Neutral: return true;
                case CharacterGroup.Invader: return true;
            }
        }

        if (attacker == CharacterGroup.Enemy) {
            return true;
        }

        if (attacker == CharacterGroup.Invader) {
            switch (target) {
                case CharacterGroup.Player: return true;
                case CharacterGroup.Ally: return true;
                case CharacterGroup.Enemy: return true;
                case CharacterGroup.Neutral: return true;
                case CharacterGroup.Invader: return false;
            }
        }

        return false;
    }
}
