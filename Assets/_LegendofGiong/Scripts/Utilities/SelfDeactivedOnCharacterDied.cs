using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDeactivedOnCharacterDied : MonoBehaviour {
    [SerializeField] private CharacterMovementManager character;

    private void Update() {
        if (character.isDead)
            gameObject.SetActive(false);
    }
}
