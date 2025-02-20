using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldSaveManager : MonoBehaviour {
    private static WorldSaveManager instance;
    public static WorldSaveManager Instance {
        get => instance;
        private set => instance = value;
    }

    [SerializeField] private string worldSceneName;

    private PlayerStatsManager playerStatsManager;
    private SaveLoadFileManager saveLoadFileManager;

    public string gamePath;

    [SerializeField] private bool isSavingGame;
    [SerializeField] private bool isLoadingGame;

    public SaveSlot currentCharSaveSlot;
    public CharacterSaveData currentCharData;
    private string saveFileName;

    public CharacterSaveData saveSlot01; public CharacterSaveData saveSlot02; public CharacterSaveData saveSlot03; public CharacterSaveData saveSlot04;
    public CharacterSaveData saveSlot05; public CharacterSaveData saveSlot06; public CharacterSaveData saveSlot07; public CharacterSaveData saveSlot08;
    public CharacterSaveData saveSlot09; public CharacterSaveData saveSlot10; public CharacterSaveData saveSlot11; public CharacterSaveData saveSlot12;
    public CharacterSaveData saveSlot13; public CharacterSaveData saveSlot14; public CharacterSaveData saveSlot15; public CharacterSaveData saveSlot16;
    public CharacterSaveData saveSlot17; public CharacterSaveData saveSlot18; public CharacterSaveData saveSlot19; public CharacterSaveData saveSlot20;

    private void Awake() {
        // there can only be one of this instance script at one time, if another exist, destroy it
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }

        if (playerStatsManager == null) {
            playerStatsManager = GameObject.FindWithTag("Player").GetComponent<PlayerStatsManager>();
        }

        SceneManager.activeSceneChanged += OnSceneChange;

        if (!Application.isEditor) {
            gamePath = Directory.GetParent(gamePath).FullName;
        } else {
            gamePath = Path.Combine(Application.dataPath, "_Test SaveLoad");
        }
    }

    private void Start() {

    }

    private void Update() {
        if (isSavingGame) {
            isSavingGame = false;
            SaveGame();
        }
        if (isLoadingGame) {
            isLoadingGame = false;
            LoadGame();
        }
    }

    private void OnDestroy() {
        SceneManager.activeSceneChanged -= OnSceneChange;
    }

    public string DecideCharacterSaveFileName(SaveSlot slot) {
        saveFileName = slot.ToString();

        return saveFileName;
    }

    public void NewGame() {
        saveLoadFileManager = new SaveLoadFileManager();
        saveLoadFileManager.saveFileDirPath = Path.Combine(gamePath, "Data", "Saves");

        foreach (SaveSlot slot in Enum.GetValues(typeof(SaveSlot))) {
            saveLoadFileManager.saveFileName = DecideCharacterSaveFileName(slot);

            if (!saveLoadFileManager.IsSaveFileExists()) {
                currentCharSaveSlot = slot;
                currentCharData = new CharacterSaveData();
                StartCoroutine(LoadWorldScene("Village"));
                return;
            }
        }

        MenuScreenManager.Instance.ShowAlertOutOfSaveSlots(true);
    }

    public void LoadGame() {
        DecideCharacterSaveFileName(currentCharSaveSlot);

        saveLoadFileManager = new SaveLoadFileManager();
        saveLoadFileManager.saveFileDirPath = Path.Combine(gamePath, "Data", "Saves");
        saveLoadFileManager.saveFileName = saveFileName;
        currentCharData = saveLoadFileManager.LoadSaveFile();

        StartCoroutine(LoadWorldScene(currentCharData.worldSceneName));
    }

    public void SaveGame() {
        DecideCharacterSaveFileName(currentCharSaveSlot);

        saveLoadFileManager = new SaveLoadFileManager();
        saveLoadFileManager.saveFileDirPath = Path.Combine(gamePath, "Data", "Saves");
        saveLoadFileManager.saveFileName = saveFileName;

        WriteGameDataToCurrentCharData();

        saveLoadFileManager.CreateSaveFile(currentCharData);
    }

    public void DeleteGame(SaveSlot slot) {
        saveLoadFileManager = new SaveLoadFileManager();
        saveLoadFileManager.saveFileDirPath = Path.Combine(gamePath, "Data", "Saves");
        saveLoadFileManager.saveFileName = slot.ToString();

        saveLoadFileManager.DeleteSaveFile();
    }

    public void WriteGameDataToCurrentCharData() {
        currentCharData.charName = playerStatsManager.charName;
        currentCharData.secondsPlayed = 1;
        currentCharData.worldSceneName = worldSceneName;
        currentCharData.locationName = SceneMetadata.Instance.locationName;
        currentCharData.xWorldPosition = playerStatsManager.transform.position.x;
        currentCharData.yWorldPosition = playerStatsManager.transform.position.y;
        currentCharData.zWorldPosition = playerStatsManager.transform.position.z;
        currentCharData.healthPoint = playerStatsManager.HealthPoint;
        currentCharData.stamPoint = playerStatsManager.StamPoint;
    }

    public void ReadGameDataFromCurrentCharData() {
        playerStatsManager.charName = currentCharData.charName;
        Vector3 currentPosition = new Vector3(currentCharData.xWorldPosition, currentCharData.yWorldPosition, currentCharData.zWorldPosition);
        playerStatsManager.transform.position = currentPosition;

        playerStatsManager.HealthPoint = currentCharData.healthPoint;
        playerStatsManager.StamPoint = currentCharData.stamPoint;
    }

    public IEnumerator LoadWorldScene(string sceneName) {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneName);

        ReadGameDataFromCurrentCharData();

        yield return null;
    }

    private void OnSceneChange(Scene previousScene, Scene currentScene) {
        worldSceneName = SceneManager.GetActiveScene().name;
    }
}
