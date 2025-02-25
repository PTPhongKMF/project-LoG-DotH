using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScreenManager : MonoBehaviour {
    private static MenuScreenManager instance;
    public static MenuScreenManager Instance {
        get => instance;
        private set => instance = value;
    }

    public AlertDeleteSaveSlot alertDeleteSaveSlot;

    [SerializeField] private GameObject titleScreen;
    [SerializeField] private GameObject loadScreen;
    [SerializeField] private GameObject selectLanguageScreen;

    [SerializeField] private GameObject alertOutOfSaveSlots;

    private void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    public void StartNewGame() {
        WorldSaveManager.Instance.NewGame();
    }

    public void OpenLoadGameMenu(bool value) {
        loadScreen.SetActive(value);
    }

    public void ShowAlertOutOfSaveSlots(bool value) {
        alertOutOfSaveSlots.SetActive(value);
    }

    public void ShowSelectLanguageScreen(bool value) {
        selectLanguageScreen.SetActive(value);
    }
}
