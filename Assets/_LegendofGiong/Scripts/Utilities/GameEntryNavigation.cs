using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEntryNavigation : MonoBehaviour {


    void Start() {
        StartCoroutine(DelayEntry(2f));

        StartCoroutine(WorldSaveManager.Instance.LoadWorldScene("MenuScreenScene"));
    }

    private IEnumerator DelayEntry(float delay) {
        yield return new WaitForSeconds(delay);
    }
}
