using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEffectsManager : MonoBehaviour {
    CharacterMovementManager characterMovementManager;

    [SerializeField] private GameObject bloodSplatterVfx;

    protected virtual void Awake() {
        characterMovementManager = GetComponent<CharacterMovementManager>();
    }

    public virtual void ProcessInstantEffect(InstantCharacterEffect instantEffect) {
        instantEffect.ProcessEffect(characterMovementManager);
    }

    public void PlayBloodSplatterVfx(Vector3 contactPoint) {
        if (bloodSplatterVfx != null) {
            GameObject bloodSplatter = Instantiate(bloodSplatterVfx, contactPoint, Quaternion.identity);
        } else {
            GameObject bloodSplatter = Instantiate(WorldCharacterEffectsManager.Instance.bloodSplatterVfx, contactPoint, Quaternion.identity);
        }
    }
}
