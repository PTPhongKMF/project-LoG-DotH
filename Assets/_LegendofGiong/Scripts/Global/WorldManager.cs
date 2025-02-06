using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldManager : MonoBehaviour {
    private static WorldManager instance;
    public static WorldManager Instance {
        get => instance;
        private set => instance = value;
    }

    [SerializeField] private int worldSceneIndex;

    public PlayerStatsManager playerStatsManager;
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
            gamePath = Application.dataPath;
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
        switch (slot) {
            case SaveSlot.SaveSlot01:
                saveFileName = "CharacterSlot_01";
                break;
            case SaveSlot.SaveSlot02:
                saveFileName = "CharacterSlot_02";
                break;
            case SaveSlot.SaveSlot03:
                saveFileName = "CharacterSlot_03";
                break;
            case SaveSlot.SaveSlot04:
                saveFileName = "CharacterSlot_04";
                break;
            case SaveSlot.SaveSlot05:
                saveFileName = "CharacterSlot_05";
                break;
            case SaveSlot.SaveSlot06:
                saveFileName = "CharacterSlot_06";
                break;
            case SaveSlot.SaveSlot07:
                saveFileName = "CharacterSlot_07";
                break;
            case SaveSlot.SaveSlot08:
                saveFileName = "CharacterSlot_08";
                break;
            case SaveSlot.SaveSlot09:
                saveFileName = "CharacterSlot_09";
                break;
            case SaveSlot.SaveSlot10:
                saveFileName = "CharacterSlot_10";
                break;
            case SaveSlot.SaveSlot11:
                saveFileName = "CharacterSlot_11";
                break;
            case SaveSlot.SaveSlot12:
                saveFileName = "CharacterSlot_12";
                break;
            case SaveSlot.SaveSlot13:
                saveFileName = "CharacterSlot_13";
                break;
            case SaveSlot.SaveSlot14:
                saveFileName = "CharacterSlot_14";
                break;
            case SaveSlot.SaveSlot15:
                saveFileName = "CharacterSlot_15";
                break;
            case SaveSlot.SaveSlot16:
                saveFileName = "CharacterSlot_16";
                break;
            case SaveSlot.SaveSlot17:
                saveFileName = "CharacterSlot_17";
                break;
            case SaveSlot.SaveSlot18:
                saveFileName = "CharacterSlot_18";
                break;
            case SaveSlot.SaveSlot19:
                saveFileName = "CharacterSlot_19";
                break;
            case SaveSlot.SaveSlot20:
                saveFileName = "CharacterSlot_20";
                break;
            default:
                break;
        }

        return saveFileName;
    }

    public void NewGame() {
        DecideCharacterSaveFileName(currentCharSaveSlot);

        currentCharData = new CharacterSaveData();
    }

    public void LoadGame() {
        DecideCharacterSaveFileName(currentCharSaveSlot);

        saveLoadFileManager = new SaveLoadFileManager();
        saveLoadFileManager.saveFileDirPath = Path.Combine(gamePath, "Data", "Saves");
        saveLoadFileManager.saveFileName = saveFileName;
        currentCharData = saveLoadFileManager.LoadSaveFile();

        StartCoroutine(LoadWorldScene(currentCharData.worldSceneIndex));
    }

    public void SaveGame() {
        DecideCharacterSaveFileName(currentCharSaveSlot);

        saveLoadFileManager = new SaveLoadFileManager();
        saveLoadFileManager.saveFileDirPath = Path.Combine(gamePath, "Data", "Saves");
        saveLoadFileManager.saveFileName = saveFileName;

        SaveGameDataToCurrentCharData();

        saveLoadFileManager.CreateSaveFile(currentCharData);
    }

    public void SaveGameDataToCurrentCharData() {
        currentCharData.charName = playerStatsManager.charName;
        currentCharData.xWorldPosition = playerStatsManager.transform.position.x;
        currentCharData.yWorldPosition = playerStatsManager.transform.position.y;
        currentCharData.zWorldPosition = playerStatsManager.transform.position.z;
    }

    public void LoadGameDataFromCurrentCharData() {
        playerStatsManager.charName = currentCharData.charName;
        Vector3 currentPosition = new Vector3(currentCharData.xWorldPosition, currentCharData.yWorldPosition, currentCharData.zWorldPosition);
        transform.position = currentPosition;
    }

    //private 

    public IEnumerator LoadWorldScene(int sceneIndex) {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneIndex);

        yield return null;
    }

    private void OnSceneChange(Scene previousScene, Scene currentScene) {
        worldSceneIndex = SceneManager.GetActiveScene().buildIndex;
    }
}
