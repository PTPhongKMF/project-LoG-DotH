using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalWar : MonoBehaviour {
    [SerializeField] private CharacterMovementManager[] characters;
    private int length;

    private void Awake() {
        length = characters.Length;
    }

    private void Update() {
        for (int i = 0; i < length; i++) {
            if (characters[i].characterGroup == CharacterGroup.Invader && !characters[i].isDead || characters[i].characterStatsManager.CurrentHealth > 0)
                return;
        }

        StartCoroutine(Delay(10));

        StartCoroutine(WorldSaveManager.Instance.LoadWorldScene("OutroCutscene"));
    }

    private IEnumerator Delay(float delay) {
        yield return new WaitForSeconds(delay);
    }
}
