using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScreenManager : MonoBehaviour {
    [SerializeField] private GameObject titleScreen;
    [SerializeField] private GameObject loadScreen;

    public void StartNewGame() {
        WorldManager.Instance.NewGame();
        StartCoroutine(WorldManager.Instance.LoadWorldScene(1));
    }

    public void OpenLoadGameMenu(bool value) {
        loadScreen.SetActive(value);
    }
}
