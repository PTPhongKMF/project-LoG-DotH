using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerUIManager : MonoBehaviour {
    private static PlayerUIManager instance;
    public static PlayerUIManager Instance {
        get => instance;
        private set => instance = value;
    }

    [HideInInspector] public PlayerUIHudManager playerUIHudManager;
    [HideInInspector] public PlayerUIPopupManager playerUIPopupManager;
    [HideInInspector] public PlayerUIStatsManager playerUIStatsManager;
    [HideInInspector] public PlayerUILoadingScreen playerUILoadingScreen;
    [HideInInspector] public MinigameManager minigameManager;
    public EatingMinigame eatingMinigame;

    public GameObject eatingMiniGame;

    public bool menuWindowIsOpen = false;
    public bool popupWindowIsOpen = false;
    public bool isPerformingLoadingOperation = false;

    private void Awake() {
        // there can only be one of this instance script at one time, if another exist, destroy it
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }

        playerUIHudManager = GetComponentInChildren<PlayerUIHudManager>();
        playerUIPopupManager = GetComponentInChildren<PlayerUIPopupManager>();
        playerUIStatsManager = GetComponentInChildren<PlayerUIStatsManager>();
        playerUILoadingScreen = GetComponentInChildren<PlayerUILoadingScreen>();
        minigameManager = GetComponentInChildren<MinigameManager>();
        if (eatingMinigame == null) eatingMinigame = GetComponentInChildren<EatingMinigame>();
    }

    //public void CloseAllMenuWindows() {
    //    playerUIMenuManager.CloseCharacterMenu();
    //}
}
